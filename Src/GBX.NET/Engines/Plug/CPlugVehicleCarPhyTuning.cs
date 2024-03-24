namespace GBX.NET.Engines.Plug;

public partial class CPlugVehicleCarPhyTuning
{
    public partial class Keys
    {
        private CFuncKeysReal? u01;
        public CFuncKeysReal? U01 { get => u01; set => u01 = value; }

        private int u02;
        /// <summary>
        /// version
        /// </summary>
        public int U02 { get => u02; set => u02 = value; }

        private int count;
        public int Count { get => count; set => count = value; }

        private short? u03;
        public short? U03 { get => u03; set => u03 = value; }

        private Vec2[]? u04;
        public Vec2[]? U04 { get => u04; set => u04 = value; }

        private int? u05;
        public int? U05 { get => u05; set => u05 = value; }

        private byte? u06;
        public byte? U06 { get => u06; set => u06 = value; }

        public void ReadWrite(GbxReaderWriter rw, int version = 0)
        {
            if (version >= 0)
            {
                rw.Int32(ref u02); // version
                var count = rw.Int32(u04?.Length ?? 0);
                switch (u02)
                {
                    case 1:
                        rw.Byte(ref u06);
                        break;
                    case 2:
                        rw.Int32(ref u05);
                        break;
                    case 3:
                        rw.Int16(ref u03);
                        break;
                }
                rw.Array<Vec2>(ref u04, count);
            }
            else
            {
                rw.NodeRef<CFuncKeysReal>(ref u01);
            }
        }
    }
}
