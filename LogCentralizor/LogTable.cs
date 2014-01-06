using System;
using System.Collections.Generic;
public class LogTable: IEnumerable<Row>
{
    public List<Row> rows;
	public LogTable()
	{
        rows = new List<Row>();
	}

    public LogTable(List<Row> logItems){
        rows = new List<Row>(logItems);
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

    IEnumerator<Row> IEnumerable<Row>.GetEnumerator()
    {
        return rows.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return rows.GetEnumerator();
    }
}
