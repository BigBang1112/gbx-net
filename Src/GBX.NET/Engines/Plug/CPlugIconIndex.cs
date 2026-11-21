namespace GBX.NET.Engines.Plug;

public partial class CPlugIconIndex
{
    public partial class Chunk09093000
    {
        private GBX.NET.Id? storedId;

        public override void ReadWrite(CPlugIconIndex n, GbxReaderWriter rw)
        {
            var id = rw.Id(GBX.NET.Id.FromStoredString(U01, storedId) ?? default);
            if (rw.Reader is not null)
            {
                storedId = id;
                U01 = GBX.NET.Id.ToStoredString(id);
            }

            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Int32(ref U07);
        }
    }

    public partial class Chunk09093003
    {
        private const int IdCount = 6;
        private GBX.NET.Id[]? storedIds;

        public override void ReadWrite(CPlugIconIndex n, GbxReaderWriter rw)
        {
            base.ReadWrite(n, rw);

            var reading = rw.Reader is not null;
            if (reading)
            {
                // Keep the original encoding available for each editable string in the fixed array.
                storedIds = new GBX.NET.Id[IdCount];
                U01 = new string[IdCount];
            }

            for (var i = 0; i < IdCount; i++)
            {
                var value = U01 is { } names && i < names.Length ? names[i] : null;
                var id = rw.Id(GBX.NET.Id.FromStoredString(value, storedIds?[i]) ?? default);
                if (reading)
                {
                    storedIds![i] = id;
                    U01![i] = GBX.NET.Id.ToStoredString(id)!;
                }
            }
        }
    }
}
