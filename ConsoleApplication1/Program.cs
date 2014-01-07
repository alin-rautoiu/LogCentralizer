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
            SqlDataReader reader = selectAll.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader["LogTime"]);
            }

        }
        

        public LogTable Read()
        {
            throw new NotImplementedException();
        }

        public void Write(LogTable log)
        {
            throw new NotImplementedException();
        }
    }
}
