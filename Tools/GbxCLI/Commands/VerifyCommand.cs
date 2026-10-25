using GBX.NET;
using GBX.NET.PAK;

namespace GbxCLI.Commands;

internal static class VerifyCommand
{
    public static async Task<int> RunAsync(IEnumerator<string> argsEnumerator, TextWriter output)
    {
        var arguments = FileArguments.Parse(argsEnumerator, "verify", output, requiresOutput: false);

        if (arguments is null)
        {
            return 0;
        }

        var inputPath = arguments.InputPath;
        var settings = new GbxReadSettings { GameVersion = arguments.Game };

        if (Directory.Exists(inputPath))
        {
            var counts = new VerifyCounts();
            var paths = Directory.EnumerateFiles(inputPath, "*", SearchOption.AllDirectories)
                .Order(StringComparer.OrdinalIgnoreCase);

            foreach (var path in paths)
            {
                counts += await VerifyFileAsync(path, Path.GetRelativePath(inputPath, path), settings, output);
            }

            await output.WriteLineAsync(counts.ToString());
            return counts.Failed == 0 ? 0 : 1;
        }

        if (!File.Exists(inputPath))
        {
            throw new FileNotFoundException($"File or directory '{inputPath}' was not found.", inputPath);
        }

        if (!await IsGbxAsync(inputPath))
        {
            throw new InvalidDataException($"The file '{inputPath}' is not a Gbx file.");
        }

        var result = await VerifyFileAsync(inputPath, inputPath, settings, output);
        await output.WriteLineAsync(result.ToString());
        return result.Failed == 0 ? 0 : 1;
    }

    private static async Task<VerifyCounts> VerifyFileAsync(string path, string displayPath, GbxReadSettings settings, TextWriter output)
    {
        try
        {
            if (!await IsGbxAsync(path))
            {
                return new(0, 0, 1);
            }

            await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

            if (stream.Length < 3)
            {
                throw new InvalidDataException("The file is too short to contain a Gbx signature.");
            }

            EnsureBodyParsed(await Gbx.ParseAsync(stream, settings));
            return new(1, 0, 0);
        }
        catch (Exception exception)
        {
            await WriteFailureAsync(output, displayPath, exception);
            return new(0, 1, 0);
        }
    }

    private static async Task<bool> IsGbxAsync(string path)
    {
        await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        var signature = new byte[Pak.Magic.Length];
        var bytesRead = await stream.ReadAtLeastAsync(signature, signature.Length, throwOnEndOfStream: false);
        var data = signature.AsSpan(0, bytesRead);

        if (data.StartsWith("GBX"u8))
        {
            return true;
        }

        // Damaged files named as Gbx are reported, but .Pack.Gbx archives are not Gbx files.
        return path.EndsWith(".gbx", StringComparison.OrdinalIgnoreCase) && !data.SequenceEqual("NadeoPak"u8);
    }

    internal static void EnsureBodyParsed(Gbx gbx)
    {
        if (gbx.Node is null)
        {
            throw new NotSupportedException($"Root class 0x{gbx.Header.ClassId:X8} is not supported; its body was not parsed.");
        }
    }

    internal static Task WriteFailureAsync(TextWriter output, string path, Exception exception)
        => output.WriteLineAsync($"{path}: {exception.GetType().Name}: {exception.Message.ReplaceLineEndings(" ")}");
}
