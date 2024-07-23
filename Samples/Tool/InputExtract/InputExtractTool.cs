using GBX.NET.Engines.Game;
using GBX.NET.Tool;

namespace InputExtract;

public class InputExtractTool : ITool, IConfigurable<InputExtractConfig>
{
    public InputExtractConfig Config { get; } = new();

    public InputExtractTool(CGameCtnReplayRecord replay)
    {

    }

    public InputExtractTool(CGameCtnGhost ghost)
    {

    }
}
