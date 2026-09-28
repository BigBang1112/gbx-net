namespace GBX.NET.Serialization;

partial interface IGbxWriter
{
    void WriteArray(string[]? value, int length);
    void WriteArray(string[]? value);
    void WriteArray_deprec(string[]? value);
    void WriteList(List<string>? value, int length);
    void WriteList(List<string>? value);
    void WriteList_deprec(List<string>? value);
    void WriteArray(GBX.NET.Ident[]? value, int length);
    void WriteArray(GBX.NET.Ident[]? value);
    void WriteArray_deprec(GBX.NET.Ident[]? value);
    void WriteList(List<GBX.NET.Ident>? value, int length);
    void WriteList(List<GBX.NET.Ident>? value);
    void WriteList_deprec(List<GBX.NET.Ident>? value);
    void WriteArray(GBX.NET.PackDesc[]? value, int length);
    void WriteArray(GBX.NET.PackDesc[]? value);
    void WriteArray_deprec(GBX.NET.PackDesc[]? value);
    void WriteList(List<GBX.NET.PackDesc>? value, int length);
    void WriteList(List<GBX.NET.PackDesc>? value);
    void WriteList_deprec(List<GBX.NET.PackDesc>? value);
}

partial class GbxWriter
{
    public void WriteArray(string[]? value)
    {
        if (value is null)
        {
            Write(0);
            return;
        }

        Write(value.Length);

        foreach (var item in value)
        {
            Write(item);
        }
    }

    public void WriteArray(string[]? value, int length)
    {
        if (value is not null)
        {
            foreach (var item in value)
            {
                Write(item);
            }
        }

        if (value is null || length > value.Length)
        {
            for (var i = value?.Length ?? 0; i < length; i++)
            {
                Write(default(string));
            }
        }
    }

    public void WriteArray_deprec(string[]? value)
    {
        WriteDeprecVersion();
        WriteArray(value);
    }

    public void WriteList(List<string>? value)
    {
        if (value is null)
        {
            Write(0);
            return;
        }

        Write(value.Count);

        foreach (var item in value)
        {
            Write(item);
        }
    }

    public void WriteList(List<string>? value, int length)
    {
        if (value is not null)
        {
            foreach (var item in value)
            {
                Write(item);
            }
        }

        if (value is null || length > value.Count)
        {
            for (var i = value?.Count ?? 0; i < length; i++)
            {
                Write(default(string));
            }
        }
    }

    public void WriteList_deprec(List<string>? value)
    {
        WriteDeprecVersion();
        WriteList(value);
    }

    public void WriteArray(GBX.NET.Ident[]? value)
    {
        if (value is null)
        {
            Write(0);
            return;
        }

        Write(value.Length);

        foreach (var item in value)
        {
            Write(item);
        }
    }

    public void WriteArray(GBX.NET.Ident[]? value, int length)
    {
        if (value is not null)
        {
            foreach (var item in value)
            {
                Write(item);
            }
        }

        if (value is null || length > value.Length)
        {
            for (var i = value?.Length ?? 0; i < length; i++)
            {
                Write(default(GBX.NET.Ident));
            }
        }
    }

    public void WriteArray_deprec(GBX.NET.Ident[]? value)
    {
        WriteDeprecVersion();
        WriteArray(value);
    }

    public void WriteList(List<GBX.NET.Ident>? value)
    {
        if (value is null)
        {
            Write(0);
            return;
        }

        Write(value.Count);

        foreach (var item in value)
        {
            Write(item);
        }
    }

    public void WriteList(List<GBX.NET.Ident>? value, int length)
    {
        if (value is not null)
        {
            foreach (var item in value)
            {
                Write(item);
            }
        }

        if (value is null || length > value.Count)
        {
            for (var i = value?.Count ?? 0; i < length; i++)
            {
                Write(default(GBX.NET.Ident));
            }
        }
    }

    public void WriteList_deprec(List<GBX.NET.Ident>? value)
    {
        WriteDeprecVersion();
        WriteList(value);
    }

    public void WriteArray(GBX.NET.PackDesc[]? value)
    {
        if (value is null)
        {
            Write(0);
            return;
        }

        Write(value.Length);

        foreach (var item in value)
        {
            Write(item);
        }
    }

    public void WriteArray(GBX.NET.PackDesc[]? value, int length)
    {
        if (value is not null)
        {
            foreach (var item in value)
            {
                Write(item);
            }
        }

        if (value is null || length > value.Length)
        {
            for (var i = value?.Length ?? 0; i < length; i++)
            {
                Write(default(GBX.NET.PackDesc));
            }
        }
    }

    public void WriteArray_deprec(GBX.NET.PackDesc[]? value)
    {
        WriteDeprecVersion();
        WriteArray(value);
    }

    public void WriteList(List<GBX.NET.PackDesc>? value)
    {
        if (value is null)
        {
            Write(0);
            return;
        }

        Write(value.Count);

        foreach (var item in value)
        {
            Write(item);
        }
    }

    public void WriteList(List<GBX.NET.PackDesc>? value, int length)
    {
        if (value is not null)
        {
            foreach (var item in value)
            {
                Write(item);
            }
        }

        if (value is null || length > value.Count)
        {
            for (var i = value?.Count ?? 0; i < length; i++)
            {
                Write(default(GBX.NET.PackDesc));
            }
        }
    }

    public void WriteList_deprec(List<GBX.NET.PackDesc>? value)
    {
        WriteDeprecVersion();
        WriteList(value);
    }

}
