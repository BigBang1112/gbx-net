using GBX.NET.Engines.Plug;
using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.GameData;

[Node(0x2E026000)]
public sealed class CGameCommonItemEntityModelEdition : CMwNod
{
    #region Enums

    public enum EItemType
    {
        Undefined,
        Ornament,
        PickUp,
        Character,
        Vehicle,
        Spot,
        Cannon,
        Group,
        Decal,
        Turret,
        Wagon,
        Block,
        EntitySpawner,
        DeprecV,
        Procedural
    }

    #endregion

    #region Fields

    private EItemType itemType;
    private CPlugCrystal meshCrystal;

    #endregion

    #region Properties

    /// <summary>
    /// Type of the item.
    /// </summary>
    public EItemType ItemType
    {
        get => itemType;
        set => itemType = value;
    }

    /// <summary>
    /// Mesh of the item model.
    /// </summary>
    public CPlugCrystal MeshCrystal
    {
        get => meshCrystal;
        set => meshCrystal = value;
    }

    #endregion

    #region Constructors

    private CGameCommonItemEntityModelEdition()
    {
        meshCrystal = null!;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CGameCommonItemEntityModelEdition 0x000 chunk
    /// </summary>
    [Chunk(0x2E026000)]
    public class Chunk2E026000 : Chunk<CGameCommonItemEntityModelEdition>, IVersionable
    {
        private int version;

        public string? U01;

        public int Version
        {
            get => version;
            set => version = value;
        }

        public override void ReadWrite(CGameCommonItemEntityModelEdition n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.EnumInt32<EItemType>(ref n.itemType);
            rw.NodeRef<CPlugCrystal>(ref n.meshCrystal!);
            rw.String(ref U01);

            for (var i = 0; i < 32; i++)
                rw.Int32();
        }
    }

    #endregion

    #endregion
}
