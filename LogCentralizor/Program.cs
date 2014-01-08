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
            
        }

        public static List<Row> GetRowsFromDocument()
        {
            String logPath = getPath("C:\\Users\\Alin\\Documents\\GitHub\\LogCentralizer\\LogCentralizor\\bin\\Debug\\conf.txt");
            String log = File.ReadAllText(logPath);
            Char[] separators = { '\n', ',', '"', '\r' };

            List<String> logItems = log.Split(separators).ToList<String>();

            logItems.RemoveAll(item => item.CompareTo("") == 0);

            return CreateRows(logItems);
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
