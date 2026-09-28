namespace GBX.NET.Engines.Game;

/// <summary>
/// Map parameters.
/// </summary>
/// <remarks>ID: 0x0305B000</remarks>
[Class(0x0305B000)]
public partial class CGameCtnChallengeParameters : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x0305B000;




    private string? tip;
    [AppliedWithChunk<Chunk0305B001>]
    [AppliedWithChunk<Chunk0305B001>]
    [AppliedWithChunk<Chunk0305B001>]
    [AppliedWithChunk<Chunk0305B001>]
    [AppliedWithChunk<Chunk0305B00A>]
    public string? Tip { get => tip; set => tip = value; }

    private TimeInt32? bronzeTime;
    [AppliedWithChunk<Chunk0305B004>]
    [AppliedWithChunk<Chunk0305B00A>]
    public TimeInt32? BronzeTime { get => bronzeTime; set => bronzeTime = value; }

    private TimeInt32? silverTime;
    [AppliedWithChunk<Chunk0305B004>]
    [AppliedWithChunk<Chunk0305B00A>]
    public TimeInt32? SilverTime { get => silverTime; set => silverTime = value; }

    private TimeInt32? goldTime;
    [AppliedWithChunk<Chunk0305B004>]
    [AppliedWithChunk<Chunk0305B00A>]
    public TimeInt32? GoldTime { get => goldTime; set => goldTime = value; }

    private TimeInt32? authorTime;
    [AppliedWithChunk<Chunk0305B004>]
    [AppliedWithChunk<Chunk0305B00A>]
    public TimeInt32? AuthorTime { get => authorTime; set => authorTime = value; }

    private TimeInt32 timeLimit;
    [AppliedWithChunk<Chunk0305B008>]
    [AppliedWithChunk<Chunk0305B00A>]
    public TimeInt32 TimeLimit { get => timeLimit; set => timeLimit = value; }

    private int authorScore;
    [AppliedWithChunk<Chunk0305B008>]
    [AppliedWithChunk<Chunk0305B00A>]
    public int AuthorScore { get => authorScore; set => authorScore = value; }

    private string? mapType;
    [AppliedWithChunk<Chunk0305B00E>]
    public string? MapType { get => mapType; set => mapType = value; }

    private string? mapStyle;
    [AppliedWithChunk<Chunk0305B00E>]
    public string? MapStyle { get => mapStyle; set => mapStyle = value; }

    private bool isValidatedForScriptModes;
    [AppliedWithChunk<Chunk0305B00E>]
    public bool IsValidatedForScriptModes { get => isValidatedForScriptModes; set => isValidatedForScriptModes = value; }


    /// <summary>
    /// CGameCtnChallengeParameters 0x000 chunk
    /// </summary>
    [Chunk(0x0305B000)]
    public partial class Chunk0305B000 : Chunk<CGameCtnChallengeParameters>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305B000;

        public int U01;
        public int U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;
        public int U07;
        public int U08;

        public override void ReadWrite(CGameCtnChallengeParameters n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Int32(ref U07);
            rw.Int32(ref U08);
        }
    }

    /// <summary>
    /// CGameCtnChallengeParameters 0x001 chunk (tips)
    /// </summary>
    [Chunk(0x0305B001, "tips")]
    public partial class Chunk0305B001 : Chunk<CGameCtnChallengeParameters>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305B001;


        public override void ReadWrite(CGameCtnChallengeParameters n, GbxReaderWriter rw)
        {
            rw.String(ref n.tip);
            rw.String(ref n.tip);
            rw.String(ref n.tip);
            rw.String(ref n.tip);
        }
    }

    /// <summary>
    /// CGameCtnChallengeParameters 0x002 chunk
    /// </summary>
    [Chunk(0x0305B002)]
    public partial class Chunk0305B002 : Chunk<CGameCtnChallengeParameters>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305B002;

        public int U01;
        public int U02;
        public int U03;
        public float U04;
        public float U05;
        public float U06;
        public int U07;
        public int U08;
        public int U09;
        public int U10;
        public int U11;
        public int U12;
        public int U13;
        public int U14;
        public int U15;
        public int U16;

        public override void ReadWrite(CGameCtnChallengeParameters n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Int32(ref U07);
            rw.Int32(ref U08);
            rw.Int32(ref U09);
            rw.Int32(ref U10);
            rw.Int32(ref U11);
            rw.Int32(ref U12);
            rw.Int32(ref U13);
            rw.Int32(ref U14);
            rw.Int32(ref U15);
            rw.Int32(ref U16);
        }
    }

    /// <summary>
    /// CGameCtnChallengeParameters 0x003 chunk
    /// </summary>
    [Chunk(0x0305B003)]
    public partial class Chunk0305B003 : Chunk<CGameCtnChallengeParameters>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305B003;

        public int U01;
        public float U02;
        public int U03;
        public int U04;
        public int U05;
        public int U06;

        public override void ReadWrite(CGameCtnChallengeParameters n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Single(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
        }
    }

    /// <summary>
    /// CGameCtnChallengeParameters 0x004 chunk (medals)
    /// </summary>
    [Chunk(0x0305B004, "medals")]
    public partial class Chunk0305B004 : Chunk<CGameCtnChallengeParameters>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305B004;

        public uint U01;

        public override void ReadWrite(CGameCtnChallengeParameters n, GbxReaderWriter rw)
        {
            rw.TimeInt32Nullable(ref n.bronzeTime);
            rw.TimeInt32Nullable(ref n.silverTime);
            rw.TimeInt32Nullable(ref n.goldTime);
            rw.TimeInt32Nullable(ref n.authorTime);
            rw.UInt32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnChallengeParameters 0x005 chunk
    /// </summary>
    [Chunk(0x0305B005)]
    public partial class Chunk0305B005 : Chunk<CGameCtnChallengeParameters>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305B005;

        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CGameCtnChallengeParameters n, GbxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    /// <summary>
    /// CGameCtnChallengeParameters 0x006 chunk (items)
    /// </summary>
    [Chunk(0x0305B006, "items")]
    public partial class Chunk0305B006 : Chunk<CGameCtnChallengeParameters>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305B006;

        public uint[]? U01;

        public override void ReadWrite(CGameCtnChallengeParameters n, GbxReaderWriter rw)
        {
            rw.Array<uint>(ref U01!);
        }
    }

    /// <summary>
    /// CGameCtnChallengeParameters 0x007 chunk
    /// </summary>
    [Chunk(0x0305B007)]
    public partial class Chunk0305B007 : Chunk<CGameCtnChallengeParameters>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305B007;

        public uint U01;

        public override void ReadWrite(CGameCtnChallengeParameters n, GbxReaderWriter rw)
        {
            rw.UInt32(ref U01);
        }
    }

    /// <summary>
    /// CGameCtnChallengeParameters 0x008 chunk (stunts)
    /// </summary>
    [Chunk(0x0305B008, "stunts")]
    public partial class Chunk0305B008 : Chunk<CGameCtnChallengeParameters>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305B008;


        public override void ReadWrite(CGameCtnChallengeParameters n, GbxReaderWriter rw)
        {
            rw.TimeInt32(ref n.timeLimit);
            rw.Int32(ref n.authorScore);
        }
    }

    /// <summary>
    /// CGameCtnChallengeParameters 0x00A skippable chunk (medals)
    /// </summary>
    [Chunk(0x0305B00A, "medals")]
    public partial class Chunk0305B00A : SkippableChunk<CGameCtnChallengeParameters>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305B00A;


        public override void ReadWrite(CGameCtnChallengeParameters n, GbxReaderWriter rw)
        {
            rw.String(ref n.tip);
            rw.TimeInt32Nullable(ref n.bronzeTime);
            rw.TimeInt32Nullable(ref n.silverTime);
            rw.TimeInt32Nullable(ref n.goldTime);
            rw.TimeInt32Nullable(ref n.authorTime);
            rw.TimeInt32(ref n.timeLimit);
            rw.Int32(ref n.authorScore);
        }
    }

    /// <summary>
    /// CGameCtnChallengeParameters 0x00D chunk (race validate ghost)
    /// </summary>
    [Chunk(0x0305B00D, "race validate ghost")]
    public partial class Chunk0305B00D : Chunk<CGameCtnChallengeParameters>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305B00D;


        public override void ReadWrite(CGameCtnChallengeParameters n, GbxReaderWriter rw)
        {
            rw.NodeRef<CGameCtnGhost>(ref n.raceValidateGhost);
        }
    }

    /// <summary>
    /// CGameCtnChallengeParameters 0x00E skippable chunk (map type)
    /// </summary>
    [Chunk(0x0305B00E, "map type")]
    public partial class Chunk0305B00E : SkippableChunk<CGameCtnChallengeParameters>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305B00E;


        public override void ReadWrite(CGameCtnChallengeParameters n, GbxReaderWriter rw)
        {
            rw.String(ref n.mapType);
            rw.String(ref n.mapStyle);
            rw.Boolean(ref n.isValidatedForScriptModes);
        }
    }

    /// <summary>
    /// CGameCtnChallengeParameters 0x00F skippable chunk (race validate ghost TM2020)
    /// </summary>
    [Chunk(0x0305B00F, "race validate ghost TM2020")]
    public partial class Chunk0305B00F : SkippableChunk<CGameCtnChallengeParameters>
    {
        /// <inheritdoc />
        public override uint Id => 0x0305B00F;

    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x0305B000 => new Chunk0305B000(),
        0x0305B001 => new Chunk0305B001(),
        0x0305B002 => new Chunk0305B002(),
        0x0305B003 => new Chunk0305B003(),
        0x0305B004 => new Chunk0305B004(),
        0x0305B005 => new Chunk0305B005(),
        0x0305B006 => new Chunk0305B006(),
        0x0305B007 => new Chunk0305B007(),
        0x0305B008 => new Chunk0305B008(),
        0x0305B00A => new Chunk0305B00A(),
        0x0305B00D => new Chunk0305B00D(),
        0x0305B00E => new Chunk0305B00E(),
        0x0305B00F => new Chunk0305B00F(),
        _ => base.NewChunk(chunkId),
    };
}
