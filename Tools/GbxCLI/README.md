# GBX.NET.CLI

A command-line interface for inspecting Gbx and PAK files with GBX.NET. Available as the `gbx` .NET tool or a standalone executable.

## Usage

| Command | Description |
| --- | --- |
| `gbx parse <file>` | Show the header, body, references, and node properties. |
| `gbx eval <file> <code...>` | Evaluate C# against a Gbx or PAK object. |
| `gbx pak files <file>` | List every file path in a PAK, one per line. |
| `gbx pak verify <file>` | Fully parse Gbx entries and list files with exceptions. |
| `gbx decompress <file> -o <output>` | Write a Gbx with an uncompressed body. |
| `gbx compress <file> -o <output>` | Write a Gbx with a compressed body. |

```sh
gbx parse "My Map.Map.Gbx"
gbx eval "My Map.Map.Gbx" "Node.Chunks.Count"
gbx pak files "Resource.pak"
gbx pak verify "Resource.pak"
gbx parse "Resource.pak/Media/Texture/WhiteFlare1.Texture.gbx"
```

Gbx and PAK formats are detected from their contents. PAK keys are selected automatically by archive filename. Private file indexes need a matching built-in key. `.Pack.Gbx` archives are supported too.

Append a stored file path to an archive to use it with `parse` or `eval`. Both slash styles work inside the archive, and entry lookup ignores case.

`pak verify` detects Gbx entries by filename or signature, including hashed names. It continues after exceptions, prints failing paths and messages, and exits with code `1` if any files failed.

In `eval`, expressions use members of the root `Gbx` or `Pak` object. Pass several quoted expressions to run them in order. Output previews are limited, so narrow queries when inspecting large collections. C# runs with the CLI's permissions, so only evaluate trusted code.

Use `gbx --help` for options.

## Build and install

Requires .NET 10. From the repository root:

```sh
dotnet run --project Tools/GbxCLI -- parse "My Map.Map.Gbx"
dotnet pack Tools/GbxCLI/GbxCLI.csproj -c Release -o artifacts/packages
dotnet tool install --global GBX.NET.CLI --version 0.1.0 --add-source artifacts/packages
```

To publish a standalone Windows executable:

```sh
dotnet publish Tools/GbxCLI/GbxCLI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PackAsTool=false -o artifacts/gbx/win-x64
```
