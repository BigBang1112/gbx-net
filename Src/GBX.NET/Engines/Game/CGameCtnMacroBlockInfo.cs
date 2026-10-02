namespace GBX.NET.Engines.Game;

public partial class CGameCtnMacroBlockInfo
{
    private Int3 clipTriggerSize = new(3, 1, 3);
    public partial Int3 ClipTriggerSize { get => clipTriggerSize; set => clipTriggerSize = value; }
}
