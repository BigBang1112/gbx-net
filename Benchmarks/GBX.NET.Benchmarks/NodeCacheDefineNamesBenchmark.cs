using BenchmarkDotNet.Attributes;

namespace GBX.NET.Benchmarks;

[MemoryDiagnoser]
[CustomBenchmark]
public class NodeCacheDefineNamesBenchmark : Benchmark
{
    [Benchmark]
    public void DefineNames()
    {
        var names = new Dictionary<uint, string>();
        var extensions = new Dictionary<uint, string>();
        NodeCacheManager.DefineNames(names, extensions);
    }

    [Benchmark(Baseline = true)]
    public void DefineNames2()
    {
        var names = new Dictionary<uint, string>();
        var extensions = new Dictionary<uint, string>();
        NodeCacheManager.DefineNames2(names, extensions);
    }
}
