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
        //SqlConnection myConnection;

        public DataBase(string connStr)
        {
            this.ConnectionString = connStr;
        }

        /*
        // The first, hardcoded method
        public void InsertData()
        {
            // Pass in variables instead of hardcoding
            string sql = "INSERT INTO dbo.SystemIOFields " +
                "(Filepath, Name, Directory, Extension, CreationTime, LastAccessTime, Length) " +
                "VALUES (@Filepath,@Name,@Directory,@Extension,@CreationTime,@LastAccessTime,@Length);";

            using (SqlCommand cmd = new SqlCommand(sql, myConnection))
            {
                cmd.Parameters.AddWithValue("@Filepath", "filepath");
                cmd.Parameters.AddWithValue("@Name", "test name");
                cmd.Parameters.AddWithValue("@Directory", "testdirectory");
                cmd.Parameters.AddWithValue("@Extension", ".mp3");
                cmd.Parameters.AddWithValue("@CreationTime", "20171016");
                cmd.Parameters.AddWithValue("@LastAccessTime", "20171016");
                cmd.Parameters.AddWithValue("@Length", 16);

                try
                {
                    myConnection = new SqlConnection(ConnectionString);
                    myConnection.Open();
                    cmd.Connection = myConnection;
                    int result = cmd.ExecuteNonQuery();
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database");
                }
                catch (SqlException ex) 
                {
                    Console.WriteLine("Could not write to database. - " + ex.Message);
                }
            }
        }
        */

            /*
        public void InsertData(MasterFile file)
        {

            string sqlStatement = $"INSERT INTO {table} (";
            string sqlColumns = "";
            string sqlValues = "";

            for (int i = 0; i < columns.Length; i++)
            {
                if (!IsValidInput(columns[i]) || !IsValidInput(values[i]))
                {
                    throw new ArgumentException("The data to insert in the database does not pass validation. " +
                        $"{columns[i]} --- {values[i]} failed validation.");
                }

                if (i == columns.Length - 1)
                {
                    sqlColumns += $"{columns[i]}";
                    sqlValues += $"{values[i]}";
                }

                else
                {
                    sqlColumns += $"{columns[i]}, ";
                    sqlValues += $"{values[i]},";
                }
            }

            // Pass in variables instead of hardcoding
            string sql = $"INSERT INTO {table} " +
                $"({sqlColumns}) VALUES ({sqlValues});";

            using (SqlCommand cmd = new SqlCommand(sql, myConnection))
            {
                
                cmd.Parameters.AddWithValue("@Filepath", "filepath");
                cmd.Parameters.AddWithValue("@Name", "test name");
                cmd.Parameters.AddWithValue("@Directory", "testdirectory");
                cmd.Parameters.AddWithValue("@Extension", ".mp3");
                cmd.Parameters.AddWithValue("@CreationTime", "20171016");
                cmd.Parameters.AddWithValue("@LastAccessTime", "20171016");
                cmd.Parameters.AddWithValue("@Length", 16);
                

                try
                {
                    myConnection = new SqlConnection(ConnectionString);
                    myConnection.Open();
                    cmd.Connection = myConnection;
                    int result = cmd.ExecuteNonQuery();
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not write to database. - " + ex.Message);
                }
            }
        }
*/
        public void InsertData(MasterFile file)
        {
            foreach (var item in file.TagLibProps)
            {

            }
            foreach (var item in file.SysIOProps)
            {

            }
            //IsMatchingType(file, "SystemIOFields", "Extension");
            ConstructSqlCommand();
            // ExecuteSqlCommand();
        }

        private bool IsValidInput(string sqlInput)
        {
            string textPattern = "(''|[^'])*";
            string semiColonPattern = ";";
            string sqlStatementPattern = "\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE){0,1}" +
                "|INSERT( +INTO){0,1}|MERGE|SELECT|UPDATE|UNION( +ALL){0,1})\b";
            Regex textBlocks = new Regex(textPattern);
            Regex statementBreaks = new Regex(semiColonPattern);
            Regex sqlStatements = new Regex(sqlStatementPattern, RegexOptions.IgnoreCase);

            // Fix this so it's not the only one opposite
            if (!textBlocks.IsMatch(sqlInput))
                return false;
            if (statementBreaks.IsMatch(sqlInput))
            {
                return false;
            }
            if (sqlStatements.IsMatch(sqlInput))
            {
                return false;
            }
            else return true;
        }

        /*
        private bool IsMatchingType(MasterFile file, string table, object property)
        {
            if (!IsValidInput(table) || !IsValidInput(property.ToString()))
                throw new ArgumentException("Cannot check for matching types. Input parameters failed validation.");
            

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT 1 FROM " + table +  Table = @table " WHERE Prop = @prop", conn))
                {
                    //cmd.Parameters.AddWithValue("@table", table);
                    cmd.Parameters.AddWithValue("@prop", property.ToString());
                    conn.Open();
                    var found = (int)cmd.ExecuteScalar();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                        }
                    }
                }
            }
            // Check types against what the db expects
            return false;
        }
        */

        private string ConstructSqlCommand()
        {
            return "";
            // Construct the sql query
        }

        private void ExecuteSqlCommand(string sqlCommand)
        {
            // Execute the sql command
        }
    }
}
