namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker clip group.
/// </summary>
/// <remarks>ID: 0x0307A000</remarks>
[Class(0x0307A000)]
public partial class CGameCtnMediaClipGroup : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0307A000;





    /// <summary>
    /// CGameCtnMediaClipGroup 0x001 chunk
    /// </summary>
    [Chunk(0x0307A001)]
    public partial class Chunk0307A001 : Chunk<CGameCtnMediaClipGroup>
    {
        /// <inheritdoc />
        public override uint Id => 0x0307A001;

    }

    /// <summary>
    /// CGameCtnMediaClipGroup 0x002 chunk
    /// </summary>
    [Chunk(0x0307A002)]
    public partial class Chunk0307A002 : Chunk<CGameCtnMediaClipGroup>
    {
        /// <inheritdoc />
        public override uint Id => 0x0307A002;

    }

    /// <summary>
    /// CGameCtnMediaClipGroup 0x003 chunk
    /// </summary>
    [Chunk(0x0307A003)]
    public partial class Chunk0307A003 : Chunk<CGameCtnMediaClipGroup>
    {
        /// <inheritdoc />
        public override uint Id => 0x0307A003;

    }

    /// <summary>
    /// CGameCtnMediaClipGroup 0x004 skippable chunk
    /// </summary>
    [Chunk(0x0307A004)]
    public partial class Chunk0307A004 : SkippableChunk<CGameCtnMediaClipGroup>
    {
        /// <inheritdoc />
        public override uint Id => 0x0307A004;

        public int U01;

        public override void ReadWrite(CGameCtnMediaClipGroup n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }


    public sealed partial class Trigger : IReadable, IWritable
    {
        public List<Int3>? Coords { get; set; }
        public int U01 { get; set; }
        public int U02 { get; set; }
        public int U03 { get; set; }
        public int U04 { get; set; }
        public ECondition Condition { get; set; }
        public float ConditionValue { get; set; }

        public void Read(GbxReader r, int v = 0)
        {
            if (v == 0)
            {
                return;
            }
            if (v <= 2)
            {
                Coords = r.ReadList<Int3>();
            }
            if (v >= 2)
            {
                U01 = r.ReadInt32();
                U02 = r.ReadInt32();
                U03 = r.ReadInt32();
                U04 = r.ReadInt32();
                if (v >= 3)
                {
                    Condition = (ECondition)r.ReadInt32();
                    ConditionValue = r.ReadSingle();
                    Coords = r.ReadList<Int3>();
                }
            }
        }

        public void Write(GbxWriter w, int v = 0)
        {
            if (v == 0)
            {
                return;
            }
            if (v <= 2)
            {
                w.WriteList<Int3>(Coords);
            }
            if (v >= 2)
            {
                w.Write(U01);
                w.Write(U02);
                w.Write(U03);
                w.Write(U04);
                if (v >= 3)
                {
                    w.Write((int)Condition);
                    w.Write(ConditionValue);
                    w.WriteList<Int3>(Coords);
                }
            }
        }
    }


    public enum ECondition
    {
        None,
        RaceTimeLessThan,
        RaceTimeGreaterThan,
        AlreadyTriggered,
        SpeedLessThan,
        SpeedGreaterThan,
        NotAlreadyTriggered,
        MaxPlayCount,
        RandomOnce,
        Random,
    }


    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0307A001 => new Chunk0307A001(),
        0x0307A002 => new Chunk0307A002(),
        0x0307A003 => new Chunk0307A003(),
        0x0307A004 => new Chunk0307A004(),
        _ => base.NewChunk(chunkId),
    };
}
