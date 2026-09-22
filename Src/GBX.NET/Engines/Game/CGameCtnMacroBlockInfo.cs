namespace GBX.NET.Engines.Game;

public partial class CGameCtnMacroBlockInfo
{
    private CScriptTraitsMetadata? scriptMetadata;
    private Int3 clipTriggerSize = new(3, 1, 3);
    private CGameCtnMediaClipGroup? clipGroupInGame;
    private CGameCtnMediaClipGroup? clipGroupEndRace;

    [AppliedWithChunk<Chunk0310D00B>]
    public CScriptTraitsMetadata? ScriptMetadata { get => scriptMetadata; set => scriptMetadata = value; }

    [AppliedWithChunk<Chunk0310D011>]
    public Int3 ClipTriggerSize { get => clipTriggerSize; set => clipTriggerSize = value; }

    [AppliedWithChunk<Chunk0310D011>]
    public CGameCtnMediaClipGroup? ClipGroupInGame { get => clipGroupInGame; set => clipGroupInGame = value; }

    [AppliedWithChunk<Chunk0310D011>]
    public CGameCtnMediaClipGroup? ClipGroupEndRace { get => clipGroupEndRace; set => clipGroupEndRace = value; }

    public partial class Chunk0310D00B
    {
        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.Encapsulated(rw =>
            {
                rw.Node<CScriptTraitsMetadata>(ref n.scriptMetadata);
            });
        }
    }

    public partial class Chunk0310D011 : IVersionable
    {
        public int Version { get; set; }

        public Int3 U02;

        public override void ReadWrite(CGameCtnMacroBlockInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Encapsulated(rw =>
            {
                // SMediaTrackSpawns
                rw.Int3(ref U02);
                rw.Int3(ref n.clipTriggerSize);
                rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupInGame);
                rw.NodeRef<CGameCtnMediaClipGroup>(ref n.clipGroupEndRace);
                //
            });
        }
    }
}
