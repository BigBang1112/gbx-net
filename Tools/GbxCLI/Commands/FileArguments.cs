using GBX.NET;

namespace GbxCLI.Commands;

internal sealed record FileArguments(string InputPath, string? OutputPath, bool Interactive = false, GameVersion Game = GameVersion.Unspecified)
{
    public static FileArguments? Parse(
        IEnumerator<string> argsEnumerator,
        string command,
        TextWriter output,
        bool requiresOutput,
        string? inputPath = null,
        bool allowsOutput = false,
        Func<string, bool>? tryHandleOption = null)
    {
        string? outputPath = null;
        var interactive = command == "parse" && inputPath is not null;
        var game = GameVersion.Unspecified;

        while (argsEnumerator.MoveNext())
        {
            var arg = argsEnumerator.Current;

            if (Cli.TryHandleGlobalOption(arg, output, command))
            {
                return null;
            }

            if (tryHandleOption?.Invoke(arg) == true)
            {
                continue;
            }

            switch (arg)
            {
                case "-i":
                case "--interactive":
                    if (command != "parse")
                    {
                        throw new ArgumentException($"The {command} command does not accept {arg}.");
                    }

                    interactive = true;
                    continue;
                case "-o":
                case "--output":
                    if (outputPath is not null)
                    {
                        throw new ArgumentException("Output can only be specified once.");
                    }

                    if (!argsEnumerator.MoveNext() || string.IsNullOrWhiteSpace(argsEnumerator.Current))
                    {
                        throw new ArgumentException($"Missing file path after {arg}.");
                    }

                    outputPath = argsEnumerator.Current;
                    continue;
                case "-g":
                case "--game":
                    if (command is not ("verify" or "pak verify"))
                    {
                        throw new ArgumentException($"The {command} command does not accept {arg}.");
                    }

                    if (game != GameVersion.Unspecified)
                    {
                        throw new ArgumentException("Game can only be specified once.");
                    }

                    if (!argsEnumerator.MoveNext() || string.IsNullOrWhiteSpace(argsEnumerator.Current))
                    {
                        throw new ArgumentException($"Missing game version after {arg}.");
                    }

                    game = ParseGame(argsEnumerator.Current);
                    continue;
            }

            if (inputPath is not null)
            {
                throw new ArgumentException($"Unexpected argument: {arg}.");
            }

            inputPath = arg;
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw new ArgumentException($"Missing input file for {command}.");
        }

        if (requiresOutput && outputPath is null)
        {
            throw new ArgumentException($"Missing --output file for {command}.");
        }

        if (!requiresOutput && !allowsOutput && outputPath is not null)
        {
            throw new ArgumentException($"The {command} command does not accept --output.");
        }

        return new FileArguments(inputPath, outputPath, interactive, game);
    }

    private static GameVersion ParseGame(string value)
    {
        // Names only: numbers and combined flags do not select a single game context.
        if (!char.IsDigit(value[0])
            && Enum.TryParse<GameVersion>(value, ignoreCase: true, out var game)
            && game != GameVersion.Unspecified
            && Enum.IsDefined(game))
        {
            return game;
        }

        var names = Enum.GetNames<GameVersion>().Where(static name => name != nameof(GameVersion.Unspecified));
        throw new ArgumentException($"Unknown game version: {value}. Use one of: {string.Join(", ", names)}.");
    }
}
