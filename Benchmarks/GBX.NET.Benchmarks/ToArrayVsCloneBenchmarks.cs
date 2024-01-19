using BenchmarkDotNet.Attributes;

namespace GBX.NET.Benchmarks;

[MemoryDiagnoser]
public class ToArrayVsCloneBenchmarks
{
    private static readonly Random random = new(123);
    private static readonly byte[] data = new byte[4000];

    public ToArrayVsCloneBenchmarks()
    {
        random.NextBytes(data);
    }

    [Benchmark]
    public byte[] ToArray()
    {
        return data.ToArray();
    }

    [Benchmark]
    public byte[] Clone()
    {
        return (byte[])data.Clone();
    }

    [Benchmark]
    public byte[] CopyTo()
    {
        var array = new byte[data.Length];
        data.CopyTo(array, 0);
        return array;
    }
}
