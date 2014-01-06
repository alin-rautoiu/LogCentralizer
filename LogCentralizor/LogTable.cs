using System;
using System.Collections.Generic;
public class LogTable
{
    List<Row> rows;
	public LogTable()
	{
        rows = new List<Row>();
	}
    public void Update(List<Row> rows)
    {
        foreach(var row in rows)
        {
            if(!rows.Contains(row))
            {
                rows.Add(row);
            }
        }
    }
}
