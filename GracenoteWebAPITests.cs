using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataOrganizer;

namespace MasterFileTests
{
    [TestClass]
    public class GracenoteWebAPITests
    {
        MasterFile file = MasterFile.GetMasterFileFromFilepath
            (@"D:\My Documents\Visual Studio 2017\Projects\MusicOrganizer\Test Files\Classic Cars.mp3");

        [TestMethod]
        public void Gn_Query_Test_ReturnsValidResponse()
        {
            var songFromAPI = GracenoteWebAPI.Query(file);
            if (String.IsNullOrEmpty(songFromAPI.Artist))
                Assert.Fail();
            if (String.IsNullOrEmpty(songFromAPI.Album))
                Assert.Fail();
            if (String.IsNullOrEmpty(songFromAPI.Title))
                Assert.Fail();
        }

        [TestMethod]
        public void RESPONSE_CheckMetadataEquality_Test_Equal()
        {
            /*
            var response = GracenoteWebAPI.Query(file);
            response.ALBUM.ARTIST = file.TagLibProps["Artist"].ToString();
            response.ALBUM.TITLE = file.TagLibProps["Album"].ToString();
            response.ALBUM.TRACK.TITLE = file.TagLibProps["Title"].ToString();
            response.ALBUM.TRACK.TRACK_NUM = Convert.ToInt32(file.TagLibProps["Track"]);
            response.ALBUM.DATE = file.TagLibProps["Year"].ToString();
            response.ALBUM.GENRE = file.TagLibProps["Genres"].ToString();

            var results = response.CheckMetadataEquality(file);
            var matches = results.Where(pair => pair.Value == false)
                  .Select(pair => pair.Key);
            if (matches.Count() > 0)
                Assert.Fail();
            */
        }

        [TestMethod]
        public void RESPONSE_CheckMetadataEquality_Test_OneFieldNotEqual()
        {
            /*
            var response = GracenoteWebAPI.Query(file);
            Random random = new Random();
            switch (random.Next(0, 5))
            {
                case 0:
                    response.ALBUM.ARTIST = "Fake Artist";
                    break;
                case 1:
                    response.ALBUM.TITLE = "Fake Album";
                    break;
                case 2:
                    response.ALBUM.TRACK.TITLE = "Fake Song Title";
                    break;
                case 3:
                    response.ALBUM.TRACK.TRACK_NUM = 0;
                    break;
                case 4:
                    response.ALBUM.DATE = "2080";
                    break;
                case 5:
                    response.ALBUM.GENRE = "Fake genre";
                    break;
            }
            var results = response.CheckMetadataEquality(file);
            var matches = results.Where(pair => pair.Value == false)
                  .Select(pair => pair.Key);
            if (matches.Count() <= 0)
                Assert.Fail();
            */
        }

        [TestMethod]
        public void RESPONSE_CheckMetadataEquality_Test_MultipleFieldsNotEqual()
        {
            /*
            var response = GracenoteWebAPI.Query(file);
            response.ALBUM.ARTIST = "Fake Artist";
            response.ALBUM.TITLE = "Fake Album";
            response.ALBUM.TRACK.TITLE = "Fake Song Title";
            response.ALBUM.TRACK.TRACK_NUM = 0;
            response.ALBUM.DATE = "2080";
            response.ALBUM.GENRE = "Fake genre";
            var results = response.CheckMetadataEquality(file);
            var matches = results.Where(pair => pair.Value == false)
                  .Select(pair => pair.Key);
            if (matches.Count() <= 0)
                Assert.Fail();
            */
        }
    }
}
