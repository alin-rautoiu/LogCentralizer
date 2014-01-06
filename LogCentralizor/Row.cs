using System;
using System.Collections.Generic;

public class Row : IEnumerable<String>
{
    List<String> cells;

    public Row()
    {
        cells = new List<String>();
    }

    public Row(List<String> items)
    {
        cells = new List<String>(items);
    }

    public IEnumerator<string> GetEnumerator()
    {
        return cells.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return cells.GetEnumerator();
    }

    public override string ToString()
    {
        String s = "";

        foreach (var cell in cells)
        {
            s = s + cell + " ";
        }
        return s;
    }
}
