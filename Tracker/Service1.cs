using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Tracker
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            DatabaseConnection.Program prog = new DatabaseConnection.Program();
            LogReader.Program reader = new LogReader.Program();
            prog.Write(new LogTable(LogReader.Program.GetRowsFromDocument()));

            System.Diagnostics.Process.Start("http://localhost:11182/WebForm1.aspx");
        }

        protected override void OnStop()
        {
        }
    }
}
