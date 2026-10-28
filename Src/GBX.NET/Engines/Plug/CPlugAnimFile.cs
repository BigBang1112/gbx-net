namespace GBX.NET.Engines.Plug;

public partial class CPlugAnimFile
{
    internal bool HasLegacyRootMotion => Chunks.Get<Chunk090B0003>()?.Version < 7;

    public partial class LegacyEdition
    {
        private CPlugAnimSkelEdition edition = new();

        public CPlugAnimSkelEdition Edition { get => edition; set => edition = value; }

        public void Read(GbxReader r, CPlugAnimFile n, int v = 0)
        {
            using var rw = new GbxReaderWriter(r);
            ReadWrite(rw, n, v);
        }

        public void Write(GbxWriter w, CPlugAnimFile n, int v = 0)
        {
            using var rw = new GbxReaderWriter(w);
            ReadWrite(rw, n, v);
        }

        public void ReadWrite(GbxReaderWriter rw, CPlugAnimFile n, int v = 0)
        {
            rw.Int32(ref skelIndex);
            if (rw.Reader is not null)
            {
                edition = new CPlugAnimSkelEdition();
            }
            edition.FlagsVersion = n.ClipFlagsVersion;
            edition.ReadWrite(rw, v);
        }
    }
}
