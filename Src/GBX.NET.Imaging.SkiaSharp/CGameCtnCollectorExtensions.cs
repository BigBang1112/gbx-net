﻿using System.Runtime.InteropServices;
using GBX.NET.Engines.GameData;
using SkiaSharp;

namespace GBX.NET.Imaging.SkiaSharp;

/// <summary>
/// Imaging extensions for <see cref="CGameCtnCollector"/>.
/// </summary>
public static class CGameCtnCollectorExtensions
{
    /// <summary>
    /// Gets the collector's icon as <see cref="SKBitmap"/>.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <returns>Icon as <see cref="SKBitmap"/>. Null if <see cref="CGameCtnCollector.Icon"/> and <see cref="CGameCtnCollector.IconWebP"/> is <see langword="null" />.</returns>
    public static SKBitmap? GetIconBitmap(this CGameCtnCollector node)
    {
        if (node.Icon is null)
        {
            if (node.IconWebP is null)
            {
                return null;
            }

            using var webpBitmap = SKBitmap.Decode(node.IconWebP);
            return webpBitmap.Rotate180FlipX();
        }

        var width = node.Icon.GetLength(0);
        var height = node.Icon.GetLength(1);

        var data = new int[width * height];

#if NET6_0_OR_GREATER
        if (RuntimeInformation.ProcessArchitecture == Architecture.Wasm)
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    data[y * width + x] = node.Icon[x, y].ToRgba();
                }
            }
        }
        else
        {
#endif
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    data[y * width + x] = node.Icon[x, y].ToArgb();
                }
            }
#if NET6_0_OR_GREATER
        }
#endif

        return GetBitmap(data, width, height);
    }

    /// <summary>
    /// Exports the collector's icon.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <param name="stream">Stream to export to.</param>
    /// <param name="format">Image format to use.</param>
    /// <param name="quality">The quality level to use for the image. This is in the range from 0-100. Not all formats (for example, PNG) respect or support it.</param>
    /// <returns>True if successful. False if <see cref="CGameCtnCollector.Icon"/> and <see cref="CGameCtnCollector.IconWebP"/> is <see langword="null" />.</returns>
    public static bool ExportIcon(this CGameCtnCollector node, Stream stream, SKEncodedImageFormat format, int quality)
    {
        using var icon = node.GetIconBitmap();
        return icon is not null && icon.Encode(stream, format, quality);
    }

    /// <summary>
    /// Exports the collector's icon as PNG.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <param name="stream">Stream to export to.</param>
    /// <returns>True if successful. False if <see cref="CGameCtnCollector.Icon"/> and <see cref="CGameCtnCollector.IconWebP"/> is <see langword="null" />.</returns>
    public static bool ExportIcon(this CGameCtnCollector node, Stream stream)
    {
        return ExportIcon(node, stream, SKEncodedImageFormat.Png, 100);
    }

    /// <summary>
    /// Exports the collector's icon.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <param name="fileName">File to export to.</param>
    /// <param name="format">Image format to use.</param>
    /// <param name="quality">The quality level to use for the image. This is in the range from 0-100. Not all formats (for example, PNG) respect or support it.</param>
    /// <returns>True if successful. False if <see cref="CGameCtnCollector.Icon"/> and <see cref="CGameCtnCollector.IconWebP"/> is <see langword="null" />.</returns>
    public static bool ExportIcon(this CGameCtnCollector node, string fileName, SKEncodedImageFormat format, int quality)
    {
        if (node.Icon is null && node.IconWebP is null)
        {
            return false;
        }

        using var fs = File.Create(fileName);
        return node.ExportIcon(fs, format, quality);
    }

    /// <summary>
    /// Exports the collector's icon as PNG.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <param name="fileName">File to export to.</param>
    /// <returns>True if successful. False if <see cref="CGameCtnCollector.Icon"/> and <see cref="CGameCtnCollector.IconWebP"/> is <see langword="null" />.</returns>
    public static bool ExportIcon(this CGameCtnCollector node, string fileName)
    {
        return ExportIcon(node, fileName, SKEncodedImageFormat.Png, 100);
    }

    /// <summary>
    /// Replaces the collector's raw RGB icon with a WebP encoded icon. WebP is only accepted in TM2020.
    /// </summary>
    /// <param name="node">CGameCtnCollector</param>
    /// <returns>True if successful. False if <see cref="CGameCtnCollector.Icon"/> is <see langword="null" />.</returns>
    public static bool UpgradeIconToWebP(this CGameCtnCollector node)
    {
        if (node.Icon is null)
        {
            return false;
        }

        int width = node.Icon.GetLength(0);
        int height = node.Icon.GetLength(1);
        int[] array = new int[width * height];

#if NET6_0_OR_GREATER
        if (RuntimeInformation.ProcessArchitecture == Architecture.Wasm)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    array[y * width + x] = node.Icon[x, height - y - 1].ToRgba();
                }
            }
        }
        else
        {
#endif
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    array[y * width + x] = node.Icon[x, height - y - 1].ToArgb();
                }
            }
#if NET6_0_OR_GREATER
        }
#endif

        using var bitmap = GetBitmap(array, width, height);
        using var iconStream = new MemoryStream();
        bitmap.Encode(iconStream, SKEncodedImageFormat.Webp, 100);
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
    public static SKBitmap ImportIcon(this CGameCtnCollector node, Stream stream, bool webp = false)
    {
        using var bitmap = SKBitmap.Decode(stream);
        using var rotated = bitmap.Rotate180FlipX();
        using var ms = new MemoryStream();

        if (webp)
        {
            rotated.Encode(ms, SKEncodedImageFormat.Webp, 100);
            node.IconWebP = ms.ToArray();
            return rotated;
        }

        // later replace with GetPixels
        var width = rotated.Width;
        var height = rotated.Height;
        var data = new Color[width, height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                data[x, y] = new Color((int)(uint)rotated.GetPixel(x, y));
            }
        }

        node.Icon = data;

        return rotated;
    }

    /// <summary>
    /// Replaces an icon (any popular image format) to use for the collector.
    /// </summary>
    /// <param name="node">CGameCtnChallenge</param>
    /// <param name="fileName">File to import from.</param>
    /// <param name="webp">If icon should be imported as WebP, which is used in TM2020 since April 2022.</param>
    public static SKBitmap ImportIcon(this CGameCtnCollector node, string fileName, bool webp = false)
    {
        using var fs = File.OpenRead(fileName);
        return node.ImportIcon(fs, webp);
    }

    private static SKBitmap GetBitmap(int[] data, int width, int height)
    {
        var bitmap = new SKBitmap();

        var gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);

        // install the pixels with the color type of the pixel data
        var info = new SKImageInfo(width, height, SKImageInfo.PlatformColorType, SKAlphaType.Unpremul);
        bitmap.InstallPixels(info, gcHandle.AddrOfPinnedObject(), info.RowBytes, delegate { gcHandle.Free(); });
        
        return bitmap;
    }
}
