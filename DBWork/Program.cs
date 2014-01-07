using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBWork
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection sqlConnection = new SqlConnection("user id=user;" +
                                      "password=1234%asd;" + " Server=localhost\\SQLExpress;" + "Trusted_Connection=yes;"
                                               + "Database=LogDB;");
            SqlCommand selectAll = new SqlCommand("SELECT * FROM Name", sqlConnection);
            sqlConnection.Open();
            SqlDataReader dataReader = null;
            dataReader = selectAll.ExecuteReader();
            while (dataReader.Read())
            {
                Console.WriteLine(dataReader["Id"].ToString());
                Console.WriteLine(dataReader["Name"].ToString());
            }
            Console.Read();
        }
    }
}
