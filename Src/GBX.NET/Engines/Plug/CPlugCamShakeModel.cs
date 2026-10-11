using GBX.NET.Serialization;

namespace GBX.NET.Engines.Plug;

public partial class CPlugCamShakeModel
{
    private CamShakeParams[] @params = [new(), new()];

    public partial class Chunk0910B000
    {
        public override void ReadWrite(CPlugCamShakeModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.ReadableWritable<CamShakeParams>(ref n.@params[0], Version);
            if (Version >= 3)
            {
                rw.ReadableWritable<CamShakeParams>(ref n.@params[1], Version);
            }
        }
    }
}
