using GBX.NET.Tool.CLI.Exceptions;
using GBX.NET.Tool.CLI.Inputs;

namespace GBX.NET.Tool.CLI;

internal sealed class ArgsResolver
{
    private readonly string[] args;
    private readonly HttpClient client;

    public bool HasArgs => args.Length > 0;

    public ArgsResolver(string[] args, HttpClient http)
    {
        this.args = args;
        this.client = http;
    }

    public ToolSettings Resolve(ConsoleSettings consoleOptions)
    {
        if (!HasArgs)
        {
            return new ToolSettings { ConsoleSettings = consoleOptions };
        }

        var configOverwrites = new Dictionary<string, string>();
        var inputs = new List<InputArgument>();

        var argsEnumerator = args.AsEnumerable().GetEnumerator();

        while (argsEnumerator.MoveNext())
        {
            var arg = argsEnumerator.Current;

            if (arg == "--disable-update-check")
            {
                consoleOptions.DisableUpdateCheck = true;
                continue;
            }

            if (arg == "--skip-intro")
            {
                consoleOptions.SkipIntro = true;
                continue;
            }

            if (arg == "--no-pause")
            {
                consoleOptions.NoPause = true;
                continue;
            }

            if (arg == "--config" || arg == "-c")
            {
                if (!argsEnumerator.MoveNext())
                {
                    throw new ConsoleProblemException("Missing config name.");
                }

                var configName = argsEnumerator.Current;

                if (!File.Exists(configName))
                {
                    throw new ConsoleProblemException("Config does not exist.");
                }
                
                consoleOptions.ConfigName = configName;
                continue;
            }

            if (arg.StartsWith("--config:") || arg.StartsWith("-c:"))
            {
                var configKey = arg.Substring(arg.IndexOf(':') + 1);

                if (!argsEnumerator.MoveNext())
                {
                    throw new ConsoleProblemException("Missing config value.");
                }

                var configValue = argsEnumerator.Current;

                configOverwrites[configKey] = configValue;
                continue;
            }

            if (arg == "--output" || arg == "-o")
            {
                if (!argsEnumerator.MoveNext())
                {
                    throw new ConsoleProblemException("Missing output path.");
                }

                var outputPath = argsEnumerator.Current;

                consoleOptions.OutputDirPath = outputPath;
                continue;
            }

            if (arg == "--direct-output" || arg == "-d")
            {
                consoleOptions.DirectOutput = true;
                continue;
            }

            if (arg.StartsWith('-'))
            {
                continue;
            }

            // - check http:// and https:// for URLs
            // - check for individual files and files in zip archives
            // - check for folders
            // - check for stdin (maybe?)
            // - check for configured user data path
            if (Directory.Exists(arg))
            {
                inputs.Add(new DirectoryInputArgument(arg));
                continue;
            }

            if (File.Exists(arg))
            {
                inputs.Add(new FileInputArgument(arg));
                continue;
            }

            if (Uri.TryCreate(arg, UriKind.Absolute, out var uri))
            {
                if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                {
                    inputs.Add(new UriInputArgument(client, uri));
                }

                continue;
            }
        }

        return new ToolSettings
        {
            InputArguments = inputs,
            ConfigOverwrites = configOverwrites,
            ConsoleSettings = consoleOptions
        };
    }
}
