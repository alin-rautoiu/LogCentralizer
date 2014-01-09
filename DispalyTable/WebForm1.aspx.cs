using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DispalyTable
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        static List<String> startDates;
        static List<String> endDates;
        static int index;
        static LogTable log;
        List<String> ipList;
        public DatabaseConnection.Program newConnection;
        protected void Reset()
        {
            log = newConnection.Read();

            StartDate.DataSource = startDates;
            EndDate.DataSource = endDates;
            StartDate.DataBind();
            EndDate.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            newConnection = new DatabaseConnection.Program();

            if (!IsPostBack)
            {
                log = newConnection.Read();
                newConnection.Write(new LogTable(LogReader.Program.GetRowsFromDocument()));

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

                IpSelect.DataSource = ipList;
                IpSelect.DataBind();

                StartDate.DataSource = startDates.GetRange(0, startDates.Count);
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
            endDates.Clear();
            endDates.AddRange(startDates.GetRange(index, startDates.IndexOf(startDates.Last()) - index + 1));
            EndDate.DataSource = endDates;
            EndDate.DataBind();
        }

        protected void DateFilterClick(object sender, EventArgs e)
        {
            if((IpSelect.SelectedValue.CompareTo("_") == 0) && (IpText.Text.CompareTo("") == 0)){
                log = newConnection.Filter(StartDate.SelectedValue, EndDate.SelectedValue);
            }
            else
            {
                if (IpRadioList.Checked)
                {
                    log = newConnection.Filter(StartDate.SelectedValue, EndDate.SelectedValue, IpSelect.SelectedValue);
                }
                else if(IpRadioText.Checked && IpTextFilter()[0].CompareTo("err") != 0)
                {
                    String address = IpTextFilter()[0];
                    String mask = IpTextFilter()[1];
                    log = newConnection.Filter(StartDate.SelectedValue, EndDate.SelectedValue, address, mask);
                }
            }

            WebLogDataGrid.DataSource = createGrid(log);
            WebLogDataGrid.DataBind();
            Reset();
        }

        protected void IpFilter()
        {
            log.rows.RemoveAll(r => r.ElementAt(5).CompareTo(IpSelect.SelectedValue) != 0);
        }

        protected String[] IpTextFilter()
        {
            String[] address = IpText.Text.Split('/');
            IPAddress ip;
            int mask;
            bool check = Regex.Match(address[0], "[0-9]+.[0-9]+.[0-9]+.[0-9]+").Success;
            IPAddress.TryParse(address[0], out ip);
            if (!check)
            {
                Label1.Text = "Adresa in format gresit";
                address[0] = "err";
                return address;
            }
            else
            {
                Label1.Text = "";
                if (address.Length == 1)
                {
                    String[] newAddress = { address[0], "32" };
                    return newAddress;
                }
                return address;
            }

        }

    }
}
