using GBX.NET.Components;

namespace GBX.NET.Engines.GameData;

public partial class CGameItemModel
{
    private CMwNod? phyModelCustom;
    public CMwNod? PhyModelCustom
    {
        get => phyModelCustomFile?.GetNode(ref phyModelCustom) ?? phyModelCustom;
        set => phyModelCustom = value;
    }
    private GbxRefTableFile? phyModelCustomFile;
    public GbxRefTableFile? PhyModelCustomFile { get => phyModelCustomFile; set => phyModelCustomFile = value; }
    public CMwNod? GetPhyModelCustom(GbxReadSettings settings = default) => phyModelCustomFile?.GetNode(ref phyModelCustom, settings);

    private CMwNod? visModelCustom;
    public CMwNod? VisModelCustom { get => visModelCustom; set => visModelCustom = value; }

    private string? defaultWeaponName;
    public string? DefaultWeaponName { get => defaultWeaponName; set => defaultWeaponName = value; }

    private CGameActionModel[]? actions;
    public CGameActionModel[]? Actions { get => actions; set => actions = value; }

    private EDefaultCam defaultCam;
    public EDefaultCam DefaultCam { get => defaultCam; set => defaultCam = value; }

    private CMwNod? entityModelEdition;
    public CMwNod? EntityModelEdition { get => entityModelEdition; set => entityModelEdition = value; }

    private CMwNod? entityModel;
    public CMwNod? EntityModel { get => entityModel; set => entityModel = value; }

    public partial class Chunk2E002019 : IVersionable
    {
        public int Version { get; set; }

        public int? U01;
        public CMwNod? U02;
        public CMwNod? U03;

        public override void ReadWrite(CGameItemModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            var itemTypeVersion = GetItemTypeVersion((int)n.itemType);

            if (itemTypeVersion.HasValue && Version < itemTypeVersion)
            {
                rw.NodeRef<CMwNod>(ref n.phyModelCustom, ref n.phyModelCustomFile);
                rw.NodeRef<CMwNod>(ref n.visModelCustom);
            }

            if (Version >= 3)
            {
                rw.Id(ref n.defaultWeaponName);

                if (Version >= 4)
                {
                    if (n.phyModelCustom is not null)
                    {
                        rw.Int32(-1);
                    }
                    else
                    {
                        rw.NodeRef<CMwNod>(ref n.phyModelCustom);
                    }

                    if (Version >= 5)
                    {
                        rw.NodeRef<CMwNod>(ref n.visModelCustom);

                        if (Version >= 6)
                        {
                            rw.ArrayNodeRef<CGameActionModel>(ref n.actions);

                            if (Version >= 7)
                            {
                                rw.EnumInt32<EDefaultCam>(ref n.defaultCam);

                                if (Version >= 8)
                                {
                                    rw.NodeRef<CMwNod>(ref n.entityModelEdition); // CGameCommonItemEntityModelEdition, CGameBlockItem

                                    if (n.entityModelEdition is null)
                                    {
                                        if (n.itemType == EItemType.Block)
                                        {
                                            throw new Exception("EntityModel cannot exist for Block");
                                        }

                                        rw.NodeRef<CMwNod>(ref n.entityModel);
                                    }

                                    if (Version >= 13)
                                    {
                                        rw.NodeRef(ref U02);

                                        if (Version >= 15)
                                        {
                                            rw.NodeRef(ref U03);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static int? GetItemTypeVersion(int itemType) => itemType switch
        {
            1 or 2 => 9,
            4 => 10,
            5 => 9,
            11 => null,
            _ => 12
        };
    }
}
