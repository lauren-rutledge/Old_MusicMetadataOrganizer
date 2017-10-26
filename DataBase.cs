using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public void InsertData(MasterFile file)
        {
            if (this.Contains(file))
                return;
            var cmds = ConstructSqlCommands(file);
            ExecuteSqlCommands(cmds);
        }

        private bool IsValidInput(string sqlInput)
        {
            var isValidInput = true;
            string textPattern = "(''|[^'])*";
            string semiColonPattern = ";";
            string sqlStatementPattern = "\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE){0,1}" +
                "|INSERT( +INTO){0,1}|MERGE|SELECT|UPDATE|UNION( +ALL){0,1})\b";
            Regex textBlocks = new Regex(textPattern);
            Regex statementBreaks = new Regex(semiColonPattern);
            Regex sqlStatements = new Regex(sqlStatementPattern, RegexOptions.IgnoreCase);

            // Fix this so it's not the only one opposite
            // This is separated so that when error handling is added, specific exceptions can be thrown
            if (!textBlocks.IsMatch(sqlInput))
                isValidInput = false;
            if (statementBreaks.IsMatch(sqlInput))
                isValidInput = false;
            if (sqlStatements.IsMatch(sqlInput))
                isValidInput = false;
            return isValidInput;
        }

        private SqlCommand[] ConstructSqlCommands(MasterFile file)
        {
            SqlCommand cmd1 = ConstructSqlCmdTagLib(file.TagLibProps);
            SqlCommand cmd2 = ConstructSqlCmdSysIO(file.SysIOProps);
            return new SqlCommand[] { cmd1, cmd2 };
        }

        private static SqlCommand ConstructSqlCmdTagLib(Dictionary<string, object> tagLibProps)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO dbo.TagLibFields " +
                        "(Filepath, BitRate, MediaType, Artist, Album, Genres, Lyrics, Title, Track, Year, Rating, IsCover, IsLive) " +
                        "VALUES (@Filepath,@BitRate,@MediaType,@Artist,@Album,@Genres,@Lyrics,@Title,@Track,@Year,@Rating,@IsCover,@IsLive);");

            cmd.Parameters.AddWithValue("@Filepath", tagLibProps["Filepath"]);
            cmd.Parameters.AddWithValue("@BitRate", Convert.ToInt32(tagLibProps["BitRate"]));
            cmd.Parameters.AddWithValue("@MediaType", tagLibProps["MediaType"].ToString());
            cmd.Parameters.AddWithValue("@Artist", tagLibProps["Artist"].ToString());
            cmd.Parameters.AddWithValue("@Album", tagLibProps["Album"].ToString());
            cmd.Parameters.AddWithValue("@Genres", tagLibProps["Genres"].ToString());
            cmd.Parameters.AddWithValue("@Lyrics", tagLibProps["Lyrics"].ToString());
            cmd.Parameters.AddWithValue("@Title", tagLibProps["Title"].ToString());
            cmd.Parameters.AddWithValue("@Track", Convert.ToInt32(tagLibProps["Track"]));
            cmd.Parameters.AddWithValue("@Year", Convert.ToInt32(tagLibProps["Year"]));
            cmd.Parameters.AddWithValue("@Rating", (byte)tagLibProps["Rating"]);
            cmd.Parameters.AddWithValue("@IsCover", (bool)tagLibProps["IsCover"] == true ? 1 : 0);
            cmd.Parameters.AddWithValue("@IsLive", (bool)tagLibProps["IsLive"] == true ? 1 : 0);
            return cmd;
        }

        private static SqlCommand ConstructSqlCmdSysIO(Dictionary<string, object> sysIOProps)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO dbo.SystemIOFields " +
                             "(Filepath, Name, Directory, Extension, CreationTime, LastAccessTime, Length) " +
                             "VALUES (@Filepath,@Name,@Directory,@Extension,@CreationTime,@LastAccessTime,@Length);");

            cmd.Parameters.AddWithValue("@Filepath", sysIOProps["Filepath"].ToString());
            cmd.Parameters.AddWithValue("@Name", sysIOProps["Name"].ToString());
            cmd.Parameters.AddWithValue("@Directory", sysIOProps["Directory"].ToString());
            cmd.Parameters.AddWithValue("@Extension", sysIOProps["Extension"].ToString());
            cmd.Parameters.AddWithValue("@CreationTime", Convert.ToDateTime(sysIOProps["CreationTime"]));
            cmd.Parameters.AddWithValue("@LastAccessTime", Convert.ToDateTime(sysIOProps["LastAccessTime"]));
            cmd.Parameters.AddWithValue("@Length", (long)sysIOProps["Length"]);
            return cmd;
        }

        private void ExecuteSqlCommands(SqlCommand[] sqlCommands)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
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
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public bool Contains(MasterFile file)
        {
            bool entryExists = false;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.TagLibFields WHERE (Filepath = @path)", connection))
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
                using (SqlCommand cmd = new SqlCommand("SELECT [TABLE_NAME] FROM INFORMATION_SCHEMA.TABLES", connection))
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
    }
}
