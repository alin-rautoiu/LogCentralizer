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
            string logPath = "C:\\Users\\Alin\\Downloads\\20110307_023937AM_vms4pplog download\\20110307_023937AM_vms4pplog download.csv";
            String log = File.ReadAllText(logPath);
            Char[] separators = { '\n', ',', '"', '\r' };

            List<String> logItems = log.Split(separators).ToList<String>();

            logItems.RemoveAll(item => item.CompareTo("") == 0);

            return CreateRows(logItems);
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
