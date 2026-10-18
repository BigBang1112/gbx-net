# GBX.NET.CLI

GBX.NET.CLI is a command-line interface for GBX.NET, available as the `gbx` command through a .NET tool or a standalone executable. The NuGet package is named `GBX.NET.CLI`.

Running `gbx` without arguments shows help and waits at "Press any key to continue..." when terminal input and output are available. Redirected input or output and environments without key input skip the pause. Explicit `gbx --help` always exits without waiting.

Use `gbx parse "My Map.Map.Gbx"` for a noninteractive summary. The CLI checks for a file before reporting an unknown command or option and suggesting help. Explicit commands and global help/version options retain their usual meaning.

`parse` detects the format from the file signature (`GBX` or `NadeoPak`), not the extension. Renamed files and files without an extension work too. Unsupported formats produce an error without trying a parser based on the filename.

For Gbx files, `parse` reads the full node and prints sections for the header, body, external references, and node properties. The summary includes class and chunk IDs, versions, compression sizes, and nested values. Unset properties are counted instead of listed individually. Collections, nesting, long strings, and binary data have preview limits, with `gbx eval` available for further inspection. Stored node references show external paths without resolving them. Computed aliases that might load references are left for `eval`.

## C# evaluation

Use `eval` to execute C# against a file. It parses the full Gbx, or the PAK metadata and file index, and evaluates against the root `Gbx` or `Pak` object. Expressions and statements are positional arguments after the file path. Supply several quoted arguments to run them in order in one session, including variable declarations. Standard .NET, LINQ, GBX.NET, and the current object's namespace are imported. Async expressions such as `await OpenGbxFileAsync(Files.Values.First())` work too, subject to the archive's encryption and file format support.

```sh
gbx eval "My Map.Map.Gbx" "Node.Chunks.Count"
gbx eval "GameData.pak" "Files.Keys.Take(10).ToArray()"
gbx eval "GameData.pak" "var files = Files.Values.Where(file => file.IsCompressed).ToArray();" "files.Length"
```

Unlike `parse`, `eval` prints only evaluation results, without a file summary or expression echo. Each response starts with `Result:` and uses the same indented properties, chunk details, and binary previews as `parse`. Previews show up to 10 items per collection (100 chunks), 4 nesting levels, 240 characters per value, and 1000 detail lines per response. Use a narrower query for larger results. A null result, including a statement without a return value, prints `Result: (null)`. Compilation or runtime errors stop noninteractive evaluation, go to standard error, and return exit code `1`. The old `parse -e` and `parse --eval` options are no longer supported.

`--interactive` and `-i` are reserved for a future C# REPL. They currently print the interactive placeholder without evaluating any expressions.

Only evaluate trusted C# code. Evaluation is not sandboxed and has the same permissions as the CLI, including filesystem access. Scripts can change objects, call methods, and write files explicitly.

## .NET tool

Pack from the repository root, then install from the local package directory:

```sh
dotnet pack Tools/GbxCLI/GbxCLI.csproj -c Release -o artifacts/packages
dotnet tool install --global GBX.NET.CLI --version 0.1.0 --add-source artifacts/packages
gbx --help
```

The tool requires the .NET 8 runtime. Once the package is published to a configured NuGet feed, it can be installed with `dotnet tool install --global GBX.NET.CLI`.

For a repository-local installation instead:

```sh
dotnet new tool-manifest
dotnet tool install GBX.NET.CLI --version 0.1.0 --add-source artifacts/packages
dotnet tool run gbx --help
```

## Standalone executable

Publish a self-contained single-file executable from the repository root:

```sh
dotnet publish Tools/GbxCLI/GbxCLI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PackAsTool=false -o artifacts/gbx/win-x64
```

The output contains `gbx.exe`, which runs without an installed .NET runtime. Add its directory to `PATH` to invoke `gbx` from any directory. Replace `win-x64` with another supported runtime identifier, such as `linux-x64` or `osx-arm64`, to produce a `gbx` executable for that platform.

Single-file builds extract their bundled assemblies at startup so the C# scripting compiler can read assembly metadata. The project enables `IncludeAllContentForSelfExtract` automatically when `PublishSingleFile` is enabled.

For local development:

```sh
dotnet run --project Tools/GbxCLI -- parse "My Map.Map.Gbx"
```
