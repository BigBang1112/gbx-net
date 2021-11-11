using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GBX.NET;

public class Id
{
    public ILookbackable Owner { get; }
    public string? String { get; set; }

    public int Index
    {
        get => String is null ? -1 : Owner.IdStrings.IndexOf(String);
    }

    public static Dictionary<int, string> CollectionIDs { get; }

    public Id(string? str, ILookbackable lookbackable)
    {
        Owner = lookbackable;
        String = str;
    }

    public static implicit operator string(Id s)
    {
        if (s is null) return "";
        return s.ToString();
    }

    public override string ToString()
    {
        return String ?? string.Empty;
    }

    static Id()
    {
        CollectionIDs = new Dictionary<int, string>();

        var startTimestamp = DateTime.Now;

        using var reader = new StringReader(Resources.CollectionID);

        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            var keys = line.Split(' ');
            if (keys.Length >= 2)
                CollectionIDs[Convert.ToInt32(keys[0])] = keys[1];
        }

        Debug.WriteLine("Collection IDs named in " + (DateTime.Now - startTimestamp).TotalMilliseconds + "ms");
    }
}
