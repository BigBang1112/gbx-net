using BenchmarkDotNet.Attributes;
using GBX.NET.Engines.Game;

namespace GBX.NET.Benchmarks;

[CustomBenchmark(FileBenchmark = true)]
public class MapParseBenchmark : GameBoxParseBenchmark<CGameCtnChallenge>
{
    public MapParseBenchmark() : base(folder: "Maps")
    {

    }

    [Benchmark]
    public CGameCtnChallenge ParseMap()
    {
        return GameBox.ParseNode<CGameCtnChallenge>(stream);
    }
}
