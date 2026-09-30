namespace GBX.NET.Engines.Game;

public partial class CGameCtnMacroBlockInfo
{
    private CScriptTraitsMetadata? scriptMetadata;
    private Int3 clipTriggerSize = new(3, 1, 3);
    private CGameCtnMediaClipGroup? clipGroupInGame;
    private CGameCtnMediaClipGroup? clipGroupEndRace;

    public partial CScriptTraitsMetadata? ScriptMetadata { get => scriptMetadata; set => scriptMetadata = value; }

    public partial Int3 ClipTriggerSize { get => clipTriggerSize; set => clipTriggerSize = value; }

    public partial CGameCtnMediaClipGroup? ClipGroupInGame { get => clipGroupInGame; set => clipGroupInGame = value; }

    public partial CGameCtnMediaClipGroup? ClipGroupEndRace { get => clipGroupEndRace; set => clipGroupEndRace = value; }
}
