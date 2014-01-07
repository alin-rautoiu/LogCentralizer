using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection
{
    public class Program : DBRead, DBWrite
    {
        static void Main(string[] args)
        {
            
        }

        public void EmptyDB()
        {
            String connectionString = "server=ENGI-PC\\SQLEXPRESS;" +
                                      "Trusted_Connection=yes;" +
                                      "database=LogBD; " +
                                      "connection timeout=30";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand deleteAll = new SqlCommand("DELETE FROM LogTable", connection);
            deleteAll.ExecuteNonQuery();


            connection.Close();
        }

        public LogTable Read()
        {
            String connectionString = "server=ENGI-PC\\SQLEXPRESS;" +
                                      "Trusted_Connection=yes;" +
                                      "database=LogBD; " +
                                      "connection timeout=30";

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand selectAll = new SqlCommand("SELECT * FROM LogTable", connection);
            SqlDataReader reader = selectAll.ExecuteReader();

            List<Row> rows = new List<Row>();
            List<String> row = new List<string>();
            
            row.Add("LogTime");
            row.Add("Action");
            row.Add("FolderPath");
            row.Add("Filename");
            row.Add("Username");
            row.Add("IPADDRESS");
            row.Add("XferSize");
            row.Add("Duration");
            row.Add("AgentBrand");
            row.Add("AgentVersion");
            row.Add("Error");
            
            rows.Add(new Row(row));

            while (reader.Read())
            {
                row = new List<string>();
                for (int i = 0; i < 11; i++)
                {
                    row.Add((string)reader[i]);
                }
                rows.Add(new Row(row));
                row.Clear();
            }
            reader.Close();
            connection.Close();

            return new LogTable(rows);
        }

        public void Write(LogTable log)
        {
            String connectionString = "server=ENGI-PC\\SQLEXPRESS;" +
                                       "Trusted_Connection=yes;" +
                                       "database=LogBD; " +
                                       "connection timeout=30";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand selectAll = new SqlCommand("SELECT * FROM LogTable", connection);

            connection.Open();

            List<String> commands = TableToCommandString(log);

            try
            {
                foreach (var command in commands)
                {
                    SqlCommand insertRow = new SqlCommand(command, connection);
                    insertRow.ExecuteNonQuery();
                }

            }

            catch (Exception)
            {
                throw;
            }

            connection.Close();
        }

        public static List<String> TableToCommandString(LogTable log)
        {
            List<String> commands = new List<string>();

            foreach (var row in log.rows)
            {
                String command = "EXEC InsertWithValidation ";
                foreach (var cell in row)
                {
                    command = command + "'" + cell + "',";
                }
                command = command.Remove(command.Length-1);
                commands.Add(command);
            }

            return commands;
        }
    }
}
