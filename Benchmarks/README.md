# Benchmarks

The BenchmarkDotNet project measures GBX.NET source generation and Gbx fixture reads and writes. It also includes the existing string-reader and array-copy benchmarks.

The separate [cross-library comparison](#cross-library-map-comparison) runs the three supplied maps through GBX.NET, gbx-py, gbx-ts, and ManiaPlanetSharp.

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

## Cross-library map comparison

[Comparison/run.mjs](Comparison/run.mjs) runs these original files without rewriting or recompressing them:

- `A01-Race.Challenge.Gbx`
- `Canyon A01.Map.Gbx`
- `Deep_Dip_2r1.Map.Gbx`

### Setup and run

You need the .NET 10 SDK, Node.js 20 or newer with npm, Python 3.10 or newer with a compatible `gbx-py` wheel, and Git. The measured run uses CPython 3.13. Setup uses an isolated Python environment and local Node modules, and it checks out the gbx-py parser and ManiaPlanetSharp at the revisions in [dependencies.json](Comparison/dependencies.json). It does not install the Python GUI dependencies or change system-wide packages.

Run from the repository root on Windows:

```powershell
./Benchmarks/Comparison/setup.ps1 -Python python
node Benchmarks/Comparison/run.mjs --update-readme
```

If your Python command is `py`, use `-Python py`. If necessary, supply an executable path. PowerShell 7 can also run setup on Linux or macOS. The equivalent setup with a POSIX shell is:

```sh
cd Benchmarks/Comparison
python3 -m venv .venv
.venv/bin/python -m pip install -r requirements.txt
npm ci --ignore-scripts --no-audit --no-fund
git clone --no-checkout https://github.com/schadocalex/gbx-py.git .deps/gbx-py
git -C .deps/gbx-py checkout --detach 342419ce4e937a4303b8a36fc2d3ed8093d95cf5
git clone --no-checkout https://github.com/stefan-baumann/ManiaPlanetSharp.git .deps/ManiaPlanetSharp
git -C .deps/ManiaPlanetSharp checkout --detach dfac322673d7a5260381d107a7ee34191da2a5fd
cd ../..
node Benchmarks/Comparison/run.mjs --update-readme
```

The runner builds standalone Release .NET workers, then starts a fresh process for each library/map pair, one at a time. A default run takes several minutes because the Python parse of Deep Dip 2 is much slower than the smaller cases. Nothing is installed automatically during measurement.

The ManiaPlanetSharp worker compiles the unchanged upstream sources through a local project that keeps its .NET 8 target, dependencies, resources, and parser flags, without the upstream Windows-only post-build copy. It uses the default runtime-generated parsers. The initial container parser compilation happens before timing. The upstream default body-parsing path regenerates chunk parsers during parse calls, and that work remains inside the timer. The optional precompiled parser assembly is not included.

The command writes raw samples, dependency/runtime versions, fixture SHA-256 hashes, parsing diagnostics, environment details, and Markdown into a timestamped directory under `Comparison/results`. Results are ignored by Git. `--update-readme` refreshes the compact performance table near the top of the [repository README](../README.md#benchmark) and the detailed results in this page, replacing only their marked sections. The compact table shows means in ms or s. The detailed table includes sample standard deviations and uses µs, ms, or s. Both tables retain partial-parse labels. Without that flag, both READMEs stay unchanged.

Both reports include a [QuickChart](https://quickchart.io/documentation/) image generated from the same mean times. Its time axis is logarithmic so the smaller cases remain visible. An asterisk marks partial parses, and failed cases are omitted. The chart URL contains the measured values and refreshes with each report.

For a smoke check rather than a performance measurement:

```powershell
node Benchmarks/Comparison/run.mjs --samples 2 --warmup-seconds 0.1 --sample-ms 10
```

Use `--map <path>` repeatedly to compare other maps, `--python <path>` to select another Python environment, or `--output <directory>` to choose the report directory. `--samples`, `--warmup-seconds`, and `--sample-ms` control measurements. `--timeout-seconds` limits each worker (default 600 s). `--no-build` is useful for repeated runs with the same binaries, but rebuild after library changes. `--help` lists all options. Errors are recorded rather than stopping the remaining cases, and any failed case makes the runner exit with code 1. Partial parses are reported but do not fail the run.

### What is measured

All four workers preload the same compressed file bytes. The timed operation creates a fresh parser/stream and calls its body-parsing API:

| Library | Operation | Source and decompressor |
| --- | --- | --- |
| GBX.NET | `Gbx.Parse(new MemoryStream(bytes), settings)` | Local project reference, strict body reads, GBX.NET.LZO. |
| gbx-py | `src.parser.parse_bytes(bytes)` | Pinned source checkout, Construct, native `gbx.lzo` from the pinned PyPI package. |
| gbx-ts | `await new GBX(bytes, 0).parse()` | Pinned npm package `gbx`, with `lzo-ts`. |
| [ManiaPlanetSharp](https://github.com/stefan-baumann/ManiaPlanetSharp) | `GameBoxFile.Parse(new MemoryStream(bytes))`, then `ParseBody()` | Pinned source checkout, runtime-generated parsers, upstream MiniLZO. |

The published `gbx-py` 0.3.1 wheel contains the LZO extension, not the parser. That is why setup also downloads the parser source. The `gbx-ts` project is published under the npm name `gbx`, and the tested version is `1.0.0-rc12`. [requirements.txt](Comparison/requirements.txt) and [package-lock.json](Comparison/package-lock.json) pin the external packages. The upstream parsers are not patched.

Startup, imports, disk reads, validation, and reporting are outside the timer. Body decompression, object construction, allocations, and normal garbage collection are inside it. The adapters discard library console diagnostics while measuring, and retain parsing checks from the initial validation call. No file writes, round-trips, embedded-item extraction, or external resource loading are requested. Each worker warms up before selecting an adaptive batch size, then records the configured number of batch averages. The table shows their mean and sample standard deviation, not a confidence interval. Small standard deviations do not remove differences between runtimes or parsing coverage. These are lightweight cross-runtime measurements, not BenchmarkDotNet results or allocation comparisons.

### Parsing limitations

The runner checks for a parsed block list, normalizes away `flags == -1` block placeholders, and compares block counts with GBX.NET. GBX.NET disables body exception suppression and skippable-chunk fallback, and reports unknown root map chunks. The Python adapter scans returned containers for unknown/failed chunks, trailing data, and a missing body end marker. The TypeScript adapter scans returned nodes for skipped chunks and captures parser errors. The ManiaPlanetSharp adapter records recognized body chunk IDs, unknown header chunks, missing blocks, and available console diagnostics. Its body parser scans bytes for recognized chunks and silently suppresses parsing failures in Release builds, so its timings are always marked partial. These checks identify known fallbacks, not every omission in a parser.

**Partial timings measure the library's available work, not an equivalent full parse.** In this run, gbx-py does not finish parsing the A01-Race body. The Python parser also retains unknown chunks in the newer maps. gbx-ts skips newer map chunks, including embedded item data, and does not offer the same nested-node coverage as GBX.NET. Neither timings nor matching block counts prove that every field was read. For that reason the table does not calculate speedup ratios. JSON reports retain grouped Python diagnostics and skipped TypeScript chunk IDs so you can inspect what each library actually processed.

### Measured results

<!-- gbx-comparison:start -->
Measured 2026-10-14 on AMD Ryzen 9 9900X 12-Core Processor (Windows_NT 10.0.26200 x64).

In-memory parse calls on the original compressed files, including LZO decompression. Mean ± sample standard deviation per parse, using µs, ms, or s as appropriate. Values are rounded to three significant digits. Lower is faster, but parsing coverage differs.

![Gbx benchmark mean parse times on a logarithmic axis. Asterisks mark partial parsing. Failed cases are omitted.](https://quickchart.io/chart?version=4&format=png&width=960&height=520&devicePixelRatio=2&backgroundColor=white&c=%7B%22type%22%3A%22bar%22%2C%22data%22%3A%7B%22labels%22%3A%5B%22A01-Race%22%2C%22Canyon+A01%22%2C%22Deep+Dip+2%22%5D%2C%22datasets%22%3A%5B%7B%22label%22%3A%22GBX.NET%22%2C%22backgroundColor%22%3A%22%2310b981%22%2C%22data%22%3A%5B0.026646321066666662%2C0.3494430210970464%2C159.2979333333333%5D%2C%22timings%22%3A%5B%220.0266+ms%22%2C%220.349+ms%22%2C%22159+ms%22%5D%7D%2C%7B%22label%22%3A%22gbx-py%22%2C%22backgroundColor%22%3A%22%23f59e0b%22%2C%22data%22%3A%5B11.938503703654364%2C50.95481066964567%2C12683.67350663369%5D%2C%22timings%22%3A%5B%2211.9+ms*%22%2C%2251+ms*%22%2C%2212.7+s*%22%5D%7D%2C%7B%22label%22%3A%22gbx-ts%22%2C%22backgroundColor%22%3A%22%233b82f6%22%2C%22data%22%3A%5B2.1429126945793207%2C26.455683330694836%2C701.4049333413442%5D%2C%22timings%22%3A%5B%222.14+ms%22%2C%2226.5+ms*%22%2C%22701+ms*%22%5D%7D%2C%7B%22label%22%3A%22ManiaPlanetSharp%22%2C%22backgroundColor%22%3A%22%238b5cf6%22%2C%22data%22%3A%5B14.560090740740739%2C24.85206933333334%2C2369.30252%5D%2C%22timings%22%3A%5B%2214.6+ms*%22%2C%2224.9+ms*%22%2C%222.37+s*%22%5D%7D%5D%7D%2C%22options%22%3A%7B%22indexAxis%22%3A%22y%22%2C%22font%22%3A%7B%22size%22%3A13%7D%2C%22layout%22%3A%7B%22padding%22%3A%7B%22right%22%3A95%7D%7D%2C%22scales%22%3A%7B%22x%22%3A%7B%22type%22%3A%22logarithmic%22%2C%22min%22%3A0.01%2C%22title%22%3A%7B%22display%22%3Atrue%2C%22text%22%3A%22Mean+parse+time+%28ms%2C+logarithmic+scale%29.+Lower+is+faster.%22%7D%2C%22ticks%22%3A%7B%22maxTicksLimit%22%3A8%7D%7D%2C%22y%22%3A%7B%22grid%22%3A%7B%22display%22%3Afalse%7D%2C%22ticks%22%3A%7B%22autoSkip%22%3Afalse%7D%7D%7D%2C%22plugins%22%3A%7B%22title%22%3A%7B%22display%22%3Atrue%2C%22text%22%3A%22Gbx+library+benchmarks+%282026-10-14%29%22%2C%22font%22%3A%7B%22size%22%3A20%7D%7D%2C%22subtitle%22%3A%7B%22display%22%3Atrue%2C%22text%22%3A%22Original+compressed+files%2C+including+LZO.+*+Partial+parsing.+Coverage+differs.%22%7D%2C%22legend%22%3A%7B%22position%22%3A%22bottom%22%7D%2C%22datalabels%22%3A%7B%22anchor%22%3A%22end%22%2C%22align%22%3A%22end%22%2C%22color%22%3A%22%2317212b%22%2C%22font%22%3A%7B%22size%22%3A12%7D%2C%22formatter%22%3Afunction%28value%2Ccontext%29%7Breturn+context.dataset.timings%5Bcontext.dataIndex%5D%3B%7D%7D%7D%7D%7D)

| Map | File bytes | GBX.NET | gbx-py | gbx-ts | ManiaPlanetSharp |
| --- | ---: | ---: | ---: | ---: | ---: |
| A01-Race.Challenge.Gbx | 18,977 | 26.6 ± 0.543 µs | 11.9 ± 0.626 ms (partial) | 2.14 ± 0.0898 ms | 14.6 ± 2.4 ms (partial) |
| Canyon A01.Map.Gbx | 408,180 | 349 ± 85.2 µs | 51 ± 3.81 ms (partial) | 26.5 ± 0.317 ms (partial) | 24.9 ± 2.62 ms (partial) |
| Deep_Dip_2r1.Map.Gbx | 5,998,704 | 159 ± 44.6 ms | 12.7 ± 0.567 s (partial) | 701 ± 24.4 ms (partial) | 2.37 ± 0.0414 s (partial) |

**Partial results are not equivalent full parses.** They include skipped chunks or a body that was not fully parsed. Failed calls have no timing. These numbers do not establish a like-for-like speedup.

GBX.NET 2.5.0.0 (.NET 10.0.12), gbx-py parser 342419ce4e93, LZO package 0.3.1 (CPython 3.13.2), gbx-ts 1.0.0-rc12 (Node.js v20.10.0), ManiaPlanetSharp source dfac322673d7, runtime-generated, upstream MiniLZO (.NET 10.0.12). GBX.NET.LZO 2.1.6.0, lzo-ts 0.2.3, Construct 2.10.70.

GBX.NET source: `03c8f91fefdff39da183e52348903272dc556ffb` with local library changes. gbx-py parser source: `342419ce4e937a4303b8a36fc2d3ed8093d95cf5`. ManiaPlanetSharp source: `dfac322673d7a5260381d107a7ee34191da2a5fd`.

15 samples per case after at least 2 s of warmup, with an adaptive batch targeting 200 ms (at least one parse per sample). Worker processes run sequentially. Startup, imports, file reads, validation, and report writing are excluded. Normal garbage collection remains enabled.

<!-- gbx-comparison:end -->
