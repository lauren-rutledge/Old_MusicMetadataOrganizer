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

        public MasterFile(string filepath)
        {
            this.Filepath = filepath;
            CreateTagLibFile();
            CreateSysIOFile();

            TagLibProps = new Dictionary<string, object>();
            SysIOProps = new Dictionary<string, object>();
            PopulateFields();
        }

        // TO-DO: Create another constructor that takes db data as params

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
                // Maybe should move the file into an 'error folder' 
            }
        }

        private void CreateSysIOFile()
        {
            try
            {
                SysIOFile = new FileInfo(Filepath);
            }
            // Write to log on catch
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
            try
            {
                var tag = file.GetTag(TagLib.TagTypes.Id3v2);
                var frame = TagLib.Id3v2.TextInformationFrame.Get((TagLib.Id3v2.Tag)tag, "TOPE", true);
                var originalArtist = frame.ToString();

                if (TagLibProps.TryGetValue("Artist", out object value))
                {
                    if (!String.IsNullOrEmpty(originalArtist) && originalArtist != value.ToString())
                        isCover = true;
                }
                return isCover;
            }
            catch (Exception ex)
            {
                var log = new LogWriter($"Could not find TagLib 'cover/original' artist metadata for {this.ToString()}. \"{ex.Message}\"");
                return isCover;
            }
        }

        private bool IsLive(TagLib.File file)
        {
            var isLive = false;
            try
            {
                var comment = TagLibFile.Tag.Comment;
                //var tag = TagLibFile.GetTag(TagLib.TagTypes.Id3v2);
                //var frame = TagLib.Id3v2.CommentsFrame.GetPreferred((TagLib.Id3v2.Tag)tag, "COMM", "XXX");

                Regex textWithLive = new Regex(@"\blive\b");
                if (textWithLive.IsMatch(comment.ToLower()))
                //if (textWithLive.IsMatch(frame.ToString().ToLower()))
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
            SysIOProps.Add("Length", SysIOFile.Length);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((MasterFile)obj);
        }

        //public bool Equals(MasterFile obj)
        //{
        //    if (obj.Artist != this.Artist || obj.Album != this.Album || obj.Length != this.Length
        //        || obj.Track != this.Track || obj.BitRate != this.BitRate || obj.IsLive != this.IsLive
        //        || obj.IsCover != this.IsCover)
        //        return false;
        //    else return true;
        //}

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
