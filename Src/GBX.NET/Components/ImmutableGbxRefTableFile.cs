namespace GBX.NET.Components;

public sealed record ImmutableGbxRefTableFile(int Flags, bool UseFile, string Name) : ImmutableGbxRefTableNode(Flags, UseFile)
{
    public override string ToString()
    {
        return Name;
    }
}
