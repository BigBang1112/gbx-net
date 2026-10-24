namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockInfoVariant
{
    public bool IsObsoleteVariant
    {
        get => IsFlagBitSet(0);
        set => SetFlagBit(0, value);
    }

    public bool IsFakeReplacement
    {
        get => IsFlagBitSet(1);
        set => SetFlagBit(1, value);
    }

    public bool AutoChangeVariantOff
    {
        get => IsFlagBitSet(2);
        set => SetFlagBit(2, value);
    }

    public bool DontPlaySound1
    {
        get => IsFlagBitSet(3);
        set => SetFlagBit(3, value);
    }

    public bool DontPlaySound2
    {
        get => IsFlagBitSet(4);
        set => SetFlagBit(4, value);
    }

    public bool IsNoPillarBelowVariant
    {
        get => IsFlagBitSet(5);
        set => SetFlagBit(5, value);
    }

    [Obsolete("Use FlockEmitter instead.")]
    public FlockEmitterState? FlockEmmiter
    {
        get => FlockEmitter;
        set => FlockEmitter = value;
    }

    private bool IsFlagBitSet(int bit) => (Flags & (1 << bit)) != 0;

    private void SetFlagBit(int bit, bool value)
    {
        Flags = (ushort)(value ? Flags | (1 << bit) : Flags & ~(1 << bit));
    }

    public partial class PlacedPillarParam
    {
        [Obsolete("Use BlockInfo instead.")]
        public CMwNod? U01
        {
            get => BlockInfo;
            set => BlockInfo = (CGameCtnBlockInfo?)value;
        }

        [Obsolete("Use Offset.X instead.")]
        public int U02
        {
            get => Offset.X;
            set => Offset = Offset with { X = value };
        }

        [Obsolete("Use Offset.Y instead.")]
        public int U03
        {
            get => Offset.Y;
            set => Offset = Offset with { Y = value };
        }

        [Obsolete("Use Offset.Z instead.")]
        public int U04
        {
            get => Offset.Z;
            set => Offset = Offset with { Z = value };
        }

        [Obsolete("Use Dir instead.")]
        public int U05
        {
            get => (int)Dir;
            set => Dir = (ECardinalDir)value;
        }
    }

    public partial class ReplacedPillarParam
    {
        [Obsolete("Use BlockInfo instead.")]
        public CMwNod? U01
        {
            get => BlockInfo;
            set => BlockInfo = (CGameCtnBlockInfo?)value;
        }

        [Obsolete("Use Offset2D.X instead.")]
        public int U02
        {
            get => Offset2D.X;
            set => Offset2D = Offset2D with { X = value };
        }

        [Obsolete("Use Offset2D.Y instead.")]
        public int U03
        {
            get => Offset2D.Y;
            set => Offset2D = Offset2D with { Y = value };
        }

        [Obsolete("Use Offset2D.Z instead.")]
        public int U04
        {
            get => Offset2D.Z;
            set => Offset2D = Offset2D with { Z = value };
        }

        [Obsolete("Use MultiDir instead.")]
        public int U05
        {
            get => (int)MultiDir;
            set => MultiDir = (EMultiDir)value;
        }

        [Obsolete("Use IsOnFlyingBase instead.")]
        public byte U06
        {
            get => IsOnFlyingBase ? (byte)1 : (byte)0;
            set => IsOnFlyingBase = value != 0;
        }
    }

    public partial class WaterVolume
    {
        [Obsolete("Use Boxes instead.")]
        public BoxInt3[]? U01
        {
            get => Boxes;
            set => Boxes = value;
        }

        [Obsolete("Use Offset.X instead.")]
        public float U02
        {
            get => Offset.X;
            set => Offset = Offset with { X = value };
        }

        [Obsolete("Use Offset.Y instead.")]
        public float U03
        {
            get => Offset.Y;
            set => Offset = Offset with { Y = value };
        }

        [Obsolete("Use Offset.Z instead.")]
        public float U04
        {
            get => Offset.Z;
            set => Offset = Offset with { Z = value };
        }

        [Obsolete("Use RotationY instead.")]
        public float U05
        {
            get => RotationY;
            set => RotationY = value;
        }

        [Obsolete("Use BoxSize.X instead.")]
        public float U06
        {
            get => BoxSize.X;
            set => BoxSize = BoxSize with { X = value };
        }

        [Obsolete("Use BoxSize.Y instead.")]
        public float U07
        {
            get => BoxSize.Y;
            set => BoxSize = BoxSize with { Y = value };
        }

        [Obsolete("Use BoxSize.Z instead.")]
        public float U08
        {
            get => BoxSize.Z;
            set => BoxSize = BoxSize with { Z = value };
        }

        [Obsolete("Use WaterId instead.")]
        public string? U09
        {
            get => WaterId;
            set => WaterId = value;
        }
    }

    public partial class FlockEmitterState
    {
        [Obsolete("Use Radius instead.")]
        public float U01
        {
            get => Radius;
            set => Radius = value;
        }

        [Obsolete("Use Power instead.")]
        public float U02
        {
            get => Power;
            set => Power = value;
        }

        [Obsolete("Use SpawnCount instead.")]
        public int U03
        {
            get => SpawnCount;
            set => SpawnCount = value;
        }

        [Obsolete("Use IsRepulsor instead.")]
        public bool U04
        {
            get => IsRepulsor;
            set => IsRepulsor = value;
        }

        [Obsolete("Use IsLandingArea instead.")]
        public bool U05
        {
            get => IsLandingArea;
            set => IsLandingArea = value;
        }
    }
}
