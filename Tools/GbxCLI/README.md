# GBX.NET.CLI

A command-line interface for inspecting Gbx and PAK files with GBX.NET. Available as the `gbx` .NET tool or a standalone executable.

## Usage

| Command | Description |
| --- | --- |
| `gbx parse <file>` | Show the header, body, references, and node properties. |
| `gbx eval <file> <code...>` | Evaluate C# against a Gbx or PAK object. |
| `gbx script <file.csx> [-- args...]` | Run a C# script with GBX.NET available. |
| `gbx pak files <file> [--resolved \| --unresolved]` | List all PAK or ZIP entries, or filter by name resolution. |
| `gbx verify <file>` | Fully parse a Gbx file, or all Gbx files in a folder and its subfolders. |
| `gbx pak verify <file>` | Fully parse matching Gbx entries of an archive, or of all archives in a folder. |
| `gbx pak extract <file> [-o <directory>]` | Extract every file or matching paths from a PAK or ZIP. |
| `gbx decompress <file> -o <output>` | Write a Gbx with an uncompressed body. |
| `gbx compress <file> -o <output>` | Write a Gbx with a compressed body. |

```sh
gbx parse "My Map.Map.Gbx"
gbx eval "My Map.Map.Gbx" "Node.Chunks.Count"
gbx script inspect.csx -- "My Map.Map.Gbx"
gbx pak files "Resource.pak"
gbx pak files "Resource.pak" --resolved
gbx pak files "Resource.pak" --unresolved
gbx pak files "Resource.pak/Media/Texture/*.gbx"
gbx pak verify "Resource.pak"
gbx pak verify "Resource.pak/*.Shader.Gbx"
gbx pak verify "Packs"
gbx verify "My Map.Map.Gbx"
gbx verify "Maps"
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
| `script` | `--` | Pass remaining arguments to the script, including `--help` and `--version`. |
| `pak files` | `--resolved` or `--unresolved` | Filter named/resolved files or unresolved hashes. Omit both to list all entries. |
| `verify` | `-g, --game <version>` | Verify a Gbx file or a folder tree. Optionally select the game version. |
| `pak verify` | `-g, --game <version>` | Verify all entries, append an entry pattern, or pass a folder of archives. Optionally select the game version. |
| `pak extract` | `-o, --output <directory>` | Optional destination. Defaults to `<archive>.extracted`. |
| `compress`, `decompress` | `-o, --output <file>` | Required destination file. |

Interactive mode is not implemented yet. Built-in output commands never overwrite existing files. Scripts control their own file writes.

## Verification

`verify` fully parses a Gbx file. With a folder, it parses every Gbx file in the folder and its subfolders, detected by `GBX` signature or `.gbx` extension. Other files, including `.Pack.Gbx` archives, are skipped. It continues after exceptions, prints failing paths relative to the folder with their messages, and ends with the number of parsed, failed, and skipped files. The exit code is `1` if any file failed. Unknown root classes are reported as failures.

Some games reuse a class ID for a different class. Pass `--game` with a `GameVersion` name, such as `TMSX` or `TMNESWC`, to read those classes as that game does. Names ignore case. Numbers and combined values are rejected. `pak verify` accepts the same option. Without it, version 3 PAK archives are read as `TMF`, and other files use the default classes.

```sh
gbx verify "GameData" --game TMSX
```

## PAK files

Gbx and PAK formats are detected from their contents. PAK keys are selected automatically by archive filename. Private file indexes need a matching built-in key. `.Pack.Gbx` archives are supported too.

The CLI compresses [Resources/FileHashes.txt](../../Resources/FileHashes.txt) with gzip during the build and embeds the compressed catalogue. It decompresses the catalogue when hash lookup is first needed. Known hashed filenames are resolved automatically for listing, verification messages, and extraction. Stored folders are preserved, and unknown or unresolved hashes keep their stored names. No separate hash file is needed beside the executable.

`pak files` lists every matching entry by default, including unresolved hashes. `--resolved` lists files that already have names and hashes with known names. `--unresolved` lists only hashes without known names. The options can appear before or after the archive path and cannot be combined. They also work with entry patterns. A filter with no matching entries prints nothing and succeeds.

Append a resolved path, stored path, hash, filename, or quoted wildcard pattern to an archive to select entries. Both slash styles work inside the archive, and matching ignores case. `pak files`, `pak verify`, and `pak extract` accept multiple matches. `parse` and `eval` require exactly one match and report ambiguous selections. Exact stored paths take precedence over resolved aliases, followed by paths relative to any parent folder. For exact names outside the catalogue, the CLI also computes the PAK filename hash, including paths relative to a stored folder.

Wildcard matching supports `*` (any number of characters, including folder separators) and `?` (one character). Patterns can match full paths or paths relative to any parent folder. In `eval` on an archive, `Files` still contains the original stored keys.

`pak verify` detects Gbx entries by filename or signature, including hashed names. It continues after exceptions, prints failing paths and messages, and exits with code `1` if any files failed. Unknown root classes are reported as failures because their bodies cannot be parsed. Newer title packs list entries without stored data. These entries are skipped and counted separately in the summary.

Pass a folder to `pak verify` to verify every PAK and ZIP archive directly inside it, such as a game's `Packs` folder. Subfolders are not searched. Archives are detected from their contents. Failing entries are prefixed with the archive name, and a summary line follows each archive. Archives that cannot be opened, for example without a matching key, are reported and the run continues. The last line totals all archives.

`pak extract` preserves stored folder paths, restores known filenames, and extracts file bytes without parsing Gbx contents. Without `-o`, it writes beside the archive to `<archive>.extracted`, for example `Resource.pak.extracted`. No matches, existing destination files, or entries that resolve to the same destination produce an error. Existing files are never overwritten.

## ZIP files

`pak files`, `pak verify`, and `pak extract` also accept ZIP archives, such as the ZIP files in game `Packs` folders. ZIP archives are detected from their contents. Entry patterns work as they do for PAK files. ZIP entries always have names, so `--unresolved` lists nothing for them. Folder entries are skipped, and `pak extract` rejects entries that would be written outside the destination.

## Evaluation

In `eval`, expressions use members of the root `Gbx` or `Pak` object. Pass several quoted expressions to run them in order. Output previews are limited, so narrow queries when inspecting large collections. C# runs with the CLI's permissions, so only evaluate trusted code.

## Scripts

`gbx script` runs a C# script without opening an input file automatically. For example, save this as `inspect.csx`:

```cs
var gbx = await Gbx.ParseAsync(Args[0]);
Print($"{Path.GetFileName(Args[0])}: {gbx.Node?.GetType().Name}");
Print(gbx.Node?.Chunks.Count);
```

Run it with:

```sh
gbx script inspect.csx -- "My Map.Map.Gbx"
```

`Args` contains the arguments after the script path. `Print(value)` and `Output` write to standard output, and `Console.WriteLine` also works. Scripts support top-level `await`, relative `#load` and `#r` paths, and the CLI's GBX.NET, PAK, compression, cryptography, hashing, and TmEssentials references. Common system namespaces, `System.IO`, `GBX.NET`, `GBX.NET.PAK`, `GBX.NET.Engines.Game`, and `GBX.NET.Engines.MwFoundations` are imported.

File operations use the current working directory. `#load` and `#r` paths resolve relative to the script. Return values are ignored. Compilation and runtime errors go to standard error and exit with code `1`. Scripts run with the CLI's permissions, so only run trusted code.

## Build and install

Requires .NET 10. From the repository root:

```sh
dotnet run --project Tools/GbxCLI -- parse "My Map.Map.Gbx"
dotnet pack Tools/GbxCLI/GbxCLI.csproj -c Release -o artifacts/packages
dotnet tool install --global GBX.NET.CLI --version 0.1.0 --add-source artifacts/packages
```

Publishing produces a compressed, trimmed, self-contained single executable for the current platform by default:

```sh
dotnet publish Tools/GbxCLI/GbxCLI.csproj -o artifacts/gbx
```

To target Windows explicitly:

```sh
dotnet publish Tools/GbxCLI/GbxCLI.csproj -c Release -r win-x64 --self-contained true -o artifacts/gbx/win-x64
```

Partial trimming preserves the CLI, Gbx models, and core script references for reflection and C# evaluation. Debug symbols are removed from trimmed publishes.

Use `-p:PublishSingleFile=false` to publish separate files instead, `-p:PublishTrimmed=false` to disable trimming, or `-p:EnableCompressionInSingleFile=false` to disable bundle compression. `dotnet pack` continues to produce an untrimmed .NET tool package.
