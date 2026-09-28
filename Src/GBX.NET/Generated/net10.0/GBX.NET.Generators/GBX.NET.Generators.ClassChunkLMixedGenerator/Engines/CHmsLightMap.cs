namespace GBX.NET.Engines.Hms;

/// <remarks>ID: 0x06021000</remarks>
[Class(0x06021000)]
public partial class CHmsLightMap : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x06021000;




    /// <summary>
    /// Creates a new instance of <see cref="CHmsLightMap"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CHmsLightMap() { }


    /// <summary>
    /// CHmsLightMap 0x001 chunk
    /// </summary>
    [Chunk(0x06021001)]
    public partial class Chunk06021001 : Chunk<CHmsLightMap>
    {
        /// <inheritdoc />
        public override uint Id => 0x06021001;

        public CPlugPointsInSphereOpt? U01;
        public Components.GbxRefTableFile? U01File;

        public override void ReadWrite(CHmsLightMap n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugPointsInSphereOpt>(ref U01, ref U01File);
        }
    }

    /// <summary>
    /// CHmsLightMap 0x002 chunk
    /// </summary>
    [Chunk(0x06021002)]
    public partial class Chunk06021002 : Chunk<CHmsLightMap>
    {
        /// <inheritdoc />
        public override uint Id => 0x06021002;

        public CMwNod? U01;
        public Components.GbxRefTableFile? U01File;

        public override void ReadWrite(CHmsLightMap n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwNod>(ref U01, ref U01File);
        }
    }

    /// <summary>
    /// CHmsLightMap 0x003 chunk
    /// </summary>
    [Chunk(0x06021003)]
    public partial class Chunk06021003 : Chunk<CHmsLightMap>
    {
        /// <inheritdoc />
        public override uint Id => 0x06021003;

        public CHmsLightMapMood? U01;
        public Components.GbxRefTableFile? U01File;

        public override void ReadWrite(CHmsLightMap n, GbxReaderWriter rw)
        {
            rw.NodeRef<CHmsLightMapMood>(ref U01, ref U01File);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x06021001 => new Chunk06021001(),
        0x06021002 => new Chunk06021002(),
        0x06021003 => new Chunk06021003(),
        _ => base.NewChunk(chunkId),
    };
}
