using System.Collections;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace GBX.NET.PAK;

public class NadeoPakList : IReadOnlyCollection<NadeoPakListItem>
{
    public byte Version { get; }
    public uint CRC32 { get; }
    public uint Salt { get; }
    public ReadOnlyCollection<NadeoPakListItem> Packs { get; }
    public byte[] Signature { get; }

    public int Count => Packs.Count;

    public NadeoPakList(byte version, uint crc32, uint salt, IList<NadeoPakListItem> packs, byte[] signature)
    {
        Version = version;
        CRC32 = crc32;
        Salt = salt;
        Packs = new ReadOnlyCollection<NadeoPakListItem>(packs);
        Signature = signature;
    }

    public static NadeoPakList Parse(Stream stream)
    {
        using var r = new GameBoxReader(stream);

        var version = r.ReadByte();
        var numPacks = r.ReadByte();
        var crc32 = r.ReadUInt32();
        var salt = r.ReadUInt32();

        var nameKey = ComputeMD5("6611992868945B0B59536FC3226F3FD0" + salt);

        var packs = new NadeoPakListItem[numPacks];

        for (var i = 0; i < numPacks; i++)
        {
            var flags = r.ReadByte();
            var nameLength = r.ReadByte();
            var encryptedName = r.ReadBytes(nameLength);
            var encryptedKeyString = r.ReadBytes(32);

            for (var j = 0; j < encryptedName.Length; j++)
                encryptedName[j] ^= nameKey[j % nameKey.Length];

            var name = Encoding.ASCII.GetString(encryptedName);

            var keyStringKey = ComputeMD5(name + salt + "B97C1205648A66E04F86A1B5D5AF9862");

            for (var j = 0; j < encryptedKeyString.Length; j++)
                encryptedKeyString[j] ^= keyStringKey[j % keyStringKey.Length];

            var key = ComputeMD5(Encoding.ASCII.GetString(encryptedKeyString) + "NadeoPak");

            packs[i] = new NadeoPakListItem(name, flags, key);
        }

        var signature = r.ReadBytes(0x10);

        return new NadeoPakList(version, crc32, salt, packs, signature);
    }

    public static NadeoPakList Parse(string fileName)
    {
        using var stream = File.OpenRead(fileName);
        return Parse(stream);
    }

    private static byte[] ComputeMD5(string str)
    {
        byte[] nameKey;
        using (var md5 = MD5.Create())
            nameKey = md5.ComputeHash(Encoding.ASCII.GetBytes(str));
        return nameKey;
    }

    public IEnumerator<NadeoPakListItem> GetEnumerator()
    {
        return Packs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Packs.GetEnumerator();
    }
}
