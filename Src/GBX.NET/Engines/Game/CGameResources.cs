namespace GBX.NET.Engines.Game;

public partial class CGameResources
{
    public partial class ResourceData
    {
        public byte[]? Data { get; set; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.String(ref name);
            var size = rw.Int32(Data is null ? 0 : checked(Data.Length + 4));

            if (size == 0)
            {
                Data = null;
                return;
            }

            if (size < 4 || size > 0x9FFFFF)
            {
                throw new InvalidDataException($"Invalid resource data size: {size}.");
            }

            Data = rw.Data(Data, size - 4);
        }
    }
}
