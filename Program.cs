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
            //var mf = MasterFile.GetMasterFileFromFilepath(@"C:\Users\Ashie\Desktop\The Adventure.mp3");
            //var db = new DataBase();

            //var masterFile = MasterFile.GetMasterFileFromDB(db.QueryRecord(mf.Filepath));
            //db.InsertSelectUpdateDeleteRecord(mf, StatementType.Insert);

            //var file1 = MasterFile.GetMasterFileFromFilepath(@"C:\Users\Ashie\Desktop\Going Away to College.mp3");

            //Testing the Gracenote API and converting the xml to an object
            //var text = Xml.CreateRequest("flying lotus", "until the quiet comes", "all in");
            //var response = GracenoteWebAPI.Query(mf);
            //if (response.Equals(mf))
            //    Console.WriteLine("Equal");

            var text = XmlGenerator.CreateRequest("Angels and Airwaves", "The Adventure");//, "We Don't Need to Whisper");
            var result1 = GracenoteWebAPI.PostXmlData(text);
            var gnFileList = XmlParser.XmlToObject(result1);
            Console.WriteLine(gnFileList[0].ALBUM.ARTIST);


            //DataBase db = new DataBase(@"Data Source=Ashie-PC\SQLExpress;" +
            //                            "Initial Catalog=MusicMetadata;" +
            //                            "Integrated Security=True");
            //db.DeleteAllRecords();
            //foreach (var file in searcher.files)
            //{
            //    db.InsertUpdateDeleteRecord(file, StatementType.Insert);
            //    var mf = db.GetMasterFile(db, file.Filepath);
            //    if (mf == null)
            //    {
            //        db.InsertUpdateDeleteRecord(MasterFile.GetMasterFileFromFilepath(file.Filepath), StatementType.Insert);
            //        var log = new LogWriter($"Could not create a MasterFile object for {file.Filepath}. " +
            //            $"This file has been added to the database.");
            //        continue;
            //    }
            //    FileWriter.UpdateFile(mf);

            //    //dbFiles.Add(FileWriter.GetMasterFile(db, file.Filepath));
            //    //db.InsertUpdateDeleteRecord(file, StatementType.Update);
            //    Debug.WriteLine(file.ToString());
            //}
        }
    }
}
