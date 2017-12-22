using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataOrganizer;

namespace MasterFileTests
{
    [TestClass]
    public class FileManipulatorTests
    {
        string testDir = @"C:\_TempForTesting\We Don't Need to Whisper\";
        string exceedsMaxCharPath = @"C:\_TempForTesting\QWERTYUIOPASDFGHJKLZXCVBNMQWERTYUIOPASDFGHJKLZXCVBNM" +
                                    @"QWERTYUIOPASDFGHJKLZXCVBNMQWERTYUIOPASDFGHJKLZXCVBNMQWERTYUIOPASDFJKL" +
                                    @"ZXCVBNMQWERTYUIOPASDFGHJKLZXCVBNMQWERTYUIOPASDFGHJKLZXCVBNMQWERTYUIOP" +
                                    @"ASDFGHJKLZXCVBNMQWERTYUIOPASDFGHJKLZXCVBNMQWERTYUIOPASDFGHJKLZXCVBNM\";
        MasterFile file = MasterFile.GetMasterFileFromFilepath(@"C:\_TempForTesting\We Don't Need to Whisper\The Adventure.mp3");

        private void CopyFileToTestDir()
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
            Directory.CreateDirectory(testDir);
            string sourcePath = Path.Combine(testDir, "The Adventure.mp3");
            File.Copy(@"C:\Users\Ashie\Desktop\The Adventure.mp3", sourcePath);
        }

        [TestMethod]
        public void Fm_RenameDirectory_Test_OtherFilesExistInSourceDirectory()
        {
            CopyFileToTestDir();
            File.Copy(@"C:\Users\Ashie\Desktop\Going Away to College.mp3",
                      @"C:\_TempForTesting\We Don't Need to Whisper\Going Away to College.mp3");
            string destinationDirectory = @"C:\_TempForTesting\_Testing\";
            FileManipulator.RenameDirectory(file, testDir, destinationDirectory);

            if (File.Exists(@"C:\_TempForTesting\We Don't Need to Whisper\The Adventure.mp3"))
                Assert.Fail();
            if (!File.Exists(@"C:\_TempForTesting\_Testing\The Adventure.mp3"))
                Assert.Fail();

            Directory.Delete(testDir, true);
            Directory.Delete(destinationDirectory, true);
        }

        [TestMethod]
        public void Fm_RenameDirectory_Test_ValidInputSingleRename()
        {
            CopyFileToTestDir();
            string destDir = @"C:\_TempForTesting\_temp\";
            FileManipulator.RenameDirectory(file, testDir, destDir);
            if (Directory.Exists(testDir))
            {
                Assert.Fail();
                Directory.Delete(testDir);
            }
            Directory.Delete(destDir, true);
        }

        [TestMethod]
        public void Fm_RenameDirectory_Test_RenameOnlyCaseDifference()
        {
            CopyFileToTestDir();
            string destDir = @"C:\_TempForTesting\WE DON'T NEED TO WHISPER\";
            FileManipulator.RenameDirectory(file, testDir, destDir);
            string directoryName = new DirectoryInfo(destDir).FullName;
            if (testDir.Equals(directoryName))
            {
                Assert.Fail();
                Directory.Delete(testDir);
            }
            Directory.Delete(destDir, true);
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void Fm_RenameDirectory_Test_InputSourceDirectoryDoesNotExist()
        {
            CopyFileToTestDir();
            string invalidSourceDir = @"C:\Invalid\This Directory Doesn't Exist\";
            FileManipulator.RenameDirectory(file, invalidSourceDir, testDir);
            Directory.Delete(testDir);
        }

        // The method this calls currently throws an unhandled exception. Not sure if I want to handle it later.
        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void Fm_RenameDirectory_Test_InputExceedsMaxPathCharLimit()
        {
            CopyFileToTestDir();
            FileManipulator.RenameDirectory(file, testDir, exceedsMaxCharPath);
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void Fm_RenameDirectory_Test_DestDirectoryOnDifferentDrive()
        {
            CopyFileToTestDir();
            string invalidDestDir = @"D:\Documents\_TempForTesting\";
            FileManipulator.RenameDirectory(file, testDir, invalidDestDir);
        }

        [TestMethod]
        public void Fm_RenameDirectory_Test_InvalidCharactersInInputCaughtException()
        {
            CopyFileToTestDir();
            char[] invalidPathChars = Path.GetInvalidPathChars();
            Random random = new Random();
            char invalidChar = invalidPathChars[random.Next(0, invalidPathChars.Length - 1)];
            FileManipulator.RenameDirectory(file, testDir, testDir.Replace('D', invalidChar));
        }

        [TestMethod]
        public void Fm_RenameFile_Test_ValidInputSingleRename()
        {
            CopyFileToTestDir();
            FileInfo originalFile = new FileInfo(@"C:\_TempForTesting\We Don't Need to Whisper\The Adventure.mp3");
            string newFilepath = @"C:\_TempForTesting\We Don't Need to Whisper\Test.mp3";
            FileManipulator.RenameFile(originalFile.FullName, "Test.mp3");
            if (!File.Exists(newFilepath))
            {
                Assert.Fail();
            }
            if (File.Exists(originalFile.FullName))
            {
                Assert.Fail();
            }
            Directory.Delete(originalFile.Directory.ToString(), true);
        }
        
        [TestMethod]
        public void Fm_RenameFile_Test_RenameOnlyCaseDifference()
        {
            CopyFileToTestDir();
            FileInfo originalFile = new FileInfo(@"C:\_TempForTesting\We Don't Need to Whisper\The Adventure.mp3");
            string newFilepath = @"C:\_TempForTesting\We Don't Need to Whisper\THE ADVENTURE.mp3";
            FileManipulator.RenameFile(originalFile.FullName, "THE ADVENTURE.mp3");
            if (originalFile.Name.Equals(new FileInfo(newFilepath).Name))
            { 
                Assert.Fail();
            }
            Directory.Delete(originalFile.Directory.ToString(), true);
        }
        
        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void Fm_RenameFile_Test_InputSourceFileDoesNotExist()
        {
            string invalidSourceFile = @"C:\_TempForTesting\Not a File.mp3";
            FileManipulator.RenameFile(invalidSourceFile, "NewName.mp3");
        }
        
        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void Fm_RenameFile_Test_InputExceedsMaxPathCharLimit()
        {
            string filepath = Path.Combine(exceedsMaxCharPath, "OriginalName.mp3");
            FileManipulator.RenameFile(filepath, "NewName.mp3");
        }

        [TestMethod]
        public void Fm_RenameFile_Test_InvalidCharactersInInputCaughtException()
        {
            CopyFileToTestDir();
            string validFilepath = @"C:\_TempForTesting\We Don't Need to Whisper\The Adventure.mp3";
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            Random random = new Random();
            char invalidChar = invalidFileNameChars[random.Next(0, invalidFileNameChars.Length - 1)];
            string newFileName = "The Adventure.mp3".Replace('u', invalidChar);

            FileManipulator.RenameFile(validFilepath, newFileName);
        }
    }
}
