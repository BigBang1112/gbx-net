namespace GBX.NET.Engines.Plug;

public partial class CPlugFileGen : IVersionable
{
    public int Version { get; set; }

    [Obsolete("Use IntParams instead.")]
    public int[]? U01 { get => IntParams; set => IntParams = value; }

    [Obsolete("Use ColorParams instead.")]
    public Vec4[]? U02 { get => ColorParams; set => ColorParams = value; }

    [Obsolete("Use RealParams instead.")]
    public float[]? U03 { get => RealParams; set => RealParams = value; }

    [Obsolete("Use Script instead.")]
    public string? U04 { get => Script; set => Script = value; }

    [Obsolete("Use Functions instead.")]
    public CFuncKeysReal[]? U05 { get => Functions; set => Functions = value; }

    void IReadableWritable.ReadWrite(GbxReaderWriter rw, int version) => ReadWrite(rw);

    public void Read(GbxReader r, int v = 0)
    {
        using var rw = new GbxReaderWriter(r);
        ReadWrite(rw);
    }

    public void Write(GbxWriter w, int v = 0)
    {
        using var rw = new GbxReaderWriter(w);
        ReadWrite(rw);
    }

    public override void ReadWrite(GbxReaderWriter rw)
    {
        if (rw.Reader is null)
        {
            if (Version < 0 || Version > 6)
            {
                throw new VersionNotSupportedException(Version);
            }

            // Versions 0 and 1 contain parameters that are converted on load.
            Version = Math.Max(Version, 2);
        }

        var prefix = Version | int.MinValue;
        rw.Int32(ref prefix);
        var legacy = prefix >= 0;
        Version = legacy ? 0 : prefix & int.MaxValue;

        if (Version > 6)
        {
            throw new VersionNotSupportedException(Version);
        }

        if (legacy)
        {
            GenKind = prefix;
        }

        ReadWrite(rw, legacy ? -1 : Version);

        if (rw.Reader is not null)
        {
            ApplyLegacyFixups();
        }
    }

    // Require the payload version so a call with only rw always processes the archive prefix.
    public void ReadWrite(GbxReaderWriter rw, int v)
    {
        if (v >= 0) rw.Int32(ref genKind);
        rw.Array<int>(ref intParams);
        rw.Array<Vec4>(ref colorParams);
        if (v >= 0) rw.Array<float>(ref realParams);
        if (v >= 3) rw.String(ref script);
        if (v >= 4) rw.ArrayNode<CFuncKeysReal>(ref functions!);
        if (v >= 6) rw.ListNodeRef(ref inputImages);
        if (GenKind == (int)EGenKind.Pixels) rw.Data(ref pixelData, PixelDataLength);
    }

    private void ApplyLegacyFixups()
    {
        if (Version == 0 && (GenKind == (int)EGenKind.Render || GenKind == (int)EGenKind.RenderCube))
        {
            var index = GenKind == (int)EGenKind.Render ? 2 : 1;
            if (IntParams is null || IntParams.Length <= index)
            {
                throw new InvalidDataException("Legacy render parameters are incomplete.");
            }

            IntParams[index] = IntParams[index] != 0 ? 4 : 3;
        }
        else if (Version == 1 && GenKind == (int)EGenKind.Pixels && IntParams is { Length: >= 5 })
        {
            IntParams[4] = IntParams[4] == 0 ? 1 : 0;
        }
    }

    private int PixelDataLength
    {
        get
        {
            if (IntParams is not { Length: >= 3 })
            {
                throw new InvalidDataException("Pixel generator parameters are incomplete.");
            }

            var volume = IntParams.Length >= 6;
            var width = IntParams[0];
            var height = IntParams[1];
            var depth = volume ? IntParams[2] : 1;
            var components = IntParams[volume ? 3 : 2];
            var formatIndex = volume ? 4 : 3;
            var format = IntParams.Length > formatIndex ? IntParams[formatIndex] : 0;
            var mipIndex = volume ? 5 : 4;
            var mipCount = IntParams.Length > mipIndex ? IntParams[mipIndex] & 31 : 1;
            if (Version == 1 && !volume && IntParams.Length >= 5)
            {
                mipCount = IntParams[4] == 0 ? 1 : 0;
            }

            if (width <= 0 || height <= 0 || depth <= 0 || components < 1 || components > 4)
            {
                throw new InvalidDataException("Invalid pixel generator dimensions or component count.");
            }

            // EPlugImageFormat stores DXT blocks, packed pixels, or separate components.
            var blockBytes = format switch
            {
                1 or 10 => 8,
                2 or 3 or 4 or 5 or 11 or 12 or 13 or 14 or 18 => 16,
                _ => 0
            };
            var pixelBytes = format switch
            {
                0 or 16 => components,
                6 or 7 => components * 2,
                8 => components * 4,
                15 or 17 => 4,
                _ when blockBytes != 0 => 0,
                _ => throw new NotSupportedException($"Pixel generator image format {format} is not supported.")
            };

            var size = 0;
            checked
            {
                for (var level = 0; ; level++)
                {
                    size += blockBytes == 0
                        ? width * height * depth * pixelBytes
                        : ((width + 3) / 4) * ((height + 3) / 4) * depth * blockBytes;

                    if ((mipCount != 0 && level + 1 >= mipCount) || (width == 1 && height == 1 && depth == 1))
                    {
                        return size;
                    }

                    width = Math.Max(width / 2, 1);
                    height = Math.Max(height / 2, 1);
                    depth = Math.Max(depth / 2, 1);
                }
            }
        }
    }
}
