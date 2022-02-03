using BenchmarkDotNet.Attributes;
using GBX.NET.Managers;

namespace GBX.NET.Benchmarks;

[MemoryDiagnoser]
[CustomBenchmark]
public class NodeCacheDefineMappingsBenchmark : Benchmark
{
    [Benchmark]
    public void DefineMappings()
    {
        var mappings = new Dictionary<uint, uint>();
        NodeCacheManager.DefineMappings(mappings);
    }

    [Benchmark(Baseline = true)]
    public void DefineMappings2()
    {
        var mappings = new Dictionary<uint, uint>();
        NodeCacheManager.DefineMappings2(mappings);
    }
}
