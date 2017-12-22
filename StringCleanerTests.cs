using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataOrganizer;

namespace MasterFileTests
{
    [TestClass]
    public class StringCleanerTests
    {
        [TestMethod]
        public void Sc_ToActualTitleCase_Test_AllLowercaseInput()
        {
            var dirtyInput = "i want this to be correct title case.";
            var cleanOutput = "I Want This to Be Correct Title Case.";
            Assert.AreEqual(StringCleaner.ToActualTitleCase(dirtyInput), cleanOutput);
        }

        [TestMethod]
        public void Sc_ToActualTitleCase_Test_FirstLetterOfEveryWordCapitalizedInInput()
        {
            var dirtyInput = "This Is Another Test For My Regex Stuff";
            var cleanOutput = "This Is Another Test for My Regex Stuff";
            Assert.AreEqual(StringCleaner.ToActualTitleCase(dirtyInput), cleanOutput);
        }

        [TestMethod]
        public void Sc_ToActualTitleCase_Test_EmptyStringInput()
        {
            var emptyInput = "";
            Assert.AreEqual(StringCleaner.ToActualTitleCase(emptyInput), "");
        }

        //string destinationPath = @"C:\_TempForTesting\We Don't Need to Whisper\" + newFileName;
        //if (invalidChar == ':')
        //{
        //    if (!File.Exists(destinationPath.Replace(':', '-')))
        //        Assert.Fail();
        //}
        //else
        //{
        //    if (!File.Exists(destinationPath.Replace(invalidChar.ToString(), string.Empty)))
        //        Assert.Fail();
        //}
        //Directory.Delete(testDir, true);

        [TestMethod]
        public void Sc_RemoveInvalidDirectoryCharacters_Test_Condition()
        {

        }

        [TestMethod]
        public void Sc_RemoveInvalidFileNameCharacters_Test_Condition()
        {

        }
        // Make test methods for the new RemoveInvalidChars methods
    }
}
