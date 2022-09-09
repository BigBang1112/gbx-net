namespace GBX.NET.Engines.Hms;

public partial class CHmsLightMapCache
{
    public class SMap
    {
        private TimeSpan? dayTime;
        private uint? u01;
        private Vec4 u02;
        private bool u03;
        private short[]? u04;
        private bool u05;
        private int u06;
        private int u07;
        private Vec3 u08;
        private Vec4 u09;
        private Vec3 u10;
        private int u11;

        public TimeSpan? DayTime { get => dayTime; set => dayTime = value; }

        public uint? U01 { get => u01; set => u01 = value; }
        public Vec4 U02 { get => u02; set => u02 = value; }
        public bool U03 { get => u03; set => u03 = value; }
        public short[]? U04 { get => u04; set => u04 = value; }
        public bool U05 { get => u05; set => u05 = value; }
        public int U06 { get => u06; set => u06 = value; }
        public int U07 { get => u07; set => u07 = value; }
        public Vec3 U08 { get => u08; set => u08 = value; }
        public Vec4 U09 { get => u09; set => u09 = value; }
        public Vec3 U10 { get => u10; set => u10 = value; }
        public int U11 { get => u11; set => u11 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version)
        {
            rw.Int32(0);
            rw.TimeOfDay(ref dayTime);

            if (version >= 10)
            {
                rw.UInt32(ref u01);
            }

            rw.Vec4(ref u02); // 4 floats
            rw.Boolean(ref u03);

            // GmVec3_ArchiveAsReal16
            rw.Array<short>(ref u04, count: 3);

            if (version >= 4)
            {
                rw.Boolean(ref u05);
                rw.Int32(ref u06);
            }
            else
            {
                u05 = true;
                u06 = 1;
            }

            if (version >= 13)
            {
                rw.Int32(ref u07);
            }
            else
            {
                U07 = 2;
            }

            if (version < 8)
            {
                rw.Vec3(ref u08); // 3 floats
            }

            if (version == 8)
            {
                rw.Vec4(ref u09); // 4 floats
            }

            if (version >= 9)
            {
                rw.Vec3(ref u10); // 3 floats
            }

            if (version >= 7)
            {
                rw.Int32(ref u11);
            }
        }
    }
}
