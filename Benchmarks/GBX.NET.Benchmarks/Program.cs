using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using GBX.NET.Benchmarks;
using Perfolizer.Horology;
using System.Globalization;

var fileConfig = new ManualConfig()
    .AddLogger(new ConsoleLogger())
    .AddColumn(TargetMethodColumn.Method)
    .AddColumn(new ParamColumn("FileName"))
    .AddColumn(StatisticColumn.Median)
    .AddDiagnoser(MemoryDiagnoser.Default)
    .WithSummaryStyle(new SummaryStyle(
        CultureInfo.InvariantCulture,
        printUnitsInHeader: true,
        SizeUnit.KB,
        TimeUnit.Millisecond)
        .WithMaxParameterColumnWidth(100)
    );

var results = BenchmarkRunner.Run(typeof(MapParseBenchmark), fileConfig);