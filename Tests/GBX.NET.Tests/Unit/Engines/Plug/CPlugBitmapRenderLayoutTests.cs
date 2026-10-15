using GBX.NET.Components;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugBitmapRenderLayoutTests
{
    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
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
    public async Task ReadWrite_Chunks_PreserveFieldOrderAndPackedValues(int offset)
    {
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            WriteSettings(writer, offset);
            writer.Write(0x11223344);
        }
        var expected = payload.ToArray()[..^4];
        payload.Position = 0;
        var node = new CPlugBitmapRender();
        var chunk = CreateChunk(offset);
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            chunk.ReadWrite(node, rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
        }
        if (offset is 0 or 1 or 4 or 5 or 6 or 7 or 9 or 11 or 16)
        {
            await Assert.That(node.ClearColor).IsEqualTo(0x80402010u);
            await Assert.That(offset is 11 or 16 ? node.RenderFlags : node.LegacyRenderFlags).IsEqualTo(0xD3A56B79u);
        }
        if (offset is 1 or 4 or 5 or 6 or 7 or 9 or 11 or 16)
        {
            await Assert.That(node.BitmapClear).IsNull();
            await Assert.That(node.References.BitmapClear.File).IsNull();
            await Assert.That(node.BitmapClearUV).IsEqualTo(new Vec2(-1.25f, 2.5f));
        }
        if (offset is 2 or 3)
        {
            await Assert.That(node.SVIdMask).IsEqualTo((ushort)0xFEDC);
            await Assert.That(node.SVIdRef).IsEqualTo((ushort)0xBA98);
        }
        if (offset == 3)
        {
            await Assert.That(node.IVIdMask).IsEqualTo((ushort)0x9876);
            await Assert.That(node.IVIdRef).IsEqualTo((ushort)0xABCD);
        }
        if (offset == 8)
        {
            await Assert.That(node.RenderPath).IsEqualTo(CPlugBitmapRender.ERenderPath.PlaneDToAlpha);
            await Assert.That(node.RenderPathFails).IsEqualTo(CPlugBitmapRender.ERenderPathFails.Ignore);
            await Assert.That(chunk is ISkippableChunk).IsTrue();
        }
        if (offset == 10) await Assert.That(node.FogCustomFarZ).IsEqualTo(-12.5f);
        if (offset == 12)
        {
            await Assert.That(node.RenderSub).IsNull();
            await Assert.That(node.References.RenderSub.File).IsNull();
        }
        if (offset is 13 or 14 or 15) await Assert.That(node.RenderFlags2).IsEqualTo(0xF1350246u);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(expected, CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(4)]
    [Arguments(5)]
    [Arguments(6)]
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
    public async Task ReadNode_NodeBodies_RecognizeLegacyAndModernChunks(int offset)
    {
        using var settings = new MemoryStream();
        using (var writer = new GbxWriter(settings)) WriteSettings(writer, offset);
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            writer.Write(CPlugBitmapRender.Id + (uint)offset);
            if (offset == 8)
            {
                writer.Write(0x534B4950u);
                writer.Write((int)settings.Length);
            }
            writer.Write(settings.ToArray());
            writer.Write(0xFACADE01u);
        }
        payload.Position = 0;
        using var reader = new GbxReader(payload);
        var node = reader.ReadNode<CPlugBitmapRender>()!;
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        await Assert.That(node.Chunks.Count).IsEqualTo(1);
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved)) writer.WriteNode(node);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(1, false)]
    [Arguments(4, false)]
    [Arguments(5, false)]
    [Arguments(6, false)]
    [Arguments(7, false)]
    [Arguments(9, false)]
    [Arguments(11, false)]
    [Arguments(12, false)]
    [Arguments(16, false)]
    [Arguments(1, true)]
    [Arguments(4, true)]
    [Arguments(5, true)]
    [Arguments(6, true)]
    [Arguments(7, true)]
    [Arguments(9, true)]
    [Arguments(11, true)]
    [Arguments(12, true)]
    [Arguments(16, true)]
    public async Task ReadWrite_References_PreserveEmbeddedNodesAndExternalFiles(int offset, bool external)
    {
        var file = new GbxRefTableFile(new GbxRefTable(), 0, true, "Bitmap.Gbx");
        using var payload = new MemoryStream();
        using (var writer = new GbxWriter(payload))
        {
            if (offset != 12)
            {
                writer.Write(0x80402010u);
                writer.Write(0xD3A56B79u);
            }
            if (external) writer.Write(1);
            else if (offset == 12) writer.WriteNodeRef(new CPlugBitmapRenderSub());
            else writer.WriteNodeRef(new CPlugBitmap());
            if (offset != 12)
            {
                writer.Write(-1.25f);
                writer.Write(2.5f);
            }
        }
        payload.Position = 0;
        var node = new CPlugBitmapRender();
        var chunk = CreateChunk(offset);
        using (var reader = new GbxReader(payload))
        using (var rw = new GbxReaderWriter(reader))
        {
            if (external) reader.LoadRefTable(new Dictionary<int, GbxRefTableNode> { [1] = file });
            chunk.ReadWrite(node, rw);
        }
        await Assert.That(payload.Position).IsEqualTo(payload.Length);
        if (external) await Assert.That(offset == 12 ? node.References.RenderSub.File : node.References.BitmapClear.File).IsSameReferenceAs(file);
        else if (offset == 12) await Assert.That(node.RenderSub).IsNotNull();
        else await Assert.That(node.BitmapClear).IsNotNull();
        using var saved = new MemoryStream();
        using (var writer = new GbxWriter(saved))
        using (var rw = new GbxReaderWriter(writer)) chunk.ReadWrite(node, rw);
        await Assert.That(saved.ToArray()).IsEquivalentTo(payload.ToArray(), CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(GameVersion.Unspecified, 0x8138, 0x7800u, 0u)]
    [Arguments(GameVersion.TM10, 0, 0x7800u, 0x0E8007E1u)]
    [Arguments(GameVersion.TMPU, 0, 0x7800u, 0x2E8007E1u)]
    [Arguments(GameVersion.TMSX, 0x138, 0x7800u, 0u)]
    [Arguments(GameVersion.TMNESWC, 0x338, 0u, 0u)]
    [Arguments(GameVersion.VSK5, 0x338, 0u, 0u)]
    [Arguments(GameVersion.TMF, 0x338, 0x7800u, 0u)]
    [Arguments(GameVersion.TMT, 0x8138, 0x7800u, 0u)]
    [Arguments(GameVersion.MP4, 0x8138, 0x7800u, 0u)]
    [Arguments(GameVersion.TM2020, 0x8130, 0x7800u, 0u)]
    public async Task Constructor_Defaults_MatchConstruction(GameVersion gameVersion, int mask, uint flags2, uint legacyFlags)
    {
        var node = new CPlugBitmapRender(gameVersion);
        await Assert.That(node.ClearColor).IsEqualTo(0u);
        await Assert.That(node.LegacyRenderFlags).IsEqualTo(legacyFlags);
        await Assert.That(node.RenderFlags).IsEqualTo(0x2E8007E2u);
        await Assert.That(node.RenderFlags2).IsEqualTo(flags2);
        await Assert.That(node.IVIdMask).IsEqualTo((ushort)mask);
        await Assert.That(node.IVIdRef).IsEqualTo((ushort)0);
        await Assert.That(node.SVIdMask).IsEqualTo((ushort)mask);
        await Assert.That(node.SVIdRef).IsEqualTo((ushort)0);
        await Assert.That(node.RenderPath).IsEqualTo(CPlugBitmapRender.ERenderPath.Normal);
        await Assert.That(node.RenderPathFails).IsEqualTo(CPlugBitmapRender.ERenderPathFails.Hide);
        await Assert.That(node.FogCustomFarZ).IsEqualTo(50f);
        await Assert.That(node.BitmapClear).IsNull();
        await Assert.That(node.References.BitmapClear.File).IsNull();
        await Assert.That(node.BitmapClearUV).IsEqualTo(new Vec2(0, 0));
        await Assert.That(node.RenderSub).IsNull();
        await Assert.That(node.References.RenderSub.File).IsNull();
    }

    private static Chunk<CPlugBitmapRender> CreateChunk(int offset) => offset switch
    {
        0 => new CPlugBitmapRender.Chunk09086000(),
        1 => new CPlugBitmapRender.Chunk09086001(),
        2 => new CPlugBitmapRender.Chunk09086002(),
        3 => new CPlugBitmapRender.Chunk09086003(),
        4 => new CPlugBitmapRender.Chunk09086004(),
        5 => new CPlugBitmapRender.Chunk09086005(),
        6 => new CPlugBitmapRender.Chunk09086006(),
        7 => new CPlugBitmapRender.Chunk09086007(),
        8 => new CPlugBitmapRender.Chunk09086008(),
        9 => new CPlugBitmapRender.Chunk09086009(),
        10 => new CPlugBitmapRender.Chunk0908600A(),
        11 => new CPlugBitmapRender.Chunk0908600B(),
        12 => new CPlugBitmapRender.Chunk0908600C(),
        13 => new CPlugBitmapRender.Chunk0908600D(),
        14 => new CPlugBitmapRender.Chunk0908600E(),
        15 => new CPlugBitmapRender.Chunk0908600F(),
        16 => new CPlugBitmapRender.Chunk09086010(),
        _ => throw new ArgumentOutOfRangeException(nameof(offset))
    };

    private static void WriteSettings(GbxWriter writer, int offset)
    {
        switch (offset)
        {
            case 0:
            case 1:
            case 4:
            case 5:
            case 6:
            case 7:
            case 9:
            case 11:
            case 16:
                writer.Write(0x80402010u);
                writer.Write(0xD3A56B79u);
                if (offset == 0) break;
                writer.Write(-1);
                writer.Write(-1.25f);
                writer.Write(2.5f);
                break;
            case 3:
                writer.Write((ushort)0x9876);
                writer.Write((ushort)0xABCD);
                goto case 2;
            case 2:
                writer.Write((ushort)0xFEDC);
                writer.Write((ushort)0xBA98);
                break;
            case 8:
                writer.Write(6);
                writer.Write(0);
                break;
            case 10:
                writer.Write(-12.5f);
                break;
            case 12:
                writer.Write(-1);
                break;
            case 13:
            case 14:
            case 15:
                writer.Write(0xF1350246u);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(offset));
        }
    }
}
