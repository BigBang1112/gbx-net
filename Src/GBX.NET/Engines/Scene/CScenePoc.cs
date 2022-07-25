namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A009000</remarks>
[Node(0x0A009000)]
public abstract class CScenePoc : CSceneObject
{
    private bool isActive;

    public bool IsActive { get => isActive; set => isActive = value; }

    protected CScenePoc()
    {
        
    }

    #region 0x000 chunk

    /// <summary>
    /// CScenePoc 0x000 chunk
    /// </summary>
    [Chunk(0x0A009000)]
    public class Chunk0A009000 : Chunk<CScenePoc>
    {
        public override void ReadWrite(CScenePoc n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.isActive);
        }
    }

    #endregion
}