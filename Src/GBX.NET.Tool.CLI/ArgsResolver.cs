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

    public ToolConfiguration Resolve()
    {
        if (!HasArgs)
        {
            return new();
        }

        var inputs = new List<Input>();

        var argsEnumerator = args.GetEnumerator();

        while (argsEnumerator.MoveNext())
        {
            var arg = argsEnumerator.Current as string ?? string.Empty;

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
            Inputs = inputs
        };
    }
}
