using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MusicMetadataOrganizer
{
    public class MasterFile
    {
        public string Filepath { get; internal set; }
        public Dictionary<string, object> TagLibProps;
        public Dictionary<string, object> SysIOProps;
        public bool CheckForUpdates = true;

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

        public static MasterFile GetMasterFileFromFilepath(string filepath)
        {
            if (String.IsNullOrEmpty(filepath))
            {
                var message = "MasterFile.GetMasterFileFromFilepath() - Cannot create a MasterFile object from a null or empty filepath.";
                var log = new LogWriter(message);
                throw new ArgumentNullException(message);
            }
            else if (!File.Exists(filepath))
            {
                var message = $"MasterFile.GetMasterFileFromFilepath() - Cannot create a MasterFile object from a filepath " +
                    $"that does not exist. Filepath given - {filepath}.";
                var log = new LogWriter(message);
                throw new ArgumentException(message);
            }
            return new MasterFile(filepath);
        }

        public static MasterFile GetMasterFileFromDB(Dictionary<string, object>[] properties)
        {
            if (properties == null || properties.Length == 0)
            {
                var log = new LogWriter("MasterFile.GetMasterFileFromDB() - Cannot create a MasterFile object from null or empty properties.");
                throw new ArgumentNullException();
            }

            foreach (var collection in properties)
            {
                if (collection == null || collection.Count() == 0)
                {
                    var message = "MasterFile.GetMasterFileFromDB() - Cannot create a MasterFile object from a null or empty collection " +
                        "of properties -- One or more of the tables from the database returned no values. Check to see if the file record " +
                        "exists in the database.";
                    var log = new LogWriter(message);
                    throw new ArgumentNullException(message);
                }
            }
            return new MasterFile(properties);
        }

        private void CreateTagLibFile()
        {
            try
            {
                TagLibFile = TagLib.File.Create(Filepath);
            }
            catch (Exception ex)
            {
                var log = new LogWriter($"MasterFile.CreateTagLibFile() - Could not create a TagLibFile object from '{Filepath}'. " +
                    $"{ex.GetType()}: \"{ex.Message}\"");
                return;
                // Maybe should move the file into an 'error folder' 
            }
        }

        private void CreateSysIOFile()
        {
            SysIOFile = new FileInfo(Filepath);
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
            // Consider turning this into a loop
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

        // Messy - refactor
        private bool IsCover(TagLib.File file)
        {
            var isCover = false;
            var originalArtist = "";
            var tagType = file.TagTypes.ToString();

            if (tagType.Contains("Id3v2"))
            {
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
                var log = new LogWriter($"MasterFile.IsCover() - Could not find TagLib 'cover/original' artist metadata for " +
                    $"'{this.Filepath}'. TagType: {file.TagTypes}.");
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
                var log = new LogWriter($"MasterFile.IsLive() - Could not find TagLib 'Comment' metadata for '{this.ToString()}'. " +
                    $"{ex.GetType()}: \"{ex.Message}\"");
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

        public void Update(GracenoteSong songFromAPI, IEnumerable<string> properties)
        {
            foreach (var property in properties)
            {
                switch (property)
                {
                    case "Artist":
                        TagLibProps["Artist"] = songFromAPI.Artist;
                        break;
                    case "Album":
                        TagLibProps["Album"] = songFromAPI.Album;
                        break;
                    case "Title":
                        TagLibProps["Title"] = songFromAPI.Title;
                        break;
                    case "Track":
                        TagLibProps["Track"] = songFromAPI.Track;
                        break;
                    case "Year":
                        TagLibProps["Year"] = songFromAPI.Year;
                        break;
                    case "Genres":
                        TagLibProps["Genres"] = songFromAPI.Genres;
                        break;
                    default:
                        break;
                }
            }
        }

        public void Save()
        {
            SaveMetadata();
            SaveFileData();
            SaveToDatabase();
        }

        // The commented out areas are functional, but are never actually updated/changed via api or
        // other so they are currently pointless to update
        private void SaveMetadata()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            TagLibFile.Tag.Artists = new string[] { TagLibProps["Artist"].ToString() };
#pragma warning restore CS0618 // Type or member is obsolete
            TagLibFile.Tag.AlbumArtists = new string[] { TagLibProps["Artist"].ToString() };
            TagLibFile.Tag.Performers = new string[] { TagLibProps["Artist"].ToString() };
            TagLibFile.Tag.Album = TagLibProps["Album"].ToString();
            TagLibFile.Tag.Genres = new string[] { TagLibProps["Genres"].ToString() };
            //TagLibFile.Tag.Lyrics = TagLibProps["Lyrics"].ToString();
            TagLibFile.Tag.Title = TagLibProps["Title"].ToString();
            TagLibFile.Tag.Track = Convert.ToUInt32(TagLibProps["Track"]);
            TagLibFile.Tag.Year = Convert.ToUInt32(TagLibProps["Year"]);
            //try
            //{
            //    var tag = TagLibFile.GetTag(TagLib.TagTypes.Id3v2);
            //    var frame = TagLib.Id3v2.PopularimeterFrame.Get((TagLib.Id3v2.Tag)tag, "WindowsUser", true);
            //    frame.Rating = Convert.ToByte(TagLibProps["Rating"]);
            //}
            //catch (Exception ex)
            //{
            //    var log = new LogWriter($"Can not save rating metadata to {Filepath}. {ex.GetType()}: \"{ex.Message}\" " + 
            //      "Possibly invalid tag type (Not Id3v2/Windows)");
            //}
            //try
            //{
            //    TagLibFile.Tag.Comment = (bool)TagLibProps["IsLive"] ? "Live" : "";
            //}
            //catch (Exception ex)
            //{
            //    var log = new LogWriter($"Can not save database comment data to {Filepath}. {ex.GetType()}: \"{ex.Message}\"");
            //}
            try
            {
                TagLibFile.Save();
            }
            catch (IOException ex)
            {
                var log = new LogWriter($"MasterFile.SaveMetadata() - Can not save taglib data to '{Filepath}'. {ex.GetType()}: \"{ex.Message}\"");
            }
        }

        private void SaveFileData()
        {
            MoveToCorrectArtistLocation();
            MoveToCorrectAlbumLocation();
            RenameFile();
        }

        private void MoveToCorrectArtistLocation()
        {
            Regex directoryArtistRegex = new Regex(@"([^\\]+)\\([^\\]+)\\([^\\]+)$");
            string currentArtistFolderName = directoryArtistRegex.Match(Filepath).Groups[1].ToString();
            string validArtistFolderName = StringCleaner.RemoveInvalidDirectoryChars(TagLibProps["Artist"].ToString());
            if (currentArtistFolderName != validArtistFolderName)
            {
                string currentDirectory = SysIOProps["Directory"].ToString();
                string newDirectory = currentDirectory.Replace(currentArtistFolderName, validArtistFolderName);
                FileManipulator.RenameFolder(this, currentDirectory, newDirectory);
                FileManipulator.DeleteEmptyFolders(new DirectoryInfo(currentDirectory));
            }
        }

        private void MoveToCorrectAlbumLocation()
        {
            Regex directoryAlbumRegex = new Regex(@"([^\\]+)\\([^\\]+)$");
            var currentAlbumFolderName = directoryAlbumRegex.Match(Filepath).Groups[1].ToString();
            var validAlbumFolderName = StringCleaner.RemoveInvalidDirectoryChars(TagLibProps["Album"].ToString());
            if (currentAlbumFolderName != validAlbumFolderName)
            {
                var currentDirectory = SysIOProps["Directory"].ToString();
                var newDirectory = currentDirectory.Replace(currentAlbumFolderName, validAlbumFolderName);
                FileManipulator.RenameFolder(this, currentDirectory, newDirectory);
                FileManipulator.DeleteEmptyFolders(new DirectoryInfo(currentDirectory));
            }
        }

        private void RenameFile()
        {
            var currentFileName = SysIOProps["Name"].ToString();
            var newFileName = TagLibProps["Title"].ToString() + SysIOProps["Extension"].ToString();
            var validFileName = StringCleaner.RemoveInvalidFileNameCharacters(newFileName);
            if (currentFileName != validFileName)
            {
                FileManipulator.RenameFile(Filepath, validFileName);
                SysIOProps["Name"] = validFileName;
            }
            Filepath = Path.Combine(SysIOProps["Directory"].ToString(), SysIOProps["Name"].ToString());
        }

        private void SaveToDatabase()
        {
            var db = new Database();
            if (db.Contains(this))
                db.InsertUpdateDeleteRecord(this, StatementType.Update);
            else
                db.InsertUpdateDeleteRecord(this, StatementType.Insert);
        }

        public bool Exists()
        {
            return File.Exists(this.Filepath);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((MasterFile)obj);
        }

        private bool Equals(MasterFile mf)
        {
            if (mf.TagLibProps["Artist"].ToString() != TagLibProps["Artist"].ToString() ||
                mf.TagLibProps["Album"].ToString() != TagLibProps["Album"].ToString() ||
                Convert.ToUInt32(mf.TagLibProps["Track"]) != Convert.ToUInt32(TagLibProps["Track"]) ||
                Convert.ToInt32(mf.TagLibProps["BitRate"]) != Convert.ToInt32(TagLibProps["BitRate"]) ||
                mf.TagLibProps["IsLive"].ToString() != TagLibProps["IsLive"].ToString() ||
                mf.TagLibProps["IsCover"].ToString() != TagLibProps["IsCover"].ToString() ||
                (TimeSpan)mf.TagLibProps["Duration"] != (TimeSpan)TagLibProps["Duration"])
                return false;
            else return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{TagLibProps["Artist"].ToString()} - {TagLibProps["Title"].ToString()}";
        }
    }
}
