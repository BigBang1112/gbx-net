namespace GBX.NET.Engines.Game;

public partial class CGameUserFileList
{
    public partial class FileInfo
    {

        public override string ToString()
        {
            return Name ?? "[unknown file]";
        }
    }
}
