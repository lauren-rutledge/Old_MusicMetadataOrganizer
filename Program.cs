using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
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
            MasterFile mf = new MasterFile(@"C:\Users\Ashie\Desktop\The Adventure.mp3");
            DataBase db = new DataBase(@"Data Source=Ashie-PC\SQLExpress;" +
                                        "Initial Catalog=MusicMetadata;" +
                                        "Integrated Security=True");
            db.InsertData(mf);           
            
            //var file1 = new MasterFile(@"C:\Users\Ashie\Desktop\The Adventure.mp3");
            //var file2 = new MasterFile(@"C:\Users\Ashie\Desktop\Going Away to College.mp3");

            //Console.WriteLine(file1.Equals(file2));


            //TagLib.CombinedTag tag = new CombinedTag(file1.TagLibFile.Tag);

            /*
            Type myType = file1.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            var test = file1.TagLibFile.Tag;

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

            var spinner = new ConsoleSpinner();

            var searcher = new FileSearcher();
            searcher.SelectDirectory();

            spinner.Start();
            searcher.ExtractFiles(searcher.Directory);
            spinner.Stop();

            foreach (var file in searcher.files)
            {
                Console.WriteLine(file.ToString());
            }
            */
        }
    }
}
