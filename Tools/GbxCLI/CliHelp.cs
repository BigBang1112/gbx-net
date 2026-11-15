using Spectre.Console;

namespace GbxCLI;

internal static class CliHelp
{
    public static void Write(TextWriter output, string distribution)
    {
        var console = new CliConsole(output);
        console.WriteLine($"[bold]GBX.NET.CLI[/] [grey]({Markup.Escape(distribution)})[/]");
        console.WriteLine(string.Empty);
        console.WriteLine("""
            [bold]Usage:[/] [cyan]gbx[/] [[command]] <file> [[options]]

            [bold]Commands:[/]
              [cyan]inspect[/] <file>                Inspect a Gbx, PAK or ZIP file (default).
              [cyan]eval[/] <file> [[code...]]         Evaluate C# against a Gbx, PAK or ZIP file.
              [cyan]script[/] <file> [[-- args...]]    Run a C# script with GBX.NET available.
              [cyan]verify[/] <path>                 Fully parse Gbx files, also inside folders and PAK or ZIP archives.
              [cyan]pack list[/] <file>              List PAK or ZIP files, resolving known hashes.
              [cyan]pack verify[/] <path>            Same as verify.
              [cyan]pack extract[/] <file>           Extract all or matching PAK or ZIP entries.
              [cyan]compress[/] <file>               Compress a Gbx body.
              [cyan]decompress[/] <file>             Decompress a Gbx body.

            [bold]Global options:[/]
              [cyan]-h[/], [cyan]--help[/]                     Show this help.
              [cyan]-v[/], [cyan]--version[/]                  Show the version.
            """);
    }
}
