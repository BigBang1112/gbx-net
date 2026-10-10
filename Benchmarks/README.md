# Benchmarks

The BenchmarkDotNet project measures GBX.NET source generation and Gbx fixture reads and writes. It also includes the existing string-reader and array-copy benchmarks.

## Structure

One console runner discovers all suites. Workload code lives in separate folders and namespaces:

| Folder | Contents |
| --- | --- |
| `GBX.NET.Benchmarks/Generation` | Generator cases and in-memory layout inputs. |
| `GBX.NET.Benchmarks/Serialization` | Fixture catalog and file read/write cases. |
| `GBX.NET.Benchmarks/Micro` | String-reader and array-copy comparisons, with the legacy reader under `Legacy`. |
| `GBX.NET.Benchmarks/Infrastructure` | Shared reporting configuration. |

Each suite has a `BenchmarkCategory`: `Generator`, `Serialization`, or `Micro`. Add new cases to the matching folder and category. Keep input loading and validation in setup methods so benchmark methods contain the operation being measured. The fixture benchmarks cover the old single-map parse case.

## Run

Use the .NET 10 SDK and run from the repository root in Release configuration. List the available benchmarks:

```powershell
dotnet run --project Benchmarks/GBX.NET.Benchmarks -c Release -- --list flat
```

Run the generator benchmarks:

```powershell
dotnet run --project Benchmarks/GBX.NET.Benchmarks -c Release -- --filter '*GeneratorBenchmarks*'
```

Run the fixture benchmarks:

```powershell
dotnet run --project Benchmarks/GBX.NET.Benchmarks -c Release -- --filter '*GbxFixtureBenchmarks*'
```

For a quick check that every new case works, use the `Dry` job:

```powershell
dotnet run --project Benchmarks/GBX.NET.Benchmarks -c Release -- --filter '*GeneratorBenchmarks*' '*GbxFixtureBenchmarks*' --job Dry
```

`Dry` runs one measurement per case and is for validation. Use the default job for performance comparisons, or `--job Short` for a shorter exploratory run. See BenchmarkDotNet's [command-line options](https://benchmarkdotnet.org/articles/guides/console-args.html) for filters, exporters, and job settings.

The runner returns a nonzero exit code if benchmark validation, building, or execution fails, so the dry command can also serve as an automated smoke check.

Reports are written to `BenchmarkDotNet.Artifacts/results` under the working directory. This directory is ignored by Git. Use `--artifacts <path>` to select another location. All suites report elapsed time, allocations, and garbage collections through `MemoryDiagnoser`. Shared configuration adds [full JSON exports](https://benchmarkdotnet.org/articles/configs/exporters.html) to the default Markdown, CSV, and HTML reports.

## Share and compare results

Use separate artifact directories to keep results from different revisions:

```powershell
dotnet run --project Benchmarks/GBX.NET.Benchmarks -c Release -- --filter '*GeneratorBenchmarks*' --artifacts BenchmarkDotNet.Artifacts/baseline
dotnet run --project Benchmarks/GBX.NET.Benchmarks -c Release -- --filter '*GeneratorBenchmarks*' --artifacts BenchmarkDotNet.Artifacts/candidate
```

Share the GitHub Markdown tables from each `results/` directory in a PR, along with the commit SHAs and run commands. The full JSON reports include measurements and environment details. Raw results stay outside version control so measurements from different machines do not become repository baselines. Commit the changes before a run when someone else must reproduce it from the recorded SHA.

Run baseline and candidate on the same machine with the same suite, job, SDK, and compression implementation. Compare matching methods, fixture names, and compression values in the reports. Keep the confidence intervals and allocation columns when sharing a comparison. `Dry` validates execution and does not establish a performance baseline.

The manual [Benchmarks workflow](../.github/workflows/benchmarks.yml) runs on Ubuntu using `dotnet run` directly. It accepts a BenchmarkDotNet filter and job, then uploads reports and available benchmark logs as an artifact for 14 days, including diagnostics from failed runs. The artifact name includes the commit SHA and job. Hosted-runner measurements are useful for exploration. Use the same local machine for performance decisions. The workflow runs only when dispatched and does not enforce timing thresholds on pull requests.

## Generator workload

`GeneratorBenchmarks` runs `GbxGenerator` against every repository `.chunkl` layout, the six resource tables supplied by the library project, and the handwritten C# sources. The project copies these inputs into its output directory during the build, so benchmark processes can resolve them independently of the working directory. Generated C#, `bin`, and `obj` are excluded.

The setup loads and parses C# inputs with .NET 10 Release preprocessor symbols. `GbxGenerator` currently analyzes declarations syntactically, so the input compilation does not need metadata references. The measurements cover Roslyn driver execution, ChunkL parsing, planning, and source emission. They exclude disk reads, C# input parsing, compilation of generated code, and the other source generators used by the library build.

| Method | Workload |
| --- | --- |
| `FreshGeneration` | Run from a driver that has never generated output. |
| `CachedGeneration` | Rerun a warmed driver with unchanged inputs. |
| `ChangedLayoutGeneration` | Rerun a warmed driver after adding one chunk and property to the map layout in memory. |

Roslyn drivers are immutable. Each invocation starts from the same driver snapshot, so the changed-layout case performs generation every time. It cannot become an unchanged cached run after the first invocation. Setup rejects generator errors and checks that the layout edit produces the added property. The repository layouts are never changed by the benchmark.

## Fixture workload

`GbxFixtureBenchmarks` uses these files from `Tests/GBX.NET.Tests/Files/Gbx`:

- `CGameCtnChallenge TMU 001.Challenge.Gbx`
- `CGameCtnChallenge TM2020 001.Map.Gbx`
- `CGameCtnGhost MP4 001.Ghost.Gbx`
- `CPlugCrystal MP4 001.Crystal.Gbx`

Each fixture runs with both compressed and uncompressed bodies. Setup parses the original file, saves it with the selected compression, and uses those prepared bytes for `Read`. `Write` saves the prepared object into a reusable memory stream whose position and length are reset each time. These measurements include body compression or decompression and library allocations, while excluding filesystem access and fixture preparation.

Setup primes serialization caches and checks that the saved file can be parsed with the expected class and compression. Reads use strict settings, with body exceptions and skippable-chunk fallback disabled. The fixture benchmarks measure repeated saves of an already prepared object. To add coverage, extend `Serialization/FixtureCatalog.cs` with a short display name and another writable fixture path relative to the fixture root, then run the dry check.

For comparisons, use the same job, fixtures, runtime, compression implementation, and machine across revisions. Rebuild after changing layouts or source files, as `--no-build` uses the previous input snapshot. Run `dotnet clean Benchmarks/GBX.NET.Benchmarks -c Release` before rebuilding after removing or renaming inputs to clear old copies.
