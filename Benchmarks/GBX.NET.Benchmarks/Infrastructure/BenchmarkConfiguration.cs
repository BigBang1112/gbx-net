using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters.Json;

namespace GBX.NET.Benchmarks.Infrastructure;

internal static class BenchmarkConfiguration
{
    public static IConfig Create() => ManualConfig.Create(DefaultConfig.Instance)
        .AddExporter(JsonExporter.Full);
}
