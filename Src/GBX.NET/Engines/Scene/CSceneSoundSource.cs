namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A00E000</remarks>
[Node(0x0A00E000)]
public class CSceneSoundSource : CScenePoc
{
    protected CSceneSoundSource()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CSceneSoundSource 0x000 chunk
    /// </summary>
    [Chunk(0x0A00E000)]
    public class Chunk0A00E000 : Chunk<CSceneSoundSource>
    {
        public CHmsSoundSource? U01;

        public override void Read(CSceneSoundSource n, GameBoxReader r, ILogger? logger)
        {
            U01 = Parse<CHmsSoundSource>(r, 0x0600D000, progress: null, logger); // Gliding/AfterBurnout/Burnout
        }
    }

    #endregion
}