namespace GbxCLI;

/// <summary>
/// What an edit of <see cref="GbxBatchRunner"/> did to one Gbx.
/// </summary>
/// <param name="Changed">Whether the Gbx changed, or null when <see cref="GbxBatchWrite.CompareWithOriginal"/> finds it out.</param>
/// <param name="Text">Shown on the line of the file, such as the changed values, or null.</param>
internal sealed record GbxBatchEdit(bool? Changed, string? Text);
