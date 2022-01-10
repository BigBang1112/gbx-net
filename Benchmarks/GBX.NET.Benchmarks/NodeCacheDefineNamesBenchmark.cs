using BenchmarkDotNet.Attributes;

namespace GBX.NET.Benchmarks;

[MemoryDiagnoser]
[CustomBenchmark]
public class NodeCacheDefineNamesBenchmark : Benchmark
{
    public Dictionary<uint, string> Names { get; set; } = new();
    public Dictionary<uint, string> Extensions { get; set; } = new();

    [Benchmark]
    public void DefineNames()
    {
        NodeCacheManager.DefineNames(Names, Extensions);
    }

    [Benchmark(Baseline = true)]
    public void DefineNames2()
    {
        NodeCacheManager.DefineNames2(Names, Extensions);
    }
}
