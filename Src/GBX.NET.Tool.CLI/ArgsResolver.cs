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

    public ToolConfiguration Resolve(ConsoleOptions consoleOptions)
    {
        if (!HasArgs)
        {
            return new ToolConfiguration { ConsoleOptions = consoleOptions };
        }

        var configOverwrites = new Dictionary<string, string>();
        var inputs = new List<Input>();

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
                    throw new ConsoleProblemException("Missing config file path.");
                }

                var configPath = argsEnumerator.Current;

                if (!File.Exists(configPath))
                {
                    throw new ConsoleProblemException("Config file does not exist.");
                }
                
                consoleOptions.ConfigFilePath = configPath;
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
                inputs.Add(new DirectoryInput(arg));
                continue;
            }

            if (File.Exists(arg))
            {
                inputs.Add(new FileInput(arg));
                continue;
            }

            if (Uri.TryCreate(arg, UriKind.Absolute, out var uri))
            {
                if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                {
                    inputs.Add(new UriInput(client, uri));
                }

                continue;
            }
        }

        return new ToolConfiguration
        {
            Inputs = inputs,
            ConfigOverwrites = configOverwrites,
            ConsoleOptions = consoleOptions
        };
    }
}
