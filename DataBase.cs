using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataOrganizer
{
    public class DataBase
    {
        string ConnectionString = "";

        public DataBase(string connStr)
        {
            ConnectionString = connStr;
        }

        private void ExecuteSqlCommands(SqlCommand[] sqlCommands)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    foreach (var cmd in sqlCommands)
                    {
                        using (cmd)
                        {
                            cmd.Connection = connection;
                            cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    var log = new LogWriter($"Could not execute SQL command. \"{ex.Message}\"");
                }
            }
        }

        public bool Contains(MasterFile file)
        {
            var entryExists = false;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.TagLibFields WHERE (Filepath = @path)", connection))
                {
                    cmd.Parameters.AddWithValue("@path", file.Filepath);
                    if ((int)cmd.ExecuteScalar() > 0)
                        entryExists = true;
                }
            }
            return entryExists;
        }

        public void DeleteAllRecords()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand("SELECT [TABLE_NAME] FROM INFORMATION_SCHEMA.TABLES", connection))
                {
                    var tableNames = new List<string>();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        tableNames.Add(reader[0].ToString());
                    }
                    reader.Close();

                    foreach (var table in tableNames)
                    {
                        cmd.CommandText = $"DELETE FROM {table};";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void InsertSelectUpdateDeleteRecord(MasterFile file, StatementType statementType)
        {
            if (statementType == StatementType.Insert && this.Contains(file))
                return;

            var sysIOcmd = new SqlCommand("dbo.usp_SysIO_MasterInsertUpdateDelete");
            sysIOcmd.CommandType = CommandType.StoredProcedure;
            sysIOcmd.Parameters.Add("@Filepath", SqlDbType.NVarChar).Value = file.Filepath;
            sysIOcmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = file.SysIOProps["Name"].ToString();
            sysIOcmd.Parameters.Add("@Directory", SqlDbType.NVarChar).Value = file.SysIOProps["Directory"].ToString();
            sysIOcmd.Parameters.Add("@Extension", SqlDbType.NVarChar).Value = file.SysIOProps["Extension"].ToString();
            sysIOcmd.Parameters.Add("@CreationTime", SqlDbType.DateTime).Value = Convert.ToDateTime(file.SysIOProps["CreationTime"]);
            sysIOcmd.Parameters.Add("@LastAccessTime", SqlDbType.DateTime).Value = Convert.ToDateTime(file.SysIOProps["LastAccessTime"]);
            sysIOcmd.Parameters.Add("@Length", SqlDbType.BigInt).Value = Convert.ToInt64(file.SysIOProps["Length"]);
            sysIOcmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = statementType.ToString();

            var tagLibcmd = new SqlCommand("dbo.usp_TagLib_MasterInsertUpdateDelete");
            tagLibcmd.CommandType = CommandType.StoredProcedure;
            tagLibcmd.Parameters.Add("@Filepath", SqlDbType.NVarChar).Value = file.Filepath;
            tagLibcmd.Parameters.Add("@BitRate", SqlDbType.Int).Value = Convert.ToInt32(file.TagLibProps["BitRate"]);
            tagLibcmd.Parameters.Add("@MediaType", SqlDbType.NVarChar).Value = file.TagLibProps["MediaType"].ToString();
            tagLibcmd.Parameters.Add("@Artist", SqlDbType.NVarChar).Value = file.TagLibProps["Artist"].ToString();
            tagLibcmd.Parameters.Add("@Album", SqlDbType.NVarChar).Value = file.TagLibProps["Album"].ToString();
            tagLibcmd.Parameters.Add("@Genres", SqlDbType.NVarChar).Value = file.TagLibProps["Genres"].ToString();
            tagLibcmd.Parameters.Add("@Lyrics", SqlDbType.NVarChar).Value = file.TagLibProps["Lyrics"].ToString();
            tagLibcmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = file.TagLibProps["Title"].ToString();
            tagLibcmd.Parameters.Add("@Track", SqlDbType.Int).Value = Convert.ToInt32(file.TagLibProps["Track"]);
            tagLibcmd.Parameters.Add("@Year", SqlDbType.Int).Value = Convert.ToInt32(file.TagLibProps["Year"]);
            tagLibcmd.Parameters.Add("@Rating", SqlDbType.TinyInt).Value = Convert.ToByte(file.TagLibProps["Rating"]);
            tagLibcmd.Parameters.Add("@IsCover", SqlDbType.Bit).Value = (bool)file.TagLibProps["IsCover"] == true ? 1 : 0;
            tagLibcmd.Parameters.Add("@IsLive", SqlDbType.Bit).Value = (bool)file.TagLibProps["IsLive"] == true ? 1 : 0;
            tagLibcmd.Parameters.Add("@StatementType", SqlDbType.NVarChar).Value = statementType.ToString();

            // Have to delete data in SysIO table first because of primary/foreign key relationship to TagLib table
            if (statementType == StatementType.Delete)
                ExecuteSqlCommands(new SqlCommand[] { sysIOcmd, tagLibcmd });
            else
                ExecuteSqlCommands(new SqlCommand[] { tagLibcmd, sysIOcmd });
        }
    }

    public enum StatementType
    {
        Insert,
        Select,
        Update,
        Delete
    }
}
