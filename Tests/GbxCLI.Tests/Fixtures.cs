using GBX.NET;
using GBX.NET.Crypto;
using GBX.NET.Engines.Game;
using GBX.NET.PAK;
using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;

namespace GbxCLI.Tests;

/// <summary>
/// Builds small Gbx, PAK and ZIP files for the tests.
/// </summary>
internal static class Fixtures
{
    public const string TMNextKey = "5F4FC43233B989DAED3918C1190F4A5A";

    /// <summary>
    /// A Gbx header with a body that cannot be parsed.
    /// </summary>
    public static byte[] BrokenGbx => [.. "GBX"u8, 6, 0, (byte)'B'];

    public static byte[] CreateGbx(bool compressed = false)
    {
        Gbx.LZO = new GBX.NET.LZO.Lzo();

        var node = new CGameCtnMediaBlockTime
        {
            Keys = [new() { Time = new(1.5f), TimeValue = 42.5f, Tangent = 0.25f }]
        };
        node.CreateChunk<CGameCtnMediaBlockTime.Chunk03085000>();

        using var stream = new MemoryStream();
        new Gbx<CGameCtnMediaBlockTime>(node)
        {
            BodyCompression = compressed ? GbxCompression.Compressed : GbxCompression.Uncompressed
        }.Save(stream);

        return stream.ToArray();
    }

    public static byte[] CreateZip(params (string Name, byte[] Contents)[] entries)
    {
        using var stream = new MemoryStream();

        using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var (name, contents) in entries)
            {
                using var entryStream = zip.CreateEntry(name).Open();
                entryStream.Write(contents);
            }
        }

        return stream.ToArray();
    }

    /// <summary>
    /// A version 6 PAK with an unencrypted header and one empty entry, <c>sample [blue].txt</c>.
    /// </summary>
    public static byte[] CreatePakData(bool privateHeader = false)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write("NadeoPak"u8);
        writer.Write(6);
        writer.Write(new byte[32]);
        writer.Write(privateHeader ? 1u : 0u);

        if (privateHeader)
        {
            return stream.ToArray();
        }

        writer.Write(new byte[16]);
        writer.Write(0u);
        writer.Write(0);
        writer.Write(0);
        writer.Write(0);
        writer.Write(new byte[16]);
        writer.Write(0);
        WritePakString(writer, "author [red]");
        WritePakString(writer, "Author");
        WritePakString(writer, string.Empty);
        WritePakString(writer, string.Empty);
        writer.Write(0u);
        writer.Write(0);
        writer.Write(1);
        writer.Write(-1);
        WritePakString(writer, "sample [blue].txt");
        writer.Write(0);
        writer.Write(0);
        writer.Write(0);
        writer.Write(0u);
        writer.Write(0u);
        writer.Write(0x4000000000000UL);
        return stream.ToArray();
    }

    /// <summary>
    /// A version 6 PAK with an unencrypted header and unencrypted entries.
    /// </summary>
    public static byte[] CreatePak(bool compressed, params (string Name, byte[] Contents)[] files)
    {
        var original = CreatePakData();
        var fileCountOffset = original.Length - (36 + Encoding.UTF8.GetByteCount("sample [blue].txt")) - 4;
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write(original.AsSpan(0, fileCountOffset));
        stream.Position = 68; // Version 6 data start.
        writer.Write(4096);
        stream.Position = fileCountOffset;
        writer.Write(files.Length);
        var offset = 0U;
        var payloads = new List<byte[]>();

        foreach (var (name, contents) in files)
        {
            using var payload = new MemoryStream();

            if (compressed)
            {
                using var compressor = new ZLibStream(payload, CompressionLevel.Fastest, leaveOpen: true);
                compressor.Write(contents);
            }
            else
            {
                payload.Write(contents);
            }

            var bytes = payload.ToArray();
            payloads.Add(bytes);
            writer.Write(-1);
            var nameBytes = Encoding.UTF8.GetBytes(name);
            writer.Write(nameBytes.Length);
            writer.Write(nameBytes);
            writer.Write(0U);
            writer.Write(contents.Length);
            writer.Write(bytes.Length);
            writer.Write(offset);
            writer.Write(0U);
            writer.Write(0x4000000000000UL | (compressed ? 4UL : 0UL));
            offset += (uint)bytes.Length;
        }

        stream.SetLength(4096);
        stream.Position = 4096;

        foreach (var payload in payloads)
        {
            writer.Write(payload);
        }

        // Padding after the last entry, which extraction must not include.
        writer.Write(new byte[16]);

        return stream.ToArray();
    }

    /// <summary>
    /// A PAK encrypted with <paramref name="baseKey"/>, with one encrypted entry.
    /// </summary>
    /// <param name="titleId">Title ID in the public header, which version 18 stores.</param>
    public static byte[] CreateEncryptedPak(string baseKey, bool publicHeader, int version = 6, byte[]? fileContents = null, string entryPath = "sample [blue].txt", string titleId = "")
    {
        fileContents ??= "Built-in Pak key"u8.ToArray();
        var original = CreatePakData();
        var header = original.AsSpan(48).ToArray();

        if (entryPath != "sample [blue].txt")
        {
            var nameOffset = header.Length - Encoding.UTF8.GetByteCount("sample [blue].txt") - 32;
            using var renamedHeader = new MemoryStream();
            using var nameWriter = new BinaryWriter(renamedHeader, Encoding.UTF8, leaveOpen: true);
            nameWriter.Write(header.AsSpan(0, nameOffset));
            var nameBytes = Encoding.UTF8.GetBytes(entryPath);
            nameWriter.Write(nameBytes.Length);
            nameWriter.Write(nameBytes);
            nameWriter.Write(header.AsSpan(header.Length - 28));
            header = renamedHeader.ToArray();
        }

        BinaryPrimitives.WriteInt32LittleEndian(header.AsSpan(20), 4096);
        BinaryPrimitives.WriteInt32LittleEndian(header.AsSpan(header.Length - 24), fileContents.Length);
        BinaryPrimitives.WriteInt32LittleEndian(header.AsSpan(header.Length - 20), fileContents.Length);
        BinaryPrimitives.WriteUInt64LittleEndian(header.AsSpan(header.Length - 8), 0);
        var key = Pak.ComputeKey(Convert.FromHexString(baseKey));
        var salt = version >= 18 && !publicHeader ? Convert.FromHexString("1032547698BADCFE0123456789ABCDEF") : new byte[16];

        using var headerReader = new BinaryReader(new MemoryStream(header));
        headerReader.BaseStream.Position = 48;
        headerReader.ReadInt32();

        for (var i = 0; i < 4; i++)
        {
            headerReader.ReadBytes(headerReader.ReadInt32());
        }

        var authorEnd = (int)headerReader.BaseStream.Position;
        var author = header.AsSpan(48, authorEnd - 48).ToArray();

        if (version >= 18)
        {
            using var modernHeader = new MemoryStream();
            using var headerWriter = new BinaryWriter(modernHeader, Encoding.UTF8, leaveOpen: true);
            headerWriter.Write(new byte[16]); // Header MD5.
            headerWriter.Write(0U); // Gbx headers offset.
            headerWriter.Write(0UL); // Gbx header sizes.
            headerWriter.Write(new byte[16]); // Gbx headers checksum.
            headerWriter.Write(4096U + (uint)fileContents.Length + 8); // Archive size.
            headerWriter.Write(new byte[16]); // Pack mask.
            headerWriter.Write(header.AsSpan(authorEnd, header.Length - authorEnd - 8));
            headerWriter.Write(64); // Stored file size.
            headerWriter.Write(new byte[16]); // File checksum.
            headerWriter.Write(0UL); // File flags.
            header = modernHeader.ToArray();
        }

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write("NadeoPak"u8);
        writer.Write(version);
        writer.Write(new byte[32]);
        writer.Write(publicHeader ? 4U : 7U);

        if (version >= 18)
        {
            writer.Write(4096);
            writer.Write(author);
            writer.Write(0); // Manialink URL.
            writer.Write(0); // Download URL.
            writer.Write(0L); // Creation date.

            writer.Write(0); // Comments.
            writer.Write(0); // XML.
            WritePakString(writer, titleId);
            writer.Write(0); // Usage directory.
            writer.Write(0); // Build.

            writer.Write(salt);
            writer.Write(0); // Included packs.
        }

        if (publicHeader)
        {
            writer.Write(header);
        }
        else
        {
            var headerKey = (byte[])key.Clone();
            var defaultHeaderKey = Convert.FromHexString("56EECBBBDEB6BC90A17DFCEB761D59CE");

            for (var i = 0; i < headerKey.Length; i++)
            {
                headerKey[i] ^= (byte)(defaultHeaderKey[i] ^ salt[i]);
            }

            writer.Write(Encrypt(header, headerKey, version));
        }

        stream.SetLength(4096);
        stream.Position = 4096;

        for (var i = 0; i < key.Length; i++)
        {
            key[i] ^= salt[i];
        }

        writer.Write(Encrypt(fileContents, key, version));
        return stream.ToArray();
    }

    private static byte[] Encrypt(byte[] plaintext, byte[] key, int version)
    {
        var output = new byte[8 + (plaintext.Length + 7) / 8 * 8];
        plaintext.CopyTo(output, 8);
        var iv = 0x0123456789ABCDEFUL;
        BinaryPrimitives.WriteUInt64LittleEndian(output, iv);
        var blowfish = new Blowfish(key, version >= 18 ? BlowfishTrick.LittleEndianPak18 : BlowfishTrick.LittleEndian);

        for (var offset = 8; offset < output.Length; offset += 8)
        {
            var block = output.AsSpan(offset, 8);
            BinaryPrimitives.WriteUInt64LittleEndian(block, BinaryPrimitives.ReadUInt64LittleEndian(block) ^ iv);

            if (version >= 18)
            {
                blowfish.Decrypt(block);
            }
            else
            {
                blowfish.Encrypt(block);
            }

            var ciphertext = BinaryPrimitives.ReadUInt64LittleEndian(block);
            iv = version >= 12 ? (iv >> 47) ^ (iv * 9) ^ ciphertext : ciphertext;
        }

        return output;
    }

    private static void WritePakString(BinaryWriter writer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        writer.Write(bytes.Length);
        writer.Write(bytes);
    }
}
