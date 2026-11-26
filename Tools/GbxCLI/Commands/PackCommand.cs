namespace GbxCLI.Commands;

internal static class PackCommand
{
    public static async Task RunAsync(IEnumerator<string> args, CliContext context)
    {
        if (!args.MoveNext())
        {
            throw new ArgumentException("Missing command for pack: list, extract or verify. Use gbx pack --help for usage.");
        }

        switch (args.Current)
        {
            case "-h" or "--help":
                await CliHelp.WriteCommandAsync(context.Console, "pack");
                break;
            case "list":
                await PackListCommand.RunAsync(args, context);
                break;
            case "extract":
                await PackExtractCommand.RunAsync(args, context);
                break;
            case "verify":
                await VerifyCommand.RunAsync(args, context);
                break;
            case var arg:
                throw ArgumentExtensions.Unexpected("pack", arg);
        }
    }
}
