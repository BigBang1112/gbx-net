using BenchmarkDotNet.Attributes;

namespace GBX.NET.Benchmarks;

[MemoryDiagnoser]
[CustomBenchmark]
public class NodeCacheDefineMappingsBenchmark : Benchmark
{
    public Dictionary<uint, uint> Mappings { get; set; } = new();

    [Benchmark]
    public void DefineMappings()
    {
        NodeCacheManager.DefineMappings(Mappings);
    }

    [Benchmark(Baseline = true)]
    public void DefineMappings2()
    {
        NodeCacheManager.DefineMappings2(Mappings);
    }
}
