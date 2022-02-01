using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using GBX.NET.Benchmarks;
using Perfolizer.Horology;
using System.Globalization;
using System.Reflection;

var benchmarkTypes = typeof(Program).Assembly
    .GetTypes()
    .Where(x => x.IsSubclassOf(typeof(Benchmark)) && !x.IsAbstract)
    .ToArray();

var fileConfig = new ManualConfig()
    .AddLogger(new ConsoleLogger())
    .AddColumn(TargetMethodColumn.Method)
    .AddColumn(new ParamColumn("FileName"))
    .AddColumn(StatisticColumn.Median)
    .WithSummaryStyle(new SummaryStyle(
        CultureInfo.InvariantCulture,
        printUnitsInHeader: true,
        SizeUnit.KB,
        TimeUnit.Millisecond)
        .WithMaxParameterColumnWidth(100)
    );

while (true)
{
    Console.WriteLine("-1. Table generators");
    Console.WriteLine();

    for (int i = 0; i < benchmarkTypes.Length; i++)
    {
        Console.WriteLine("{0}. {1}", i + 1, benchmarkTypes[i].Name);
    }

    Console.WriteLine();
    Console.WriteLine("0. Exit");

    Console.WriteLine();
    Console.Write("Select a benchmark: ");

    var input = Console.ReadLine();

    Console.WriteLine();

    if (!int.TryParse(input, out int num))
        continue;

    if (num == 0)
        break;

    if (num < 0)
    {
        DoPreset();

        continue;
    }

    num--;

    if (num > benchmarkTypes.Length)
        continue;

    var benchmarkType = benchmarkTypes[num];

    var customBenchmarkAttribute = benchmarkType.GetCustomAttribute<CustomBenchmarkAttribute>();

    var config = default(IConfig?);

    if (customBenchmarkAttribute?.FileBenchmark == true)
    {
        config = fileConfig;
    }

    var results = BenchmarkRunner.Run(benchmarkType, config, args);

    Console.WriteLine();
}

void DoPreset()
{
    var readBenchmarkResults = BenchmarkRunner.Run<MapParseBenchmark>(fileConfig, args);
    var readHeaderBenchmarkResults = BenchmarkRunner.Run<MapHeaderParseBenchmark>(fileConfig, args);
    var writeBenchmarkResults = BenchmarkRunner.Run<MapSaveBenchmark>(fileConfig, args);

    Console.WriteLine("| File name | Read [ms] | Read header [ms] | Write [ms]");
    Console.WriteLine("| --- | --- | --- | ---");

    foreach (var report in readBenchmarkResults.Reports)
    {
        var key = report.BenchmarkCase.Parameters.ValueInfo;

        var readMean = report.ResultStatistics.Mean / 1_000_000;

        var readHeaderMean = readHeaderBenchmarkResults.Reports
            .First(x => x.BenchmarkCase.Parameters.ValueInfo == key)
            .ResultStatistics.Mean / 1_000_000;

        var writeMean = writeBenchmarkResults.Reports
            .First(x => x.BenchmarkCase.Parameters.ValueInfo == key)
            .ResultStatistics.Mean / 1_000_000;

        var mapName = key
            .Replace("[FileName=", "")
            .Replace("]", "")
            .Replace(".Map.Gbx", "", StringComparison.OrdinalIgnoreCase)
            .Replace(".Challenge.Gbx", "", StringComparison.OrdinalIgnoreCase);

        Console.WriteLine("| {0} | {1} | {2} | {3}",
            mapName,
            readMean.ToString("0.00") + " ms",
            readHeaderMean.ToString("0.00") + " ms",
            writeMean.ToString("0.00") + " ms");
    }
}