using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TagLib;
using TagLib.Id3v2;

namespace MusicMetadataOrganizer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //MasterFile mf = new MasterFile(@"C:\Users\Ashie\Desktop\The Adventure.mp3");
            //DataBase db = new DataBase(@"Data Source=Ashie-PC\SQLExpress;" +
            //                            "Initial Catalog=MusicMetadata;" +
            //                            "Integrated Security=True");

            //var masterFile = new MasterFile(db.QueryRecord(mf.Filepath));
            //db.InsertSelectUpdateDeleteRecord(mf, StatementType.Insert);

            //var file1 = new MasterFile(@"C:\Users\Ashie\Desktop\The Adventure.mp3");
            //var file2 = new MasterFile(@"C:\Users\Ashie\Desktop\Going Away to College.mp3");

            /*
            string genre = "Hip-Hop, Rock"; 
            var matchingFiles = Directory.GetFiles
                (@"Folder\SubFolder", "*.mp3", SearchOption.AllDirectories)
                .Where(x => 
                {
                    var f = TagLib.File.Create(x);
                    return ((TagLib.Id3v2.Tag)f
                    .GetTag(TagTypes.Id3v2))
                    .JoinedGenres == genre;
                });
            foreach (string f in matchingFiles)
            {
                System.IO.File.Move(f, Path.Combine(@"D:\NewFolder", new FileInfo(f).Name));
            }
            */
          
            var spinner = new ConsoleSpinner();
            var searcher = new FileSearcher();
            searcher.SelectDirectory();
            spinner.Start();
            searcher.ExtractFiles(searcher.Directory);
            spinner.Stop();

            DataBase db = new DataBase(@"Data Source=Ashie-PC\SQLExpress;" +
                                        "Initial Catalog=MusicMetadata;" +
                                        "Integrated Security=True");
            db.DeleteAllRecords();
            //var dbFiles = new List<MasterFile>();
            foreach (var file in searcher.files)
            {
                db.InsertUpdateDeleteRecord(file, StatementType.Insert);
                var mf = db.GetMasterFile(db, file.Filepath);
                if (mf == null)
                {
                    db.InsertUpdateDeleteRecord(MasterFile.GetMasterFileFromFilepath(file.Filepath), StatementType.Insert);
                    var log = new LogWriter($"Could not create a MasterFile object for {file.Filepath}. " +
                        $"This file has been added to the database.");
                    continue;
                }
                FileWriter.UpdateFile(mf);

                //dbFiles.Add(FileWriter.GetMasterFile(db, file.Filepath));
                //db.InsertUpdateDeleteRecord(file, StatementType.Update);
                Debug.WriteLine(file.ToString());
            }
        }
    }
}
