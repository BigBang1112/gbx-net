namespace GBX.NET.Engines.Game;

public partial class CGameUserFileList
{
    public class FileInfo
    {
        private string? name;
        private string? mapUid;
        private string? mapName;
        private string? ghostKind;

        public ulong U01;
        public ulong U02;
        public int U03;
        public int? U04;

        public string? Name { get => name; set => name = value; }
        public string? MapUid { get => mapUid; set => mapUid = value; }
        public string? MapName { get => mapName; set => mapName = value; }
        public string? GhostKind { get => ghostKind; set => ghostKind = value; }

        public static void ReadWrite(GameBoxReaderWriter rw, FileInfo fileInfo)
        {
            fileInfo.ReadWrite(rw);
        }

        public void ReadWrite(GameBoxReaderWriter rw)
        {
            rw.String(ref name!);
            rw.Byte(0);

            rw.UInt64(ref U01);
            rw.UInt64(ref U02);
            rw.Int32(ref U03);

            switch (U03)
            {
                case 0:
                    rw.Id(ref mapUid!);
                    rw.String(ref mapName);
                    rw.Byte(0);
                    break;
                case 1:
                    rw.String(ref ghostKind);
                    rw.Byte(0);
                    rw.Id(ref mapUid!);
                    rw.Int32(ref U04);
                    break;
            }
        }

        public override string ToString()
        {
            return name ?? "[unknown file]";
        }
    }
}
