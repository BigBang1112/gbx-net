using GBX.NET.Serialization;
using System.Collections;
using System.Text;
using GBX.NET.Crypto;

#if !NETSTANDARD2_0
using System.Diagnostics.CodeAnalysis;
#endif

namespace GBX.NET.PAK;

public sealed partial class PakList : IReadOnlyDictionary<string, PakListItem>
{
    private readonly IReadOnlyDictionary<string, PakListItem> packs;

    private const string NameKeySaltTM = "6611992868945B0B59536FC3226F3FD0";
    private const string NameKeySaltVsk5 = "33751203003810C43D169A5B608FC820";

    private const string KeyStringKeySaltTM = "B97C1205648A66E04F86A1B5D5AF9862";
    private const string KeyStringKeySaltVsk5 = "5A8BFF30451E627EAB838DADFB120FB6";

    public byte Version { get; init; }
    public uint CRC32 { get; init; }
    public uint Salt { get; init; }
    public byte[] Signature { get; init; }

    public IEnumerable<string> Keys => packs.Keys;
    public IEnumerable<PakListItem> Values => packs.Values;

    public int Count => packs.Count;

    public PakListItem this[string key] => packs[key];

    public PakList(byte version, uint crc32, uint salt, byte[] signature, IReadOnlyDictionary<string, PakListItem> packs)
    {
        Version = version;
        CRC32 = crc32;
        Salt = salt;
        Signature = signature;

        this.packs = packs;
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<PakList> ParseAsync(Stream stream, string nameKeySalt, string keyStringKeySalt, CancellationToken cancellationToken = default)
    {
        using var r = new GbxReader(stream);

        var version = r.ReadByte();
        var numPacks = r.ReadByte();
        var crc32 = r.ReadUInt32();
        var salt = r.ReadUInt32();

        var nameKey = await MD5.ComputeAsync(nameKeySalt + salt, cancellationToken);

        var packs = new Dictionary<string, PakListItem>(numPacks, StringComparer.OrdinalIgnoreCase);

        for (var i = 0; i < numPacks; i++)
        {
            var flags = r.ReadByte();
            var nameLength = r.ReadByte();
            var encryptedName = await r.ReadBytesAsync(nameLength, cancellationToken);
            var encryptedKeyString = await r.ReadBytesAsync(32, cancellationToken);

            for (var j = 0; j < encryptedName.Length; j++)
            {
                encryptedName[j] ^= nameKey[j % nameKey.Length];
            }

            var name = Encoding.ASCII.GetString(encryptedName);

            var keyStringKey = await MD5.ComputeAsync(name + salt + keyStringKeySalt, cancellationToken);

            for (var j = 0; j < encryptedKeyString.Length; j++)
            {
                encryptedKeyString[j] ^= keyStringKey[j % keyStringKey.Length];
            }

            var key = await MD5.ComputeAsync(Encoding.ASCII.GetString(encryptedKeyString) + Pak.Magic, cancellationToken);

            packs[name] = new PakListItem(key, flags);
        }

        var signature = await r.ReadBytesAsync(0x10, cancellationToken);

        return new PakList(version, crc32, salt, signature, packs);
    }

    public static async Task<PakList> ParseAsync(string filePath, string nameKeySalt, string keyStringKeySalt, CancellationToken cancellationToken = default)
    {
#if !NETSTANDARD2_0
        await
#endif
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
        return await ParseAsync(fs, nameKeySalt, keyStringKeySalt, cancellationToken);
    }

    public static PakList Parse(string filePath, string nameKeySalt, string keyStringKeySalt)
    {
        using var fs = File.OpenRead(filePath);
        return Parse(fs, nameKeySalt, keyStringKeySalt);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public static async Task<PakList> ParseAsync(Stream stream, PakListGame game = PakListGame.TM, CancellationToken cancellationToken = default)
    {
        switch (game)
        {
            case PakListGame.TM:
                return await ParseAsync(stream, NameKeySaltTM, KeyStringKeySaltTM, cancellationToken);
            case PakListGame.Vsk5:
                return await ParseAsync(stream, NameKeySaltVsk5, KeyStringKeySaltVsk5, cancellationToken);
            default:
                throw new ArgumentOutOfRangeException(nameof(game));
        }
    }

    public static async Task<PakList> ParseAsync(string filePath, PakListGame game = PakListGame.TM, CancellationToken cancellationToken = default)
    {
#if !NETSTANDARD2_0
        await
#endif
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
        return await ParseAsync(fs, game, cancellationToken);
    }

    public static PakList Parse(string filePath, PakListGame game = PakListGame.TM)
    {
        using var fs = File.OpenRead(filePath);
        return Parse(fs, game);
    }

    public bool ContainsKey(string key)
    {
        return packs.ContainsKey(key);
    }

#if NETSTANDARD2_0
    public bool TryGetValue(string key, out PakListItem value)
#else
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out PakListItem value)
#endif
    {
        return packs.TryGetValue(key, out value);
    }

    public IEnumerator<KeyValuePair<string, PakListItem>> GetEnumerator()
    {
        return packs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)packs).GetEnumerator();
    }

    public Dictionary<string, PakKeyInfo> ToKeyInfoDictionary()
    {
        var keys = new Dictionary<string, PakKeyInfo>(packs.Count, StringComparer.OrdinalIgnoreCase);

        foreach (var (name, item) in packs)
        {
            keys[name] = new PakKeyInfo(item.Key);
        }

        return keys;
    }
}