using GBX.NET;

namespace GbxCLI;

/// <summary>
/// Saves a Gbx only after reading the saved data back, so a save that loses or changes data doesn't replace a file.
/// </summary>
internal static class CheckedGbxWriter
{
    /// <summary>
    /// Saves the Gbx into memory, parses the result again and compares its properties with <paramref name="gbx"/>.
    /// </summary>
    /// <param name="force">Return the saved data even when the check finds a difference.</param>
    /// <returns>The saved data, and the difference that <paramref name="force"/> let through, or null.</returns>
    /// <exception cref="NotSupportedException">The root class is unknown, so the body was not parsed.</exception>
    /// <exception cref="InvalidDataException">The check found a difference, and <paramref name="force"/> is false.</exception>
    public static async Task<(byte[] Data, string? Difference)> SaveAsync(Gbx gbx, bool force, CancellationToken cancellationToken = default)
    {
        if (gbx.Node is null)
        {
            throw new NotSupportedException($"Root class 0x{gbx.Header.ClassId:X8} is not supported, so its body cannot be saved.");
        }

        cancellationToken.ThrowIfCancellationRequested();

        using var saved = new MemoryStream();
        gbx.Save(saved);
        saved.Position = 0;

        string? difference;

        try
        {
            // The saved data is not encrypted, also when the Gbx came from an encrypted PAK entry.
            var reparsed = await Gbx.ParseAsync(saved, gbx.ReadSettings with { EncryptionInitializer = null }, cancellationToken);
            difference = GbxPropertyComparer.FindDifference(gbx, reparsed, cancellationToken) is { } changed
                ? $"reading it back changed {changed}"
                : null;
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            difference = $"it cannot be read back: {exception.GetType().Name}: {exception.Message.ReplaceLineEndings(" ").TrimEnd('.')}.";
        }

        if (difference is not null && !force)
        {
            throw new InvalidDataException($"The saved Gbx was not written, because {difference} Use --force to write it anyway.");
        }

        return (saved.ToArray(), difference);
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

        var backupPath = await OutputFile.WriteAsync(path, (destination, token) => destination.WriteAsync(data, token).AsTask(), backup, cancellationToken);

        await console.WriteLineAsync($"{CliConsole.Written}Written:{CliConsole.Reset} {CliConsole.Escape(path)}");

        if (backupPath is not null)
        {
            await console.WriteLineAsync($"{CliConsole.Written}Backup:{CliConsole.Reset} {CliConsole.Escape(backupPath)}");
        }
    }
}
