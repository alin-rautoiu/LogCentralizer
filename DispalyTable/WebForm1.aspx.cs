using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DispalyTable
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            LogTable log = new LogTable(LogCentralizor.Program.Go());

            foreach (var row in log)
            {
                TableRow rowDispaly = new TableRow();
                foreach (var cell in row)
                {
                    TableCell cellDisplay = new TableCell();
                    cellDisplay.Text = cell;

                    rowDispaly.Cells.Add(cellDisplay);
                }

                WebLogServer.Rows.Add(rowDispaly);
            }
        }
    }
}