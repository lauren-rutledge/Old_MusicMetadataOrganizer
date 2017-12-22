using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicMetadataOrganizer;

namespace MasterFileTests
{
    [TestClass]
    public class DatabaseTests
    {
        Database db = new Database();
        MasterFile masterFile1 = MasterFile.GetMasterFileFromFilepath(@"C:\Users\Ashie\Desktop\Going Away to College.mp3");
        MasterFile masterFile2 = MasterFile.GetMasterFileFromFilepath(@"C:\Users\Ashie\Desktop\The Adventure.mp3");


        [TestMethod]
        public void Db_GetMasterFile_Test_FilepathRecordExists()
        { 
            if (!db.Contains(masterFile1))
            {
                db.InsertUpdateDeleteRecord(masterFile1, StatementType.Insert);
            }
            db.GetMasterFile(masterFile1.Filepath);
            Assert.IsInstanceOfType(masterFile1, typeof(MasterFile));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Db_GetMasterFile_Test_FilepathRecordDoesNotExist()
        {
            if (db.Contains(masterFile1))
            {
                db.InsertUpdateDeleteRecord(masterFile1, StatementType.Delete);
            }
            db.GetMasterFile(masterFile1.Filepath);
        }

        [TestMethod]
        public void Db_Contains_Test_DoesContain()
        {
            db.InsertUpdateDeleteRecord(masterFile1, StatementType.Insert);
            Assert.IsTrue(db.Contains(masterFile1));
            db.InsertUpdateDeleteRecord(masterFile1, StatementType.Delete);
        }

        [TestMethod]
        public void Db_Contains_Test_DoesNotContain()
        {
            db.InsertUpdateDeleteRecord(masterFile1, StatementType.Delete);
            Assert.IsFalse(db.Contains(masterFile1));
        }

        [TestMethod]
        public void Db_DeleteAllRecords_Test_RecordsSuccessfullyDeleted()
        {
            db.InsertUpdateDeleteRecord(masterFile1, StatementType.Insert);
            db.InsertUpdateDeleteRecord(masterFile2, StatementType.Insert);
            db.DeleteAllRecords();
            if (db.Contains(masterFile1) || db.Contains(masterFile2))
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Db_InsertUpdateDeleteRecord_Test_DatabaseContainsRecordAfterInsert()
        {
            db.InsertUpdateDeleteRecord(masterFile1, StatementType.Delete);
            db.InsertUpdateDeleteRecord(masterFile1, StatementType.Insert);
            if (!db.Contains(masterFile1))
            {
                Assert.Fail();
            }
            db.InsertUpdateDeleteRecord(masterFile1, StatementType.Delete);
        }

        [TestMethod]
        public void Db_InsertUpdateDeleteRecord_Test_DatabaseRecordUpdatedAfterUpdate()
        {
            db.InsertUpdateDeleteRecord(masterFile1, StatementType.Delete);
            db.InsertUpdateDeleteRecord(masterFile1, StatementType.Insert);
            masterFile1.TagLibProps["Artist"] = "Unit Test Sample Artist";
            db.InsertUpdateDeleteRecord(masterFile1, StatementType.Update);
            var masterFileResult = MasterFile.GetMasterFileFromDB(db.QueryRecord(masterFile1.Filepath));
            if (masterFileResult.TagLibProps["Artist"].ToString() != masterFile1.TagLibProps["Artist"].ToString())
            {
                Assert.Fail();
            }
            db.InsertUpdateDeleteRecord(masterFile1, StatementType.Delete);
        }

        [TestMethod]
        public void Db_InsertUpdateDeleteRecord_Test_DatabaseDoesNotContainRecordAfterDelete()
        {
            db.InsertUpdateDeleteRecord(masterFile1, StatementType.Insert);
            db.InsertUpdateDeleteRecord(masterFile1, StatementType.Delete);
            if (db.Contains(masterFile1))
            {
                Assert.Fail();
            }
        }
    }
}
