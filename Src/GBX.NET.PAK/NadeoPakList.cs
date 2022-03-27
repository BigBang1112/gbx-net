using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace GBX.NET.PAK;

public class NadeoPakList : IReadOnlyDictionary<string, NadeoPakListItem>
{
    private const string NameKeySalt = "6611992868945B0B59536FC3226F3FD0";
    private const string KeyStringKeySalt = "B97C1205648A66E04F86A1B5D5AF9862";

    public byte Version { get; init; }
    public uint CRC32 { get; init; }
    public uint Salt { get; init; }
    public IReadOnlyDictionary<string, NadeoPakListItem> Packs { get; init; }
    public byte[] Signature { get; init; }

    public int Count => Packs.Count;

    public IEnumerable<string> Keys => Packs.Keys;
    public IEnumerable<NadeoPakListItem> Values => Packs.Values;

    public NadeoPakListItem this[string name] => Packs[name];

    public NadeoPakList(byte version, uint crc32, uint salt, IDictionary<string, NadeoPakListItem> packs, byte[] signature)
    {
        Version = version;
        CRC32 = crc32;
        Salt = salt;
        Packs = new ReadOnlyDictionary<string, NadeoPakListItem>(packs);
        Signature = signature;
    }

    public bool ContainsKey(string key)
    {
        return Packs.ContainsKey(key);
    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public bool TryGetValue(string key, [NotNullWhen(false)] out NadeoPakListItem value)
    {
        return Packs.TryGetValue(key, out value!);
    }
#else
    public bool TryGetValue(string key, out NadeoPakListItem value)
    {
        return Packs.TryGetValue(key, out value);
    }
#endif

    public IEnumerator<KeyValuePair<string, NadeoPakListItem>> GetEnumerator()
    {
        return Packs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Packs.GetEnumerator();
    }

    public static NadeoPakList Parse(Stream stream, Func<string, byte[]> md5func)
    {
        using var r = new GameBoxReader(stream);

        var version = r.ReadByte();
        var numPacks = r.ReadByte();
        var crc32 = r.ReadUInt32();
        var salt = r.ReadUInt32();

        var nameKey = md5func(NameKeySalt + salt);

        var packs = new Dictionary<string, NadeoPakListItem>(numPacks);

        for (var i = 0; i < numPacks; i++)
        {
            var flags = r.ReadByte();
            var nameLength = r.ReadByte();
            var encryptedName = r.ReadBytes(nameLength);
            var encryptedKeyString = r.ReadBytes(32);

            for (var j = 0; j < encryptedName.Length; j++)
            {
                encryptedName[j] ^= nameKey[j % nameKey.Length];
            }

            var name = Encoding.ASCII.GetString(encryptedName);

            var keyStringKey = md5func(name + salt + KeyStringKeySalt);

            for (var j = 0; j < encryptedKeyString.Length; j++)
            {
                encryptedKeyString[j] ^= keyStringKey[j % keyStringKey.Length];
            }

            var key = md5func(Encoding.ASCII.GetString(encryptedKeyString) + "NadeoPak");

            packs[name] = new NadeoPakListItem(key, flags);
        }

        var signature = r.ReadBytes(0x10);

        return new NadeoPakList(version, crc32, salt, packs, signature);
    }

    public static NadeoPakList Parse(Stream stream)
    {
        return Parse(stream, ComputeMD5);
    }

    public static NadeoPakList Parse(string fileName)
    {
        using var stream = File.OpenRead(fileName);
        return Parse(stream);
    }

    public static async Task<NadeoPakList> ParseAsync(Stream stream, Func<string, Task<byte[]>> md5func)
    {
        using var r = new GameBoxReader(stream);

        var version = r.ReadByte();
        var numPacks = r.ReadByte();
        var crc32 = r.ReadUInt32();
        var salt = r.ReadUInt32();

        var nameKey = await md5func(NameKeySalt + salt);

        var packs = new Dictionary<string, NadeoPakListItem>(numPacks);

        for (var i = 0; i < numPacks; i++)
        {
            var flags = r.ReadByte();
            var nameLength = r.ReadByte();
            var encryptedName = r.ReadBytes(nameLength);
            var encryptedKeyString = r.ReadBytes(32);

            for (var j = 0; j < encryptedName.Length; j++)
            {
                encryptedName[j] ^= nameKey[j % nameKey.Length];
            }

            var name = Encoding.ASCII.GetString(encryptedName);

            var keyStringKey = await md5func(name + salt + KeyStringKeySalt);

            for (var j = 0; j < encryptedKeyString.Length; j++)
            {
                encryptedKeyString[j] ^= keyStringKey[j % keyStringKey.Length];
            }

            var key = await md5func(Encoding.ASCII.GetString(encryptedKeyString) + "NadeoPak");

            packs[name] = new NadeoPakListItem(key, flags);
        }

        var signature = r.ReadBytes(0x10);

        return new NadeoPakList(version, crc32, salt, packs, signature);
    }

    private static byte[] ComputeMD5(string str)
    {
        byte[] nameKey;
        using (var md5 = MD5.Create())
        {
            nameKey = md5.ComputeHash(Encoding.ASCII.GetBytes(str));
        }
        return nameKey;
    }
}
