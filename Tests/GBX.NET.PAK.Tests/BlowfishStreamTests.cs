using GBX.NET.Crypto;
using System.Buffers.Binary;

namespace GBX.NET.PAK.Tests;

public class BlowfishStreamTests
{
    [Test]
    [Arguments(17)]
    [Arguments(18)]
    public async Task Read_CounterMode_XorsCounterKeyStreamPerChunk(int pakVersion)
    {
        const ulong iv = 0x0123456789ABCDEF;
        var key = Enumerable.Range(1, 16).Select(x => (byte)x).ToArray();
        var plain = Enumerable.Range(0, 0x210).Select(x => (byte)(x * 7)).ToArray();
        var blowfish = new Blowfish(key, pakVersion >= 18 ? BlowfishTrick.LittleEndianPak18 : BlowfishTrick.LittleEndian);
        var encrypted = new byte[plain.Length];

        for (var block = 0; block < plain.Length / 8; block++)
        {
            // Each 256-byte chunk restarts at the next chunk offset after the IV.
            var keyStream = new byte[8];
            BinaryPrimitives.WriteUInt64LittleEndian(keyStream, iv + 8 + (ulong)(block / 32 * 0x100 + block % 32));
            if (pakVersion >= 18) blowfish.Decrypt(keyStream.AsSpan());
            else blowfish.Encrypt(keyStream.AsSpan());
            for (var i = 0; i < 8; i++) encrypted[block * 8 + i] = (byte)(plain[block * 8 + i] ^ keyStream[i]);
        }

        using var stream = new BlowfishStream(new MemoryStream(encrypted), key, iv, pakVersion, counterMode: true);
        var actual = new byte[plain.Length];
        stream.ReadExactly(actual);

        await Assert.That(actual.SequenceEqual(plain)).IsTrue();
    }

    [Test]
    public async Task Read_CounterMode_IgnoresClassInitialization()
    {
        var key = new byte[16];
        var data = Enumerable.Range(0, 32).Select(x => (byte)x).ToArray();
        using var initialized = new BlowfishStream(new MemoryStream(data), key, 5, 18, counterMode: true);
        using var plain = new BlowfishStream(new MemoryStream(data), key, 5, 18, counterMode: true);
        initialized.Initialize(BitConverter.GetBytes(0x24005000), 0, 4);
        var initializedData = new byte[32];
        var plainData = new byte[32];
        initialized.ReadExactly(initializedData);
        plain.ReadExactly(plainData);

        await Assert.That(initializedData.SequenceEqual(plainData)).IsTrue();
    }
}
