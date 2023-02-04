namespace GBX.NET.Engines.Plug;

public partial class CPlugSolid2Model
{
    public class ShadedGeom : IReadableWritable // ShadedGeom %2d : iVisual=%2d, iMaterial=%2d, LodMask=%2d, DefaultVisible=%2d \r\
    {
        private int visualIndex;
        private int materialIndex;
        private int? lod;
        private int? u05;

        internal int VisualIndex { get => visualIndex; set => visualIndex = value; }
        internal int MaterialIndex { get => materialIndex; set => materialIndex = value; }

        public int? Lod { get => lod; set => lod = value; } // LodMask?
        public int? U05 { get => u05; set => u05 = value; }

        public CPlugVisual? Visual { get; internal set; }
        public ExternalNode<CPlugMaterial>? Material { get; internal set; }
        public CPlugMaterialUserInst? MaterialInst { get; internal set; }
        public Material? CustomMaterial { get; internal set; }
        public string? MaterialId { get; internal set; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref visualIndex); // Can be either index to CPlugSolid2Mode.Visuals, or to ref table (not implemented support)
            rw.Int32(ref materialIndex);

            if (rw.Int32(-1) != -1) // Location, always -1
            {
                throw new Exception("Location is not -1");
            }

            if (version >= 1)
            {
                rw.Int32(ref lod, defaultValue: -1);

                if (version >= 32)
                {
                    rw.Int32(ref u05);
                }
            }
        }
    }
}
