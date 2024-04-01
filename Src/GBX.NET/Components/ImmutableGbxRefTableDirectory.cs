using System.Collections.Immutable;

namespace GBX.NET.Components;

public sealed record ImmutableGbxRefTableDirectory(string Name, ImmutableList<ImmutableGbxRefTableDirectory> Children, ImmutableList<ImmutableGbxRefTableFile> Files)
{
    public override string ToString()
    {
        return Name;
    }
}
