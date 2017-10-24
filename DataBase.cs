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
            this.ConnectionString = connStr;
        }

        public void InsertData(MasterFile file)
        {
            var cmds = ConstructSqlCommands(file);
            ExecuteSqlCommands(cmds);
        }

        // Not currently using
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
            var tagLibSql = "INSERT INTO dbo.TagLibFields " +
                        "(BitRate, MediaType, Artist, Album, Genres, Lyrics, Title, Track, Year, Rating, IsCover, IsLive) " +
                        "VALUES (@BitRate,@MediaType,@Artist,@Album,@Genres,@Lyrics,@Title,@Track,@Year,@Rating,@IsCover,@IsLive);";

            SqlCommand cmd = new SqlCommand(tagLibSql);

            cmd.Parameters.AddWithValue("@BitRate", Convert.ToInt32(tagLibProps["BitRate"]));
            cmd.Parameters.AddWithValue("@MediaType", tagLibProps["MediaType"].ToString());
            cmd.Parameters.AddWithValue("@Artist", tagLibProps["Artist"].ToString());
            cmd.Parameters.AddWithValue("@Album", tagLibProps["Album"].ToString());
            cmd.Parameters.AddWithValue("@Genres", tagLibProps["Genres"].ToString());
            cmd.Parameters.AddWithValue("@Lyrics", tagLibProps["Lyrics"].ToString());
            cmd.Parameters.AddWithValue("@Title", tagLibProps["Title"].ToString());
            // May not need this null check... If null, value is assigned '0' when I checked
            try
            {
                cmd.Parameters.AddWithValue("@Track", Convert.ToInt32(tagLibProps["Track"]));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                cmd.Parameters.AddWithValue("@Track", null);
            }
            cmd.Parameters.AddWithValue("@Year", Convert.ToInt32(tagLibProps["Year"]));
            cmd.Parameters.AddWithValue("@Rating", (byte)tagLibProps["Rating"]);
            if ((bool)tagLibProps["IsCover"] == true)
                cmd.Parameters.AddWithValue("@IsCover", 1);
            else
                cmd.Parameters.AddWithValue("@IsCover", 0);
            if ((bool)tagLibProps["IsLive"] == true)
                cmd.Parameters.AddWithValue("@IsLive", 1);
            else
                cmd.Parameters.AddWithValue("@IsLive", 0);
            return cmd;
        }

        private static SqlCommand ConstructSqlCmdSysIO(Dictionary<string, object> sysIOProps)
        {
            var sysIOSql = "INSERT INTO dbo.SystemIOFields " +
                             "(Filepath, Name, Directory, Extension, CreationTime, LastAccessTime, Length) " +
                             "VALUES (@Filepath,@Name,@Directory,@Extension,@CreationTime,@LastAccessTime,@Length);";
            SqlCommand cmd = new SqlCommand(sysIOSql);

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
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
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
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
