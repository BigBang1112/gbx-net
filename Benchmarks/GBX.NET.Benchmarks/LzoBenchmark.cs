using BenchmarkDotNet.Attributes;
using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Benchmarks;

[MemoryDiagnoser]
public class LzoBenchmark : Benchmark
{
    public MemoryStream MapCompressed { get; set; } = new();
    public MemoryStream MapUncompressed { get; set; } = new();

    [GlobalSetup]
    public void Setup()
    {
        using var fs = File.OpenRead("Maps\\Community\\CCP#04 - ODYSSEY.Map.Gbx");

        fs.CopyTo(MapCompressed);

        MapCompressed.Seek(0, SeekOrigin.Begin);

        GameBox.Decompress(MapCompressed, MapUncompressed);

        MapCompressed.Seek(0, SeekOrigin.Begin);
        MapUncompressed.Seek(0, SeekOrigin.Begin);
    }

    [Benchmark(Baseline = true)]
    public CMwNod ParseMapCompressed()
    {
        MapCompressed.Position = 0;
        return GameBox.ParseNode(MapCompressed)!;
    }

    [Benchmark]
    public CMwNod ParseMapUncompressed()
    {
        MapUncompressed.Position = 0;
        return GameBox.ParseNode(MapUncompressed)!;
    }
}
