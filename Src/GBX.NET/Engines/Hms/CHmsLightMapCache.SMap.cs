namespace GBX.NET.Engines.Hms;

public partial class CHmsLightMapCache
{
    public class SMap
    {
        private TimeSpan? dayTime;

        public TimeSpan? DayTime { get => dayTime; set => dayTime = value; }

        public uint? U01;
        public Vec4 U02;
        public bool U03;
        public short[]? U04;
        public bool U05;
        public int U06;
        public int U07;
        public Vec3 U08;
        public Vec4 U09;
        public Vec3 U10;
        public int U11;

        public void ReadWrite(GameBoxReaderWriter rw, int version)
        {
            rw.Int32(0);
            rw.TimeOfDay(ref dayTime);

            if (version >= 10)
            {
                rw.UInt32(ref U01);
            }

            rw.Vec4(ref U02); // 4 floats
            rw.Boolean(ref U03);

            // GmVec3_ArchiveAsReal16
            rw.Array<short>(ref U04, count: 3);

            if (version >= 4)
            {
                rw.Boolean(ref U05);
                rw.Int32(ref U06);
            }
            else
            {
                U05 = true;
                U06 = 1;
            }

            if (version >= 13)
            {
                rw.Int32(ref U07);
            }
            else
            {
                U07 = 2;
            }

            if (version < 8)
            {
                rw.Vec3(ref U08); // 3 floats
            }

            if (version == 8)
            {
                rw.Vec4(ref U09); // 4 floats
            }

            if (version >= 9)
            {
                rw.Vec3(ref U10); // 3 floats
            }

            if (version >= 7)
            {
                rw.Int32(ref U11);
            }
        }
    }
}
