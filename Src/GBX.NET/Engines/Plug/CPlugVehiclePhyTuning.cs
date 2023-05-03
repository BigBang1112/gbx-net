namespace GBX.NET.Engines.Scene;

[Node(0x090EB000)] // previously CSceneVehicleTuning
public class CPlugVehiclePhyTuning : CMwNod
{
    private string name = "";
    
    [NodeMember]
    [AppliedWithChunk<Chunk090EB000>]
    [AppliedWithChunk<CPlugVehicleCarPhyTuning.Chunk090ED001>]
    public string Name { get => name; set => name = value; }

    internal CPlugVehiclePhyTuning()
	{

    }

    #region 0x000 chunk

    /// <summary>
    /// CPlugVehiclePhyTuning 0x000 chunk
    /// </summary>
    [Chunk(0x090EB000)]
    public class Chunk090EB000 : Chunk<CPlugVehiclePhyTuning>
    {
        public CFuncKeysReal? U01;
        public float U02;

        public override void ReadWrite(CPlugVehiclePhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.name!);
            rw.NodeRef<CFuncKeysReal>(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x001 chunk

    /// <summary>
    /// CPlugVehiclePhyTuning 0x001 chunk
    /// </summary>
    [Chunk(0x090EB001)]
    public class Chunk090EB001 : Chunk<CPlugVehiclePhyTuning>
    {
        public bool U01;

        public override void ReadWrite(CPlugVehiclePhyTuning n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    #endregion
}