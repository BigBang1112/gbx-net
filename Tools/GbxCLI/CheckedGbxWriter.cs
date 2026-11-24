using GBX.NET;

namespace GbxCLI;

/// <summary>
/// Saves a Gbx only after reading the saved data back, so a save that loses or changes data doesn't replace a file.
/// </summary>
internal static class CheckedGbxWriter
{
    /// <summary>
    /// Saves the Gbx into memory, without checking the result.
    /// </summary>
    /// <exception cref="NotSupportedException">The body was not parsed, or parsing it failed.</exception>
    public static byte[] Save(Gbx gbx)
    {
        if (gbx.Node is null)
        {
            throw new NotSupportedException($"Root class 0x{gbx.Header.ClassId:X8} is not supported, so its body cannot be saved.");
        }

        if (gbx.Body.Exception is { } exception)
        {
            throw new NotSupportedException($"The body was only partly parsed, so it cannot be saved: {exception.Message}");
        }

        using var saved = new MemoryStream();
        gbx.Save(saved);
        return saved.ToArray();
    }

    /// <summary>
    /// Parses <paramref name="data"/> saved from <paramref name="gbx"/> again and compares its properties with <paramref name="gbx"/>.
    /// </summary>
    /// <returns>The first difference as a sentence, or null if there is none.</returns>
    public static async Task<string?> FindDifferenceAsync(Gbx gbx, byte[] data, CancellationToken cancellationToken = default)
    {
        try
        {
            using var saved = new MemoryStream(data, writable: false);

            // The saved data is not encrypted, also when the Gbx came from an encrypted PAK entry.
            var reparsed = await Gbx.ParseAsync(saved, gbx.ReadSettings with { EncryptionInitializer = null }, cancellationToken);

            return GbxPropertyComparer.FindDifference(gbx, reparsed, cancellationToken) is { } changed
                ? $"reading it back changed {changed}"
                : null;
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return $"it cannot be read back: {exception.GetType().Name}: {exception.Message.ReplaceLineEndings(" ").TrimEnd('.')}.";
        }
    }

    /// <summary>
    /// The error for a save that <see cref="FindDifferenceAsync"/> rejected.
    /// </summary>
    public static InvalidDataException NotWritten(string difference)
        => new($"The saved Gbx was not written, because {difference} Use --force to write it anyway.");

    /// <summary>
    /// Saves the Gbx into memory, parses the result again and compares its properties with <paramref name="gbx"/>.
    /// </summary>
    /// <param name="force">Return the saved data even when the check finds a difference.</param>
    /// <returns>The saved data, and the difference that <paramref name="force"/> let through, or null.</returns>
    /// <exception cref="NotSupportedException">The body was not parsed, or parsing it failed.</exception>
    /// <exception cref="InvalidDataException">The check found a difference, and <paramref name="force"/> is false.</exception>
    public static async Task<(byte[] Data, string? Difference)> SaveAsync(Gbx gbx, bool force, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var data = Save(gbx);
        var difference = await FindDifferenceAsync(gbx, data, cancellationToken);

        return difference is not null && !force ? throw NotWritten(difference) : (data, difference);
    }

    /// <summary>
    /// Saves and checks the Gbx like <see cref="SaveAsync"/>, then writes it through <see cref="OutputFile"/> and reports the written files.
    /// </summary>
    /// <param name="backup">Keep an existing file as <c>&lt;path&gt;.bak</c>.</param>
    /// <param name="force">Write even when the check finds a difference, which is reported as a warning.</param>
    public static async Task WriteAsync(Gbx gbx, string path, CliConsole console, bool backup = true, bool force = false, CancellationToken cancellationToken = default)
    {
        var (data, difference) = await SaveAsync(gbx, force, cancellationToken);

        if (difference is not null)
        {
            await console.WriteLineAsync($"{CliConsole.Error}warning:{CliConsole.Reset} {CliConsole.Escape($"Writing anyway, although {difference}")}");
        }

        var backupPath = await WriteDataAsync(data, path, backup, cancellationToken);

        await console.WriteLineAsync($"{CliConsole.Written}Written:{CliConsole.Reset} {CliConsole.Escape(path)}");

        if (backupPath is not null)
        {
            await console.WriteLineAsync($"{CliConsole.Written}Backup:{CliConsole.Reset} {CliConsole.Escape(backupPath)}");
        }
    }

    /// <inheritdoc cref="OutputFile.WriteAsync"/>
    public static Task<string?> WriteDataAsync(byte[] data, string path, bool backup, CancellationToken cancellationToken = default)
        => OutputFile.WriteAsync(path, (destination, token) => destination.WriteAsync(data, token).AsTask(), backup, cancellationToken);
}
