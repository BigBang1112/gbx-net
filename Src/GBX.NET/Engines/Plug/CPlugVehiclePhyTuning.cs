using System.Text;

namespace GBX.NET.Engines.Plug;

public partial class CPlugVehiclePhyTuning
{
    public partial class Chunk090EB000
    {
        public override void ReadWrite(CPlugVehiclePhyTuning n, GbxReaderWriter rw)
        {
            rw.Id(ref n.name);
            rw.NodeRef(ref U01Ref);
            rw.Single(ref U02);

            if (rw.Reader?.Settings.EncryptionInitializer is not null && n.name is { Length: >= 4 })
            {
                rw.Reader.Settings.EncryptionInitializer.Initialize(Encoding.ASCII.GetBytes(n.name.Substring(0, 4)), 0, 4);
            }
        }
    }

    public override string ToString()
    {
        return Name;
    }
}
