using GBX.NET.PAK;

namespace GbxCLI.Commands;

internal static class PakCommand
{
    public static async Task<int> RunAsync(IEnumerator<string> argsEnumerator, TextWriter output)
    {
        if (!argsEnumerator.MoveNext())
        {
            throw new ArgumentException("Missing PAK subcommand. Use gbx pak files <file> or gbx pak verify <file>.");
        }

        var command = argsEnumerator.Current;

        if (Cli.TryHandleGlobalOption(command, output))
        {
            return 0;
        }

        if (command is not ("files" or "verify"))
        {
            throw new ArgumentException($"Unknown PAK subcommand: {command}. Use gbx pak files <file> or gbx pak verify <file>.");
        }

        var arguments = FileArguments.Parse(argsEnumerator, $"pak {command}", output, requiresOutput: false);

        if (arguments is null)
        {
            return 0;
        }

        await using var file = await ParsedFile.OpenAsync(arguments.InputPath, fullGbx: false);

        if (file.Value is not Pak pak)
        {
            throw new InvalidDataException($"The pak {command} command requires a PAK (NadeoPak) archive.");
        }

        if (!file.HasPakFileIndex)
        {
            throw new InvalidDataException("The PAK file index is unavailable because the private header requires a matching key.");
        }

        if (command == "verify")
        {
            return await VerifyAsync(pak, output);
        }

        foreach (var path in pak.Files.Keys.Order(StringComparer.Ordinal))
        {
            await output.WriteLineAsync(path);
        }

        return 0;
    }

    private static async Task<int> VerifyAsync(Pak pak, TextWriter output)
    {
        var parsed = 0;
        var failed = 0;
        var skipped = 0;

        foreach (var (path, file) in pak.Files.OrderBy(pair => pair.Key, StringComparer.Ordinal))
        {
            try
            {
                if (!await IsGbxAsync(pak, file))
                {
                    skipped++;
                    continue;
                }

                if (file.UncompressedSize < 3)
                {
                    throw new InvalidDataException("The file is too short to contain a Gbx signature.");
                }

                var gbx = await pak.OpenGbxFileAsync(file);

                if (gbx.Node is null)
                {
                    throw new NotSupportedException($"Root class 0x{gbx.Header.ClassId:X8} is not supported; its body was not parsed.");
                }

                parsed++;
            }
            catch (Exception exception)
            {
                failed++;
                await output.WriteLineAsync($"{path}: {exception.GetType().Name}: {exception.Message.ReplaceLineEndings(" ")}");
            }
        }

        await output.WriteLineAsync($"Parsed {parsed} Gbx files; {failed} failed; {skipped} non-Gbx files skipped.");
        return failed == 0 ? 0 : 1;
    }

    private static async Task<bool> IsGbxAsync(Pak pak, PakFile file)
    {
        if (file.Name.EndsWith(".gbx", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (file.UncompressedSize < 3)
        {
            return false;
        }

        using var stream = pak.OpenFile(file, out _);
        var signature = new byte[3];
        await stream.ReadExactlyAsync(signature);
        return signature.AsSpan().SequenceEqual("GBX"u8);
    }
}
