using GBX.NET.Exceptions;

namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A083000</remarks>
[Node(0x0A083000)]
public class CSceneVehicleCarMarksSamples : CMwNod
{
    private uint[]? stops;
    private string? name;
    private bool disabled;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0A083002))]
    public uint[]? Stops { get => stops; set => stops = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0A083003))]
    public string? Name { get => name; set => name = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk0A083004))]
    public bool Disabled { get => disabled; set => disabled = value; }

    protected CSceneVehicleCarMarksSamples()
	{
        
	}

    #region 0x002 chunk

    /// <summary>
    /// CSceneVehicleCarMarksSamples 0x002 chunk
    /// </summary>
    [Chunk(0x0A083002)]
    public class Chunk0A083002 : Chunk<CSceneVehicleCarMarksSamples>
    {
        public override void ReadWrite(CSceneVehicleCarMarksSamples n, GameBoxReaderWriter rw)
        {
            rw.Array<uint>(ref n.stops);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CSceneVehicleCarMarksSamples 0x003 chunk
    /// </summary>
    [Chunk(0x0A083003)]
    public class Chunk0A083003 : Chunk<CSceneVehicleCarMarksSamples>
    {
        public override void ReadWrite(CSceneVehicleCarMarksSamples n, GameBoxReaderWriter rw)
        {
            rw.String(ref n.name);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CSceneVehicleCarMarksSamples 0x004 chunk
    /// </summary>
    [Chunk(0x0A083004)]
    public class Chunk0A083004 : Chunk<CSceneVehicleCarMarksSamples>
    {
        public override void ReadWrite(CSceneVehicleCarMarksSamples n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref n.disabled);
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CSceneVehicleCarMarksSamples 0x006 chunk
    /// </summary>
    [Chunk(0x0A083006)]
    public class Chunk0A083006 : Chunk<CSceneVehicleCarMarksSamples>, IVersionable
    {
        private int version;
        private int U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CSceneVehicleCarMarksSamples n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version > 0)
            {
                throw new ChunkVersionNotSupportedException(version);
            }

            rw.Int32(ref U01);

            if (U01 > 0)
            {
                throw new Exception("U01 > 0");
            }
        }
    }

    #endregion
}
