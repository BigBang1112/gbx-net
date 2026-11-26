# GBX.NET.CLI

`gbx` is a command-line tool for inspecting and transforming Gbx files with GBX.NET. It can summarize, browse and edit a Gbx, change members of many files at once, compare two Gbx files, verify whole folders and game archives, list and extract PAK and ZIP archives, and compress or decompress Gbx bodies.

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
| `eval` | `<path> <code>... [-i \| -w \| -o <path> \| -n]` | Evaluate C# expressions against one or many files, and optionally write the Gbx files. |
| `set` | `<path> <member>=<value>... [-o <path>] [-n]` | Change members of one or many Gbx files and write them. |
| `script` | `<script.csx> [args...]` | Run a C# script file with GBX.NET available. |
| `diff` | `<path> <path> [--max <n>] [--strict]` | List the properties that differ between two Gbx files. |
| `verify` (alias `pack verify`) | `<path> [-g <game>] [--roundtrip] [--strict]` | Fully parse every Gbx in a file, folder or archive. |
| `pack list` | `<archive>[/pattern] [--resolved \| --unresolved]` | List archive entries, resolving known hashes. |
| `pack extract` | `<archive>[/pattern] [-o <dir>] [--overwrite \| --skip-existing]` | Extract all or matching entries. |
| `compress` | `<file> [-o <file>] [--no-backup]` | Compress a Gbx body. |
| `decompress` | `<file> [-o <file>] [--no-backup]` | Decompress a Gbx body. |

`gbx --help` lists the commands, and `gbx <command> --help` describes one of them.

### Exit codes

A command that fails exits with `1` and writes `gbx: <message>` to standard error. Anything else exits with `0`. A file that fails verification is a result, not an error, so `verify` exits with `0` unless you pass `--strict`. `eval` and `set` with [many files](#many-files) exit with `1` after the summary when any file failed.

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

`inspect` needs exactly one entry, and so do `eval` and `set` with a path without wildcards, so they report an ambiguous path. `verify`, `pack list` and `pack extract` accept many matches, and so do `eval` and `set` with a pattern, but a pattern that matches nothing is an error.

Private PAK files are decrypted with built-in keys, looked up by filename (also without a `_yyyy-MM-dd` suffix) and by the title ID in the public header. Without a key, only the public header is available.

## Many files

`eval` and `set` run on many Gbx files when the path is a folder or has a `*` or `?`. Any other path opens one file, as described above. Quote a pattern, so the shell passes it as it is.

```
gbx set "Maps/*.Map.Gbx" AuthorLogin=bigbang1112 --dry-run
```

```
Night.Map.Gbx: would change (Node.AuthorLogin: someone -> bigbang1112)
Old/Day.Map.Gbx: unchanged
Old/Broken.Map.Gbx: ChunkReadException: ...
1 Gbx files would change; 1 unchanged; 1 failed; 0 non-Gbx files skipped. Dry run, so nothing was written.
```

- A folder selects the files in it and all its subfolders.
- A pattern on disk starts from the folder before its first wildcard, and matches like an entry pattern, so `*` also matches subfolders. `Maps/*.Map.Gbx` selects the maps in `Maps` and below it.
- On disk, only files whose name ends with `.Gbx` are selected, so backups and other files are left out. A selected file without a Gbx signature is skipped and counted.
- A pattern inside an archive, such as `Packs/Stadium.pak/Maps/*`, selects the matching entries. Entries that aren't a Gbx are skipped and counted.

Each file gets one line with its result, and a summary follows. A file that fails is reported, and the others continue. Ctrl+C stops between files.

When writing, only files that changed are written. `set` compares the values it assigns, and `eval` compares the properties with a second, unchanged copy of each file. A file that GBX.NET can't save, such as a replay, only fails if it changed. Each changed file is checked and written like [one Gbx](#writing-a-gbx), and the dry run (`-n`) runs the check without writing.

With many files, `-o` is a folder. Each changed file is written there at its path under the selected folder, or at its entry path in an archive. Gbx files inside an archive are only written with `-o`.

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

### Editing in the browser

`set <number> <value>` changes a value of the current object, such as `set 1 My map`. The value is converted like in [set](#set), and it can be in double quotes, so `set 1 ""` sets an empty string. A list or another object with values of its own is opened first, and its values are set there.

After a change, the prompt shows a `*`, such as `CGameCtnChallenge*>`. `save` writes the Gbx back to its file, and `save as <path>` writes it to another file, which later saves use too. Both check and back up the file like [writing a Gbx](#writing-a-gbx), and `save --force` or `save as --force <path>` writes despite the check. A Gbx opened from a PAK or ZIP archive can only be saved with `save as`.

`q`, and `..` or `/` when they close a changed file, ask before the changes are lost. Enter `y` to discard them.

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

To keep the changes from the expressions, pass `-w` (`--write`) to write the Gbx back to its file, or `-o <file>` to write it to another file. `-n` (`--dry-run`) runs the check below, but writes nothing.

```
gbx eval "My.Map.Gbx" "Node.MapName = \"Night run\"" "Node.AuthorTime = null" -w
```

- `--no-backup` doesn't keep an existing output as `<output>.bak`.
- `--force` writes even when the check below finds a difference, and shows it as a warning.

With [many files](#many-files), each file shows the values of its expressions on one line, separated by `|`. A statement that ends with `;` shows nothing, like in the C# prompt. With `-w`, `-o` or `-n`, each file shows whether it changed instead. The expressions are compiled once for each node type, so a folder with many maps doesn't compile them again for each file. `-i` only opens one file.

```
gbx eval "Maps" "Node.MapName" "Node.AuthorTime"
```

### Writing a Gbx

`eval`, `set`, the C# prompt and `SaveAsync` in scripts write a Gbx the same way.

1. The Gbx is saved into memory.
2. The saved data is parsed again, and its properties are compared with the changed Gbx, like `verify --roundtrip` does.
3. If anything differs, nothing is written and the command fails with the first difference. A value that the file format can't store is caught this way, such as a block coordinate of 300 in a TMF map, which stores it in one byte.
4. The data is written to a temporary file, which then replaces the output. An existing output is kept as `<output>.bak`.

A Gbx inside a PAK or ZIP archive is only written with `-o`, because archives are never changed. A Gbx whose root class is unknown can't be written, because its body wasn't parsed, and neither can a Gbx whose body was only partly parsed. Some classes, such as replays, can't be written by GBX.NET at all, and the command fails with that error.

Compressed data inside a Gbx, such as a lightmap, is compared after decompressing it, because GBX.NET can compress it differently than the game did.

### set

Assigns values to members of a Gbx, then writes it like `eval -w`. Without `-o`, the input file is replaced. A folder or pattern changes [many files](#many-files). Each change is shown with its old and new value.

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

`SaveAsync(gbx, path)` writes a Gbx with the same check and backup as `eval -w`. It also takes `backup: false` and `force: true`. `GetGbxFiles(path)` returns the full paths of the Gbx files that a folder or a pattern on disk selects, like [many files](#many-files) in `eval` and `set`.

```cs
foreach (var path in GetGbxFiles(Args[0]))
{
    var gbx = Gbx.Parse<CGameCtnChallenge>(path);
    gbx.Node.MapName = "Night run";
    await SaveAsync(gbx, path);
}
```

### diff

Lists the properties that differ between two Gbx files, one per line. Both paths can continue inside an archive.

```
gbx diff "Old.Map.Gbx" "New.Map.Gbx"
```

```
Node.AuthorTime: 0:08.810 -> 0:12.000
Node.Blocks[2].Direction: North -> West
Node.MapName: My map -> Night run
3 differences.
```

The properties are compared like `verify --roundtrip` does. List items are compared by index, so a block removed at the start of a list makes every later block differ. Chunks are compared by ID. Compressed data, such as a lightmap, is compared after decompressing it, and a byte array shows only its first different byte.

- `--max <n>` stops after `n` differences. The default is 200, and `0` lists all of them.
- `--strict` exits with `1` when the files differ.

`eval` with `-w`, `-o` or `-n` uses the same comparison to list what changed, between the input file and the saved Gbx read back. So the list also shows anything that saving changed besides the expressions.

### verify

Parses every Gbx fully and continues after exceptions. A folder is walked with all subfolders in name order, and archives are opened to verify each entry. A pattern on disk, such as `Maps/*.Map.Gbx`, selects files like in [many files](#many-files), but without the `.Gbx` name filter. Files without a Gbx, PAK or ZIP signature are skipped and counted.

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
