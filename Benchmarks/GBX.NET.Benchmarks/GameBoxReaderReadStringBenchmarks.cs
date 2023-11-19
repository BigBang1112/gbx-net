using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.IO;

namespace GBX.NET.Benchmarks;

[MemoryDiagnoser]
public class GameBoxReaderReadStringBenchmarks
{
    private readonly GameBoxReader reader;

    public GameBoxReaderReadStringBenchmarks()
    {
        var stream = new MemoryStream();
        using var writer = new GameBoxWriter(stream);
        
        for (var i = 0; i < 4; i++)
        {
            writer.Write("Hello World!");
        }

        reader = new GameBoxReader(stream);
    }
    
    [Benchmark]
    public void Read4EqualStrings()
    {
        reader.BaseStream.Position = 0; // 1.5 nanosecond operation
        reader.ReadString();
        reader.ReadString();
        reader.ReadString();
        reader.ReadString();
    }
}