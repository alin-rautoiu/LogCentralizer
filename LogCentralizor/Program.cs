using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.CodeDom.Compiler;
using System.CodeDom;

namespace LogReader
{
    public class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Process.Start("http://localhost:11182/WebForm1.aspx");
        }

        public static List<Row> GetRowsFromDocument()
        {
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
            String logPath = getPath(@"C:\Conf\conf.txt");
            String[] filesPaths = Directory.GetFiles(logPath, "*.csv");
            foreach (var file in filesPaths)
            {
                String log = "";
                try
                {
                    log = File.ReadAllText(file);
                }
                catch (Exception e)
                {
                    using (StreamWriter w = File.AppendText(@"C:\Log\log.txt"))
                    {
                        w.WriteLine("At: " + DateTime.Today.ToString("dd/mm/yy hh:mm:ss"));
                        w.WriteLine("Message: " + e.Message);
                        w.WriteLine("Source: " + e.Source);
                        w.WriteLine("Stack Trace: " + e.StackTrace);
                        w.WriteLine();
                    }
                }
                Char[] separators = { '\n', ',', '"', '\r' };

                List<String> logItems = log.Split(separators).ToList<String>();

                logItems.RemoveAll(item => item.CompareTo("") == 0);

                logItems.RemoveRange(0, 11);

                try
                {
                    File.Move(file,
                                logPath + 
                                @"\AlreadyProccesed\" +
                                file.Split('\\').Last());
                }
                catch (Exception e)
                {
                    using(StreamWriter w = File.AppendText(@"C:\Log\log.txt")){
                        w.WriteLine("At: " + DateTime.Today.ToString("dd/mm/yy hh:mm:ss"));
                        w.WriteLine("Message: " + e.Message);
                        w.WriteLine("Source: " + e.Source);
                        w.WriteLine("Stack Trace: " + e.StackTrace);
                        w.WriteLine();
                    }
                }

                rows.AddRange(CreateRows(logItems));
            }



            return rows;
        }

        public static String getPath(String filePath)
        {
            String text = File.ReadAllText(filePath);
            String line = text.Split(';')[0];
            line = line.Replace(" = ", "=");
            line = line.Replace(@"\", @"\\");
            String path = line.Split('=')[1];
            return path;
        }

        static List<Row> CreateRows(List<String> logItems)
        {
            List<Row> rows = new List<Row>();
            Row row;
            for (int i = 0; i < logItems.Count; i += 11)
            {
                row = new Row(logItems.GetRange(i, 11));
                rows.Add(row);
            }
            return rows;
        }

    }
}
