﻿using System;
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
        protected void Reset()
        {
            DatabaseConnection.Program newConnection = new DatabaseConnection.Program();
            log = newConnection.Read();

            StartDate.DataSource = startDates;
            EndDate.DataSource = endDates;
            StartDate.DataBind();
            EndDate.DataBind();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DatabaseConnection.Program newConnection = new DatabaseConnection.Program();

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

                StartDate.DataSource = startDates.GetRange(0,startDates.Count);
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

            log.rows.RemoveAll(r => r.ElementAt(0).CompareTo(StartDate.SelectedValue) < 0);
            log.rows.RemoveAll(r => r.ElementAt(0).CompareTo(EndDate.SelectedValue) > 0);

            if (!(IpSelect.SelectedValue.CompareTo("_") == 0))
            {
                IpFilter();
            }

            if (!(IpText.Text.CompareTo("") == 0))
            { 
                IpTextFilter();
            }

            WebLogDataGrid.DataSource = createGrid(log);
            WebLogDataGrid.DataBind();
            Reset();
        }

        protected void IpFilter()
        {
            log.rows.RemoveAll(r => r.ElementAt(5).CompareTo(IpSelect.SelectedValue) != 0);
        }

        protected void IpTextFilter()
        {
            String[] address = IpText.Text.Split('/');
                IPAddress ip;
                int mask;
                bool check = Regex.Match(address[0], "[0-9]+.[0-9]+.[0-9]+.[0-9]+").Success;
                IPAddress.TryParse(address[0],out ip);
                if (!check)
                {
                    Label1.Text = "Adresa in format gresit";
                }
                else
                {
                    Label1.Text = "";
                    try
                    {
                        mask = Int32.Parse(address[1]);
                    }
                    catch (IndexOutOfRangeException exception){
                        mask = 32;
                    }
                    byte[] maskBytes = new Byte[4];

                    for (int i = 0; i < 4; i++)
                    {
                        if (mask / 8 >= 1)
                        {
                            maskBytes[i] = 255;
                        }
                        else
                        {
                            maskBytes[i] = (byte)Math.Pow(2,8 - mask % 8);
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

                    foreach (var row in log.rows)
                    {
                        IPAddress oldip = IPAddress.Parse(row.ElementAt(5));
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
                            newLog.rows.Add(row);
                    }
                }
        
            

            }

        }
    }
