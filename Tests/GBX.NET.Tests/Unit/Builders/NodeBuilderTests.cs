using GBX.NET.Builders.Engines.Game;
using GBX.NET.Components;
using GBX.NET.Engines.Control;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.Plug;
using GBX.NET.Interfaces.Game;
using GBX.NET.Interfaces.Control;
using GBX.NET.Interfaces.Plug;

namespace GBX.NET.Tests.Unit.Builders;

[Category("Unit")]
public class NodeBuilderTests
{
    [Test]
    public async Task GeneratedCommonMembers_PreserveExistingGameInterfaceAccessorSlots()
    {
        foreach (var contract in new[] { typeof(IGameCtnBlockTM10), typeof(IGameCtnBlockTMSX), typeof(IGameCtnBlockMP4) })
        {
            // Older callers refer to these declared accessors, even when the
            // generated common interface also exposes Variant.
            var property = contract.GetProperty("Variant", System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            await Assert.That(property).IsNotNull();
        }
    }

    [Test]
    public async Task DerivedInterfaces_PreserveTheBasePropertyWhenTheNodeHidesIt()
    {
        var node = new CPlugVehicleCarPhyTuning();
        ((CPlugVehiclePhyTuning)node).Name = "Base tuning";
        node.Name = null;
        IPlugVehiclePhyTuning baseView = node;
        await Assert.That(baseView.Name).IsEqualTo("Base tuning");
    }

    [Test]
    public async Task Build_TMFClip_CreatesItsChunksAndDefaults()
    {
        CGameCtnMediaClip node = CGameCtnMediaClip.Create().WithName("Intro").ForTMF().Build();

        await Assert.That(node.Name).IsEqualTo("Intro");
        await Assert.That(node.LocalPlayerClipEntIndex).IsEqualTo(-1);
        await Assert.That(node.Tracks).IsEmpty();
        await Assert.That(node.GetChunk<CGameCtnMediaClip.Chunk03079004>()).IsNotNull();
        await Assert.That(node.GetChunk<CGameCtnMediaClip.Chunk03079005>()).IsNotNull();
        await Assert.That(node.GetChunk<CGameCtnMediaClip.Chunk03079007>()).IsNotNull();
        await Assert.That(node.GetChunk<CGameCtnMediaClip.Chunk03079003>()).IsNull();
    }

    [Test]
    public async Task BuildInterface_ProjectsTracksThroughTheSelectedGame()
    {
        IGameCtnMediaTrackTMF track = CGameCtnMediaTrack.Create().ForTMF().WithName("Camera").BuildInterface();
        IGameCtnMediaClipTMF clip = CGameCtnMediaClip.Create().ForTMF().WithTracks(track).BuildInterface();

        await Assert.That(clip.Tracks[0]).IsSameReferenceAs(track);
        await Assert.That(clip.Tracks[0].Name).IsEqualTo("Camera");
    }

    [Test]
    public async Task ScopedCollection_FollowsReplacementWithoutValidatingChildGames()
    {
        var node = CGameCtnMediaClip.Create().ForTMF().Build();
        IGameCtnMediaClipTMF view = node;
        var tracks = view.Tracks;
        var replacement = CGameCtnMediaTrack.Create().ForTMF().WithName("Replacement").BuildInterface();
        view.Tracks = [replacement];
        await Assert.That(tracks[0]).IsSameReferenceAs(replacement);

        var otherGame = CGameCtnMediaTrack.Create().ForTM2020().Build();
        node.Tracks = [otherGame];
        await Assert.That(tracks[0]).IsSameReferenceAs(otherGame);
    }

    [Test]
    public async Task BuildInterface_DerivedBlocksConvertToTheirBaseGameInterface()
    {
        IGameCtnMediaBlockTextTMF text = CGameCtnMediaBlockText.Create().ForTMF().WithText("Hello").BuildInterface();
        var track = CGameCtnMediaTrack.Create().ForTMF().WithBlocks(text).BuildInterface();
        await Assert.That(track.Blocks![0]).IsSameReferenceAs(text);
        using var stream = new MemoryStream();
        // This call must resolve through the derived view without ambiguity
        // between its own and its base node's IClassVersion<T> contracts.
        var file = text.ToGbx();
        file.BodyCompression = GbxCompression.Uncompressed;
        file.Save(stream);
        await Assert.That(stream.Length).IsGreaterThan(0);
    }

    [Test]
    public async Task Build_MP4Aliases_SelectTheSameFormat()
    {
        var builder = CGameCtnMediaClip.Create().WithName("Intro");
        CGameCtnMediaClipBuilder.MP4[] selections = [builder.ForMP4(), builder.ForTM2(), builder.ForSM()];
        foreach (var selected in selections)
        {
            var node = selected.WithStopWhenRespawn(true).Build();
            await Assert.That(node.GetChunk<CGameCtnMediaClip.Chunk0307900D>()!.Version).IsEqualTo(0);
            await Assert.That(node.StopWhenRespawn).IsTrue();
            IGameCtnMediaClipMP4 view = selected.BuildInterface();
            await Assert.That(view.Name).IsEqualTo("Intro");
        }
    }

    [Test]
    public async Task Build_TM2020_UsesGameSpecificChunkVersions()
    {
        var node = CGameCtnMediaClip.Create().ForTM2020().Build();
        await Assert.That(node.GetChunk<CGameCtnMediaClip.Chunk0307900D>()!.Version).IsEqualTo(1);
        await Assert.That(node.GetChunk<CGameCtnMediaClip.Chunk0307900E>()!.Version).IsEqualTo(1);
    }

    [Test]
    public async Task Build_ReusesPendingStateWithoutSharingCollectionContainers()
    {
        var track = CGameCtnMediaTrack.Create().ForTMF().Build();
        var outer = CGameCtnMediaClip.Create().WithName("Original").WithTracks(track);
        var selected = outer.ForTMF();
        outer.WithName("Changed");
        outer.WithTracks(Array.Empty<CGameCtnMediaTrack>());

        var first = selected.Build();
        first.Tracks.Clear();
        var second = selected.Build();
        await Assert.That(second.Name).IsEqualTo("Original");
        await Assert.That(second.Tracks.Count).IsEqualTo(1);
        await Assert.That(second.Tracks[0]).IsSameReferenceAs(track);
        await Assert.That(outer.ForTMF().Build().Tracks).IsEmpty();
    }

    [Test]
    public async Task Build_DictionaryMembers_DoNotShareContainers()
    {
        var sound = new NodeRef<CPlugSound>();
        var musics = new Dictionary<string, NodeRef<CPlugSound>>(StringComparer.OrdinalIgnoreCase) { ["Day"] = sound };
        var builder = CGameCtnDecorationAudio.Create().WithMusics(musics).ForTMF();
        musics.Clear();

        var first = builder.Build();
        first.Musics!.Clear();
        var second = builder.Build();
        await Assert.That(second.Musics).IsNotSameReferenceAs(first.Musics);
        await Assert.That(second.Musics!["day"]).IsSameReferenceAs(sound);
    }

    [Test]
    public async Task Build_ReferenceCollections_PreserveUnresolvedDescriptorsWithoutIO()
    {
        var descriptor = new GbxRefTableFile(new GbxRefTable(), 0, true, "missing.Track.Gbx");
        var reference = new NodeRef<CGameCtnMediaTrack> { Descriptor = descriptor };
        var tracks = new NodeRefList<CGameCtnMediaTrack>();
        tracks.Add(reference);
        var builder = CGameCtnMediaClip.Create().WithTracks(tracks).ForTMF();
        tracks.Clear();

        var node = builder.Build();
        await Assert.That(node.Tracks.Get(0).Descriptor).IsSameReferenceAs(descriptor);
        await Assert.That(node.Tracks.Get(0).State).IsEqualTo(NodeRefState.Unresolved);
        node.Tracks.Get(0).Clear();
        await Assert.That(builder.Build().Tracks.Get(0).Descriptor).IsSameReferenceAs(descriptor);
    }

    [Test]
    public async Task Build_ScalarReference_PreservesUnresolvedDescriptor()
    {
        var descriptor = new GbxRefTableFile(new GbxRefTable(), 0, true, "missing.Effect.Gbx");
        var reference = new NodeRef<CControlEffectSimi> { Descriptor = descriptor };
        var builder = CGameCtnMediaBlockText.Create().ForTMF().WithEffectReference(reference);
        reference.Clear();

        var node = builder.Build();
        await Assert.That(node.References.Effect.Descriptor).IsSameReferenceAs(descriptor);
        await Assert.That(node.References.Effect.State).IsEqualTo(NodeRefState.Unresolved);
    }

    [Test]
    public async Task Build_NullScopedChild_ClearsAnEarlierAssignment()
    {
        var effect = CControlEffectSimi.Create().ForTMF().BuildInterface();
        var node = CGameCtnMediaBlockText.Create().ForTMF().WithEffect(effect)
            .WithEffect((IControlEffectSimiTMF?)null).Build();
        await Assert.That(node.References.Effect.Kind).IsEqualTo(NodeRefKind.None);
    }

    [Test]
    public async Task Build_PreservesSuppliedChildrenWithoutValidatingTheirGame()
    {
        var wrong = CGameCtnMediaTrack.Create().ForTM2020().Build();
        var selected = CGameCtnMediaClip.Create().ForTMF().WithTracks(wrong);
        await Assert.That(selected.Build().Tracks[0]).IsSameReferenceAs(wrong);
        await Assert.That(selected.BuildInterface().Tracks[0]).IsSameReferenceAs(wrong);
        var replacement = CGameCtnMediaTrack.Create().ForTMF().Build();
        await Assert.That(selected.WithTracks(replacement).Build().Tracks[0]).IsSameReferenceAs(replacement);
    }

    [Test]
    public async Task Build_BlockFlags_PreserveSetterOrdering()
    {
        var node = CGameCtnBlock.Create().ForTMF().WithFlags(0).WithIsGround(true).WithFlags(0).Build();
        await Assert.That(node.IsGround).IsFalse();
    }

    [Test]
    public async Task Build_Clip_RoundTripsThroughSerialization()
    {
        var track = CGameCtnMediaTrack.Create().WithName("Camera").ForTMF().Build();
        var original = CGameCtnMediaClip.Create().WithName("Intro").WithTracks(track).ForTMF().Build();
        var gbx = original.ToGbx();
        gbx.BodyCompression = GbxCompression.Uncompressed;
        using var stream = new MemoryStream();
        gbx.Save(stream);
        stream.Position = 0;

        var restored = Gbx.ParseNode<CGameCtnMediaClip>(stream);
        await Assert.That(restored.Name).IsEqualTo(original.Name);
        await Assert.That(restored.Tracks.Count).IsEqualTo(1);
        await Assert.That(restored.Tracks[0].Name).IsEqualTo("Camera");
    }
}
