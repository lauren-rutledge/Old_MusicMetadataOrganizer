using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using TagLib;
using TagLib.Id3v2;

namespace MusicMetadataOrganizer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            /*
            var files = FileSearcher.ExtractFiles();
            foreach (var file in files)
            {
                //var db = new Database();
                //db.InsertUpdateDeleteRecord(file, StatementType.Update);

                var response = GracenoteWebAPI.Query(file);
                var results = response.CheckMetadataEquality(file);
                // Do this part in the mf.update method
                var matches = results.Where(pair => pair.Value == false)
                  .Select(pair => pair.Key);
                if (matches.Count() > 0)
                {
                    Console.WriteLine(file + " has new or different data. Updating...");
                    file.Update(response, matches);
                    file.Save();
                }
                else
                {
                    Console.WriteLine(file + " has no new or different data. Not updating.");
                }
            }
            Console.WriteLine("Complete");
            */

            // Test MasterFiles
            //var mfFromFP = MasterFile.GetMasterFileFromFilepath(@"C:\Users\Ashie\Desktop\The Adventure.mp3");
            //var mfFromFp2 = MasterFile.GetMasterFileFromFilepath(@"C:\Users\Ashie\Desktop\Going Away to College.mp3");
            //var mfFromDB = MasterFile.GetMasterFileFromDB(db.QueryRecord(mf.Filepath));

            // Delete all files from Database and reinsert them
            var db = new Database();
            db.DeleteAllRecords();
            foreach (var file in FileSearcher.ExtractFiles())
            {
                db.InsertUpdateDeleteRecord(file, StatementType.Insert);
                //var mf = db.GetMasterFile(db, file.Filepath);
                //if (mf == null)
                //{
                //    db.InsertUpdateDeleteRecord(MasterFile.GetMasterFileFromFilepath(file.Filepath), StatementType.Insert);
                //    var log = new LogWriter($"Could not create a MasterFile object for {file.Filepath}. " +
                //        $"This file has been added to the database.");
                //    continue;
                //}
                //db.InsertUpdateDeleteRecord(file, StatementType.Update);
                Debug.WriteLine(file.ToString());
            }
        }
    }
}
