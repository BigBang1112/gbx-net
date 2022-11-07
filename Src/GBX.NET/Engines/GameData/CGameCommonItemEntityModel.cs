namespace GBX.NET.Engines.GameData;

/// <remarks>ID: 0x2E027000</remarks>
[Node(0x2E027000)]
public class CGameCommonItemEntityModel : CMwNod
{
    private CMwNod? phyModel;
    private CMwNod? visModel;
    private CMwNod? staticObject;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E027000))]
    public CMwNod? PhyModel { get => phyModel; set => phyModel = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E027000))]
    public CMwNod? VisModel { get => visModel; set => visModel = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk(typeof(Chunk2E027000), sinceVersion: 4)]
    public CMwNod? StaticObject { get => staticObject; set => staticObject = value; }

    internal CGameCommonItemEntityModel()
    {
        
    }

    #region 0x000 chunk

    /// <summary>
    /// CGameCommonItemEntityModel 0x000 chunk
    /// </summary>
    [Chunk(0x2E027000)]
    public class Chunk2E027000 : Chunk<CGameCommonItemEntityModel>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CGameCommonItemEntityModel n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version >= 4)
            {
                rw.NodeRef<CMwNod>(ref n.staticObject);
                return;
            }

            rw.NodeRef<CMwNod>(ref n.phyModel);
            rw.NodeRef<CMwNod>(ref n.visModel);
        }
    }

    #endregion
}
