using System.Text;

namespace GBX.NET.Engines.Plug;

public partial class CPlugVehiclePhyTuning
{
    public partial class Chunk090EB000
    {
        public override void ReadWrite(CPlugVehiclePhyTuning n, GbxReaderWriter rw)
        {
            rw.Id(ref n.name);
            rw.NodeRef<CFuncKeysReal>(ref U01);
            rw.Single(ref U02);

            if (rw.Reader?.Settings.EncryptionInitializer is not null)
            {
                if (n.name is null || n.name.Length < 4)
                {
                    throw new InvalidOperationException("Name length must be at least 4 characters.");
                }

                rw.Reader.Settings.EncryptionInitializer.Initialize(Encoding.ASCII.GetBytes(n.name.Substring(0, 4)), 0, 4);
            }
        }
    }
}
