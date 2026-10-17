namespace GBX.NET.Serialization;

partial interface IGbxReader
{
    string[] ReadArrayString(int length);
    string[] ReadArrayString();
    string[] ReadArrayString_deprec();
    List<string> ReadListString(int length);
    List<string> ReadListString();
    List<string> ReadListString_deprec();
    GBX.NET.Ident[] ReadArrayIdent(int length);
    GBX.NET.Ident[] ReadArrayIdent();
    GBX.NET.Ident[] ReadArrayIdent_deprec();
    List<GBX.NET.Ident> ReadListIdent(int length);
    List<GBX.NET.Ident> ReadListIdent();
    List<GBX.NET.Ident> ReadListIdent_deprec();
    GBX.NET.PackDesc[] ReadArrayPackDesc(int length);
    GBX.NET.PackDesc[] ReadArrayPackDesc();
    GBX.NET.PackDesc[] ReadArrayPackDesc_deprec();
    List<GBX.NET.PackDesc> ReadListPackDesc(int length);
    List<GBX.NET.PackDesc> ReadListPackDesc();
    List<GBX.NET.PackDesc> ReadListPackDesc_deprec();
    GBX.NET.ZlibData[] ReadArrayZlibData(int length);
    GBX.NET.ZlibData[] ReadArrayZlibData();
    GBX.NET.ZlibData[] ReadArrayZlibData_deprec();
    List<GBX.NET.ZlibData> ReadListZlibData(int length);
    List<GBX.NET.ZlibData> ReadListZlibData();
    List<GBX.NET.ZlibData> ReadListZlibData_deprec();
}

partial class GbxReader
{
    public string[] ReadArrayString(int length)
    {
        if (length == 0)
        {
            return [];
        }

        EnsureValidLength(length);

        var array = new string[length];

        for (int i = 0; i < length; i++)
        {
            array[i] = ReadString();
        }

        return array;
    }

    public string[] ReadArrayString() => ReadArrayString(ReadInt32());

    public string[] ReadArrayString_deprec()
    {
        ReadDeprecVersion();
        return ReadArrayString();
    }

    public List<string> ReadListString(int length)
    {
        if (length == 0)
        {
            return new List<string>();
        }

        EnsureValidLength(length);

        var list = new List<string>(length);

        for (int i = 0; i < length; i++)
        {
            list.Add(ReadString());
        }

        return list;
    }

    public List<string> ReadListString() => ReadListString(ReadInt32());

    public List<string> ReadListString_deprec()
    {
        ReadDeprecVersion();
        return ReadListString();
    }

    public GBX.NET.Ident[] ReadArrayIdent(int length)
    {
        if (length == 0)
        {
            return [];
        }

        EnsureValidLength(length);

        var array = new GBX.NET.Ident[length];

        for (int i = 0; i < length; i++)
        {
            array[i] = ReadIdent();
        }

        return array;
    }

    public GBX.NET.Ident[] ReadArrayIdent() => ReadArrayIdent(ReadInt32());

    public GBX.NET.Ident[] ReadArrayIdent_deprec()
    {
        ReadDeprecVersion();
        return ReadArrayIdent();
    }

    public List<GBX.NET.Ident> ReadListIdent(int length)
    {
        if (length == 0)
        {
            return new List<GBX.NET.Ident>();
        }

        EnsureValidLength(length);

        var list = new List<GBX.NET.Ident>(length);

        for (int i = 0; i < length; i++)
        {
            list.Add(ReadIdent());
        }

        return list;
    }

    public List<GBX.NET.Ident> ReadListIdent() => ReadListIdent(ReadInt32());

    public List<GBX.NET.Ident> ReadListIdent_deprec()
    {
        ReadDeprecVersion();
        return ReadListIdent();
    }

    public GBX.NET.PackDesc[] ReadArrayPackDesc(int length)
    {
        if (length == 0)
        {
            return [];
        }

        EnsureValidLength(length);

        var array = new GBX.NET.PackDesc[length];

        for (int i = 0; i < length; i++)
        {
            array[i] = ReadPackDesc();
        }

        return array;
    }

    public GBX.NET.PackDesc[] ReadArrayPackDesc() => ReadArrayPackDesc(ReadInt32());

    public GBX.NET.PackDesc[] ReadArrayPackDesc_deprec()
    {
        ReadDeprecVersion();
        return ReadArrayPackDesc();
    }

    public List<GBX.NET.PackDesc> ReadListPackDesc(int length)
    {
        if (length == 0)
        {
            return new List<GBX.NET.PackDesc>();
        }

        EnsureValidLength(length);

        var list = new List<GBX.NET.PackDesc>(length);

        for (int i = 0; i < length; i++)
        {
            list.Add(ReadPackDesc());
        }

        return list;
    }

    public List<GBX.NET.PackDesc> ReadListPackDesc() => ReadListPackDesc(ReadInt32());

    public List<GBX.NET.PackDesc> ReadListPackDesc_deprec()
    {
        ReadDeprecVersion();
        return ReadListPackDesc();
    }

    public GBX.NET.ZlibData[] ReadArrayZlibData(int length)
    {
        if (length == 0)
        {
            return [];
        }

        EnsureValidLength(length);

        var array = new GBX.NET.ZlibData[length];

        for (int i = 0; i < length; i++)
        {
            array[i] = ReadZlibData();
        }

        return array;
    }

    public GBX.NET.ZlibData[] ReadArrayZlibData() => ReadArrayZlibData(ReadInt32());

    public GBX.NET.ZlibData[] ReadArrayZlibData_deprec()
    {
        ReadDeprecVersion();
        return ReadArrayZlibData();
    }

    public List<GBX.NET.ZlibData> ReadListZlibData(int length)
    {
        if (length == 0)
        {
            return new List<GBX.NET.ZlibData>();
        }

        EnsureValidLength(length);

        var list = new List<GBX.NET.ZlibData>(length);

        for (int i = 0; i < length; i++)
        {
            list.Add(ReadZlibData());
        }

        return list;
    }

    public List<GBX.NET.ZlibData> ReadListZlibData() => ReadListZlibData(ReadInt32());

    public List<GBX.NET.ZlibData> ReadListZlibData_deprec()
    {
        ReadDeprecVersion();
        return ReadListZlibData();
    }

}
