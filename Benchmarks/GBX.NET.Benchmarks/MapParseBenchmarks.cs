using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace GBX.NET.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class MapParseBenchmarks
{
    private readonly MemoryStream stream;

    public MapParseBenchmarks()
    {
        stream = new MemoryStream(File.ReadAllBytes(Path.Combine("Maps", "8_Trackmania 2020", "20211001", "Fall 2021 - 16.Map.Gbx")));
    }

    [Benchmark]
    public GameBox ParseHeavyMap()
    {
        stream.Position = 0;
        return GameBox.Parse(stream);
    }
}
