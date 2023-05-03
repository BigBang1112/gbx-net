namespace GBX.NET.Engines.Plug;

public partial class CPlugSolid2Model
{
    public class Material : IReadableWritable
    {
        private string materialName = "";
        private CPlugMaterialUserInst? materialUserInst;

        public string MaterialName { get => materialName; set => materialName = value; }
        public CPlugMaterialUserInst? MaterialUserInst { get => materialUserInst; set => materialUserInst = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.String(ref materialName!);

            if (materialName.Length == 0)
            {
                rw.NodeRef<CPlugMaterialUserInst>(ref materialUserInst);
            }
        }
    }
}
