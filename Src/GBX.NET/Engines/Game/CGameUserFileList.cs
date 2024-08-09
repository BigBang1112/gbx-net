namespace GBX.NET.Engines.Game;

public partial class CGameUserFileList
{
    public partial class FileInfo
    {
        private string name = "";
        private string mapUid = "";
        private string? mapName;
        private string? ghostKind;
        private FileType type;

        public ulong U01;
        public ulong U02;
        public int? U04;

        public string Name { get => name; set => name = value; }
        public string MapUid { get => mapUid; set => mapUid = value; }
        public string? MapName { get => mapName; set => mapName = value; }
        public string? GhostKind { get => ghostKind; set => ghostKind = value; }
        public FileType Type { get => type; set => type = value; }

        public void ReadWrite(GbxReaderWriter rw)
        {
            rw.String(ref name!);
            rw.Byte(0);

            rw.UInt64(ref U01);
            rw.UInt64(ref U02);
            rw.EnumInt32<FileType>(ref type);

            switch (type)
            {
                case FileType.Map:
                    rw.Id(ref mapUid!);
                    rw.String(ref mapName);
                    rw.Byte(0);
                    break;
                case FileType.Ghost:
                    rw.String(ref ghostKind);
                    rw.Byte(0);
                    rw.Id(ref mapUid!);
                    rw.Int32(ref U04);
                    break;
                default:
                    throw new ThisShouldNotHappenException();
            }
        }

        public override string ToString()
        {
            return name ?? "[unknown file]";
        }
    }
}
