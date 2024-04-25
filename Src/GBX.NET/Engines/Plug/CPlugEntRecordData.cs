using GBX.NET.Managers;
using Microsoft.Extensions.Logging;

namespace GBX.NET.Engines.Plug;

public partial class CPlugEntRecordData : IReadableWritable
{
    private TimeInt32 start;
    private TimeInt32 end;
    private EntRecordDesc[] entRecordDescs = [];
    private NoticeRecordDesc[] noticeRecordDescs = [];
    private IList<EntRecordListElem> entList = [];
    private IList<NoticeRecordListElem> bulkNoticeList = [];
    private IList<CustomModulesDeltaList> customModulesDeltaLists = [];

    [ZLibData]
    public CompressedData CompressedData { get; set; }

    public TimeInt32 Start { get => start; }
    public TimeInt32 End { get => end; }
    public EntRecordDesc[] EntRecordDescs { get => entRecordDescs; set => entRecordDescs = value; }
    public NoticeRecordDesc[] NoticeRecordDescs { get => noticeRecordDescs; set => noticeRecordDescs = value; }
    public IList<EntRecordListElem> EntList { get => entList; }
    public IList<NoticeRecordListElem> BulkNoticeList { get => bulkNoticeList; }
    public IList<CustomModulesDeltaList> CustomModulesDeltaLists { get => customModulesDeltaLists; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        if (v >= 1)
        {
            rw.TimeInt32(ref start);
            rw.TimeInt32(ref end);
        }

        rw.ArrayReadableWritable<EntRecordDesc>(ref entRecordDescs);

        if (v >= 2)
        {
            rw.ArrayReadableWritable<NoticeRecordDesc>(ref noticeRecordDescs!, version: v);
        }

        if (rw.Reader is not null)
        {
            entList = ReadEntList(rw.Reader, v).ToList();

            if (v >= 3)
            {
                bulkNoticeList = ReadBulkNoticeList(rw.Reader).ToList();

                // custom modules
                customModulesDeltaLists = ReadCustomModulesDeltaLists(rw.Reader, v).ToList();
            }
        }

        if (rw.Writer is not null)
        {
            throw new NotSupportedException("Write is not supported");
        }
    }

    public partial class Chunk0911F000 : IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CPlugEntRecordData n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version < 5)
            {
                n.ReadWrite(rw, Version);
                return;
            }

            if (rw.Reader is GbxReader r)
            {
                var uncompressedSize = r.ReadInt32();
                var data = r.ReadData();
                n.CompressedData = new(uncompressedSize, data);

                if (Gbx.ZLib is null)
                {
                    r.Logger?.LogWarning("CPlugEntRecordData was not read due to missing ZLib.");
                }
                else
                {
                    try
                    {
                        using var uncompressedMs = n.CompressedData.OpenDecompressedMemoryStream();
                        using var rBuffer = new GbxReader(uncompressedMs);
                        using var rwBuffer = new GbxReaderWriter(rBuffer);

                        n.ReadWrite(rwBuffer, Version);
                    }
                    catch (Exception ex)
                    {
                        r.Logger?.LogError(ex, "Failed to read CPlugEntRecordData");
                    }
                }
            }

            if (rw.Writer is GbxWriter w)
            {
                w.Write(n.CompressedData.UncompressedSize);
                w.WriteData(n.CompressedData.Data);
            }
        }
    }

    private IEnumerable<EntRecordListElem> ReadEntList(GbxReader r, int version)
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

    private static IEnumerable<EntRecordDelta2> ReadEntRecordDeltas2(GbxReader r)
    {
        while (r.ReadBoolean(asByte: true))
        {
            yield return new()
            {
                Type = r.ReadInt32(),
                Time = r.ReadTimeInt32(),
                Data = r.ReadData()
            };
        }
    }

    private static IEnumerable<EntRecordDelta> ReadEntRecordDeltas(GbxReader r, EntRecordDesc desc)
    {
        // Reads byte on every loop until the byte is 0, should be 1 otherwise
        while (r.ReadBoolean(asByte: true))
        {
            yield return ReadEntRecordDelta(r, desc);
        }
    }

    private static EntRecordDelta ReadEntRecordDelta(GbxReader r, EntRecordDesc desc)
    {
        var time = r.ReadTimeInt32();
        var data = r.ReadData(); // MwBuffer

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
            using var rr = new GbxReader(ms);

            delta.Read(ms, rr);

            var sampleProgress = (int)ms.Position;
        }

        return delta;
    }
    private static IEnumerable<CustomModulesDeltaList> ReadCustomModulesDeltaLists(GbxReader r, int version)
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

    private static CustomModulesDeltaList ReadCustomModulesDeltaList(GbxReader r, int version)
    {
        var deltas = new List<CustomModulesDelta>();

        while (r.ReadBoolean(asByte: true))
        {
            var u01 = r.ReadInt32();
            var data = r.ReadData(); // MwBuffer
            var u02 = version >= 9 ? r.ReadData() : [];

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

    private static IEnumerable<NoticeRecordListElem> ReadBulkNoticeList(GbxReader r)
    {
        while (r.ReadBoolean(asByte: true))
        {
            yield return new()
            {
                U01 = r.ReadInt32(),
                U02 = r.ReadInt32(),
                Data = r.ReadData()
            };
        }
    }

    public partial class EntRecordDesc
    {
        public override string ToString()
        {
            var name = ClassManager.GetName(classId);
            return string.IsNullOrEmpty(name)
                ? $"0x{classId:X8}"
                : $"{name} (0x{classId:X8})";
        }
    }

    public partial class NoticeRecordDesc
    {
        public override string ToString()
        {
            if (classId is null)
            {
                return $"{U01}, {U02}";
            }

            var name = ClassManager.GetName(classId.Value);
            return string.IsNullOrEmpty(name)
                ? $"0x{classId:X8} {U01}, {U02}"
                : $"{name} (0x{classId:X8}) {U01}, {U02}";
        }
    }

    public sealed class EntRecordListElem
    {
        public int Type { get; set; }
        public int U01 { get; set; }
        public int U02 { get; set; }
        public int U03 { get; set; }
        public int U04 { get; set; }
        public IList<EntRecordDelta> Samples { get; set; } = [];
        public IList<EntRecordDelta2> Samples2 { get; set; } = [];
    }

    public class EntRecordDelta
    {
        public TimeInt32 Time { get; }
        public byte[] Data { get; }

        public EntRecordDelta(TimeInt32 time, byte[] data)
        {
            Time = time;
            Data = data;
        }

        public virtual void Read(MemoryStream ms, GbxReader r)
        {
            
        }

        public override string ToString()
        {
            return $"{Time}, {Data.Length} bytes";
        }
    }

    public class EntRecordDelta2
    {
        public TimeInt32 Time { get; set; }
        public int Type { get; set; }
        public byte[] Data { get; set; } = [];

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
