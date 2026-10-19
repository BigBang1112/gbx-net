namespace GbxCLI.Commands;

internal sealed record FileArguments(string InputPath, string? OutputPath, bool Interactive = false)
{
    public static FileArguments? Parse(
        IEnumerator<string> argsEnumerator,
        string command,
        TextWriter output,
        bool requiresOutput,
        string? inputPath = null,
        bool allowsOutput = false)
    {
        string? outputPath = null;
        var interactive = command == "parse" && inputPath is not null;

        while (argsEnumerator.MoveNext())
        {
            var arg = argsEnumerator.Current;

            if (Cli.TryHandleGlobalOption(arg, output))
            {
                return null;
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

        return new FileArguments(inputPath, outputPath, interactive);
    }
}
