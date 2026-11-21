using GBX.NET;
using GBX.NET.PAK;
using System.IO.Compression;

namespace GbxCLI.Commands;

/// <summary>
/// Fully parses every Gbx in a file, folder or archive, and continues after exceptions.
/// </summary>
internal sealed class VerifyCommand
{
    private readonly CliContext context;
    private readonly GbxReadSettings settings;
    private readonly bool roundTrip;

    private VerifyCommand(CliContext context, GbxReadSettings settings, bool roundTrip)
    {
        this.context = context;
        this.settings = settings;
        this.roundTrip = roundTrip;
    }

    public static async Task RunAsync(IEnumerator<string> args, CliContext context)
    {
        string? inputPath = null;
        var settings = new GbxReadSettings();
        var strict = false;
        var roundTrip = false;

        while (args.MoveNext())
        {
            switch (args.Current)
            {
                case "-h" or "--help":
                    await CliHelp.WriteCommandAsync(context.Console, "verify");
                    return;
                case "-g" or "--game":
                    settings = settings with { GameVersion = ParseGameVersion(args.NextValue("--game")) };
                    break;
                case "--strict":
                    strict = true;
                    break;
                case "--roundtrip":
                    roundTrip = true;
                    break;
                case var arg when inputPath is null:
                    inputPath = arg;
                    break;
                case var arg:
                    throw ArgumentExtensions.Unexpected("verify", arg);
            }
        }

        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw ArgumentExtensions.MissingInput("verify");
        }

        var counts = await new VerifyCommand(context, settings, roundTrip).VerifyPathAsync(inputPath);

        await context.Console.WriteLineAsync(counts.ToString());

        if (strict && counts.Failed > 0)
        {
            throw new InvalidDataException($"{counts.Failed} files failed verification.");
        }
    }

    /// <summary>
    /// Accepts the name of exactly one defined game version. <see cref="GameVersion"/> is a flags enum,
    /// so numbers and combinations would parse too.
    /// </summary>
    internal static GameVersion ParseGameVersion(string value)
    {
        if (!Enum.TryParse<GameVersion>(value, ignoreCase: true, out var game)
            || char.IsAsciiDigit(value.TrimStart()[0]) || value.TrimStart()[0] == '-' || value.Contains(',') || !Enum.IsDefined(game))
        {
            throw new ArgumentException($"Unknown game version: {value}. Use one of: {string.Join(", ", Enum.GetNames<GameVersion>())}.");
        }

        return game;
    }

    private async Task<VerifyCounts> VerifyPathAsync(string inputPath)
    {
        if (Directory.Exists(inputPath))
        {
            var counts = new VerifyCounts();

            foreach (var path in Directory.EnumerateFiles(inputPath, "*", SearchOption.AllDirectories).Order(StringComparer.OrdinalIgnoreCase))
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                await using var stream = File.OpenRead(path);
                counts += await VerifyAsync(stream, Path.GetRelativePath(inputPath, path), entryPath: null);
            }

            return counts;
        }

        // The path can continue inside a PAK or ZIP archive, also nested in a ZIP.
        var (filePath, entryPath) = FileObject.Split(inputPath);
        await using var file = File.OpenRead(filePath);

        return await VerifyAsync(file, filePath, entryPath);
    }

    /// <summary>
    /// Fully parses a Gbx, or the Gbx files inside a PAK or ZIP archive. Other files are skipped.
    /// </summary>
    /// <param name="entryPath">Path or pattern of the entries to verify inside the archive, or null for all of them.</param>
    private Task<VerifyCounts> VerifyAsync(Stream stream, string path, string? entryPath) => ReportAsync(path, async () =>
    {
        var kind = await FileSignature.ReadAsync(stream, context.CancellationToken);

        switch (kind)
        {
            case FileKind.Pak:
                return await VerifyPakAsync(stream, path, entryPath);
            case FileKind.Zip:
                return await VerifyZipAsync(stream, path, entryPath);
        }

        if (entryPath is not null)
        {
            throw new InvalidDataException($"'{path}' is not a PAK or ZIP archive, so it has no entry '{entryPath}'.");
        }

        if (kind != FileKind.Gbx) return new(0, 0, 1);

        return await VerifyGbxAsync(() =>
        {
            stream.Position = 0;
            return Gbx.ParseAsync(stream, settings, context.CancellationToken);
        });
    });

    private async Task<VerifyCounts> VerifyPakAsync(Stream stream, string path, string? entryPath)
    {
        var (pak, hasKey) = await PakLoader.ParseAsync(stream, path, context.CancellationToken);
        using var _ = pak;

        pak.EnsureIndex(hasKey);

        var counts = new VerifyCounts();
        var matched = false;

        foreach (var entry in pak.Select(entryPath))
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            matched = true;

            if (entry.IsUnstored)
            {
                counts += new VerifyCounts(0, 0, 0, 1);
                continue;
            }

            counts += await ReportAsync(Path.Combine(path, PathPattern.ToDisplay(entry.Path)), async () =>
                await pak.CheckFileIsGbxAsync(entry.File, context.CancellationToken)
                    ? await VerifyGbxAsync(() => pak.OpenGbxFileAsync(entry.File, settings, cancellationToken: context.CancellationToken))
                    : new(0, 0, 1));
        }

        if (entryPath is not null && !matched)
        {
            throw new FileNotFoundException($"No files matching '{entryPath}' were found in PAK '{path}'.", path);
        }

        return counts;
    }

    private async Task<VerifyCounts> VerifyZipAsync(Stream stream, string path, string? entryPath)
    {
        using var zip = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true);

        // A path can select one entry, and whatever is left of it continues inside that entry.
        if (entryPath is not null && zip.Find(entryPath) is var (found, innerPath))
        {
            return await VerifyZipEntryAsync(found, path, innerPath);
        }

        // Otherwise, the pattern is matched against the entries of this ZIP.
        var counts = new VerifyCounts();
        var matched = false;

        foreach (var entry in zip.Select(entryPath))
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            matched = true;
            counts += await VerifyZipEntryAsync(entry, path, innerPath: null);
        }

        if (entryPath is not null && !matched)
        {
            throw new FileNotFoundException($"No files matching '{entryPath}' were found in ZIP '{path}'.", path);
        }

        return counts;
    }

    private async Task<VerifyCounts> VerifyZipEntryAsync(ZipArchiveEntry entry, string path, string? innerPath)
    {
        var entryPath = Path.Combine(path, PathPattern.ToDisplay(entry.FullName));

        // A ZIP entry can be a nested archive, and Gbx and PAK parsing need a seekable stream.
        return await ReportAsync(entryPath, async () =>
        {
            using var data = await FileObject.BufferAsync(entry, context.CancellationToken);
            return await VerifyAsync(data, entryPath, innerPath);
        });
    }

    private async Task<VerifyCounts> VerifyGbxAsync(Func<Task<Gbx>> parse)
    {
        var original = await parse();
        var counts = Parsed(original);

        if (roundTrip)
        {
            // Some serializers normalize properties while writing. Keep the first parsed model for comparison.
            await CheckRoundTripAsync(original, await parse(), context.CancellationToken);
        }

        return counts;
    }

    private static async Task CheckRoundTripAsync(Gbx original, Gbx toSave, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var saved = new MemoryStream();
        toSave.Save(saved);
        saved.Position = 0;

        var reparsed = await Gbx.ParseAsync(saved, original.ReadSettings with { EncryptionInitializer = null }, cancellationToken);
        if (GbxPropertyComparer.FindDifference(original, reparsed, cancellationToken) is { } difference)
        {
            throw new InvalidDataException($"Round trip changed {difference}");
        }
    }

    /// <summary>
    /// A Gbx whose root class is unknown counts as a failure, because its body was not parsed.
    /// </summary>
    private static VerifyCounts Parsed(Gbx gbx) => gbx.Node is null
        ? throw new NotSupportedException($"Root class 0x{gbx.Header.ClassId:X8} is not supported (its body was not parsed).")
        : new(1, 0, 0);

    /// <summary>
    /// Runs the verification, reporting any exception as a failure of <paramref name="path"/>.
    /// </summary>
    private async Task<VerifyCounts> ReportAsync(string path, Func<Task<VerifyCounts>> verify)
    {
        try
        {
            return await verify();
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            await context.Console.WriteLineAsync(CliConsole.Escape($"{path}: {exception.GetType().Name}: {exception.Message.ReplaceLineEndings(" ")}"));
            return new(0, 1, 0);
        }
    }
}
