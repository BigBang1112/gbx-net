using BenchmarkDotNet.Attributes;
using GBX.NET.Engines.Game;

namespace GBX.NET.Benchmarks;

public class MapHeaderParseBenchmark : GameBoxParseBenchmark<CGameCtnChallenge>
{
    public MapHeaderParseBenchmark() : base(folder: "Maps")
    {

    }

    [Benchmark]
    public CGameCtnChallenge ParseMapHeader()
    {
        return GameBox.ParseNodeHeader<CGameCtnChallenge>(stream);
    }
}
