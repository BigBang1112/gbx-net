namespace GBX.NET.Benchmarks.Serialization;

public sealed record GbxFixture(string Name, string RelativePath)
{
    public override string ToString() => Name;
}
