using System.Runtime.InteropServices;
using GBX.NET.Engines.GameData;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace GBX.NET.Imaging.ImageSharp;

/// <summary>
/// Imaging extensions for <see cref="CGameCtnCollector"/>.
/// </summary>
public static partial class CGameCtnCollectorExtensions
{
    /// <summary>
    /// Gets the collector's icon as <see cref="Image"/>.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Icon as <see cref="Image"/>. Null if <see cref="CGameCtnCollector.Icon"/> and <see cref="CGameCtnCollector.IconWebP"/> is <see langword="null" />.</returns>
    public static async ValueTask<Image?> GetIconImageAsync(this CGameCtnCollector node, CancellationToken cancellationToken = default)
    {
        if (node.Icon is not null)
        {
            return GetIconImage(node.Icon);
        }

        if (node.IconWebP is null)
        {
            return null;
        }

        await using var ms = new MemoryStream(node.IconWebP);
        var webpBitmap = await Image.LoadAsync(ms, cancellationToken);

        webpBitmap.Mutate(x =>
        {
            x.Rotate(RotateMode.Rotate180);
            x.Flip(FlipMode.Horizontal);
        });

        return webpBitmap;
    }

    /// <summary>
    /// Gets the collector's icon as <see cref="Image"/>.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Icon as <see cref="Image"/>. Null if <see cref="CGameCtnCollector.Icon"/> and <see cref="CGameCtnCollector.IconWebP"/> is <see langword="null" />.</returns>
    public static Image? GetIconImage(this CGameCtnCollector node)
    {
        if (node.Icon is not null)
        {
            return GetIconImage(node.Icon);
        }

        if (node.IconWebP is null)
        {
            return null;
        }

        var webpBitmap = Image.Load(node.IconWebP);

        webpBitmap.Mutate(x =>
        {
            x.Rotate(RotateMode.Rotate180);
            x.Flip(FlipMode.Horizontal);
        });

        return webpBitmap;
    }

    /// <summary>
    /// Exports the collector's icon.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <param name="stream">Stream to export to.</param>
    /// <param name="encoder">Image encoder to use.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if successful. False if <see cref="CGameCtnCollector.Icon"/> and <see cref="CGameCtnCollector.IconWebP"/> is <see langword="null" />.</returns>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<bool> ExportIconAsync(this CGameCtnCollector node, Stream stream, IImageEncoder encoder, CancellationToken cancellationToken = default)
    {
        using var icon = await node.GetIconImageAsync(cancellationToken);

        if (icon is null)
        {
            return false;
        }

        await icon.SaveAsync(stream, encoder, cancellationToken);

        return true;
    }

    /// <summary>
    /// Exports the collector's icon as PNG.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <param name="stream">Stream to export to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if successful. False if <see cref="CGameCtnCollector.Icon"/> and <see cref="CGameCtnCollector.IconWebP"/> is <see langword="null" />.</returns>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<bool> ExportIconAsync(this CGameCtnCollector node, Stream stream, CancellationToken cancellationToken = default)
    {
        return await ExportIconAsync(node, stream, new PngEncoder(), cancellationToken);
    }

    /// <summary>
    /// Exports the collector's icon.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <param name="fileName">File to export to.</param>
    /// <param name="encoder">Image encoder to use.</param>
    /// <returns>True if successful. False if <see cref="CGameCtnCollector.Icon"/> and <see cref="CGameCtnCollector.IconWebP"/> is <see langword="null" />.</returns>
    public static bool ExportIcon(this CGameCtnCollector node, string fileName, IImageEncoder encoder)
    {
        if (node.Icon is null && node.IconWebP is null)
        {
            return false;
        }

        using var fs = File.Create(fileName);
        return node.ExportIcon(fs, encoder);
    }

    /// <summary>
    /// Exports the collector's icon.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <param name="fileName">File to export to.</param>
    /// <param name="encoder">Image encoder to use.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if successful. False if <see cref="CGameCtnCollector.Icon"/> and <see cref="CGameCtnCollector.IconWebP"/> is <see langword="null" />.</returns>
    public static async ValueTask<bool> ExportIconAsync(this CGameCtnCollector node, string fileName, IImageEncoder encoder, CancellationToken cancellationToken = default)
    {
        if (node.Icon is null && node.IconWebP is null)
        {
            return false;
        }

        await using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        return await node.ExportIconAsync(fs, encoder, cancellationToken);
    }

    /// <summary>
    /// Exports the collector's icon as PNG.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <param name="fileName">File to export to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if successful. False if <see cref="CGameCtnCollector.Icon"/> and <see cref="CGameCtnCollector.IconWebP"/> is <see langword="null" />.</returns>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<bool> ExportIconAsync(this CGameCtnCollector node, string fileName, CancellationToken cancellationToken = default)
    {
        return await ExportIconAsync(node, fileName, new PngEncoder(), cancellationToken);
    }

    /// <summary>
    /// Replaces the collector's raw RGB icon with a WebP encoded icon. WebP is only accepted in TM2020.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if successful. False if <see cref="CGameCtnCollector.Icon"/> is <see langword="null" />.</returns>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async ValueTask<bool> UpgradeIconToWebPAsync(this CGameCtnCollector node, CancellationToken cancellationToken = default)
    {
        if (node.Icon is null)
        {
            return false;
        }

        using var image = GetFlippedIconImage(node.Icon);
        await using var iconStream = new MemoryStream();
        await image.SaveAsWebpAsync(iconStream, cancellationToken);
        node.IconWebP = iconStream.ToArray();
        node.Icon = null;
        return true;
    }

    // public static bool DowngradeIconToRaw(this CGameCtnCollector node)

    /// <summary>
    /// Replaces an icon (any popular image format) to use for the collector.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <param name="stream">Stream to import from.</param>
    /// <param name="webp">If icon should be imported as WebP, which is used in TM2020 since April 2022.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<Image> ImportIconAsync(this CGameCtnCollector node, Stream stream, bool webp = false, CancellationToken cancellationToken = default)
    {
        using var image = await Image.LoadAsync<Rgba32>(stream, cancellationToken);

        image.Mutate(x =>
        {
            x.Rotate(RotateMode.Rotate180);
            x.Flip(FlipMode.Horizontal);
        });

        await using var ms = new MemoryStream();

        if (webp)
        {
            await image.SaveAsWebpAsync(ms, cancellationToken);
            node.IconWebP = ms.ToArray();
            return image;
        }

        var width = image.Width;
        var height = image.Height;
        var data = new Color[width, height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                data[x, y] = new((int)image[x, height - y - 1].Rgba);
            }
        }

        node.Icon = data;

        return image;
    }

    /// <summary>
    /// Replaces an icon (any popular image format) to use for the collector.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="fileName">File to import from.</param>
    /// <param name="webp">If icon should be imported as WebP, which is used in TM2020 since April 2022.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public static async Task<Image> ImportIconAsync(this CGameCtnCollector node, string fileName, bool webp = false, CancellationToken cancellationToken = default)
    {
        await using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
        return await node.ImportIconAsync(fs, webp, cancellationToken);
    }

    /// <summary>
    /// Replaces an icon (any popular image format) to use for the collector.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="fileName">File to import from.</param>
    /// <param name="webp">If icon should be imported as WebP, which is used in TM2020 since April 2022.</param>
    public static Image ImportIcon(this CGameCtnCollector node, string fileName, bool webp = false)
    {
        using var fs = File.OpenRead(fileName);
        return node.ImportIcon(fs, webp);
    }

    private static Image<Rgba32> GetImage(int[] data, int width, int height)
    {
        var image = new Image<Rgba32>(width, height);

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                image[x, y] = new Rgba32((uint)data[y * width + x]);
            }
        }

        return image;
    }

    private static Image<Rgba32>? GetIconImage(Color[,] icon)
    {
        var width = icon.GetLength(0);
        var height = icon.GetLength(1);

        var data = new int[width * height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                data[y * width + x] = icon[x, y].ToArgb();
            }
        }

        return GetImage(data, width, height);
    }

    private static Image<Rgba32> GetFlippedIconImage(Color[,] icon)
    {
        int width = icon.GetLength(0);
        int height = icon.GetLength(1);
        int[] array = new int[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                array[y * width + x] = icon[x, height - y - 1].ToArgb();
            }
        }

        return GetImage(array, width, height);
    }
}
