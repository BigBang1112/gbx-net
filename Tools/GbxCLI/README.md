# GBX.NET.CLI

A command-line interface for inspecting Gbx and PAK files with GBX.NET. Available as the `gbx` .NET tool or a standalone executable.

## Usage

| Command | Description |
| --- | --- |
| `gbx parse <file>` | Show the header, body, references, and node properties. |
| `gbx eval <file> <code...>` | Evaluate C# against a Gbx or PAK object. |
| `gbx pak files <file> [--resolved \| --unresolved]` | List all PAK entries, or filter by name resolution. |
| `gbx pak verify <file>` | Fully parse matching Gbx entries and list files with exceptions. |
| `gbx pak extract <file> [-o <directory>]` | Extract every file or matching paths from a PAK. |
| `gbx decompress <file> -o <output>` | Write a Gbx with an uncompressed body. |
| `gbx compress <file> -o <output>` | Write a Gbx with a compressed body. |

```sh
gbx parse "My Map.Map.Gbx"
gbx eval "My Map.Map.Gbx" "Node.Chunks.Count"
gbx pak files "Resource.pak"
gbx pak files "Resource.pak" --resolved
gbx pak files "Resource.pak" --unresolved
gbx pak files "Resource.pak/Media/Texture/*.gbx"
gbx pak verify "Resource.pak"
gbx pak verify "Resource.pak/*.Shader.Gbx"
gbx pak extract "Resource.pak" -o "Extracted"
gbx pak extract "Resource.pak/Media/Texture/WhiteFlare1.Texture.gbx" -o "Extracted"
gbx pak extract "Resource.pak/Media/Texture/*.gbx" -o "Textures"
gbx parse "Resource.pak/Media/Texture/WhiteFlare1.Texture.gbx"
gbx eval "Resource.pak/WhiteFlare1.Texture.gbx" "Node"
```

## Command options

Use `gbx --help` for the command overview, `gbx pak --help` for PAK subcommands, and `gbx <command> --help` for a command's usage, options, and examples. For example, `gbx pak files --help` shows the resolution filters, while `gbx pak extract --help` shows the output directory option.

`-h, --help` and `-v, --version` are available for every command. Other options belong to individual commands:

| Command | Options | Behavior |
| --- | --- | --- |
| `parse` | `-i, --interactive` | Interactive placeholder. |
| `eval` | `-i, --interactive` | Interactive placeholder. Without it, at least one C# expression is required. |
| `pak files` | `--resolved` or `--unresolved` | Filter named/resolved files or unresolved hashes. Omit both to list all entries. |
| `pak verify` | No additional options | Verify all entries or append an entry pattern. |
| `pak extract` | `-o, --output <directory>` | Optional destination. Defaults to `<archive>.extracted`. |
| `compress`, `decompress` | `-o, --output <file>` | Required destination file. |

Interactive mode is not implemented yet. Output paths never overwrite existing files.

## PAK files

Gbx and PAK formats are detected from their contents. PAK keys are selected automatically by archive filename. Private file indexes need a matching built-in key. `.Pack.Gbx` archives are supported too.

The CLI compresses [Resources/FileHashes.txt](../../Resources/FileHashes.txt) with gzip during the build and embeds the compressed catalogue. It decompresses the catalogue when hash lookup is first needed. Known hashed filenames are resolved automatically for listing, verification messages, and extraction. Stored folders are preserved, and unknown or unresolved hashes keep their stored names. No separate hash file is needed beside the executable.

`pak files` lists every matching entry by default, including unresolved hashes. `--resolved` lists files that already have names and hashes with known names. `--unresolved` lists only hashes without known names. The options can appear before or after the archive path and cannot be combined. They also work with entry patterns. A filter with no matching entries prints nothing and succeeds.

Append a resolved path, stored path, hash, filename, or quoted wildcard pattern to an archive to select entries. Both slash styles work inside the archive, and matching ignores case. `pak files`, `pak verify`, and `pak extract` accept multiple matches. `parse` and `eval` require exactly one match and report ambiguous selections. Exact stored paths take precedence over resolved aliases, followed by paths relative to any parent folder. For exact names outside the catalogue, the CLI also computes the PAK filename hash, including paths relative to a stored folder.

Wildcard matching supports `*` (any number of characters, including folder separators) and `?` (one character). Patterns can match full paths or paths relative to any parent folder. In `eval` on an archive, `Files` still contains the original stored keys.

`pak verify` detects Gbx entries by filename or signature, including hashed names. It continues after exceptions, prints failing paths and messages, and exits with code `1` if any files failed. Unknown root classes are reported as failures because their bodies cannot be parsed.

`pak extract` preserves stored folder paths, restores known filenames, and extracts file bytes without parsing Gbx contents. Without `-o`, it writes beside the archive to `<archive>.extracted`, for example `Resource.pak.extracted`. No matches, existing destination files, or entries that resolve to the same destination produce an error. Existing files are never overwritten.

## Evaluation

In `eval`, expressions use members of the root `Gbx` or `Pak` object. Pass several quoted expressions to run them in order. Output previews are limited, so narrow queries when inspecting large collections. C# runs with the CLI's permissions, so only evaluate trusted code.

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
