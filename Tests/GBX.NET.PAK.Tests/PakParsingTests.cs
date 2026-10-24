using GBX.NET.Crypto;
using System.Buffers.Binary;
using System.Text;

namespace GBX.NET.PAK.Tests;

public class PakParsingTests
{
    private const int DataStart = 4096;
    private const uint UnknownFileValue = 0xFEDCBA98;
    private static readonly byte[] Payload = Encoding.ASCII.GetBytes("Pak file content");
    private static readonly byte[] Salt = Convert.FromHexString("1032547698BADCFE0123456789ABCDEF");
    private static readonly byte[] EncryptionKey = Convert.FromHexString("FFEEDDCCBBAA99887766554433221100");
    private static readonly byte[] DefaultHeaderKey = Convert.FromHexString("56EECBBBDEB6BC90A17DFCEB761D59CE");

    [Test]
    [Arguments(7)]
    [Arguments(8)]
    [Arguments(9)]
    [Arguments(10)]
    [Arguments(11)]
    [Arguments(12)]
    [Arguments(13)]
    [Arguments(14)]
    [Arguments(15)]
    [Arguments(16)]
    [Arguments(17)]
    [Arguments(18)]
    public async Task Parse_PublicHeader_PreservesVersionedFields(int version)
    {
        using var stream = CreatePak(version, headerFlags: 0, salted: false, publicFile: false);
        using var pak = await Pak.ParseAsync(stream);
        var pak6 = (Pak6)pak;
        var file = pak.Files["Test.bin"];

        await Assert.That(pak.IsHeaderEncrypted).IsFalse();
        await Assert.That(pak.HeaderMaxSize).IsEqualTo(DataStart);
        await Assert.That(pak.PackMask).IsEqualTo((UInt128?)0x1122334455667788);
        await Assert.That(pak.GbxHeadersChecksum).IsEqualTo(version >= 14 ? (UInt128?)0x8877665544332211 : null);
        await Assert.That(pak.Size).IsEqualTo(version >= 16 ? (uint?)(DataStart + Payload.Length) : null);
        await Assert.That(file.U01).IsEqualTo(UnknownFileValue);
        await Assert.That(file.Size).IsEqualTo(version >= 17 ? (int?)64 : null);
        await Assert.That(file.Checksum).IsEqualTo(version >= 14 ? (UInt128?)0x123456789ABCDEF0 : null);
        await Assert.That(pak6.IncludedPacks.Length).IsEqualTo(version >= 10 ? 1 : 0);

        if (version >= 10)
        {
            await Assert.That(pak6.IncludedPacks[0].Name).IsEqualTo("Included pack");
            await Assert.That(pak6.IncludedPacks[0].IncludeDepth).IsEqualTo(version >= 11 ? 3U : 0U);
        }

        using var data = pak.OpenFile(file, out _);
        var actual = new byte[Payload.Length];
        await data.ReadExactlyAsync(actual);
        await Assert.That(Convert.ToHexString(actual)).IsEqualTo(Convert.ToHexString(Payload));
    }

    [Test]
    [Arguments(7, 0U, true, false)]
    [Arguments(8, 2U, true, true)]
    [Arguments(12, 5U, true, false)]
    [Arguments(17, 7U, true, true)]
    [Arguments(18, 0U, true, true)]
    [Arguments(18, 2U, true, false)]
    [Arguments(18, 5U, true, true)]
    [Arguments(18, 7U, true, false)]
    [Arguments(12, 5U, false, true)]
    [Arguments(17, 7U, false, false)]
    [Arguments(18, 2U, false, true)]
    [Arguments(18, 7U, false, false)]
    public async Task Parse_EncryptionKeys_DecryptsHeaderAndData(int version, uint headerFlags, bool salted, bool synchronous)
    {
        using var stream = CreatePak(version, headerFlags, salted, publicFile: false);
        var key = (headerFlags & 4) != 0 ? (byte[])EncryptionKey.Clone() : null;
        using var pak = synchronous
            ? Pak.Parse(stream, key, KeyType.EncryptionKey)
            : await Pak.ParseAsync(stream, key, KeyType.EncryptionKey);

        await Assert.That(pak.IsHeaderEncrypted).IsTrue();
        var expectedSalt = Convert.ToHexString(salted ? Salt : new byte[16]);
        await Assert.That(Convert.ToHexString(pak.KeySalt!)).IsEqualTo(expectedSalt);
#pragma warning disable CS0618 // Verify the compatibility alias.
        await Assert.That(Convert.ToHexString(((Pak6)pak).U01!)).IsEqualTo(expectedSalt);
#pragma warning restore CS0618

        // Opening twice also verifies that salting never mutates the stored or caller's key.
        for (var i = 0; i < 2; i++)
        {
            using var data = pak.OpenFile(pak.Files["Test.bin"], out _);
            var actual = new byte[Payload.Length];
            await data.ReadExactlyAsync(actual);
            await Assert.That(Convert.ToHexString(actual)).IsEqualTo(Convert.ToHexString(Payload));
        }

        if (key is not null)
        {
            await Assert.That(Convert.ToHexString(key)).IsEqualTo(Convert.ToHexString(EncryptionKey));
        }
    }

    [Test]
    public async Task Parse_SaltedPack_PublicFileRemainsReadable()
    {
        using var stream = CreatePak(18, headerFlags: 2, salted: true, publicFile: true);
        using var pak = await Pak.ParseAsync(stream);
        using var data = pak.OpenFile(pak.Files["Test.bin"], out _);
        var actual = new byte[Payload.Length];
        await data.ReadExactlyAsync(actual);
        await Assert.That(Convert.ToHexString(actual)).IsEqualTo(Convert.ToHexString(Payload));
    }

    private static MemoryStream CreatePak(int version, uint headerFlags, bool salted, bool publicFile)
    {
        var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write(Encoding.ASCII.GetBytes(Pak.Magic));
        writer.Write(version);
        writer.Write(new byte[32]);
        writer.Write(headerFlags);

        if (version >= 15)
        {
            writer.Write(DataStart);
        }

        WriteAuthor(writer);

        if (version < 9)
        {
            WriteString(writer, "Comments");
            writer.Write(salted ? Salt : new byte[16]);

            if (version == 8)
            {
                WriteString(writer, "Build");
                WriteString(writer, "Author URL");
            }
        }
        else
        {
            WriteString(writer, "Manialink");

            if (version >= 13)
            {
                WriteString(writer, "Download");
            }

            writer.Write(0L);
            WriteString(writer, "Comments");

            if (version >= 12)
            {
                WriteString(writer, "<header/>");
                WriteString(writer, "Title");
            }

            WriteString(writer, "Usage");
            WriteString(writer, "Build");
            writer.Write(salted ? Salt : new byte[16]);

            if (version >= 10)
            {
                writer.Write(1);
                writer.Write(new byte[32]);
                WriteString(writer, "First name");
                WriteAuthor(writer);
                WriteString(writer, "Info URL");
                writer.Write(0L);
                WriteString(writer, "Included pack");

                if (version >= 11)
                {
                    writer.Write(3U);
                }
            }
        }

        using var header = new MemoryStream();
        using (var headerWriter = new BinaryWriter(header, Encoding.UTF8, leaveOpen: true))
        {
            headerWriter.Write(new byte[16]);
            headerWriter.Write(0U);

            if (version < 15)
            {
                headerWriter.Write(DataStart);
            }

            headerWriter.Write(0);
            headerWriter.Write(0);

            if (version >= 14)
            {
                WriteUInt128(headerWriter, 0x8877665544332211);
            }

            if (version >= 16)
            {
                headerWriter.Write((uint)(DataStart + Payload.Length));
            }

            WriteUInt128(headerWriter, 0x1122334455667788);
            headerWriter.Write(0U);
            headerWriter.Write(0); // No folders, so no folder-name IV initialization.
            headerWriter.Write(1);
            headerWriter.Write(-1);
            WriteString(headerWriter, "Test.bin");
            headerWriter.Write(UnknownFileValue);
            headerWriter.Write(Payload.Length);
            headerWriter.Write(Payload.Length);
            headerWriter.Write(0U);
            headerWriter.Write(0x09003000U);

            if (version >= 17)
            {
                headerWriter.Write(64);
            }

            if (version >= 14)
            {
                WriteUInt128(headerWriter, 0x123456789ABCDEF0);
            }

            headerWriter.Write(publicFile ? 0x2000000000000UL : 0UL);
        }

        var headerKey = new byte[16];
        var dataKey = new byte[16];

        for (var i = 0; i < 16; i++)
        {
            headerKey[i] = (byte)(((headerFlags & 1) != 0 ? EncryptionKey[i] : 0)
                ^ ((headerFlags & 2) != 0 ? DefaultHeaderKey[i] : 0)
                ^ (salted ? Salt[i] : 0));
            dataKey[i] = (byte)(((headerFlags & 4) != 0 ? EncryptionKey[i] : 0) ^ (salted ? Salt[i] : 0));
        }

        writer.Write(salted || (headerFlags & 3) != 0 ? Encrypt(header.ToArray(), headerKey, version) : header.ToArray());
        stream.SetLength(DataStart);
        stream.Position = DataStart;
        writer.Write(!publicFile && (salted || (headerFlags & 4) != 0) ? Encrypt(Payload, dataKey, version) : Payload);
        stream.Position = 0;
        return stream;
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

    private static void WriteString(BinaryWriter writer, string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        writer.Write(bytes.Length);
        writer.Write(bytes);
    }

    private static void WriteAuthor(BinaryWriter writer)
    {
        writer.Write(1);
        WriteString(writer, "Author");
        WriteString(writer, "Nickname");
        WriteString(writer, "Zone");
        WriteString(writer, "Extra");
    }

    private static void WriteUInt128(BinaryWriter writer, ulong value)
    {
        writer.Write(value);
        writer.Write(0UL);
    }
}
