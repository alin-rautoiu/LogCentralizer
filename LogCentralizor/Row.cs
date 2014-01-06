using System;
using System.Collections.Generic;

public class Row
{
    public String LogTime;
    public String Action;
    public String FolderPath;
    public String Filename;
    public String Username;
    public String IPADDRESS;
    public String XferSize;
    public String Duration;
    public String AgentBrand;
    public String AgentVersion;
    public String Error;

    public Row()
    {
    }

    public Row(List<String> items)
    {
        LogTime = items[0];
        Action = items[1];
        FolderPath = items[2];
        Filename = items[3];
        Username = items[4];
        IPADDRESS = items[5];
        XferSize = items[6];
        Duration = items[7];
        AgentBrand = items[8];
        AgentVersion = items[9];
        Error = items[10];
    }

    public override string ToString()
    {
        return String.Format(LogTime + ", " + Action + ", " +
        FolderPath + ", " +
        Filename + ", " +
        Username + ", " +
        IPADDRESS + ", " +
        XferSize + ", " +
        Duration + ", " +
        AgentBrand + ", " +
        AgentVersion + ", " +
        Error);
    }
}
