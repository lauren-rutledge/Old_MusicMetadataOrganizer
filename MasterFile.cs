using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Text.RegularExpressions;

namespace MusicMetadataOrganizer
{
    public class MasterFile
    {
        public string Filepath { get; private set; }
        public Dictionary<string, object> TagLibProps;
        public Dictionary<string, object> SysIOProps;

        private TagLib.File TagLibFile { get; set; }
        private FileInfo SysIOFile { get; set; }

        private MasterFile(string filepath)
        {
            this.Filepath = filepath;
            CreateTagLibFile();
            if (TagLibFile == null)
                return;
            CreateSysIOFile();

            TagLibProps = new Dictionary<string, object>();
            SysIOProps = new Dictionary<string, object>();
            PopulateFields();
        }

        private MasterFile(Dictionary<string, object>[] properties)
        {
            TagLibProps = properties[0];
            SysIOProps = properties[1];
            Filepath = TagLibProps["Filepath"].ToString();
        }

        // Change this to check for null taglib file or sysio file objects, not null filepath
        public static MasterFile GetMasterFileFromFilepath(string filepath)
        {
            if (!String.IsNullOrEmpty(filepath))
                return new MasterFile(filepath);
            else return null;
        }

        public static MasterFile GetMasterFileFromDB(Dictionary<string, object>[] properties)
        {
            foreach (var collection in properties)
            {
                if (collection == null || collection.Count == 0)
                    return null;
            }
            return new MasterFile(properties);
        }

        private void CreateTagLibFile()
        {
            try
            {
                TagLibFile = TagLib.File.Create(Filepath);
            }
            catch (IOException ex)
            {
                var log = new LogWriter($"Could not create a TagLibFile object from {Filepath}. \"{ex.Message}\"");
            }
            catch (Exception ex)
            {
                var log = new LogWriter($"Could not create a TagLibFile object from {Filepath}. \"{ex.Message}\"");
                return;
                // Maybe should move the file into an 'error folder' 
            }
        }

        private void CreateSysIOFile()
        {
            try
            {
                SysIOFile = new FileInfo(Filepath);
            }
            catch (DirectoryNotFoundException ex)
            {
                var log = new LogWriter($"Could not create a TagLibFile object from {Filepath}. \"{ex.Message}\"");
                throw new DirectoryNotFoundException("Invalid directory.");
            }
            catch (FileNotFoundException ex)
            {
                var log = new LogWriter($"Could not create a TagLibFile object from {Filepath}. \"{ex.Message}\"");
                throw new FileNotFoundException("Could not locate " + Filepath + ".");
            }
        }

        private void PopulateFields()
        {
            if (TagLibFile != null)
                PopulateTagLibFields();
            if (SysIOFile != null)
                PopulateSysIOFields();
        }

        private void PopulateTagLibFields()
        {
            TagLibProps.Add("Filepath", TagLibFile.Name);
            TagLibProps.Add("BitRate", TagLibFile.Properties.AudioBitrate);
            TagLibProps.Add("MediaType", TagLibFile.Properties.MediaTypes.ToString());
            TagLibProps.Add("Artist", TagLibFile.Tag.FirstAlbumArtist ?? "Unknown");
            TagLibProps.Add("Album", TagLibFile.Tag.Album ?? "Unknown");
            string genres = "";
            for (int i = 0; i < TagLibFile.Tag.Genres.Length; i++)
            {
                if (i < TagLibFile.Tag.Genres.Length - 1)
                    genres += TagLibFile.Tag.Genres[i] + ", ";
                else
                    genres += TagLibFile.Tag.Genres[i];
            }
            TagLibProps.Add("Genres", genres);
            TagLibProps.Add("Lyrics", TagLibFile.Tag.Lyrics ?? "");
            TagLibProps.Add("Title", TagLibFile.Tag.Title ?? "Unknown");
            TagLibProps.Add("Track", TagLibFile.Tag.Track);
            TagLibProps.Add("Year", TagLibFile.Tag.Year);
            TagLibProps.Add("Rating", GetRating(TagLibFile));
            TagLibProps.Add("IsCover", IsCover(TagLibFile));
            TagLibProps.Add("IsLive", IsLive(TagLibFile));
            TagLibProps.Add("Duration", TagLibFile.Properties.Duration);
        }

        private byte GetRating(TagLib.File file)
        {
            try
            {
                var tag = file.GetTag(TagLib.TagTypes.Id3v2);
                var frame = TagLib.Id3v2.PopularimeterFrame.Get((TagLib.Id3v2.Tag)tag, "WindowsUser", true);
                return frame.Rating;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private bool IsCover(TagLib.File file)
        {
            var isCover = false;
            var originalArtist = "";
            var tagType = file.TagTypes.ToString();
            
            if (tagType.Contains("Id3v2"))
            {
                if (file is TagLib.Matroska.File)
                {
                    return isCover;
                }
            var id3v2Tag = file.GetTag(TagLib.TagTypes.Id3v2);
            var frame = TagLib.Id3v2.TextInformationFrame.Get((TagLib.Id3v2.Tag)id3v2Tag, "TOPE", true);
            originalArtist = frame.ToString() ?? "";
            }
            else if (tagType.Contains("FlacMetadata"))
            {
                originalArtist = ((TagLib.Flac.Metadata)file.GetTag(TagLib.TagTypes.FlacMetadata)).Comment;
            }
            else if (tagType.Contains("Apple"))
            {
                originalArtist = ((TagLib.Mpeg4.AppleTag)file.GetTag(TagLib.TagTypes.Apple)).Comment;
            }
            else if (tagType.Contains("Ape"))
            {
                originalArtist = ((TagLib.Ape.Tag)file.GetTag(TagLib.TagTypes.Ape)).Comment;
            }
            else if (tagType.Contains("Xiph"))
            {
                originalArtist = ((TagLib.Ogg.XiphComment)file.GetTag(TagLib.TagTypes.Xiph)).Comment;
            }
            else
            {
                var log = new LogWriter($"Could not find TagLib 'cover/original' artist metadata for {this.Filepath}. " +
                $" TagType: {file.TagTypes}.");
                return isCover;
            }
            if (TagLibProps.TryGetValue("Artist", out object value))
            {
                if (!String.IsNullOrEmpty(originalArtist) && originalArtist != value.ToString())
                    isCover = true;
            }
            return isCover;
        }

        private bool IsLive(TagLib.File file)
        {
            var isLive = false;
            try
            {
                var comment = TagLibFile.Tag.Comment ?? "";
                if (String.IsNullOrEmpty(comment))
                    return isLive;
                Regex textWithLive = new Regex(@"\blive\b");
                if (textWithLive.IsMatch(comment.ToLower()))
                    isLive = true;
                return isLive;
            }
            catch (Exception ex)
            {
                var log = new LogWriter($"Could not find TagLib 'Comment' metadata for {this.ToString()}. \"{ex.Message}\"");
                return isLive;
            }
        }

        private void PopulateSysIOFields()
        {
            SysIOProps.Add("Filepath", SysIOFile.FullName);
            SysIOProps.Add("Name", SysIOFile.Name);
            SysIOProps.Add("Directory", SysIOFile.DirectoryName);
            SysIOProps.Add("Extension", SysIOFile.Extension);
            SysIOProps.Add("CreationTime", Convert.ToDateTime(SysIOFile.CreationTime));
            SysIOProps.Add("LastAccessTime", Convert.ToDateTime(SysIOFile.LastAccessTime));
            SysIOProps.Add("Size", SysIOFile.Length);
        }

        public bool Exists()
        {
            return File.Exists(this.Filepath);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((MasterFile)obj);
        }

        public bool Equals(MasterFile obj)
        {
            if (obj.TagLibProps["Artist"] != this.TagLibProps["Artist"] || obj.TagLibProps["Album"] != this.TagLibProps["Album"]
                || obj.TagLibProps["Size"] != this.TagLibProps["Size"] || obj.TagLibProps["Track"] != this.TagLibProps["Track"]
                || obj.TagLibProps["BitRate"] != this.TagLibProps["BitRate"] || obj.TagLibProps["IsLive"] != this.TagLibProps["IsLive"]
                || obj.TagLibProps["IsCover"] != this.TagLibProps["IsCover"] || obj.TagLibProps["Duration"] != this.TagLibProps["Duration"])
                return false;
            else return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            if (SysIOProps.TryGetValue("Name", out object name))
                return name.ToString();
            else return "";
        }
    }
}
