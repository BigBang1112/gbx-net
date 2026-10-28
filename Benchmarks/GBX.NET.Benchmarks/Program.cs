using BenchmarkDotNet.Running;
using GBX.NET.Benchmarks.Infrastructure;

var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, BenchmarkConfiguration.Create());
Environment.ExitCode = summaries.Any(summary => summary.HasCriticalValidationErrors
    || summary.Reports.Any(report => !report.Success)) ? 1 : 0;
