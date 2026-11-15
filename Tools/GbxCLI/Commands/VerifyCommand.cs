using GBX.NET;
using GBX.NET.PAK;
using System.IO.Compression;

namespace GbxCLI.Commands;

internal static class VerifyCommand
{
    public static async Task<int> RunAsync(IEnumerator<string> argsEnumerator, TextWriter output, CancellationToken cancellationToken)
    {
        string? inputPath = null;
        var settings = new GbxReadSettings();

        while (argsEnumerator.MoveNext())
        {
            var arg = argsEnumerator.Current;

            if (arg is "-g" or "--game")
            {
                if (!argsEnumerator.MoveNext())
                {
                    throw new ArgumentException($"Missing game version after {arg}.");
                }

                settings = settings with { GameVersion = Enum.Parse<GameVersion>(argsEnumerator.Current, ignoreCase: true) };
            }
            else if (inputPath is null)
            {
                inputPath = arg;
            }
            else
            {
                throw new ArgumentException($"Unexpected argument: {arg}.");
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw new ArgumentException("Missing input file for verify.");
        }

        var isDirectory = Directory.Exists(inputPath);
        IEnumerable<string> paths = isDirectory
            ? Directory.EnumerateFiles(inputPath, "*", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase)
            : [inputPath];

        var counts = new VerifyCounts();

        foreach (var path in paths)
        {
            await using var stream = File.OpenRead(path);
            counts += await VerifyAsync(stream, isDirectory ? Path.GetRelativePath(inputPath, path) : path, settings, output, cancellationToken);
        }

        await output.WriteLineAsync(counts.ToString());
        return counts.Failed == 0 ? 0 : 1;
    }

    /// <summary>
    /// Fully parses a Gbx, or every Gbx inside a PAK or ZIP archive. Other files are skipped.
    /// </summary>
    private static Task<VerifyCounts> VerifyAsync(Stream stream, string path, GbxReadSettings settings, TextWriter output, CancellationToken cancellationToken)
        => ReportAsync(path, output, async () =>
        {
            var signature = new byte[Pak.Magic.Length];
            var length = await stream.ReadAtLeastAsync(signature, signature.Length, throwOnEndOfStream: false, cancellationToken);
            stream.Position = 0;

            if (signature.AsSpan(0, length).SequenceEqual("NadeoPak"u8))
            {
                return await VerifyPakAsync(stream, path, settings, output, cancellationToken);
            }

            if (signature.AsSpan().StartsWith("PK"u8))
            {
                return await VerifyZipAsync(stream, path, settings, output, cancellationToken);
            }

            if (signature.AsSpan().StartsWith("GBX"u8))
            {
                return Parsed(await Gbx.ParseAsync(stream, settings, cancellationToken));
            }

            // Other files are skipped.
            return new(0, 0, 1);
        });

    private static async Task<VerifyCounts> VerifyPakAsync(Stream stream, string path, GbxReadSettings settings, TextWriter output, CancellationToken cancellationToken)
    {
        var (pak, hasKey) = await PakLoader.ParseAsync(stream, path, cancellationToken);
        using var _ = pak;

        if (pak.IsHeaderPrivate && !hasKey)
        {
            throw new InvalidDataException("The PAK file index is unavailable because the private header requires a matching key.");
        }

        var counts = new VerifyCounts();

        foreach (var entry in pak.Select(null))
        {
            var file = entry.File;

            // Newer title packs list entries without stored data.
            if (file.Offset == uint.MaxValue && file.UncompressedSize == 0)
            {
                counts += new VerifyCounts(0, 0, 0, 1);
                continue;
            }

            counts += await ReportAsync(Path.Combine(path, entry.Path), output, async () =>
                !await pak.CheckFileIsGbxAsync(file, cancellationToken)
                    ? new(0, 0, 1)
                    : Parsed(await pak.OpenGbxFileAsync(file, settings, cancellationToken: cancellationToken)));
        }

        return counts;
    }

    private static async Task<VerifyCounts> VerifyZipAsync(Stream stream, string path, GbxReadSettings settings, TextWriter output, CancellationToken cancellationToken)
    {
        using var zip = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true);
        var counts = new VerifyCounts();

        foreach (var entry in zip.Select(null))
        {
            // ZIP entry streams can't seek, so each entry is buffered. It can be a nested archive too.
            using var data = new MemoryStream();

            await using (var source = entry.Open())
            {
                await source.CopyToAsync(data, cancellationToken);
            }

            data.Position = 0;
            counts += await VerifyAsync(data, Path.Combine(path, entry.FullName), settings, output, cancellationToken);
        }

        return counts;
    }

    private static VerifyCounts Parsed(Gbx gbx) => gbx.Node is null
        ? throw new NotSupportedException($"Root class 0x{gbx.Header.ClassId:X8} is not supported; its body was not parsed.")
        : new(1, 0, 0);

    /// <summary>
    /// Runs the verification, reporting any exception as a failure of <paramref name="path"/>.
    /// </summary>
    private static async Task<VerifyCounts> ReportAsync(string path, TextWriter output, Func<Task<VerifyCounts>> verify)
    {
        try
        {
            return await verify();
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            await output.WriteLineAsync($"{path}: {exception.GetType().Name}: {exception.Message.ReplaceLineEndings(" ")}");
            return new(0, 1, 0);
        }
    }
}
