using GBX.NET.Engines.Control;

namespace GBX.NET.Engines.Scene;

public partial class CSceneMobil
{
    public partial class Chunk0A011005
    {
        public override void ReadWrite(CSceneMobil n, GbxReaderWriter rw)
        {
            if (n is CControlBase control)
            {
                control.HasHmsItem = rw.Boolean(control.HasHmsItem);
                if (!control.HasHmsItem)
                {
                    return;
                }
            }

            rw.Node(ref n.item);
        }
    }
}
