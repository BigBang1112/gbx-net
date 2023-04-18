namespace GBX.NET.Engines.Motion;

/// <remarks>ID: 0x08028000</remarks>
[Node(0x08028000)]
public class CMotions : CMotion
{
    private CMotion[] motions = Array.Empty<CMotion>();

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk08028001>]
    public CMotion[] Motions { get => motions; set => motions = value; }

    internal CMotions()
    {

    }

    #region 0x001 chunk

    /// <summary>
    /// CMotions 0x001 chunk
    /// </summary>
    [Chunk(0x08028001)]
    public class Chunk08028001 : Chunk<CMotions>
    {
        public override void ReadWrite(CMotions n, GameBoxReaderWriter rw)
        {
            rw.ArrayNode<CMotion>(ref n.motions!);
        }
    }

    #endregion
}