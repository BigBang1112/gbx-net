using GBX.NET.Crypto;
using GBX.NET.PAK;
using System.Buffers.Binary;
using System.Text;

namespace GbxCLI.Tests;

public class PakLoaderTests
{
    private const string TMNextKey = "5F4FC43233B989DAED3918C1190F4A5A";
    private static readonly byte[] payload = Encoding.ASCII.GetBytes("Built-in Pak key");

    [Test]
    [Arguments("TMNext.pak", TMNextKey, false, 6)]
    [Arguments("tmNEXT.PAK", TMNextKey, false, 6)]
    [Arguments("TMNext.pak", TMNextKey, true, 6)]
    [Arguments("TMNext_2014-04-30.pak", TMNextKey, false, 6)]
    [Arguments("Resource.pak", "087480148E51B70DD83AF2D86F974FEF", false, 6)]
    [Arguments("Resource.pak", "BE3BFB3192A3D3143E0CA1F1DA916C8A", false, 6)]
    [Arguments("Game.pak", "9A9A3A018B35FC65304247396A894B05", false, 6)]
    [Arguments("Game.pak", "EB2EA8E276999116DEE4CAC13E97354D", false, 6)]
    [Arguments("Stadium.pak", "21DA8E75B00B33FD68EFA7182FD2163F", false, 6)]
    [Arguments("Stadium.pak", "5B985F4CC43013C458A4C99A4B285E1E", false, 6)]
    [Arguments("TMTurbo.Title.Pack.Gbx", "5038519A8AD9270786D2097400055A5D", false, 6)]
    [Arguments("TMNext.pak", TMNextKey, false, 18)]
    [Arguments("TMNext.pak", TMNextKey, true, 18)]
    [Arguments("Resource.pak", "BE3BFB3192A3D3143E0CA1F1DA916C8A", false, 18)]
    public async Task Open_KnownArchive_DecryptsHeaderAndFile(string name, string baseKey, bool publicHeader, int version)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-keys-");

        try
        {
            var path = Path.Combine(directory.FullName, name);
            await File.WriteAllBytesAsync(path, CreateEncryptedPak(baseKey, publicHeader, version));
            await using var file = await ParsedFile.OpenAsync(path, fullGbx: true);
            var pak = (Pak)file.Value;
            await Assert.That(pak.Files.Count).IsEqualTo(1);

            using var data = pak.OpenFile(pak.Files.Values.Single(), out _);
            var actual = new byte[payload.Length];
            await data.ReadExactlyAsync(actual);
            await Assert.That(Convert.ToHexString(actual)).IsEqualTo(Convert.ToHexString(payload));
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("parse")]
    [Arguments("eval")]
    [Arguments("pak files")]
    public async Task Command_KnownPrivateArchive_ExposesDecryptedFileIndex(string command)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-keys-");

        try
        {
            var path = Path.Combine(directory.FullName, "TMNext.pak");
            await File.WriteAllBytesAsync(path, CreateEncryptedPak(TMNextKey, publicHeader: false));
            using var output = new StringWriter();
            using var error = new StringWriter();
            string[] args = command switch
            {
                "eval" => [command, path, "Files.Count"],
                "pak files" => ["pak", "files", path],
                _ => [command, path]
            };
            var exitCode = await Cli.RunAsync(args, output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            var expected = command switch
            {
                "eval" => "Result: 1",
                "pak files" => "sample [blue].txt",
                _ => "Files: 1"
            };
            await Assert.That(output.ToString()).Contains(expected);
            await Assert.That(output.ToString().Contains("Unavailable")).IsFalse();
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("Unknown.pak")]
    [Arguments("TMNext_2014-02-30.pak")]
    [Arguments("Resource.pak")]
    public async Task Command_NoMatchingPrivateHeaderKey_KeepsPublicMetadata(string name)
    {
        var directory = Directory.CreateTempSubdirectory("gbx-cli-keys-");

        try
        {
            var path = Path.Combine(directory.FullName, name);
            await File.WriteAllBytesAsync(path, CreateEncryptedPak(TMNextKey, publicHeader: false));
            using var output = new StringWriter();
            using var error = new StringWriter();
            var exitCode = await Cli.RunAsync(["parse", path], output, error);

            await Assert.That(exitCode).IsEqualTo(0);
            await Assert.That(output.ToString()).Contains("Files: Unavailable (private header requires a key).");
            await Assert.That(error.ToString()).IsEqualTo(string.Empty);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    internal static byte[] CreateEncryptedPak(string baseKey, bool publicHeader, int version = 6, byte[]? fileContents = null, string entryPath = "sample [blue].txt")
    {
        fileContents ??= payload;
        var original = ParseCommandTests.CreatePakData();
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

            for (var i = 0; i < 5; i++)
            {
                writer.Write(0); // Comments, XML, title, usage directory, and build.
            }

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
}
