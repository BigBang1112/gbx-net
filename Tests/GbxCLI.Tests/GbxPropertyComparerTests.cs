using GBX.NET;
using GBX.NET.Engines.Game;
using System.IO.Compression;

namespace GbxCLI.Tests;

public class GbxPropertyComparerTests
{
    [Test]
    public async Task FindDifference_ZlibDataWithOtherCompressionLevel_IsEqual()
    {
        Gbx.ZLib = new GBX.NET.ZLib.ZLib();
        var data = Enumerable.Range(0, 4096).Select(i => (byte)(i % 7)).ToArray();

        var left = CreateGbx(data, CompressionLevel.SmallestSize);
        var right = CreateGbx(data, CompressionLevel.Fastest);

        await Assert.That(left.Node.LightmapCacheData!.Data).IsNotEquivalentTo(right.Node.LightmapCacheData!.Data);
        await Assert.That(GbxPropertyComparer.FindDifference(left, right, CancellationToken.None)).IsNull();
    }

    [Test]
    public async Task FindDifference_ZlibDataWithOtherContent_ReportsDecompressedByte()
    {
        Gbx.ZLib = new GBX.NET.ZLib.ZLib();
        var data = new byte[64];
        var changed = (byte[])data.Clone();
        changed[10] = 1;

        var difference = GbxPropertyComparer.FindDifference(CreateGbx(data, CompressionLevel.Optimal), CreateGbx(changed, CompressionLevel.Optimal), CancellationToken.None);

        await Assert.That(difference).IsEqualTo("Node.LightmapCacheData.Decompressed[10] from 0 to 1.");
    }

    private static Gbx<CGameCtnChallenge> CreateGbx(byte[] data, CompressionLevel level)
    {
        using var compressed = new MemoryStream();

        using (var zlib = new ZLibStream(compressed, level, leaveOpen: true))
        {
            zlib.Write(data);
        }

        return new Gbx<CGameCtnChallenge>(new CGameCtnChallenge { LightmapCacheData = new ZlibData(data.Length, compressed.ToArray(), exception: null) });
    }
}
