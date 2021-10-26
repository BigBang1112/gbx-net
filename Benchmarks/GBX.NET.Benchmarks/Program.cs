using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using GBX.NET.Engines.MwFoundations;
using GBX.NET.Engines.Game;
using System.Linq.Expressions;
using BenchmarkDotNet.Reports;
using System.Globalization;
using BenchmarkDotNet.Columns;
using Perfolizer.Horology;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Diagnosers;

namespace GBX.NET.Benchmarks
{
    public class Program
    {
        static void Main(string[] args)
        {
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
        }
    }
}
