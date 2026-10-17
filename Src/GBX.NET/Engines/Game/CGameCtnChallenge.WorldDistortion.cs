using GBX.NET.Engines.Plug;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnChallenge
{
    [Obsolete("Use WorldDistortScales instead.")]
    public Vec3 WorldDistortion
    {
        get => WorldDistortScales;
        set => WorldDistortScales = value;
    }

    private NodeRef<CPlugBitmap>? worldDistortBitmapRef;

    [Obsolete("This version 0 bitmap is discarded by the game. Use WorldDistortXYZ instead.")]
    public partial CPlugBitmap? WorldDistortBitmap
    {
        get => worldDistortBitmapRef?.Resolve();
        set => NodeRef.SetEmbedded(ref worldDistortBitmapRef, value);
    }

    public partial class WorldDistortTga
    {
        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            var imageIdLength = rw.Byte(rw.Reader is null ? checked((byte)(imageId?.Length ?? 0)) : default);
            rw.Byte(ref colorMapType);
            rw.Byte(ref imageType);
            rw.UInt16(ref colorMapFirstIndex);
            rw.UInt16(ref colorMapLength);
            rw.Byte(ref colorMapEntrySize);
            rw.UInt16(ref xOrigin);
            rw.UInt16(ref yOrigin);
            rw.UInt16(ref width);
            rw.UInt16(ref height);
            rw.Byte(ref pixelDepth);
            rw.Byte(ref imageDescriptor);
            rw.Data(ref imageId, imageIdLength);

            var bytesPerPixel = imageType switch
            {
                2 or 10 when pixelDepth is 24 or 32 => pixelDepth / 8,
                3 or 11 when pixelDepth == 8 => 1,
                _ => throw new NotSupportedException($"Unsupported world distortion TGA type {imageType} with {pixelDepth}-bit pixels.")
            };
            var pixelCount = (long)width * height;
            if (pixelCount > int.MaxValue / bytesPerPixel)
            {
                throw new InvalidDataException("World distortion TGA is too large.");
            }

            if (imageType is 2 or 3)
            {
                var byteCount = (int)pixelCount * bytesPerPixel;
                if (rw.Reader is null && rw.Writer is not null && encodedPixels?.Length != byteCount)
                {
                    throw new InvalidDataException("World distortion TGA pixel data does not match its dimensions.");
                }
                rw.Data(ref encodedPixels, byteCount);
                return;
            }

            using var savedPixels = rw.Reader is null ? new MemoryStream(encodedPixels ?? []) : null;
            using var savedReader = savedPixels is null ? null : new GbxReader(savedPixels);
            var reader = rw.Reader ?? savedReader!;
            using var capturedPixels = rw.Reader is null ? null : new MemoryStream();
            while (pixelCount > 0)
            {
                var packet = reader.ReadByte();
                var packetPixelCount = (packet & 0x7F) + 1;
                if (packetPixelCount > pixelCount)
                {
                    throw new InvalidDataException("World distortion TGA RLE packet exceeds the image dimensions.");
                }
                var packetByteCount = (packet >= 0x80 ? 1 : packetPixelCount) * bytesPerPixel;
                var pixels = reader.ReadData(packetByteCount);
                capturedPixels?.WriteByte(packet);
                capturedPixels?.Write(pixels, 0, pixels.Length);
                pixelCount -= packetPixelCount;
            }

            if (capturedPixels is not null)
            {
                encodedPixels = capturedPixels.ToArray();
            }
            else
            {
                if (savedPixels!.Position != savedPixels.Length)
                {
                    throw new InvalidDataException("World distortion TGA contains trailing pixel data.");
                }
            }
            rw.Writer?.WriteData(encodedPixels!, encodedPixels!.Length);
        }
    }
}
