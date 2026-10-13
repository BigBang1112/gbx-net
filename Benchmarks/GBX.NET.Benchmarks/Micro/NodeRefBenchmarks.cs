using BenchmarkDotNet.Attributes;
using GBX.NET.Components;
using GBX.NET.Engines.Plug;

namespace GBX.NET.Benchmarks.Micro;

[MemoryDiagnoser]
[BenchmarkCategory("Micro")]
public class NodeRefBenchmarks
{
    private NodeRef<CPlugCrystal> reference = null!;

    [Params(false, true)]
    public bool External { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var node = new CPlugCrystal();
        reference = new();
        if (External)
        {
            var table = new GbxRefTable();
            table.ExternalNodes["node.Gbx"] = () => node.ToGbx();
            reference.Descriptor = table.CreateFile("node.Gbx");
            reference.Resolve();
        }
        else reference.Node = node;
    }

    [Benchmark(Baseline = true)]
    public CPlugCrystal? CachedNode() => reference.CachedNode;

    [Benchmark]
    public CPlugCrystal? Resolve() => reference.Resolve();

    [Benchmark]
    public CPlugCrystal? Node() => reference.Node;
}
