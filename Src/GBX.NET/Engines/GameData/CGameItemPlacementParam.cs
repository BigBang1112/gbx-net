namespace GBX.NET.Engines.GameData;

public partial class CGameItemPlacementParam
{
    private const int yawOnlyBit = 1;
    private const int notOnObjectBit = 2;
    private const int autoRotationBit = 3;
    private const int switchPivotManuallyBit = 4;
    private const int ghostModeBit = 5;
    private const int hasPathBit = 6;
    private const int isFreelyAnchorableBit = 0;

    public bool YawOnly
    {
        get => (Flags & (1 << yawOnlyBit)) != 0;
        set
        {
            if (value) Flags |= 1 << yawOnlyBit;
            else Flags &= ~(1 << yawOnlyBit);
        }
    }

    public bool NotOnObject
    {
        get => (Flags & (1 << notOnObjectBit)) != 0;
        set
        {
            if (value) Flags |= 1 << notOnObjectBit;
            else Flags &= ~(1 << notOnObjectBit);
        }
    }

    public bool AutoRotation
    {
        get => (Flags & (1 << autoRotationBit)) != 0;
        set
        {
            if (value) Flags |= 1 << autoRotationBit;
            else Flags &= ~(1 << autoRotationBit);
        }
    }

    public bool SwitchPivotManually
    {
        get => (Flags & (1 << switchPivotManuallyBit)) != 0;
        set
        {
            if (value) Flags |= 1 << switchPivotManuallyBit;
            else Flags &= ~(1 << switchPivotManuallyBit);
        }
    }

    public bool GhostMode
    {
        get => (Flags & (1 << ghostModeBit)) != 0;
        set
        {
            if (value) Flags |= 1 << ghostModeBit;
            else Flags &= ~(1 << ghostModeBit);
        }
    }

    public bool HasPath
    {
        get => (Flags & (1 << hasPathBit)) != 0;
        set
        {
            if (value) Flags |= 1 << hasPathBit;
            else Flags &= ~(1 << hasPathBit);
        }
    }

    public bool IsFreelyAnchorable
    {
        get => (Flags & (1 << isFreelyAnchorableBit)) != 0;
        set
        {
            if (value) Flags |= 1 << isFreelyAnchorableBit;
            else Flags &= ~(1 << isFreelyAnchorableBit);
        }
    }

    public partial class Chunk2E020003
    {
        public override void ReadWrite(CGameItemPlacementParam n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version is < 0 or > 4)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            if (Version >= 4)
            {
                rw.NodeRef(ref n.placementClassRef);
                return;
            }

            if (Version == 3)
            {
                NodeRef.SetEmbedded(ref n.placementClassRef, rw.ReadableWritable(n.placementClassRef?.CachedNode));
                return;
            }

            // The owning placement parameter defaults this flag to true before legacy chunks run.
            var placementClass = n.placementClassRef?.CachedNode;
            if (placementClass is null)
            {
                placementClass = new NPlugItemPlacement_SClass { AlignToInterior = true };
                NodeRef.SetEmbedded(ref n.placementClassRef, placementClass);
            }
            placementClass.SizeGroup = rw.Id(placementClass.SizeGroup);
            placementClass.CompatibleGroupsIds = rw.ArrayId(placementClass.CompatibleGroupsIds);
            placementClass.AlwaysUp = rw.Boolean(placementClass.AlwaysUp);
            if (Version >= 1)
            {
                placementClass.AlignToInterior = rw.Boolean(placementClass.AlignToInterior);
            }
        }
    }
}
