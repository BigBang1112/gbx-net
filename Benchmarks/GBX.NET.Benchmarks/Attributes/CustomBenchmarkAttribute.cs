namespace GBX.NET.Benchmarks.Attributes;

internal class CustomBenchmarkAttribute : Attribute
{
    public bool FileBenchmark { get; init; }
}
