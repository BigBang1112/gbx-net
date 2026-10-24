namespace GBX.NET.Engines.Plug;

public partial class CPlugSolid
{
    public partial class FileImageData
    {
        /// <summary>
        /// Size of all stored mip levels, cube faces, and array layers in bytes.
        /// </summary>
        public int PixelDataLength
        {
            get
            {
                if (Size.X <= 0 || Size.Y <= 0 || Size.Z <= 0)
                {
                    throw new InvalidDataException("Image dimensions must be positive.");
                }

                var format = (Flags >> 5) & 0x1F;
                var bytesPerBlock = format switch
                {
                    1 or 10 => 8,
                    2 or 3 or 4 or 5 or 11 or 12 or 13 or 14 or 18 => 16,
                    _ => 0
                };
                var bytesPerPixel = format switch
                {
                    0 or 16 => (int)((Flags >> 2) & 7),
                    6 or 7 => (int)((Flags >> 2) & 7) * 2,
                    8 => (int)((Flags >> 2) & 7) * 4,
                    15 or 17 => 4,
                    _ => 0
                };
                if (bytesPerBlock == 0 && bytesPerPixel == 0)
                {
                    throw new NotSupportedException($"Unsupported image format: {format}.");
                }

                var dimension = Flags & 3;
                var width = Size.X;
                var height = Size.Y;
                var depth = dimension == 2 ? Size.Z : 1;
                var mipCount = (Flags >> 15) & 0x1F;
                var level = 0;
                long byteCount = 0;

                checked
                {
                    while (true)
                    {
                        byteCount += bytesPerBlock == 0
                            ? (long)width * height * depth * bytesPerPixel
                            : ((long)width + 3) / 4 * (((long)height + 3) / 4) * depth * bytesPerBlock;
                        level++;
                        if (level == mipCount || (width == 1 && height == 1 && depth == 1))
                        {
                            break;
                        }
                        width = Math.Max(1, width / 2);
                        height = Math.Max(1, height / 2);
                        depth = Math.Max(1, depth / 2);
                    }

                    if (dimension != 2) byteCount *= Size.Z;
                    if (dimension == 3) byteCount *= 6;
                    return (int)byteCount;
                }
            }
        }
    }
}
