using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program : DBRead, DBWrite
    {
        static void Main(string[] args)
        {
            String connectionString = "server=ENGI-PC\\SQLEXPRESS;" +
                                       "Trusted_Connection=yes;" +
                                       "database=LogBD; " +
                                       "connection timeout=30";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand selectAll = new SqlCommand("SELECT * FROM LogTable", connection);

            connection.Open();

            LogTable log = new LogTable(LogCentralizor.Program.Go());
            List<String> commands = TableToCommandString(log);
            foreach(var command in commands)
            {
                Console.WriteLine(command);
            }
            
            try
            {
                foreach (var command in commands)
                {
                    SqlCommand insertRow = new SqlCommand(command,connection);
                    insertRow.ExecuteNonQuery();
                }

                SqlDataReader reader = selectAll.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader[0]);
                }
                reader.Close();
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                connection.Close();
            }
            connection.Close();
        }

        

        public LogTable Read()
        {
            return new LogTable(LogCentralizor.Program.Go());
        }

        public void Write(LogTable log)
        {
            throw new NotImplementedException();
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
