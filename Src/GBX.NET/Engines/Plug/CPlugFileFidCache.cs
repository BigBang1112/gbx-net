using System.Numerics;

namespace GBX.NET.Engines.Plug;

[Node(0x09049000)]
public class CPlugFileFidCache : CPlugFileFidContainer
{
    private int version;
    private uint? crc32;
    private string rootEnumFullName = "";
    private FolderDesc[] folderDescs = Array.Empty<FolderDesc>();
    private FileDesc[] fileDescs = Array.Empty<FileDesc>();
    private DateTime timeWrite;
    private byte[] headerData = Array.Empty<byte>();

    [NodeMember(ExactlyNamed = true)]
    public int Version { get => version; set => version = value; }

    [NodeMember]
    public uint? Crc32 { get => crc32; set => crc32 = value; }

    [NodeMember(ExactlyNamed = true)]
    public string RootEnumFullName { get => rootEnumFullName; set => rootEnumFullName = value; }

    [NodeMember(ExactlyNamed = true)]
    public FolderDesc[] FolderDescs { get => folderDescs; set => folderDescs = value; }

    [NodeMember(ExactlyNamed = true)]
    public FileDesc[] FileDescs { get => fileDescs; set => fileDescs = value; }

    [NodeMember]
    public DateTime TimeWrite { get => timeWrite; set => timeWrite = value; }

    [NodeMember]
    public byte[] HeaderData { get => headerData; set => headerData = value; }

    internal CPlugFileFidCache()
    {
        
    }

    private void ReadWrite(GameBoxReaderWriter rw)
    {
        rw.Int32(ref version);

        if (version >= 4)
        {
            rw.UInt32(ref crc32);
        }

        rw.String(ref rootEnumFullName!);
        rw.ArrayArchive<FolderDesc>(ref folderDescs!);
        rw.ArrayArchive<FileDesc>(ref fileDescs!, version);

        if (version >= 2)
        {
            rw.FileTime(ref timeWrite);
            
            for (var i = 0; i < folderDescs.Length; i++)
            {
                folderDescs[i].U01 = rw.UInt64(folderDescs[i].U01);
            }
        }

        rw.Bytes(ref headerData!);
    }

    public override string ToString()
    {
        return rootEnumFullName;
    }

    protected override void ReadChunkData(GameBoxReader r, IProgress<GameBoxReadProgress>? progress, bool ignoreZeroIdChunk)
    {
        ReadWrite(new GameBoxReaderWriter(r));
    }

    protected override Task ReadChunkDataAsync(GameBoxReader r, CancellationToken cancellationToken)
    {
        ReadWrite(new GameBoxReaderWriter(r));
        return Task.CompletedTask;
    }

    protected override void WriteChunkData(GameBoxWriter w)
    {
        ReadWrite(new GameBoxReaderWriter(w));
    }

    protected override Task WriteChunkDataAsync(GameBoxWriter w, CancellationToken cancellationToken)
    {
        ReadWrite(new GameBoxReaderWriter(w));
        return Task.CompletedTask;
    }

    public class FolderDesc : IReadableWritable
    {
        private int parentIndex;
        private string name = "";
        private ulong u01;

        public int ParentIndex { get => parentIndex; set => parentIndex = value; }
        public string Name { get => name; set => name = value; }
        public ulong U01 { get => u01; set => u01 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref parentIndex);
            rw.String(ref name!);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class FileDesc : IReadableWritable
    {
        private int u01;
        private string name = "";
        private int u03;
        private int u04;
        private int u05;
        private int u06;
        private uint classId;
        private int? u07;
        private BigInteger? u08;
        private uint flags;

        public int U01 { get => u01; set => u01 = value; }
        public string Name { get => name; set => name = value; }
        public int U03 { get => u03; set => u03 = value; }
        public int U04 { get => u04; set => u04 = value; }
        public int U05 { get => u05; set => u05 = value; }
        public int U06 { get => u06; set => u06 = value; }
        public uint ClassId { get => classId; set => classId = value; }
        public int? U07 { get => u07; set => u07 = value; }
        public BigInteger? U08 { get => u08; set => u08 = value; }
        public uint Flags { get => flags; set => flags = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref u01);
            rw.String(ref name!);
            rw.Int32(ref u03);
            rw.Int32(ref u04);
            rw.Int32(ref u05);
            rw.Int32(ref u06);
            rw.UInt32(ref classId);
            
            if (version >= 2)
            {
                rw.Int32(ref u07);
            }
            
            if (version >= 1)
            {
                //rw.Int128(ref u08);
            }

            rw.UInt32(ref flags);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
