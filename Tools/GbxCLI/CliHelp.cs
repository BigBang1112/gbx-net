namespace GbxCLI;

internal static class CliHelp
{
    private const string H = CliConsole.Heading;
    private const string L = CliConsole.Label;
    private const string Z = CliConsole.Reset;

    private static readonly Dictionary<string, string> commands = new()
    {
        ["inspect"] = $"""
            {H}Usage:{Z} gbx inspect <path> [-i]
            {L}Alias:{Z} gbx parse

            Fully parses a Gbx and writes a summary, or summarizes a PAK or ZIP archive.
            The path can continue inside an archive, such as Stadium.pak/Media/Grass.Texture.Gbx.

            {H}Options:{Z}
              -i, --interactive      Browse the file. Without a console, the summary is written instead.
            """,
        ["eval"] = $"""
            {H}Usage:{Z} gbx eval <path> <code>... [-i]

            Evaluates C# expressions against the opened Gbx, Pak or ZipArchive, which is the script global.
            Each argument is one expression, and later ones can use variables from earlier ones.
            Example: gbx eval My.Map.Gbx "Node.MapName" "Node.Author"

            {H}Options:{Z}
              -i, --interactive      Start a C# prompt after the expressions. Enter q to quit.
            """,
        ["script"] = $"""
            {H}Usage:{Z} gbx script <script.csx> [args...]
                   gbx script -- <-script.csx> [args...]

            Runs a C# script with GBX.NET available. Arguments after the script are passed as Args,
            also when they start with -. Scripts can call Print(value) and write to Output.
            """,
        ["verify"] = $"""
            {H}Usage:{Z} gbx verify <path> [-g <game>] [--strict]
            {L}Alias:{Z} gbx pack verify

            Fully parses every Gbx in a file, folder or archive, and continues after failures.

            {H}Options:{Z}
              -g, --game <game>      Game version, for games that reuse a class ID for a different class.
              --strict               Fail with exit code 1 when any file fails verification.
            """,
        ["pack"] = $"""
            {H}Usage:{Z} gbx pack <command> <archive>[/pattern] [options]

            {H}Commands:{Z}
              list                   List archive entries, resolving known hashes.
                --resolved           Only list entries with known names.
                --unresolved         Only list entries that are still hashes.
              extract                Extract all or matching entries.
                -o, --output <dir>   Output folder. Defaults to the title ID, or the archive name.
                --overwrite          Replace existing files.
                --skip-existing      Keep existing files and extract only the missing ones.
              verify                 Same as gbx verify.

            A pattern with * or ? matches whole entry paths or their ends, and * also matches folders (Media/*).
            A pattern without wildcards selects the entry with that path, or entries ending with it.
            """,
        ["compress"] = $"""
            {H}Usage:{Z} gbx compress <file> [-o <file>] [--no-backup]

            Compresses a Gbx body. Without -o, the input file is replaced.

            {H}Options:{Z}
              -o, --output <file>    Output file.
              --no-backup            Don't keep an existing output as <output>.bak.
            """,
        ["decompress"] = $"""
            {H}Usage:{Z} gbx decompress <file> [-o <file>] [--no-backup]

            Decompresses a Gbx body. Without -o, the input file is replaced.

            {H}Options:{Z}
              -o, --output <file>    Output file.
              --no-backup            Don't keep an existing output as <output>.bak.
            """,
    };

    public static async Task WriteAsync(CliConsole console)
    {
        await console.WriteLineAsync($"$oGBX.NET.CLI$z {L}({CliConsole.Escape(Cli.Distribution)}){Z}");
        await console.WriteLineAsync(string.Empty);
        await WriteLinesAsync(console, $"""
            {H}Usage:{Z} gbx <command> [arguments] [options]
                   gbx <path>                     Browse a Gbx, PAK or ZIP file.

            {H}Commands:{Z}
              inspect <path>                  Fully parse a Gbx and write a summary, or summarize a PAK or ZIP.
              eval <path> <code>...           Evaluate C# against a Gbx, PAK or ZIP file.
              script <file> [args...]         Run a C# script with GBX.NET available.
              verify <path>                   Fully parse Gbx files, also inside folders and PAK or ZIP archives.
              pack list <archive>             List PAK or ZIP entries, resolving known hashes.
              pack extract <archive>          Extract all or matching PAK or ZIP entries.
              compress <file>                 Compress a Gbx body.
              decompress <file>               Decompress a Gbx body.

            {H}Global options:{Z}
              -h, --help                      Show this help. Use gbx <command> --help for a command.
              -v, --version                   Show the version.
            """);
    }

    public static Task WriteCommandAsync(CliConsole console, string command) => WriteLinesAsync(console, commands[command]);

    private static async Task WriteLinesAsync(CliConsole console, string text)
    {
        // Each line is formatted on its own, so it ends in a reset.
        foreach (var line in text.ReplaceLineEndings("\n").Split('\n'))
        {
            await console.WriteLineAsync(line);
        }
    }
}
