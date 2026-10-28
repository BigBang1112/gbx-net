namespace GBX.NET.Components;

public sealed class GbxRefTableFile : GbxRefTableNode
{
    private string filePath;

    /// <summary>
    /// Path of the external file. Changing it invalidates cached resolution for every reference sharing this descriptor.
    /// </summary>
    /// <exception cref="ArgumentNullException">The path is null.</exception>
    /// <exception cref="InvalidOperationException">The path is changed while the descriptor is resolving.</exception>
    public string FilePath
    {
        get => filePath;
        set
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Resolution.Update(ref filePath, value);
        }
    }

    public GbxRefTableFile(
        GbxRefTable refTable,
        int flags,
        bool useFile,
        string filePath) : base(refTable, flags, useFile)
    {
        this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    public override string ToString()
    {
        return $"{FilePath}, Flags: {Flags}, UseFile: {UseFile}";
    }

    public string GetFullPath()
    {
        return RefTable.GetFullFilePath(this);
    }

    internal override CMwNod? ResolveNode() => RefTable.ResolveNode(this);

    internal override Task<CMwNod?> ResolveNodeAsync() => RefTable.ResolveNodeAsync(this);
}
