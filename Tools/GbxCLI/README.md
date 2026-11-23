# GBX.NET.CLI

`gbx` is a command-line tool for inspecting and transforming Gbx files with GBX.NET. It can summarize and browse a Gbx, change its members and save it, verify whole folders and game archives, list and extract PAK and ZIP archives, and compress or decompress Gbx bodies.

## Installation

The CLI ships in two flavors with the same commands.

| Distribution | How to get it |
| --- | --- |
| .NET tool | `dotnet tool install -g GBX.NET.CLI` |
| Standalone | Download `gbx.exe` from the releases |

The standalone executable is a trimmed, compressed single file that needs no .NET runtime. `gbx --version` shows which flavor you have.

## Usage

```
gbx <command> [arguments] [options]
gbx <path>
```

| Command | Arguments | Purpose |
| --- | --- | --- |
| `inspect` (alias `parse`) | `<path> [-i]` | Fully parse a Gbx and write a summary, or summarize a PAK or ZIP. |
| `eval` | `<path> <code>... [-i \| -w \| -o <file>]` | Evaluate C# expressions against the opened file, and optionally write the Gbx. |
| `set` | `<path> <member>=<value>... [-o <file>] [-n]` | Change members of a Gbx and write it. |
| `script` | `<script.csx> [args...]` | Run a C# script file with GBX.NET available. |
| `verify` (alias `pack verify`) | `<path> [-g <game>] [--roundtrip] [--strict]` | Fully parse every Gbx in a file, folder or archive. |
| `pack list` | `<archive>[/pattern] [--resolved \| --unresolved]` | List archive entries, resolving known hashes. |
| `pack extract` | `<archive>[/pattern] [-o <dir>] [--overwrite \| --skip-existing]` | Extract all or matching entries. |
| `compress` | `<file> [-o <file>] [--no-backup]` | Compress a Gbx body. |
| `decompress` | `<file> [-o <file>] [--no-backup]` | Decompress a Gbx body. |

`gbx --help` lists the commands, and `gbx <command> --help` describes one of them.

### Exit codes

A command that fails exits with `1` and writes `gbx: <message>` to standard error. Anything else exits with `0`. A file that fails verification is a result, not an error, so `verify` exits with `0` unless you pass `--strict`.

Ctrl+C stops a long command, such as `verify`, between files. A second Ctrl+C ends the process immediately.

Output doesn't depend on the system language. Numbers use `,` to group thousands and `.` for decimals, such as `494,112 bytes` and `79.67 %`, and C# compiler messages are in English.

## Paths inside archives

Commands that open a file accept a path that continues inside an archive. The longest leading part of the path that is an existing file is opened, and the rest is a path inside it.

```
gbx inspect "Packs/Stadium.pak/Media/Texture/Grass.Texture.Gbx"
gbx inspect "skin.zip/Model.Pack.Gbx/good.gbx"
```

The file type comes from its signature, not its extension. A ZIP entry can itself be a PAK or ZIP archive, at any depth. PAK entries are not opened as nested archives.

Entry paths follow the same rules in PAK and ZIP archives. Both slash styles work, and matching ignores case.

- A pattern with `*` or `?` is matched against the whole entry path and against every part of it that starts after a separator. `*` also matches separators, so `Media/*` selects a whole folder.
- A path without wildcards selects the entry with exactly that path. If there is none, it selects the entries whose path ends with it, such as a filename.
- In a PAK, an exact stored path wins over a resolved name. A name outside the hash catalogue is also matched by its PAK filename hash.

`inspect` and `eval` need exactly one entry, so they report an ambiguous path. `verify`, `pack list` and `pack extract` accept many matches, but a pattern that matches nothing is an error.

Private PAK files are decrypted with built-in keys, looked up by filename (also without a `_yyyy-MM-dd` suffix) and by the title ID in the public header. Without a key, only the public header is available.

## Browsing a file

`gbx <path>` opens an interactive browser, which is also what happens when you drag a file onto `gbx.exe`. Running `gbx` without arguments asks for a path, so you can drag a file into the console window. `inspect -i` opens the browser too.

```
Node (CGameCtnChallenge)
  [1] MapName: My Map
  [2] Author: bigbang1112
  [3] Blocks: 512 items
  ...
CGameCtnChallenge> 3
```

- A number opens that member. `..` goes back, `/` goes to the root, and `q` quits.
- Long lists are shown in pages of 20 items. `n` and `p` move between pages.
- In a PAK or ZIP, the root lists the archive entries. `f *.Map.Gbx` filters them with the usual patterns, and `f` alone clears the filter. Opening an entry parses it and continues in the same browser.
- Node references to other files are shown, but never loaded.

The browser needs a console for both input and output. With redirected input or output, `gbx <path>` and `inspect -i` write the `inspect` summary instead, so scripts and pipes keep working.

## Commands

### inspect

Writes the file size and a summary of the header, body, external references and node. The node tree is limited to 10 items per collection (100 chunks), 4 nesting levels, 240 characters per value and 1000 lines. Browse the file or use `eval` to see more.

For a PAK, `inspect` shows the version, header encryption and file count. For a ZIP, it shows the number of file entries.

### eval

The first argument that is not `-i` is the path. Every further argument is one C# expression, evaluated in order in one session, so later expressions can use variables from earlier ones. The opened `Gbx`, `Pak` or `ZipArchive` is the script global, so its members can be used directly.

```
gbx eval "My.Map.Gbx" "Node.MapName" "Node.Author"
```

With `-i`, a C# prompt starts after the expressions. Errors are shown, and the prompt continues until you enter `q`. Enter `save` to write the Gbx back to its file, or `save <path>` to write it to another file. `save --force` and `save --no-backup` work like the options below.

To keep the changes from the expressions, pass `-w` (`--write`) to write the Gbx back to its file, or `-o <file>` to write it to another file.

```
gbx eval "My.Map.Gbx" "Node.MapName = \"Night run\"" "Node.AuthorTime = null" -w
```

- `--no-backup` doesn't keep an existing output as `<output>.bak`.
- `--force` writes even when the check below finds a difference, and shows it as a warning.

### Writing a Gbx

`eval`, `set`, the C# prompt and `SaveAsync` in scripts write a Gbx the same way.

1. The Gbx is saved into memory.
2. The saved data is parsed again, and its properties are compared with the changed Gbx, like `verify --roundtrip` does.
3. If anything differs, nothing is written and the command fails with the first difference. A value that the file format can't store is caught this way, such as a block coordinate of 300 in a TMF map, which stores it in one byte.
4. The data is written to a temporary file, which then replaces the output. An existing output is kept as `<output>.bak`.

A Gbx inside a PAK or ZIP archive is only written with `-o`, because archives are never changed. A Gbx whose root class is unknown can't be written, because its body wasn't parsed. Some classes, such as replays, can't be written by GBX.NET at all, and the command fails with that error.

### set

Assigns values to members of a Gbx, then writes it like `eval -w`. Without `-o`, the input file is replaced. Each change is shown with its old and new value.

```
gbx set "My.Map.Gbx" "MapName=Night run" "Decoration=48x48Night,Stadium2020,Nadeo" "Blocks[0].Coord.Y=20"
```

```
Node.MapName: My map -> Night run
Node.Decoration: ("48x48Day", "Stadium2020", "Nadeo") -> ("48x48Night", "Stadium2020", "Nadeo")
Node.Blocks[0].Coord.Y: 9 -> 20
Written: My.Map.Gbx
Backup: My.Map.Gbx.bak
```

A member path starts at the opened `Gbx`, such as `Node.MapName` or `Node.Blocks[3].Coord.X`. A first name that the `Gbx` doesn't have is looked up on its node, so `MapName` means `Node.MapName`. Names ignore case when only one member matches. A member of a struct or a record, such as `Coord.Y` or `Decoration.Author`, is changed on a copy that is assigned back, so other places that share the record don't change.

The value is converted to the type of the member.

| Type | Value |
| --- | --- |
| String | The text as it is, also `null`. |
| Enum | A name, ignoring case, such as `west`. Flags also take numbers and names separated by commas. |
| Number | Invariant culture, such as `1.5`. Integers also take hexadecimal, such as `0x03043000`. |
| Time | `1:23.456`, or any other text that the type's `Parse` method accepts. |
| `Id` | A number makes a collection number, such as `26`. Other text makes a string `Id`. |
| Vector, `Ident` and other types | Constructor arguments separated by commas, such as `1,2,3` or `48x48Night,Stadium2020,Nadeo`. |
| Nullable value or object | `null` clears it. |

Collections, nodes and byte arrays can't be set from text. Use `eval` for those.

- `-o` (`--output`) writes to another file and keeps the input.
- `-n` (`--dry-run`) shows the changes and runs the check, but writes nothing. It also works inside an archive without `-o`.
- `--no-backup` and `--force` work as in `eval`.

### script

Runs a C# script file. Arguments after the script path are passed to the script as `Args`, also when they start with `-`. Use `--` before the script path only when the path itself starts with `-`. Scripts can call `Print(value)` and write to `Output`.

`SaveAsync(gbx, path)` writes a Gbx with the same check and backup as `eval -w`. It also takes `backup: false` and `force: true`.

```cs
var gbx = Gbx.Parse<CGameCtnChallenge>(Args[0]);
gbx.Node.MapName = "Night run";
await SaveAsync(gbx, Args[0]);
```

### verify

Parses every Gbx fully and continues after exceptions. A folder is walked with all subfolders in name order, and archives are opened to verify each entry. Files without a Gbx, PAK or ZIP signature are skipped and counted.

```
Packs/Stadium.pak/Media/Broken.Gbx: ChunkReadException: ...
Parsed 1532 Gbx files; 1 failed; 220 non-Gbx files skipped.
```

A Gbx whose root class is unknown counts as a failure, because its body was not parsed.

- `-g` (`--game`) sets the game version for games that reuse a class ID for a different class. It takes the name of exactly one game, such as `TMF` or `MP4`.
- `--roundtrip` saves each parsed Gbx in memory, parses it again, and compares the node and chunk properties recursively. It reports the first changed property path. Stored external references are compared by descriptor. Compression, signatures, and other file encoding details do not affect the result.
- `--strict` exits with `1` after the summary when any file failed.

### pack list

Writes one entry path per line, as plain text for scripts. `--resolved` keeps the entries with known names, and `--unresolved` keeps the entries that are still hashes. ZIP entries always have names.

### pack extract

Extracts the selected entries into the output folder, keeping their folder structure. Without `-o`, the folder is created in the working directory and named after the title ID of a title pack, such as `TMStadium@nadeo`. Other archives use their name without the whole extension, so `Model.Pack.Gbx` extracts to `Model`.

Every destination is checked before anything is written. Extraction fails without writing any file when an entry path is rooted or contains `..`, when two entries would be written to the same file, or when a destination already exists.

- `--overwrite` replaces existing files.
- `--skip-existing` keeps existing files and extracts only the missing ones, so a repeated extraction continues where the last one failed.

A PAK entry that is a Gbx is saved again through GBX.NET. Other entries are written exactly as stored.

An entry that cannot be read is reported as `<entry>: <error>`, and the other entries are still extracted. The summary counts the failed entries, and the command then exits with `1`. Each entry is written to a temporary file first, so a failed entry leaves no partial file and doesn't replace an existing one.

### compress and decompress

The result is written to a temporary file beside the output, then moved over it. Without `-o`, the input file is replaced. An existing output is kept as `<output>.bak`, unless you pass `--no-backup`.

## Colors

Output to a console uses colors. They are off when the output is redirected and when the `NO_COLOR` environment variable is set. Text that already uses Trackmania formatting, such as a map name, shows in its in-game colors. In plain output, it keeps its formatting codes.

## Build

```
dotnet build Tools/GbxCLI
dotnet pack Tools/GbxCLI
dotnet publish Tools/GbxCLI -r win-x64
```

A pack creates the .NET tool, and a publish creates the standalone executable. The publish is trimmed partially, with whole assemblies rooted, because inspection and C# scripts reach members that static analysis cannot see.
