using System;
using System.Data;
using System.Linq;

namespace MusicMetadataOrganizer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Deletes all DB records and takes selected files, queries them against the API, updates any changes, 
            // and saves the new data to the file and database
            var db = new Database();
            db.DeleteAllRecords();

            var files = FileSearcher.ExtractFiles();
            foreach (var file in files)
            {
                if ((bool)file.TagLibProps["IsCover"])
                    continue;
                // if (file.CheckForUpdates == false)
                //    continue;
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

            // Delete all files from Database and reinserts them
            /*
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
            */
        }
    }
}
