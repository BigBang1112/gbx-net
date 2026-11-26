using GBX.NET;

namespace GbxCLI;

/// <summary>
/// A Gbx saved by <see cref="CheckedGbxWriter.SaveAsync"/>.
/// </summary>
/// <param name="Data">The saved file.</param>
/// <param name="Reparsed">The saved file parsed again, or null if it couldn't be parsed.</param>
/// <param name="Difference">What the check found and <c>--force</c> let through, or null.</param>
internal sealed record CheckedSave(byte[] Data, Gbx? Reparsed, string? Difference);
