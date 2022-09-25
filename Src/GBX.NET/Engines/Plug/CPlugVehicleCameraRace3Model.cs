namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090EF000</remarks>
[Node(0x090EF000)]
[NodeExtension("Cam")]
[NodeExtension("VehicleCameraRace3Model")]
public class CPlugVehicleCameraRace3Model : CPlugCamControlModel
{
    internal CPlugVehicleCameraRace3Model()
    {

    }

    #region 0x001 chunk

    /// <summary>
    /// CPlugVehicleCameraRace3Model 0x001 chunk
    /// </summary>
    [Chunk(0x090EF001)]
    public class Chunk090EF001 : Chunk<CPlugVehicleCameraRace3Model>
    {
        public float U01;

        public override void ReadWrite(CPlugVehicleCameraRace3Model n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CPlugVehicleCameraRace3Model 0x002 chunk
    /// </summary>
    [Chunk(0x090EF002)]
    public class Chunk090EF002 : Chunk<CPlugVehicleCameraRace3Model>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleCameraRace3Model n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CPlugVehicleCameraRace3Model 0x003 chunk
    /// </summary>
    [Chunk(0x090EF003)]
    public class Chunk090EF003 : Chunk<CPlugVehicleCameraRace3Model>
    {
        public bool U01;
        public bool U02;

        public override void ReadWrite(CPlugVehicleCameraRace3Model n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CPlugVehicleCameraRace3Model 0x004 chunk
    /// </summary>
    [Chunk(0x090EF004)]
    public class Chunk090EF004 : Chunk<CPlugVehicleCameraRace3Model>
    {
        public string? U01;

        public override void ReadWrite(CPlugVehicleCameraRace3Model n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    #endregion

    #region 0x005 chunk

    /// <summary>
    /// CPlugVehicleCameraRace3Model 0x005 chunk
    /// </summary>
    [Chunk(0x090EF005)]
    public class Chunk090EF005 : Chunk<CPlugVehicleCameraRace3Model>, IVersionable
    {
        private int version;

        public int Version { get => version; set => version = value; }

        public float U01;
        public float U02;
        public float U03;
        public int U04;
        public int U05;
        public int U06;
        public float U07;
        public float U08;
        public float U09;
        public int U10;
        public int U11;
        public float U12;
        public int U13;
        public int U14;
        public float U15;
        public int U16;
        public int U17;
        public float U18;
        public int U19;
        public int U20;
        public float U21;
        public float U22;
        public int U23;
        public int U24;
        public float U25;
        public float U26;
        public float U27;
        public int U28;
        public int U29;
        public float U30;
        public int U31;
        public int U32;
        public float U33;
        public float U34;
        public int U35;
        public int U36;
        public float U37;
        public float U38;
        public float U39;
        public float U40;
        public float U41;
        public float U42;
        public int U43;
        public int U44;
        public float U45;
        public int U46;
        public int U47;
        public float U48;
        public int U49;
        public int U50;
        public float U51;
        public int U52;
        public int U53;
        public float U54;
        public int U55;
        public int U56;
        public float U57;
        public int U58;
        public int U59;
        public float U60;
        public int U61;
        public int U62;
        public float U63;
        public int U64;
        public int U65;

        public override void ReadWrite(CPlugVehicleCameraRace3Model n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Single(ref U07);
            rw.Single(ref U08);
            rw.Single(ref U09);
            rw.Int32(ref U10);
            rw.Int32(ref U11);
            rw.Single(ref U12);
            rw.Int32(ref U13);
            rw.Int32(ref U14);
            rw.Single(ref U15);
            rw.Int32(ref U16);
            rw.Int32(ref U17);
            rw.Single(ref U18);
            rw.Int32(ref U19);
            rw.Int32(ref U20);
            rw.Single(ref U21);
            rw.Single(ref U22);
            rw.Int32(ref U23);
            rw.Int32(ref U24);
            rw.Single(ref U25);
            rw.Single(ref U26);
            rw.Single(ref U27);
            rw.Int32(ref U28);
            rw.Int32(ref U29);
            rw.Single(ref U30);
            rw.Int32(ref U31);
            rw.Int32(ref U32);
            rw.Single(ref U33);
            rw.Single(ref U34);
            rw.Int32(ref U35);
            rw.Int32(ref U36);
            rw.Single(ref U37);
            rw.Single(ref U38);
            rw.Single(ref U39);
            rw.Single(ref U40);
            rw.Single(ref U41);
            rw.Single(ref U42);
            rw.Int32(ref U43);
            rw.Int32(ref U44);
            rw.Single(ref U45);
            rw.Int32(ref U46);
            rw.Int32(ref U47);
            rw.Single(ref U48);
            rw.Int32(ref U49);
            rw.Int32(ref U50);
            rw.Single(ref U51);
            rw.Int32(ref U52);
            rw.Int32(ref U53);
            rw.Single(ref U54);
            rw.Int32(ref U55);
            rw.Int32(ref U56);
            rw.Single(ref U57);
            rw.Int32(ref U58);
            rw.Int32(ref U59);
            rw.Single(ref U60);
            rw.Int32(ref U61);
            rw.Int32(ref U62);
            rw.Single(ref U63);
            rw.Int32(ref U64);
            rw.Int32(ref U65);

            // ...
        }
    }

    #endregion
}
