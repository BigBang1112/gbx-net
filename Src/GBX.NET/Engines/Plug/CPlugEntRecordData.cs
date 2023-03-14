using System.IO.Compression;

namespace GBX.NET.Engines.Plug;

/// <summary>
/// Entity data in a timeline.
/// </summary>
/// <remarks>ID: 0x0911F000</remarks>
[Node(0x0911F000)]
public class CPlugEntRecordData : CMwNod
{
    private int uncompressedSize;
    private byte[] data = Array.Empty<byte>();

    private TimeInt32 start;
    private TimeInt32 end;
    private EntRecordDesc[] entRecordDescs = Array.Empty<EntRecordDesc>();
    private NoticeRecordDesc[] noticeRecordDescs = Array.Empty<NoticeRecordDesc>();
    private IList<EntRecordListElem> entList = Array.Empty<EntRecordListElem>();
    private IList<NoticeRecordListElem> bulkNoticeList = Array.Empty<NoticeRecordListElem>();
    private IList<CustomModulesDeltaList> customModulesDeltaLists = Array.Empty<CustomModulesDeltaList>();

    [NodeMember]
    public byte[] Data { get => data; set => data = value; }

    [NodeMember]
    public TimeInt32 Start { get => start; }

    [NodeMember]
    public TimeInt32 End { get => end; }

    [NodeMember]
    public EntRecordDesc[] EntRecordDescs { get => entRecordDescs; }

    [NodeMember]
    public NoticeRecordDesc[] NoticeRecordDescs { get => noticeRecordDescs; }

    [NodeMember(ExactlyNamed = true)]
    public IList<EntRecordListElem> EntList { get => entList; }

    [NodeMember(ExactlyNamed = true)]
    public IList<NoticeRecordListElem> BulkNoticeList { get => bulkNoticeList; }

    [NodeMember]
    public IList<CustomModulesDeltaList> CustomModulesDeltaLists { get => customModulesDeltaLists; }

    internal CPlugEntRecordData()
    {

    }

    private void ReadWriteData(GameBoxReaderWriter rw, int version)
    {
        if (version >= 1)
        {
            rw.TimeInt32(ref start); // COULD BE WRONG
            rw.TimeInt32(ref end); // COULD BE WRONG
        }

        rw.ArrayArchive<EntRecordDesc>(ref entRecordDescs!);

        if (version >= 2)
        {
            rw.ArrayArchive<NoticeRecordDesc>(ref noticeRecordDescs!, version);
        }

        if (rw.Reader is not null)
        {
            entList = ReadEntList(rw.Reader, version).ToList();

            if (version >= 3)
            {
                bulkNoticeList = ReadBulkNoticeList(rw.Reader).ToList();

                // custom modules
                customModulesDeltaLists = ReadCustomModulesDeltaLists(rw.Reader, version).ToList();
            }
        }

        if (rw.Writer is not null)
        {
            throw new NotSupportedException("Write is not supported");
        }
    }

    private static IEnumerable<CustomModulesDeltaList> ReadCustomModulesDeltaLists(GameBoxReader r, int version)
    {
        var deltaListCount = version >= 8 ? r.ReadInt32() : 1;

        if (deltaListCount == 0)
        {
            yield break;
        }

        if (version >= 7)
        {
            for (var i = 0; i < deltaListCount; i++)
            {
                yield return ReadCustomModulesDeltaList(r, version);
            }
        }
    }

    private static CustomModulesDeltaList ReadCustomModulesDeltaList(GameBoxReader r, int version)
    {
        var deltas = new List<CustomModulesDelta>();

        while (r.ReadBoolean(asByte: true))
        {
            var u01 = r.ReadInt32();
            var data = r.ReadBytes(); // MwBuffer
            var u02 = version >= 9 ? r.ReadBytes() : Array.Empty<byte>();

            deltas.Add(new()
            {
                U01 = u01,
                Data = data,
                U02 = u02
            });
        }

        var period = version >= 10 ? r.ReadInt32() : default(int?);

        return new CustomModulesDeltaList
        {
            Deltas = deltas,
            Period = period
        };
    }

    private static IEnumerable<NoticeRecordListElem> ReadBulkNoticeList(GameBoxReader r)
    {
        while (r.ReadBoolean(asByte: true))
        {
            yield return new()
            {
                U01 = r.ReadInt32(),
                U02 = r.ReadInt32(),
                Data = r.ReadBytes()
            };
        }
    }

    private IEnumerable<EntRecordListElem> ReadEntList(GameBoxReader r, int version)
    {
        var hasNextElem = r.ReadBoolean(asByte: true);

        while (hasNextElem)
        {
            var type = r.ReadInt32();
            var u01 = r.ReadInt32();
            var u02 = r.ReadInt32(); // start?
            var u03 = r.ReadInt32(); // end? ghostLengthFinish ms

            var u04 = version >= 6 ? r.ReadInt32() : u01;

            var samples = ReadEntRecordDeltas(r, entRecordDescs[type]).ToList();

            hasNextElem = r.ReadBoolean(asByte: true);

            IList<EntRecordDelta2> samples2 = version >= 2 ? ReadEntRecordDeltas2(r).ToList() : Array.Empty<EntRecordDelta2>();

            yield return new EntRecordListElem
            {
                Type = type,
                U01 = u01,
                U02 = u02,
                U03 = u03,
                U04 = u04,
                Samples = samples,
                Samples2 = samples2
            };
        }
    }

    private static IEnumerable<EntRecordDelta2> ReadEntRecordDeltas2(GameBoxReader r)
    {
        while (r.ReadBoolean(asByte: true))
        {
            yield return new()
            {
                Type = r.ReadInt32(),
                Time = r.ReadTimeInt32(),
                Data = r.ReadBytes()
            };
        }
    }

    private static IEnumerable<EntRecordDelta> ReadEntRecordDeltas(GameBoxReader r, EntRecordDesc desc)
    {
        // Reads byte on every loop until the byte is 0, should be 1 otherwise
        while (r.ReadBoolean(asByte: true))
        {
            yield return ReadEntRecordDelta(r, desc);
        }
    }

    private static EntRecordDelta ReadEntRecordDelta(GameBoxReader r, EntRecordDesc desc)
    {
        var time = r.ReadTimeInt32();
        var data = r.ReadBytes(); // MwBuffer

        EntRecordDelta? delta = desc.ClassId switch
        {
            0x0A018000 => new CSceneVehicleVis.EntRecordDelta(time, data),
            _ => null
        };

        if (delta is null)
        {
            return new(time, data);
        }

        if (data.Length > 0)
        {
            using var ms = new MemoryStream(data);
            using var rr = new GameBoxReader(ms);

            delta.Read(ms, rr);

            var sampleProgress = (int)ms.Position;
        }

        return delta;
    }

    /// <summary>
    /// CPlugEntRecordData 0x000 chunk
    /// </summary>
    [Chunk(0x0911F000)]
    public class Chunk0911F000 : Chunk<CPlugEntRecordData>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CPlugEntRecordData n, GameBoxReader r)
        {
            Version = r.ReadInt32(); // 10

            if (Version >= 5)
            {
                n.uncompressedSize = r.ReadInt32();
                n.data = r.ReadBytes();
                
                using var ms = new MemoryStream(n.data);
                using var compressed = new CompressedStream(ms, CompressionMode.Decompress);
                using var rr = new GameBoxReader(compressed);
                var rw = new GameBoxReaderWriter(rr);

                n.ReadWriteData(rw, Version);
            }
            else
            {
                var rw = new GameBoxReaderWriter(r);
                n.ReadWriteData(rw, Version);
            }
        }

        public override void Write(CPlugEntRecordData n, GameBoxWriter w)
        {
            w.Write(Version);

            if (Version < 5)
            {
                throw new VersionNotSupportedException(Version);
            }

            w.Write(n.uncompressedSize);
            w.WriteByteArray(n.data);
        }
    }

    public class EntRecordDesc : IReadableWritable
    {
        private uint classId;
        private int u01;
        private int u02;
        private int u03;
        private byte[] u04 = Array.Empty<byte>();
        private int u05;

        public uint ClassId { get => classId; set => classId = value; }
        public int U01 { get => u01; set => u01 = value; }
        public int U02 { get => u02; set => u02 = value; }
        public int U03 { get => u03; set => u03 = value; }
        public byte[] U04 { get => u04; set => u04 = value; }
        public int U05 { get => u05; set => u05 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.UInt32(ref classId);
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);
            rw.Bytes(ref u04!);
            rw.Int32(ref u05);
        }

        public override string ToString()
        {
            return $"{NodeManager.GetName(classId)} (0x{classId:X8})";
        }
    }

    public class NoticeRecordDesc : IReadableWritable
    {
        private int u01;
        private int u02;
        private uint? classId;

        public int U01 { get => u01; set => u01 = value; }
        public int U02 { get => u02; set => u02 = value; }
        public uint? ClassId { get => classId; set => classId = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);

            if (version >= 4)
            {
                rw.UInt32(ref classId);
            }
        }

        public override string ToString()
        {
            return classId.HasValue
                ? $"{NodeManager.GetName(classId.Value)} (0x{classId.Value:X8}) {U01}, {U02}"
                : $"{U01}, {U02}";
        }
    }

    public class EntRecordListElem
    {
        public int Type { get; set; }
        public int U01 { get; set; }
        public int U02 { get; set; }
        public int U03 { get; set; }
        public int U04 { get; set; }
        public IList<EntRecordDelta> Samples { get; set; } = Array.Empty<EntRecordDelta>();
        public IList<EntRecordDelta2> Samples2 { get; set; } = Array.Empty<EntRecordDelta2>();
    }

    public class EntRecordDelta
    {
        public TimeInt32 Time { get; }
        public byte[] Data { get; }

        internal EntRecordDelta(TimeInt32 time, byte[] data)
        {
            Time = time;
            Data = data;
        }

        public override string ToString()
        {
            return $"{Time}, {Data.Length} bytes";
        }

        public virtual void Read(MemoryStream ms, GameBoxReader r)
        {
            
        }
    }

    public class EntRecordDelta2
    {
        public TimeInt32 Time { get; set; }
        public int Type { get; set; }
        public byte[] Data { get; set; } = Array.Empty<byte>();

        public override string ToString()
        {
            return $"{Time}, type {Type}, {Data.Length} bytes";
        }
    }

    public class NoticeRecordListElem
    {
        public int U01 { get; set; }
        public int U02 { get; set; }
        public byte[] Data { get; set; } = Array.Empty<byte>();
    }

    public class CustomModulesDeltaList
    {
        public IList<CustomModulesDelta> Deltas { get; set; } = Array.Empty<CustomModulesDelta>();
        public int? Period { get; set; }
    }

    public class CustomModulesDelta
    {
        public int U01 { get; set; }
        public byte[] Data { get; set; } = Array.Empty<byte>();
        public byte[] U02 { get; set; } = Array.Empty<byte>();
    }
}
