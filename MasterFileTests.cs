using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataOrganizer;

namespace MasterFileTests
{
    [TestClass]
    public class MasterFileTests
    {
        Dictionary<string, object>[] testPropertiesFromDatabase = new Dictionary<string, object>[]
        {
            new Dictionary<string, object>()
            {
                { "Filepath", @"C:\ExampleFilepathReturnedFromTheDatabase.Extension" },
                { "BitRate", 180 },
                { "MediaType", "Audio" },
                { "Artist", "Band Name" },
                { "Album", "Album Name" },
                { "Genres", "Genre Name" },
                { "Lyrics", "These are example lyrics" },
                { "Title", "Song Name" },
                { "Track", 1 },
                { "Year", DateTime.Now.Year },
                { "Rating", 0 },
                { "IsCover", false },
                { "IsLive", false },
                { "Duration", 1800000000 }
            },

            new Dictionary<string, object>()
            {
                { "Filepath", @"C:\ExampleFilepathReturnedFromTheDatabase.Extension" },
                { "Name", "Title.Extension" },
                { "Directory", @"C:\ExampleFilepathReturnedFromTheDatabase" },
                { "Extension", ".Extension" },
                { "CreationTime", DateTime.Now },
                { "LastAccessTime", DateTime.Now },
                { "Size", 4000000 }
            }
        };
        string validFilepath1 = @"C:\Users\Ashie\Desktop\Going Away to College.mp3";
        string validFilepath2 = @"C:\Users\Ashie\Desktop\The Adventure.mp3";
        string invalidFilepath = @"C:\Users\Ashie\This Is Not a Valid Filepath\Song.mp3";

        [TestMethod]
        public void Mf_GetMasterFileFromFilepath_Test_ValidInput()
        {
            var masterFile = MasterFile.GetMasterFileFromFilepath(validFilepath1);
            Assert.IsInstanceOfType(masterFile, typeof(MasterFile));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Mf_GetMasterFileFromFilepath_Test_InvalidFP()
        {
            var invalidMasterFile = MasterFile.GetMasterFileFromFilepath(invalidFilepath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Mf_GetMasterFileFromFilepath_Test_NullOrEmptyInput()
        {
            var path = "";
            var invalidMasterFile = MasterFile.GetMasterFileFromFilepath(path);
        }

        [TestMethod]
        public void Mf_GetMasterFileFromFilepath_Test_PopulatesPropertyDictionaries()
        {
            var masterFile = MasterFile.GetMasterFileFromFilepath(validFilepath1);
            if (masterFile.TagLibProps == null || masterFile.TagLibProps.Count == 0)
                Assert.Fail();
            if (masterFile.SysIOProps == null || masterFile.SysIOProps.Count == 0)
                Assert.Fail();
        }

        [TestMethod]
        public void Mf_GetMasterFileFromDatabase_Test_ValidInput()
        {
            var masterFile = MasterFile.GetMasterFileFromDB(testPropertiesFromDatabase);
            Assert.IsInstanceOfType(masterFile, typeof(MasterFile));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Mf_GetMasterFileFromDatabase_Test_NullOrEmptyInput()
        {
            var emptyProperties = new Dictionary<string, object>[] { };
            var invalidMasterFile = MasterFile.GetMasterFileFromDB(emptyProperties);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Mf_GetMasterFileFromDatabase_Test_PartiallyNullOrEmptyInput()
        {
            var emptyProperties = new Dictionary<string, object>[]
            {
                new Dictionary<string, object>(),
                new Dictionary<string, object>()
            };
            var invalidMasterFile = MasterFile.GetMasterFileFromDB(emptyProperties);
        }

        [TestMethod]
        public void Mf_GetMasterFileFromDatabase_Test_PopulatesPropertyDictionaries()
        {
            var masterFile = MasterFile.GetMasterFileFromDB(testPropertiesFromDatabase);
            if (masterFile.TagLibProps == null || masterFile.TagLibProps.Count == 0)
                Assert.Fail();
            if (masterFile.SysIOProps == null || masterFile.SysIOProps.Count == 0)
                Assert.Fail();
        }

        [TestMethod]
        public void Mf_Equals_Test_IdenticalMasterFiles()
        {
            var masterFile1 = MasterFile.GetMasterFileFromFilepath(validFilepath1);
            var masterFile2 = MasterFile.GetMasterFileFromFilepath(validFilepath1);
            Assert.AreEqual(masterFile1, masterFile2);
        }

        [TestMethod]
        public void Mf_Equals_Test_DifferentMasterFiles()
        {
            var masterFile1 = MasterFile.GetMasterFileFromFilepath(validFilepath1);
            var masterFile2 = MasterFile.GetMasterFileFromFilepath(validFilepath2);
            Assert.AreNotEqual(masterFile1, masterFile2);
        }
    }
}
