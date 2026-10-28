using GBX.NET.Engines.Control;

namespace GBX.NET.Engines.Game;

public partial class CGameControlGrid
{
    public CControlContainer.Child BaseLocalDataPointer { get; set; } = new();

    public partial class Chunk030B9005
    {
        public override void ReadWrite(CGameControlGrid n, GbxReaderWriter rw)
        {
            n.BaseLocalDataPointer.ReadWritePointer(rw);
            rw.Boolean(ref n.haveLocalData);
            rw.Boolean(ref n.hideLocalDataIfNone);
        }
    }
}
