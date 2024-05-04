using SkiaSharp;

namespace GBX.NET.Imaging.SkiaSharp;

internal static class SKBitmapExtensions
{
    public static SKBitmap Rotate180FlipX(this SKBitmap bitmap)
    {
        var rotated = new SKBitmap(bitmap.Width, bitmap.Height);

        using (var surface = new SKCanvas(rotated))
        {
            surface.Translate(rotated.Width, rotated.Height);
            surface.RotateDegrees(180);
            surface.Scale(-1, 1, rotated.Width / 2f, 0);
            surface.DrawBitmap(bitmap, 0, 0);
        }

        return rotated;
    }
}
