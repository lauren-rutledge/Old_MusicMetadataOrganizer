using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataOrganizer
{
    public static class FileWriter
    {
        public static void UpdateFile(MasterFile mf)
        {
            TagLib.File tglb = TagLib.File.Create(mf.Filepath);
            tglb.Tag.Artists = new string[] { mf.TagLibProps["Artist"].ToString() };
            tglb.Tag.AlbumArtists = new string[] { mf.TagLibProps["Artist"].ToString() };
            tglb.Tag.Performers = new string[] { mf.TagLibProps["Artist"].ToString() };
            tglb.Tag.Album = mf.TagLibProps["Album"].ToString();
            tglb.Tag.Genres = new string[] { mf.TagLibProps["Genres"].ToString() };
            tglb.Tag.Lyrics = mf.TagLibProps["Lyrics"].ToString();
            tglb.Tag.Title = mf.TagLibProps["Title"].ToString();
            tglb.Tag.Track = Convert.ToUInt32(mf.TagLibProps["Track"]);
            tglb.Tag.Year = Convert.ToUInt32(mf.TagLibProps["Year"]);
            try
            {
                var tag = tglb.GetTag(TagLib.TagTypes.Id3v2);
                var frame = TagLib.Id3v2.PopularimeterFrame.Get((TagLib.Id3v2.Tag)tag, "WindowsUser", true);
                frame.Rating = Convert.ToByte(mf.TagLibProps["Rating"]);
            }
            catch (Exception)
            {
            }

            // tglb original artist (cover) -- Might have to check with some API if isCover is true to retrieve original artist
            tglb.Tag.Comment = (bool)mf.TagLibProps["IsLive"] ? "Live" : "" ;
            try
            {
                tglb.Save();
            }
            catch (IOException ex)
            {
                var log = new LogWriter($"Can not save database data to {mf.Filepath}. \"{ex.Message}\"");
            }
            catch (Exception ex)
            {
                var log = new LogWriter($"Can not save database data to {mf.Filepath}. \"{ex.Message}\"");
            }

            var fileInfo = new FileInfo(mf.Filepath);
            var nameInDB = mf.SysIOProps["Name"].ToString();
            if (fileInfo.Name != nameInDB)
                Rename(fileInfo, nameInDB);
        }

        private static void Rename(this FileInfo fileInfo, string newName)
        {
            fileInfo.MoveTo(Path.Combine(fileInfo.Directory.FullName, newName));
        }
    }
}
