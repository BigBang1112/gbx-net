namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090F6000</remarks>
[Node(0x090F6000)]
[NodeExtension("Cam")]
[NodeExtension("VehicleCameraRace2Model")]
public class CPlugVehicleCameraRace2Model : CPlugCamControlModel
{
    protected CPlugVehicleCameraRace2Model()
    {

    }

    #region 0x000 chunk

    /// <summary>
    /// CPlugVehicleCameraRace2Model 0x000 chunk
    /// </summary>
    [Chunk(0x090F6000)]
    public class Chunk090F6000 : Chunk<CPlugVehicleCameraRace2Model>
    {
        public float U01;
        public int U02;
        public int U03;
        public float U04;
        public int U05;
        public int U06;
        public float U07;
        public int U08;
        public int U09;
        public float U10;
        public int U11;
        public int U12;
        public float U13;
        public int U14;
        public int U15;
        public float U16;
        public int U17;
        public int U18;
        public float U19;
        public int U20;
        public int U21;
        public float U22;
        public int U23;
        public int U24;
        public float U25;
        public int U26;
        public int U27;
        public float U28;
        public int U29;
        public int U30;
        public float U31;
        public int U32;
        public int U33;
        public float U34;
        public int U35;
        public int U36;
        public float U37;
        public int U38;
        public int U39;
        public float U40;
        public int U41;
        public int U42;
        public float U43;
        public int U44;
        public int U45;
        public float U46;
        public float U47;
        public int U48;
        public int U49;
        public int U50;
        public int U51;
        public int U52;
        public int U53;
        public float U54;
        public float U55;

        public override void ReadWrite(CPlugVehicleCameraRace2Model n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Single(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Single(ref U07);
            rw.Int32(ref U08);
            rw.Int32(ref U09);
            rw.Single(ref U10);
            rw.Int32(ref U11);
            rw.Int32(ref U12);
            rw.Single(ref U13);
            rw.Int32(ref U14);
            rw.Int32(ref U15);
            rw.Single(ref U16);
            rw.Int32(ref U17);
            rw.Int32(ref U18);
            rw.Single(ref U19);
            rw.Int32(ref U20);
            rw.Int32(ref U21);
            rw.Single(ref U22);
            rw.Int32(ref U23);
            rw.Int32(ref U24);
            rw.Single(ref U25);
            rw.Int32(ref U26);
            rw.Int32(ref U27);
            rw.Single(ref U28);
            rw.Int32(ref U29);
            rw.Int32(ref U30);
            rw.Single(ref U31);
            rw.Int32(ref U32);
            rw.Int32(ref U33);
            rw.Single(ref U34);
            rw.Int32(ref U35);
            rw.Int32(ref U36);
            rw.Single(ref U37);
            rw.Int32(ref U38);
            rw.Int32(ref U39);
            rw.Single(ref U40);
            rw.Int32(ref U41);
            rw.Int32(ref U42);
            rw.Single(ref U43);
            rw.Int32(ref U44);
            rw.Int32(ref U45);
            rw.Single(ref U46);
            rw.Single(ref U47);
            rw.Int32(ref U48);
            rw.Int32(ref U49);
            rw.Int32(ref U50);
            rw.Int32(ref U51);
            rw.Int32(ref U52);
            rw.Int32(ref U53);
            rw.Single(ref U54);
            rw.Single(ref U55);

            // ...
        }
    }

    #endregion
}
