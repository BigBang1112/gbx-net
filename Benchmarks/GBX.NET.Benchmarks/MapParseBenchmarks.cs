using BenchmarkDotNet.Attributes;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;

namespace GBX.NET.Benchmarks;

[MemoryDiagnoser]
public class MapParseBenchmarks
{
    private readonly MemoryStream stream;

    public MapParseBenchmarks()
    {
        Gbx.LZO = new MiniLZO();
        stream = new MemoryStream(File.ReadAllBytes(Path.Combine("Gbx", "CGameCtnChallenge", "GBX-NET 2 CGameCtnChallenge TMU 001.Challenge.Gbx")));
    }

    [Benchmark]
    public Gbx ParseMap()
    {
        stream.Position = 0;
        return Gbx.Parse<CGameCtnChallenge>(stream);
    }
}

