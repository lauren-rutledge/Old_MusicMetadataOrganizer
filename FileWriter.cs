using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace MusicMetadataOrganizer
{
    public static class FileWriter
    {
        public static MasterFile GetMasterFile(DataBase db, string filepath)
        {
            return MasterFile.GetMasterFileFromDB(db.QueryRecord(filepath));
        }

        // Need to flush this out and figure out what properties will need to be updated. 
        // Props like directory would have to be updated by moving the file (sep method if I do that)
        public static void UpdateFile(MasterFile mf)
        {
            var path = mf.Filepath;
            var file = ShellFile.FromFilePath(path);
            ShellPropertyWriter writer = file.Properties.GetPropertyWriter();
            try
            {
                //writer.WriteProperty(SystemProperties.System.FileName, mf.SysIOProps["Name"]);

                //file.Properties.System.FileExtension.Value = mf.SysIOProps["Extension"].ToString();
                //File.SetCreationTime(path, Convert.ToDateTime(mf.SysIOProps["CreationTime"]));
                //File.SetLastAccessTime(path, Convert.ToDateTime(mf.SysIOProps["LastAccessTime"]));
                // Length May need to convert from ms?
                //var test = Convert.ToInt64(file.Properties.System.Media.Duration.Value);
                var shellFileDuration = TimeSpan.FromTicks(Convert.ToInt64(file.Properties.System.Media.Duration.Value));
                var test = Convert.ToInt64(file.Properties.System.Size.ValueAsObject);
                // file.Properties.System.Media.Duration
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            writer.Close();
        }

        // Maybe make this public
        private static void Rename(this FileInfo fileInfo, string newName)
        {
            fileInfo.MoveTo(Path.Combine(fileInfo.Directory.FullName, newName));
        }
    }
}
