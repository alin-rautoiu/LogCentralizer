﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection
{
    public class Program : DBRead, DBWrite
    {
        static void Main(string[] args)
        {
            
        }

        public static String getConnectionString(String filePath)
        {
            String text = File.ReadAllText(filePath);
            String conn = text.Split(';')[1];
            conn = conn.Split(':')[1];
            List<String> items = conn.Split(',').ToList<String>();
            String connString = "server=" + items[0].Split('=')[1]
                                + ";Trusted_Connection=" + items[1].Split('=')[1]
                                + ";database=" + items[2].Split('=')[1]
                                + ";connection timeout=" + items[3].Split('=')[1];
            return connString;
        }

        public void EmptyDB()
        {
            String connectionString = getConnectionString(@"C:\Conf\conf.txt");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand deleteAll = new SqlCommand("DELETE FROM LogTable", connection);
            deleteAll.ExecuteNonQuery();


            connection.Close();
        }

        public LogTable Read()
        {
            String connectionString = getConnectionString(@"C:\Conf\conf.txt");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand selectAll = new SqlCommand("SELECT * FROM LogTable ORDER BY LogTime", connection);
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
            String connectionString = getConnectionString(@"C:\Conf\conf.txt");

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand selectAll = new SqlCommand("SELECT * FROM LogTable ORDER BY LogTime", connection);

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

            catch (Exception e)
            {
                using (StreamWriter w = File.AppendText(@"C:\Log\log.txt"))
                {
                    w.WriteLine("At: "+ DateTime.Today.ToString("dd/mm/yy hh:mm:ss"));
                    w.WriteLine("Message: " + e.Message);
                    w.WriteLine("Source: " + e.Source);
                    w.WriteLine("Stack Trace: " + e.StackTrace);
                    w.WriteLine();
                }
            }

            connection.Close();
        }

        public LogTable Select(SqlCommand command)
        {
            String connectionString = getConnectionString(@"C:\Conf\conf.txt");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            command.Connection = connection;
            SqlDataReader reader = command.ExecuteReader();

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

        public LogTable Filter(String startDate, String endDate)
        {
            SqlCommand selectFilter;
            selectFilter = new SqlCommand("SELECT * FROM LogTable WHERE [LogTime] >= '" + startDate +
                                                    "' AND [LogTime] <= '" + endDate +
                                                    "' ORDER By [LogTime]");

            return Select(selectFilter);
        }

        public LogTable checkIp(LogTable log, String ipString, String maskString)
        {
            byte[] maskBytes = new Byte[4];
            IPAddress ip = IPAddress.Parse(ipString);
            int mask = Int32.Parse(maskString);

            for (int i = 0; i < 4; i++)
            {
                if (mask / 8 >= 1)
                {
                    maskBytes[i] = 255;
                }
                else
                {
                    maskBytes[i] = (byte)Math.Pow(2, 8 - mask % 8);
                }
                mask -= 8;
            }

            byte[] ipBytes = ip.GetAddressBytes();
            byte[] network = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                network[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }

            LogTable newLog = new LogTable();
            newLog.header.AddRange(log.header);

            foreach (var row1 in log.rows)
            {
                IPAddress oldip = IPAddress.Parse(row1.ElementAt(5));
                byte[] oldIpBytes = oldip.GetAddressBytes();
                byte[] oldNetwork = new byte[4];
                bool contains = true;

                for (int i = 0; i < 4; i++)
                {
                    oldNetwork[i] = (byte)(oldIpBytes[i] & maskBytes[i]);
                    if (network[i] != oldNetwork[i])
                    {
                        contains = false;
                        continue;
                    }
                }
                if (contains)
                    newLog.rows.Add(row1);
            }
            return newLog;
        }
        public LogTable Filter(String startDate, String endDate, String ipString, String maskString)
        {
            SqlCommand selectFilter;
            selectFilter = new SqlCommand("SELECT * FROM LogTable WHERE [LogTime] >= '" + startDate +
                                                        "' AND [LogTime] <= '" + endDate +

                                                        "' ORDER By [LogTime]");

            LogTable log = Select(selectFilter);

            return checkIp(log, ipString, maskString);
        }

        public LogTable Filter(String startDate, String endDate, String ip)
        {
            SqlCommand selectFilter;
            selectFilter = new SqlCommand("SELECT * FROM LogTable WHERE [LogTime] >= '" + startDate +
                                                        "' AND [LogTime] <= '" + endDate +
                                                        "' AND [IPADDRESS] = '" + ip +
                                                        "' ORDER By [LogTime]");
           
            return Select(selectFilter);
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
