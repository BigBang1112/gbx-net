using GBX.NET.Builders.Engines.Plug;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using GBX.NET.Serialization.Chunking;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class CPlugGameSkinLayoutTests
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
    public async Task HeaderAndPathChunk_KeepVersionedFieldsAndBoundary(int version)
    {
        var payload = SkinPayload(version, w => w.Write("Stadium\\Media\\Icon.dds"));
        foreach (var header in new[] { true, false })
        {
            var node = new CPlugGameSkin();
            var headerChunk = new CPlugGameSkin.HeaderChunk090F4000();
            var pathChunk = new CPlugGameSkin.Chunk090F4005();
            await RoundTrip(payload, rw =>
            {
                if (header) headerChunk.ReadWrite(node, rw);
                else pathChunk.ReadWrite(node, rw);
            });

            var fid = (header ? node.HeaderFids : node.CustomizableFilePaths)![0];
            await Assert.That(fid.ClassId).IsEqualTo(0x09025000u);
            await Assert.That(fid.Name).IsEqualTo("Icon");
            await Assert.That(fid.Path).IsEqualTo("Stadium\\Media\\Icon.dds");
            await Assert.That(fid.MipMaps).IsEqualTo(version >= 2);
            await Assert.That(node.DirName).IsEqualTo("Skins\\Stadium\\");
            await Assert.That(node.PainterTextureName ?? "").IsEqualTo(version >= 1 ? "SkinDiffuse" : "");
            await Assert.That(node.DirNameAlt ?? "").IsEqualTo(version >= 4 ? "Models\\Stadium\\" : "");
            await Assert.That(node.DisableSkinAddition).IsEqualTo(version >= 5);
            await Assert.That(node.PackSubFolder ?? "").IsEqualTo(version >= 6 ? "Textures\\" : "");
            await Assert.That(header ? headerChunk.Version : pathChunk.Version).IsEqualTo(version);
        }
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(1, false)]
    [Arguments(2, false)]
    [Arguments(3, false)]
    [Arguments(3, true)]
    [Arguments(4, true)]
    [Arguments(5, false)]
    [Arguments(5, true)]
    [Arguments(8, false)]
    [Arguments(8, true)]
    public async Task ReferenceChunk_UsesPathsBeforeV3AndDistinguishesFilesFromFolders(int version, bool fileReference)
    {
        var payload = SkinPayload(version, w =>
        {
            if (version >= 3) w.Write(fileReference);
            if (fileReference) w.WriteNodeRef(new CPlugBitmap());
            else w.Write("Stadium\\Media\\");
        });
        var node = new CPlugGameSkin();
        var chunk = new CPlugGameSkin.Chunk090F4004();

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        var fid = node.CustomizableFids![0];
        await Assert.That(fid.ClassId).IsEqualTo(0x09025000u);
        await Assert.That(fid.Name).IsEqualTo("Icon");
        await Assert.That(fid.MipMaps).IsEqualTo(version >= 2);
        if (fileReference) await Assert.That(fid.Node).IsTypeOf<CPlugBitmap>();
        else await Assert.That(fid.Path).IsEqualTo("Stadium\\Media\\");
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(0, true)]
    [Arguments(1, false)]
    [Arguments(1, true)]
    [Arguments(2, false)]
    [Arguments(2, true)]
    public async Task LegacyChunks_KeepInterleavedEntriesAndSharedReferences(int offset, bool populated)
    {
        var payload = Payload(w =>
        {
            if (offset == 2) w.Write("Skins\\Legacy\\");
            var count = populated ? 2 : 0;
            w.Write(count);
            w.Write(count);
            if (offset == 2) w.Write(count);
            var bitmap = new CPlugBitmap();
            for (var i = 0; i < count; i++)
            {
                w.WriteNodeRef(bitmap);
                w.Write($"Remap{i}");
                if (offset == 2) w.Write(i == 0 ? 0x09025000u : 0x09030000u);
            }
        });
        var node = new CPlugGameSkin();
        Chunk<CPlugGameSkin> chunk = offset switch
        {
            0 => new CPlugGameSkin.Chunk090F4000(),
            1 => new CPlugGameSkin.Chunk090F4001(),
            _ => new CPlugGameSkin.Chunk090F4002()
        };

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        var fids = offset == 1 ? node.CustomizableSounds : node.CustomizableFids;
        await Assert.That(fids!.Length).IsEqualTo(populated ? 2 : 0);
        if (populated)
        {
            await Assert.That(fids[1].Name).IsEqualTo("Remap1");
            await Assert.That(fids[1].Node).IsSameReferenceAs(fids[0].Node);
            await Assert.That(fids[0].ClassId).IsEqualTo(offset == 1 ? 0x09030000u : 0x09025000u);
            await Assert.That(fids[1].ClassId).IsEqualTo(offset == 0 ? 0x09025000u : 0x09030000u);
            await Assert.That(fids[0].MipMaps).IsFalse();
        }
        if (offset == 2) await Assert.That(node.DirName).IsEqualTo("Skins\\Legacy\\");
    }

    [Test]
    [Arguments(-1, -1, -1)]
    [Arguments(1, 0, 1)]
    [Arguments(1, 1, 0)]
    public void LegacyTypedChunk_RejectsInconsistentCounts(int fidCount, int nameCount, int classIdCount)
    {
        var payload = Payload(w =>
        {
            w.Write("");
            w.Write(fidCount);
            w.Write(nameCount);
            w.Write(classIdCount);
        });
        using var input = new MemoryStream(payload);
        using var reader = new GbxReader(input);
        using var rw = new GbxReaderWriter(reader);

        Assert.Throws<InvalidDataException>(() => new CPlugGameSkin.Chunk090F4002().ReadWrite(new(), rw));
    }

    [Test]
    public async Task Defaults_MatchNativeVersionAndTrailingValues()
    {
        await Assert.That(new CPlugGameSkin.HeaderChunk090F4000().Version).IsEqualTo(0);
        await Assert.That(new CPlugGameSkin.HeaderChunk090F4000(GameVersion.TM2020).Version).IsEqualTo(8);
        await Assert.That(new CPlugGameSkin.HeaderChunk090F4000(GameVersion.TMNESWC).Version).IsEqualTo(2);
        await Assert.That(new CPlugGameSkin.HeaderChunk090F4000(GameVersion.VSK5).Version).IsEqualTo(4);
        await Assert.That(new CPlugGameSkin.HeaderChunk090F4000().U01).IsEqualTo(1);
        await Assert.That(new CPlugGameSkin.Chunk090F4004().U01).IsEqualTo(1);
        await Assert.That(new CPlugGameSkin.Chunk090F4005().U01).IsEqualTo(1);
        await Assert.That(new CPlugGameSkin().DisableSkinAddition).IsFalse();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LegacyBuilders_ExposeTextureAndSoundRemappings(bool legacySoundName)
    {
        var texture = new CPlugGameSkin.Fid { Name = "Texture" };
        var sound = new CPlugGameSkin.Fid { Name = "Sound" };
        var builder = new CPlugGameSkinBuilder();
        var node = legacySoundName
            ? builder.ForTM10().WithCustomizableFids(texture).WithCustomizableFids2(sound).Build()
            : builder.ForTMPU().WithCustomizableFids(texture).WithCustomizableSounds(sound).Build();

        await Assert.That(node.CustomizableFids![0].Name).IsEqualTo("Texture");
        await Assert.That(node.CustomizableSounds![0].Name).IsEqualTo("Sound");
        await Assert.That(node.CustomizableFids2).IsSameReferenceAs(node.CustomizableSounds);
    }

    private static byte[] SkinPayload(int version, Action<GbxWriter> writeReference) => Payload(w =>
    {
        w.Write((byte)version);
        w.Write("Skins\\Stadium\\");
        if (version >= 1)
        {
            w.Write("SkinDiffuse");
            w.Write("PainterScene");
        }
        w.Write((byte)1);
        w.Write(0x09025000u);
        w.Write("Icon");
        writeReference(w);
        if (version >= 2) w.Write(true);
        if (version >= 4) w.Write("Models\\Stadium\\");
        if (version >= 5) w.Write(true);
        if (version >= 6) w.Write("Textures\\");
        if (version >= 7) w.Write(17);
        if (version >= 8) w.Write((byte)3);
    });

    private static byte[] Payload(Action<GbxWriter> write)
    {
        using var stream = new MemoryStream();
        using var writer = new GbxWriter(stream);
        write(writer);
        return stream.ToArray();
    }

    private static async Task RoundTrip(byte[] payload, Action<GbxReaderWriter> serialize)
    {
        using var input = new MemoryStream();
        input.Write(payload);
        using (var writer = new GbxWriter(input)) writer.Write(0x11223344);
        input.Position = 0;
        using (var reader = new GbxReader(input))
        using (var rw = new GbxReaderWriter(reader))
        {
            serialize(rw);
            await Assert.That(reader.ReadInt32()).IsEqualTo(0x11223344);
            await Assert.That(input.Position).IsEqualTo(input.Length);
        }
        using var output = new MemoryStream();
        using (var writer = new GbxWriter(output))
        using (var rw = new GbxReaderWriter(writer)) serialize(rw);
        await Assert.That(output.ToArray()).IsEquivalentTo(payload, CollectionOrdering.Matching);
    }
}
