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
}
