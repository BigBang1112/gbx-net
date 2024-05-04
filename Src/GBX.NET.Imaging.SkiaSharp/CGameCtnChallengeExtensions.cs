using GBX.NET.Engines.Game;
using SkiaSharp;

namespace GBX.NET.Imaging.SkiaSharp;

/// <summary>
/// Imaging extensions for <see cref="CGameCtnChallenge"/>.
/// </summary>
public static class CGameCtnChallengeExtensions
{
    /// <summary>
    /// Gets the map thumbnail as <see cref="SKBitmap"/>.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <returns>Thumbnail as <see cref="SKBitmap"/>.</returns>
    public static SKBitmap? GetThumbnailBitmap(this CGameCtnChallenge node)
    {
        if (node.Thumbnail is null)
        {
            return null;
        }

        using var bitmap = SKBitmap.Decode(node.Thumbnail);
        return bitmap.Rotate180FlipX();
    }

    /// <summary>
    /// Exports the map's thumbnail.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="stream">Stream to export to.</param>
    /// <param name="format">Image format to use.</param>
    /// <param name="quality">The quality level to use for the image. This is in the range from 0-100. Not all formats (for example, PNG) respect or support it.</param>
    public static bool ExportThumbnail(this CGameCtnChallenge node, Stream stream, SKEncodedImageFormat format, int quality)
    {
        using var thumbnail = node.GetThumbnailBitmap();
        return thumbnail is not null && thumbnail.Encode(stream, format, quality);
    }

    /// <summary>
    /// Exports the map's thumbnail.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="fileName">File to export to.</param>
    /// <param name="format">Image format to use.</param>
    /// <param name="quality">The quality level to use for the image. This is in the range from 0-100. Not all formats (for example, PNG) respect or support it.</param>
    public static void ExportThumbnail(this CGameCtnChallenge node, string fileName, SKEncodedImageFormat format, int quality)
    {
        using var fs = File.Create(fileName);
        node.ExportThumbnail(fs, format, quality);
    }

    /// <summary>
    /// Replaces a thumbnail (any popular image format) to use for the map.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="stream">Stream to import from.</param>
    /// <param name="quality">The quality level to use for the JPEG image. This is in the range from 0-100.</param>
    public static SKBitmap ImportThumbnail(this CGameCtnChallenge node, Stream stream, int quality = 95)
    {
        using var bitmap = SKBitmap.Decode(stream);
        using var rotated = bitmap.Rotate180FlipX();
        using var ms = new MemoryStream();

        rotated.Encode(ms, SKEncodedImageFormat.Jpeg, quality);

        node.Thumbnail = ms.ToArray();

        return rotated;
    }

    /// <summary>
    /// Replaces a thumbnail (any popular image format) to use for the map.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="fileName">File to import from.</param>
    /// <param name="quality">The quality level to use for the JPEG image. This is in the range from 0-100.</param>
    public static SKBitmap ImportThumbnail(this CGameCtnChallenge node, string fileName, int quality = 95)
    {
        using var fs = File.OpenRead(fileName);
        return node.ImportThumbnail(fs, quality);
    }
}
