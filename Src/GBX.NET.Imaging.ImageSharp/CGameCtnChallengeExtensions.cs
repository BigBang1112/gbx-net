using GBX.NET.Engines.Game;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace GBX.NET.Imaging.ImageSharp;

/// <summary>
/// Imaging extensions for <see cref="CGameCtnChallenge"/>.
/// </summary>
public static partial class CGameCtnChallengeExtensions
{
    /// <summary>
    /// Gets the map thumbnail as <see cref="Image"/>.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Thumbnail as <see cref="Image"/>.</returns>
    public static async ValueTask<Image?> GetThumbnailImageAsync(this CGameCtnChallenge node, CancellationToken cancellationToken = default)
    {
        if (node.Thumbnail is null)
        {
            return null;
        }

        using var ms = new MemoryStream(node.Thumbnail);
        var image = await Image.LoadAsync(ms, cancellationToken);

        image.Mutate(x =>
        {
            x.RotateFlip(RotateMode.Rotate180, FlipMode.Horizontal);
        });

        return image;
    }

    /// <summary>
    /// Gets the map thumbnail as <see cref="Image"/>.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <returns>Thumbnail as <see cref="Image"/>.</returns>
    public static Image? GetThumbnailImage(this CGameCtnChallenge node)
    {
        if (node.Thumbnail is null)
        {
            return null;
        }

        var image = Image.Load(node.Thumbnail);

        image.Mutate(x =>
        {
            x.RotateFlip(RotateMode.Rotate180, FlipMode.Horizontal);
        });

        return image;
    }

    /// <summary>
    /// Exports the map's thumbnail.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="stream">Stream to export to.</param>
    /// <param name="encoder">Image encoder to use.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<bool> ExportThumbnailAsync(this CGameCtnChallenge node, Stream stream, IImageEncoder encoder, CancellationToken cancellationToken = default)
    {
        using var thumbnail = await node.GetThumbnailImageAsync(cancellationToken);

        if (thumbnail is null)
        {
            return false;
        }

        await thumbnail.SaveAsync(stream, encoder, cancellationToken);

        return true;
    }

    /// <summary>
    /// Exports the map's thumbnail.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="fileName">File to export to.</param>
    /// <param name="encoder">Image encoder to use.</param>
    public static void ExportThumbnail(this CGameCtnChallenge node, string fileName, IImageEncoder encoder)
    {
        using var fs = File.Create(fileName);
        node.ExportThumbnail(fs, encoder);
    }

    /// <summary>
    /// Exports the map's thumbnail.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="fileName">File to export to.</param>
    /// <param name="encoder">Image encoder to use.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public static async Task ExportThumbnailAsync(this CGameCtnChallenge node, string fileName, IImageEncoder encoder, CancellationToken cancellationToken = default)
    {
        await using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        await node.ExportThumbnailAsync(fs, encoder, cancellationToken);
    }

    /// <summary>
    /// Replaces a thumbnail (any popular image format) to use for the map.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="stream">Stream to import from.</param>
    /// <param name="quality">The quality level to use for the JPEG image. This is in the range from 1-100.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<Image> ImportThumbnailAsync(this CGameCtnChallenge node, Stream stream, int quality = 95, CancellationToken cancellationToken = default)
    {
        using var image = await Image.LoadAsync(stream, cancellationToken);

        image.Mutate(x =>
        {
            x.RotateFlip(RotateMode.Rotate180, FlipMode.Horizontal);
        });

        await using var ms = new MemoryStream();
        await image.SaveAsync(ms, new JpegEncoder { Quality = quality }, cancellationToken);

        node.Thumbnail = ms.ToArray();

        return image;
    }

    /// <summary>
    /// Replaces a thumbnail (any popular image format) to use for the map.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="fileName">File to import from.</param>
    /// <param name="quality">The quality level to use for the JPEG image. This is in the range from 1-100.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public static async Task<Image> ImportThumbnailAsync(this CGameCtnChallenge node, string fileName, int quality = 95, CancellationToken cancellationToken = default)
    {
        await using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
        return await node.ImportThumbnailAsync(fs, quality, cancellationToken);
    }

    /// <summary>
    /// Replaces a thumbnail (any popular image format) to use for the map.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="fileName">File to import from.</param>
    /// <param name="quality">The quality level to use for the JPEG image. This is in the range from 0-100.</param>
    public static Image ImportThumbnail(this CGameCtnChallenge node, string fileName, int quality = 95)
    {
        using var fs = File.OpenRead(fileName);
        return node.ImportThumbnail(fs, quality);
    }
}
