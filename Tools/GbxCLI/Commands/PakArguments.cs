using GBX.NET;

namespace GbxCLI.Commands;

internal sealed record PakArguments(string InputPath, string? OutputPath, bool? Resolved, GameVersion Game)
{
    public static PakArguments? Parse(IEnumerator<string> argsEnumerator, string command, TextWriter output)
    {
        bool? resolved = null;
        var arguments = FileArguments.Parse(argsEnumerator, $"pak {command}", output,
            requiresOutput: false, allowsOutput: command == "extract", tryHandleOption: TryHandleOption);

        return arguments is null ? null : new PakArguments(arguments.InputPath, arguments.OutputPath, resolved, arguments.Game);

        bool TryHandleOption(string arg)
        {
            if (arg is not ("--resolved" or "--unresolved"))
            {
                return false;
            }

            if (command != "files")
            {
                throw new ArgumentException($"The pak {command} command does not accept {arg}.");
            }

            var requested = arg == "--resolved";

            if (resolved is not null && resolved != requested)
            {
                throw new ArgumentException("The --resolved and --unresolved options cannot be combined.");
            }

            resolved = requested;
            return true;
        }
    }
}
