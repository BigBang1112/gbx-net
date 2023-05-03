namespace GBX.NET.Engines.Plug;

public partial class CPlugSolid2Model
{
    public class LightInst : IReadableWritable
    {
        private int modelIndex;
        private int socketIndex;
        
        public int ModelIndex { get => modelIndex; set => modelIndex = value; }
        public int SocketIndex { get => socketIndex; set => socketIndex = value; }
        
        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref modelIndex);
            rw.Int32(ref socketIndex);
        }
    }
}
