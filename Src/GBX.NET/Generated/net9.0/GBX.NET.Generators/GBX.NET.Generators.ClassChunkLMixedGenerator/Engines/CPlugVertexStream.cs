namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09056000</remarks>
[Class(0x09056000)]
public partial class CPlugVertexStream : CPlug, IClass
{
    [Hexadecimal] public static new uint Id => 0x09056000;





    /// <summary>
    /// CPlugVertexStream 0x000 chunk
    /// </summary>
    [Chunk(0x09056000)]
    public partial class Chunk09056000 : Chunk<CPlugVertexStream>
    {
        /// <inheritdoc />
        public override uint Id => 0x09056000;

    }


    public sealed partial class DataDecl : IReadableWritable
    {
    }


    public enum EPlugVDclSpace
    {
        Global3D,
        Local3D,
        Global2D,
    }

    public enum EPlugVDclType
    {
        U01,
        Float2,
        Float3,
        Float4,
        /// <summary>
        /// 4 bytes RGBA
        /// </summary>
        Color,
        Int32,
        Dec3N = 14,
    }

    public enum EPlugVDcl
    {
        Position,
        Position1,
        TgtRotation,
        BlendWeight,
        BlendIndices,
        Normal,
        Normal1,
        PointSize,
        Color0,
        Color1,
        TexCoord0,
        TexCoord1,
        TexCoord2,
        TexCoord3,
        TexCoord4,
        TexCoord5,
        TexCoord6,
        TexCoord7,
        TangentU,
        TangentU1,
        TangentV,
        TangentV1,
        Color2,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x09056000 => new Chunk09056000(),
        _ => base.NewChunk(chunkId),
    };
}
