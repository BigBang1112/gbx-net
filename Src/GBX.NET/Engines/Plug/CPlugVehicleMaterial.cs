namespace GBX.NET.Engines.Plug;

[Node(0x090F1000)]
[NodeExtension("VehicleMaterial")]
public class CPlugVehicleMaterial : CMwNod
{
    internal CPlugVehicleMaterial()
    {
        
    }

    #region 0x00F chunk

    /// <summary>
    /// CPlugVehicleMaterial 0x00F chunk
    /// </summary>
    [Chunk(0x090F100F)]
    public class Chunk090F100F : Chunk<CPlugVehicleMaterial>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugVehicleMaterial n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    #endregion

    #region 0x010 chunk

    /// <summary>
    /// CPlugVehicleMaterial 0x010 chunk
    /// </summary>
    [Chunk(0x090F1010)]
    public class Chunk090F1010 : Chunk<CPlugVehicleMaterial>, IVersionable
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public byte U05;
        public float? U06;
        public float? U07;
        public float? U08;
        public float? U09;
        public float? U10;
        public float? U11;
        public float? U12;
        public float? U13;
        public float? U14;
        public bool? U15;
        public Vec2? U16;
        public float? U17;
        public float? U18;
        public float? U19;
        public float? U20;

        public int Version { get; set; } = 7;

        public override void ReadWrite(CPlugVehicleMaterial n, GameBoxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version > 7)
            {
                throw new ChunkVersionNotSupportedException(Version);
            }

            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Byte(ref U05);

            if (Version == 1)
            {
                rw.Single(ref U06);
                rw.Single(ref U07);
            }

            if (Version >= 1)
            {
                rw.Single(ref U08);
                rw.Single(ref U09);
                rw.Single(ref U10);

                if (Version >= 2)
                {
                    rw.Single(ref U11);

                    if (Version >= 3)
                    {
                        rw.Single(ref U12);
                        rw.Single(ref U13);
                        rw.Single(ref U14);

                        if (Version >= 4)
                        {
                            rw.Boolean(ref U15);
                            rw.Vec2(ref U16);

                            if (Version >= 5)
                            {
                                rw.Single(ref U17);

                                if (Version >= 6)
                                {
                                    rw.Single(ref U18);

                                    if (Version >= 7) // not seen in code, pls check
                                    {
                                        rw.Single(ref U19);
                                        rw.Single(ref U20);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion
}
