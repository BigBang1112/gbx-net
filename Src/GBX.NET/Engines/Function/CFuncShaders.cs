using GBX.NET.Attributes;

namespace GBX.NET.Engines.Function;

/// <remarks>ID: 0x05014000</remarks>
[Node(0x05014000)]
public class CFuncShaders : CFuncShader
{
    private CFuncShader?[]? funcShaders;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk05014000>]
    public CFuncShader?[]? FuncShaders { get => funcShaders; set => funcShaders = value; }

    internal CFuncShaders()
	{

	}

    #region 0x000 chunk

    /// <summary>
    /// CFuncShaders 0x000 chunk
    /// </summary>
    [Chunk(0x05014000)]
    public class Chunk05014000 : Chunk<CFuncShaders>
    {
        public override void ReadWrite(CFuncShaders n, GameBoxReaderWriter rw)
        {
            rw.ArrayNode<CFuncShader>(ref n.funcShaders);
        }
    }

    #endregion
}
