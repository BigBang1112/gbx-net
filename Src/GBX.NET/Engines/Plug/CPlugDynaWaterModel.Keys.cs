namespace GBX.NET.Engines.Plug;

public partial class CPlugDynaWaterModel
{
    public partial class Keys : IVersionable
    {
        private int count;
        public int Count { get => count; set => count = value; }

        public void ReadWrite(GbxReaderWriter rw, int version = 0)
        {
            if (version <= 0)
            {
                rw.NodeRef<CPlugCurveSimpleNod>(ref curve, ref curveFile);
                if (rw.Reader is not null)
                {
                    var legacy = curve?.Chunks.Get<CPlugCurveSimpleNod.Chunk09185000>();
                    Flags = curve?.Version2 switch
                    {
                        1 => (ushort)(legacy?.U01 > 0 ? 1 : 0),
                        >= 3 => unchecked((ushort)(legacy?.U03 ?? 0)),
                        _ => 0
                    };
                    Values = legacy?.U04 is null ? [] : Array.ConvertAll(legacy.U04, value => new Vec2(
                        BitConverter.ToSingle(BitConverter.GetBytes((uint)value), 0),
                        BitConverter.ToSingle(BitConverter.GetBytes((uint)(value >> 32)), 0)));
                    count = Values.Length;
                }
                return;
            }

            rw.VersionInt32(this);
            count = rw.Int32(values?.Length ?? 0);
            if (Version == 1)
            {
                rw.Byte(ref flagsV1);
                if (rw.Reader is not null)
                {
                    Flags = (ushort)(flagsV1 > 0 ? 1 : 0);
                }
            }
            if (Version == 2)
            {
                rw.Int32(ref capacity);
            }
            if (Version >= 3)
            {
                rw.UInt16(ref flags);
            }
            rw.Array<Vec2>(ref values, count);
        }
    }
}
