using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using GBX.NET.Benchmarks.OldClasses;
using GBX.NET.Serialization;

namespace GBX.NET.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class GbxReaderStringBenchmarks
{
    private readonly MemoryStream stream;
    private readonly GbxReader newReader;
    private readonly GameBoxReader oldReader;

    public GbxReaderStringBenchmarks()
    {
        stream = new MemoryStream();

        using var w = new GbxWriter(stream, new() { LeaveOpen = true });
        for (var i = 0; i < 10; i++)
        {
            w.Write("1string1");
            w.Write("1string2");
        }
        stream.Position = 0;
        newReader = new GbxReader(stream, new() { LeaveOpen = true });
        oldReader = new GameBoxReader(stream, leaveOpen: true);
    }

    [Benchmark]
    public void ReadString_New()
    {
        stream.Position = 0;
        for(var i = 0; i < 10; i++)
        {
            newReader.ReadString();
        }
    }

    [Benchmark]
    public void ReadString_Old()
    {
        stream.Position = 0;
        for (var i = 0; i < 10; i++)
        {
            oldReader.ReadString();
        }
    }
}
