using System.Text;

namespace GBX.NET.Engines.Plug;

public partial class CPlugCharPhyModel
{
    public partial class Chunk090C9000
    {
        public override void ReadWrite(CPlugCharPhyModel n, GbxReaderWriter rw)
        {
            rw.Id(ref n.name);

            if (rw.Reader?.Settings.EncryptionInitializer is { } initializer && n.name is not null)
            {
                var nameBytes = Encoding.UTF8.GetBytes(n.name);
                if (nameBytes.Length >= 4)
                {
                    initializer.Initialize(nameBytes, 0, 4);
                }
            }
        }
    }
}
