using GBX.NET.Components;
using GBX.NET.Engines.Game;
using GBX.NET.Serialization;
using GBX.NET.Tests.Infrastructure;
using System.Text;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class ClassIdRemappingTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Save_PreservesForeverIdsFromHeaderAndBody(bool compressed)
    {
        using var source = new MemoryStream();
        using (var writer = new GbxWriter(source))
        {
            GbxHeaderBasic.Create(compressionOfBody: GbxCompression.Uncompressed).Write(writer);
            writer.Write(0x03051000u);
            writer.Write(12); // Header chunk table size, with an empty payload.
            writer.Write(1);
            writer.Write(0x0301A0FFu);
            writer.Write(0);
            writer.Write(1); // Root node count.
            writer.Write(0); // External reference count.
            writer.Write(0x030310FFu);
            writer.Write(0x534B4950u);
            writer.Write(0);
            writer.Write(0xFACADE01u);
        }

        using var original = new MemoryStream();
        source.Position = 0;
        if (compressed) Gbx.Compress(source, original);
        else source.CopyTo(original);
        original.Position = 0;

        var gbx = Gbx.Parse<CGameCtnBlockInfoClassic>(original);
        await Assert.That(gbx.ClassIdRemapMode).IsEqualTo(ClassIdRemapMode.TrackManiaForever);

        using var output = new MemoryStream();
        gbx.Save(output);
        await GbxAssert.HaveEqualSavedBytes(original.ToArray(), output.ToArray());
    }

    [Test]
    public async Task ResolveClassIdRemapMode_TracksReplayRootSeparatelyFromTheHeaderChunkOwner()
    {
        using var stream = new MemoryStream();
        using var reader = new GbxReader(stream);

        await Assert.That(reader.NormalizeClassId(0x2407E000, ClassIdLocation.Root)).IsEqualTo(0x03093000u);
        await Assert.That(reader.NormalizeClassId(0x2403F001, ClassIdLocation.HeaderChunk)).IsEqualTo(0x03093001u);

        var mode = reader.ResolveClassIdRemapMode();
        await Assert.That(mode).IsEqualTo(ClassIdRemapMode.TrackManiaSunriseExtreme);
        await Assert.That(ClassIdRemapping.ToWireId(mode, ClassIdLocation.Root, 0x03093000)).IsEqualTo(0x2407E000u);
        await Assert.That(ClassIdRemapping.ToWireId(mode, ClassIdLocation.HeaderChunk, 0x03093001)).IsEqualTo(0x2403F001u);
    }

    [Test]
    [Arguments(ClassIdRemapMode.Latest, 0x2E001123u)]
    [Arguments(ClassIdRemapMode.Id2008, 0x0301A123u)]
    [Arguments(ClassIdRemapMode.Id2006, 0x2400A123u)]
    public async Task WriteChunkId_PreservesTheExplicitCompatibilityModes(ClassIdRemapMode mode, uint wireId)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream) { ClassIdRemapMode = mode };

        await Assert.That(writer.WriteChunkId(0x2E001123)).IsEqualTo(wireId);
        await Assert.That(BitConverter.ToUInt32(stream.ToArray(), 0)).IsEqualTo(wireId);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ParseClassId_ReadsAnEarlyBinaryHeaderAndATextHeader(bool text)
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            GbxHeaderBasic.Create(version: text ? (ushort)6 : (ushort)3, format: text ? GbxFormat.Text : GbxFormat.Binary).Write(writer);
            if (text) writer.Write(Encoding.ASCII.GetBytes("24003000\r\n"));
            else writer.Write(0x24003000u);
        }

        stream.Position = 0;
        await Assert.That(Gbx.ParseClassId(stream, remap: false)).IsEqualTo(0x24003000u);
        await Assert.That(stream.Position).IsEqualTo(stream.Length);
        stream.Position = 0;
        await Assert.That(await Gbx.ParseClassIdAsync(stream)).IsEqualTo(0x03043000u);
    }
}
