using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DispalyTable
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        List<String> startDates;
        List<String> endDates;
        static int index;
        LogTable log;
        List<String> ipList;
        protected void Page_Load(object sender, EventArgs e)
        {
            log = new LogTable(LogCentralizor.Program.Go());
            TableRow header = new TableRow();
            startDates = new List<string>();
            endDates = new List<string>();
            ipList = new List<string>();

            ipList.Add("_");

            foreach (var row in log.rows)
            {
                startDates.Add(row.ElementAt(0));
                endDates.Add(row.ElementAt(0));
                if (!ipList.Contains(row.ElementAt(5)))
                    ipList.Add(row.ElementAt(5));
            }

            if (!IsPostBack)
            {
                IpSelect.DataSource = ipList;
                IpSelect.DataBind();

                StartDate.DataSource = startDates;
                StartDate.DataBind();

                EndDate.DataSource = endDates;
                EndDate.DataBind();
                
                WebLogDataGrid.DataSource = createGrid(log);
                WebLogDataGrid.DataBind();
            }
        }

        private DataView createGrid(LogTable log)
        {
            DataTable dt = new DataTable();

            for (int i = 0; i < 11; i++)
            {
                DataColumn column = new DataColumn(log.header[i]);
                dt.Columns.Add(column);
            }

            foreach (var row in log.rows)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < row.Count<String>(); j++)
                {
                    dr[j] = row.ElementAt(j);
                }
                dt.Rows.Add(dr);
            }
            return dt.AsDataView();
        }

        protected void StarDate_SelectedIndexChanged(object sender, EventArgs e)
        {

            index = StartDate.SelectedIndex;
            endDates.RemoveRange(0, index);
            EndDate.DataSource = endDates;
            EndDate.DataBind();
        }

        protected void Bttn_Click(object sender, EventArgs e)
        {
            int size = log.rows.Count;
            log.rows = log.rows.GetRange(StartDate.SelectedIndex, EndDate.SelectedIndex + 1);

            Label1.Text = log.rows.Count.ToString();

            WebLogDataGrid.DataSource = createGrid(log);
            WebLogDataGrid.DataBind();
            
        }

        protected void IpFilter_Click(object sender, EventArgs e)
        {

            if(IpSelect.SelectedValue.CompareTo("_") != 0)
                log.rows.RemoveAll(row => row.ElementAt(5).CompareTo(IpSelect.SelectedValue) != 0);

            WebLogDataGrid.DataSource = createGrid(log);
            WebLogDataGrid.DataBind();
        }
    }
}