using BenchmarkDotNet.Attributes;
using GBX.NET.Engines.Game;

namespace GBX.NET.Benchmarks;

public class MapSaveBenchmark : GameBoxParseBenchmark<CGameCtnChallenge>
{
    private CGameCtnChallenge map;
    private MemoryStream saveStream;

    public MapSaveBenchmark() : base(folder: "Maps")
    {
        map = null!;
        saveStream = null!;
    }

    public override void OnGlobalSetup()
    {
        map = GameBox.ParseNode<CGameCtnChallenge>(stream);
    }

    public override void OnIterationSetup()
    {
        saveStream = new MemoryStream();
    }

    [Benchmark]
    public void SaveMap()
    {
        map.Save(saveStream);
    }
}
