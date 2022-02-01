using BenchmarkDotNet.Attributes;
using GBX.NET.Engines.Game;

namespace GBX.NET.Benchmarks;

[CustomBenchmark(FileBenchmark = true)]
public class MapDiscoverBenchmark : GameBoxParseBenchmark<CGameCtnChallenge>
{
    private CGameCtnChallenge map;

    public MapDiscoverBenchmark() : base(folder: "Maps")
    {
        map = null!;
    }

    public override void OnGlobalSetup()
    {
        map = GameBox.ParseNode<CGameCtnChallenge>(stream);
    }

    [Benchmark]
    public void DiscoverMap()
    {
        try
        {
            map.DiscoverAllChunks();
        }
        catch
        {

        }
    }
}
