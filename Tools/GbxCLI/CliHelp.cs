using Spectre.Console;

namespace GbxCLI;

internal static class CliHelp
{
    public static void Write(TextWriter output, string distribution, string? command = null)
    {
        var console = new CliConsole(output);
        console.WriteLine($"[bold]GBX.NET.CLI[/] [grey]({Markup.Escape(distribution)})[/]");
        console.WriteLine(string.Empty);

        if (command is null)
        {
            WriteOverview(console);
            return;
        }

        if (command == "pak")
        {
            console.WriteLine("[bold]Usage:[/] [cyan]gbx pak[/] <command> <file> [[options]]");
            console.WriteLine(string.Empty);
            WritePakCommands(console);
            WriteGlobalOptions(console);
            console.WriteLine("Run [cyan]gbx pak <command> --help[/] for command options and examples.");
            return;
        }

        var help = GetCommandHelp(command);
        console.WriteLine($"[bold]Usage:[/] [cyan]gbx[/] {Markup.Escape(help.Usage)}");
        console.WriteLine(string.Empty);
        console.WriteLine(help.Description);

        if (help.Options.Length > 0)
        {
            console.WriteLine(string.Empty);
            console.WriteLine("[bold]Command options:[/]");
            console.WriteLine(help.Options);
        }

        WriteGlobalOptions(console);
        console.WriteLine("[bold]Example:[/]");
        console.WriteLine("  " + Markup.Escape(help.Example));
    }

    private static void WriteOverview(CliConsole console)
    {
        console.WriteLine("""
            [bold]Usage:[/] [cyan]gbx[/] [[command]] <file> [[options]]

            [bold]Commands:[/]
              [cyan]parse[/] <file>                  Inspect a Gbx or PAK file (default).
              [cyan]eval[/] <file> [[code...]]         Evaluate C# against a Gbx or PAK file.
              [cyan]script[/] <file> [[-- args...]]    Run a C# script with GBX.NET available.
              [cyan]verify[/] <file>                 Fully parse a Gbx file or a folder tree of Gbx files.
              [cyan]pak files[/] <file>              List PAK or ZIP files, resolving known hashes.
              [cyan]pak verify[/] <file>             Fully parse Gbx entries of archives and list failures.
              [cyan]pak extract[/] <file>            Extract all or matching PAK or ZIP entries.
              [cyan]compress[/] <file>               Compress a Gbx body.
              [cyan]decompress[/] <file>             Decompress a Gbx body.
            """);
        WriteGlobalOptions(console);
        console.WriteLine("Run [cyan]gbx <command> --help[/] for command options and examples.");
    }

    private static void WritePakCommands(CliConsole console)
    {
        console.WriteLine("""
            [bold]PAK commands:[/]
              [cyan]pak files[/] <file>              List all PAK or ZIP files, resolving known hashes.
              [cyan]pak verify[/] <file>             Fully parse matching Gbx entries of one or more archives.
              [cyan]pak extract[/] <file>            Extract all or matching entries without overwriting.
            """);
    }

    private static void WriteGlobalOptions(CliConsole console)
    {
        console.WriteLine(string.Empty);
        console.WriteLine("""
            [bold]Global options:[/]
              [cyan]-h, --help[/]                   Show help for the current command.
              [cyan]-v, --version[/]                Show the CLI version.

            """);
    }

    private static CommandHelp GetCommandHelp(string command) => command switch
    {
        "parse" => new(
            "parse <file> [options]",
            "Inspect the header, body, references, and node properties of a Gbx or PAK file.\nAppend a PAK entry path, filename, hash, or pattern to inspect exactly one entry.",
            "  [cyan]-i, --interactive[/]            Interactive mode (not implemented yet).",
            "gbx parse \"Resource.pak/Media/Texture/WhiteFlare1.Texture.gbx\""),
        "eval" => new(
            "eval <file> [code...] [options]",
            "Evaluate quoted C# expressions against a Gbx or PAK object, in order.\nAppend a PAK entry path, filename, hash, or pattern to evaluate exactly one entry.",
            "  [cyan]-i, --interactive[/]            Interactive mode (not implemented yet).",
            "gbx eval \"My Map.Map.Gbx\" \"Node.Chunks.Count\""),
        "script" => new(
            "script <file> [-- args...]",
            "Run a C# script file with GBX.NET references, async support, and Args.\nUse Print(value), Output, or Console.WriteLine for output. Relative #load paths use the script directory.",
            "  [cyan]--[/]                           Pass remaining arguments to the script, including --help and --version.",
            "gbx script inspect.csx -- \"My Map.Map.Gbx\""),
        "pak files" => new(
            "pak files <file> [options]",
            "List every matching PAK or ZIP entry, resolving known hashes and keeping unresolved hashes.\nAppend a path or pattern to the archive to select entries. Matching ignores case.",
            """
              [cyan]--resolved[/]                   Show named files and hashes with known names.
              [cyan]--unresolved[/]                 Show only hashes without known names.

            The filters cannot be combined. Without a filter, all matching entries are listed.
            """,
            "gbx pak files \"Resource.pak/Media/Texture/*\" --unresolved"),
        "pak verify" => new(
            "pak verify <file> [options]",
            "Fully parse matching Gbx entries and list failures. Exit with code 1 if any fail.\nAppend a path or pattern to the archive to select entries. Matching ignores case.\nPass a folder to verify every PAK and ZIP archive directly inside it.",
            "  [cyan]-g, --game[/] <version>         Select the game when class IDs are reused, for example TMSX.",
            "gbx pak verify \"Resource.pak/*.Shader.Gbx\""),
        "verify" => new(
            "verify <file> [options]",
            "Fully parse a Gbx file and report the exception if it fails. Exit with code 1 if any fail.\nPass a folder to verify every Gbx file in it and its subfolders. Other files are skipped.",
            "  [cyan]-g, --game[/] <version>         Select the game when class IDs are reused, for example TMSX.",
            "gbx verify \"Maps\""),
        "pak extract" => new(
            "pak extract <file> [options]",
            "Extract every matching entry, restoring known filenames and preserving folders.\nAppend a path or pattern to the archive to select entries. Matching ignores case.",
            "  [cyan]-o, --output[/] <directory>     Destination (default: <archive>.extracted). Never overwrite existing files.",
            "gbx pak extract \"Resource.pak/Media/Texture/*.gbx\" -o \"Textures\""),
        "compress" or "decompress" => new(
            $"{command} <file> -o <output> [options]",
            command == "compress" ? "Write a Gbx with a compressed body." : "Write a Gbx with an uncompressed body.",
            "  [cyan]-o, --output[/] <file>          Required destination file. Never overwrite existing files.",
            $"gbx {command} \"My Map.Map.Gbx\" -o \"Output.Map.Gbx\""),
        _ => throw new ArgumentException($"Unknown command: {command}.")
    };

    private sealed record CommandHelp(string Usage, string Description, string Options, string Example);
}
