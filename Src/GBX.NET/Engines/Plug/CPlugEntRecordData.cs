using GBX.NET.Managers;

namespace GBX.NET.Engines.Plug;

public partial class CPlugEntRecordData : IReadableWritable
{
    private TimeInt32? start;
    private TimeInt32? end;
    private EntRecordDesc[] entRecordDescs = [];
    private NoticeRecordDesc[] noticeRecordDescs = [];
    private List<EntRecordListElem> entList = [];
    private List<NoticeRecordListElem> bulkNoticeList = [];
    private List<CustomModulesDeltaList> customModulesDeltaLists = [];

    private ZlibData? compressedData;
    public ZlibData? CompressedData { get => compressedData; set => compressedData = value; }

#if NET9_0_OR_GREATER
    private readonly Lock CompressedDataLock = new();
#else
    private readonly object CompressedDataLock = new();
#endif

    /// <exception cref="ZLibNotDefinedException">Zlib is not defined.</exception>
    public TimeInt32 Start
    {
        get
        {
            // Record data is not compressed in version 4 and below
            if (start.HasValue || GetVersion() < 5) return start.GetValueOrDefault();
            if (CompressedData is null || CompressedData.Parsed) return start.GetValueOrDefault();

            lock (CompressedDataLock)
            {
                if (start.HasValue) return start.Value;
                ParseRecordData(); // sets start and CompressedData.Parsed to true
                return start.GetValueOrDefault();
            }
        }
        set
        {
            lock (CompressedDataLock)
            {
                start = value;
            }
        }
    }

    /// <exception cref="ZLibNotDefinedException">Zlib is not defined.</exception>
    public TimeInt32 End
    {
        get
        {
            // Record data is not compressed in version 4 and below
            if (end.HasValue || GetVersion() < 5) return end.GetValueOrDefault();
            if (CompressedData is null || CompressedData.Parsed) return end.GetValueOrDefault();

            lock (CompressedDataLock)
            {
                if (end.HasValue) return end.Value;
                ParseRecordData(); // sets end and CompressedData.Parsed to true
                return end.GetValueOrDefault();
            }
        }
        set
        {
            lock (CompressedDataLock)
            {
                end = value;
            }
        }
    }

    /// <exception cref="ZLibNotDefinedException">Zlib is not defined.</exception>
    public EntRecordDesc[] EntRecordDescs
    {
        get
        {
            // Record data is not compressed in version 4 and below
            if (entRecordDescs.Length > 0 || GetVersion() < 5) return entRecordDescs;
            if (CompressedData is null || CompressedData.Parsed) return entRecordDescs;

            lock (CompressedDataLock)
            {
                if (entRecordDescs.Length > 0) return entRecordDescs;
                ParseRecordData(); // sets entRecordDescs and CompressedData.Parsed to true
                return entRecordDescs;
            }
        }
        set
        {
            lock (CompressedDataLock)
            {
                entRecordDescs = value;
            }
        }
    }

    /// <exception cref="ZLibNotDefinedException">Zlib is not defined.</exception>
    public NoticeRecordDesc[] NoticeRecordDescs
    {
        get
        {
            // Record data is not compressed in version 4 and below
            if (noticeRecordDescs.Length > 0 || GetVersion() < 5) return noticeRecordDescs;
            if (CompressedData is null || CompressedData.Parsed) return noticeRecordDescs;

            lock (CompressedDataLock)
            {
                if (noticeRecordDescs.Length > 0) return noticeRecordDescs;
                ParseRecordData(); // sets noticeRecordDescs and CompressedData.Parsed to true
                return noticeRecordDescs;
            }
        }
        set
        {
            lock (CompressedDataLock)
            {
                noticeRecordDescs = value;
            }
        }
    }

    /// <exception cref="ZLibNotDefinedException">Zlib is not defined.</exception>
    public List<EntRecordListElem> EntList
    {
        get
        {
            if (entList.Count > 0 || GetVersion() < 5) return entList;
            if (CompressedData is null || CompressedData.Parsed) return entList;

            lock (CompressedDataLock)
            {
                if (entList.Count > 0) return entList;
                ParseRecordData(); // sets entList and CompressedData.Parsed to true
                return entList;
            }
        }
        set
        {
            lock (CompressedDataLock)
            {
                entList = value;
            }
        }
    }

    /// <exception cref="ZLibNotDefinedException">Zlib is not defined.</exception>
    public List<NoticeRecordListElem> BulkNoticeList
    {
        get
        {
            if (bulkNoticeList.Count > 0 || GetVersion() < 5) return bulkNoticeList;
            if (CompressedData is null || CompressedData.Parsed) return bulkNoticeList;

            lock (CompressedDataLock)
            {
                if (bulkNoticeList.Count > 0) return bulkNoticeList;
                ParseRecordData(); // sets bulkNoticeList and CompressedData.Parsed to true
                return bulkNoticeList;
            }
        }
        set
        {
            lock (CompressedDataLock)
            {
                bulkNoticeList = value;
            }
        }
    }

    /// <exception cref="ZLibNotDefinedException">Zlib is not defined.</exception>
    public List<CustomModulesDeltaList> CustomModulesDeltaLists
    {
        get
        {
            if (customModulesDeltaLists.Count > 0 || GetVersion() < 7) return customModulesDeltaLists;
            if (CompressedData is null || CompressedData.Parsed) return customModulesDeltaLists;

            lock (CompressedDataLock)
            {
                if (customModulesDeltaLists.Count > 0) return customModulesDeltaLists;
                ParseRecordData(); // sets customModulesDeltaLists and CompressedData.Parsed to true
                return customModulesDeltaLists;
            }
        }
        set
        {
            lock (CompressedDataLock)
            {
                customModulesDeltaLists = value;
            }
        }
    }

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
            WriteEntList(rw.Writer, v);

            if (v >= 3)
            {
                WriteBulkNoticeList(rw.Writer);
                WriteCustomModulesDeltaLists(rw.Writer, v);
            }
        }
    }

    private void ParseRecordData()
    {
        if (CompressedData is null) throw new InvalidOperationException("CompressedData not available");

        try
        {
            var version = GetVersion();

            using var r = CompressedData.OpenDecompressedReader();
            using var rw = new GbxReaderWriter(r);

            ReadWrite(rw, version);

            CompressedData.Parsed = true;
        }
        catch (Exception ex)
        {
            CompressedData.Exception = ex;
            throw;
        }
    }

    private int GetVersion()
    {
        return Chunks.Get<Chunk0911F000>()?.Version ?? throw new InvalidOperationException("Version not found (Chunk0911F000 chunk is missing)");
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

            rw.ZlibData(ref n.compressedData, n, lazyLoad: true, Version);
        }
    }

    private IEnumerable<EntRecordListElem> ReadEntList(GbxReader r, int version)
    {
        var hasNextElem = r.ReadBoolean(asByte: true);

        while (hasNextElem)
        {
            var type = r.ReadInt32();
            var sceneUId = r.ReadInt32();
            var beginTime = r.ReadTimeInt32();
            var endTime = r.ReadTimeInt32();

            var u04 = version >= 6 ? r.ReadInt32() : sceneUId;

            var desc = entRecordDescs[type];
            List<EntRecordDelta> samples;
            if (version < 11)
            {
                samples = ReadEntRecordDeltas(r, desc).ToList();
            }
            else
            {
                var buffers = ReadEncodedDeltas(r);
                samples = new(buffers.Length);

                var time = TimeInt32.Zero;
                for (var i = 0; i < buffers.Length; i++)
                {
                    var (deltaTime, data) = buffers[i];
                    time += deltaTime;
                    samples.Add(CreateEntRecordDelta(time, data, desc));
                }
            }

            hasNextElem = r.ReadBoolean(asByte: true);

            List<EntRecordDelta2> samples2 = version >= 2 ? ReadEntRecordDeltas2(r).ToList() : [];

            yield return new EntRecordListElem
            {
                Type = type,
                SceneUId = sceneUId,
                BeginTime = beginTime,
                EndTime = endTime,
                U04 = u04,
                Samples = samples,
                Samples2 = samples2
            };
        }
    }

    private static (TimeInt32 deltaTime, byte[] data)[] ReadEncodedDeltas(GbxReader r)
    {
        var numSamples = r.ReadInt32();

        if (numSamples == 0)
        {
            return [];
        }

        var sampleSize = r.ReadInt32();

        if (numSamples < 0 || sampleSize < 0)
        {
            throw new InvalidDataException($"Invalid header dimensions: {numSamples}x{sampleSize}");
        }

        var samples = new (TimeInt32 deltaTime, byte[] data)[numSamples];
        for (var i = 0; i < numSamples; i++)
        {
            samples[i] = (r.ReadTimeInt32(), new byte[sampleSize]);
        }

        // Read data (Columnar Delta Decoding)
        // The data is stored by row (one byte for every buffer), then column (next byte for every buffer)
        // It is also delta-encoded horizontally across the buffers
        for (var i = 0; i < sampleSize; i++)
        {
            // Read a slice: one byte for every buffer
            var slice = r.ReadBytes(numSamples);

            if (slice.Length != numSamples)
            {
                throw new EndOfStreamException("Unexpected end of reading data slices.");
            }

            byte accumulator = 0; // Resets for every new row (byte index)

            for (var b = 0; b < numSamples; b++)
            {
                // Apply delta: CurrentBufferVal = PreviousBufferVal + ReadVal
                accumulator += slice[b];

                samples[b].data[i] = accumulator;
            }
        }

        return samples;
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
            var time = r.ReadTimeInt32();
            var data = r.ReadData(); // MwBuffer
            yield return CreateEntRecordDelta(time, data, desc);
        }
    }

    private static EntRecordDelta CreateEntRecordDelta(TimeInt32 time, byte[] data, EntRecordDesc desc)
    {
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
            delta.Read(ms);
        }

        return delta;
    }

    private void WriteEntList(GbxWriter w, int version)
    {
        w.Write(entList.Count > 0, asByte: true);

        for (var i = 0; i < entList.Count; i++)
        {
            var elem = entList[i];

            w.Write(elem.Type);
            w.Write(elem.SceneUId);
            w.Write(elem.BeginTime);
            w.Write(elem.EndTime);

            if (version >= 6)
            {
                w.Write(elem.U04);
            }

            var desc = entRecordDescs[elem.Type];
            if (version < 11)
            {
                WriteEntRecordDeltas(w, elem.Samples);
            }
            else
            {
                WriteEncodedDeltas(w, elem.Samples);
            }

            var hasNext = i < entList.Count - 1;
            w.Write(hasNext, asByte: true);

            if (version >= 2)
            {
                WriteEntRecordDeltas2(w, elem.Samples2);
            }
        }
    }

    private static void WriteEncodedDeltas(GbxWriter w, List<EntRecordDelta> samples)
    {
        w.Write(samples.Count);

        if (samples.Count == 0)
        {
            return;
        }

        var sampleSize = samples.Count > 0 ? samples[0].Data.Length : 0;
        w.Write(sampleSize);

        var prevTime = TimeInt32.Zero;
        foreach (var sample in samples)
        {
            w.Write(sample.Time - prevTime);
            sample.Write();
            prevTime = sample.Time;
        }

        for (var i = 0; i < sampleSize; i++)
        {
            byte accumulator = 0;
            for (var b = 0; b < samples.Count; b++)
            {
                var value = samples[b].Data[i];
                var delta = (byte)(value - accumulator);
                w.Write(delta);
                accumulator = value;
            }
        }
    }

    private static void WriteEntRecordDeltas2(GbxWriter w, List<EntRecordDelta2> samples)
    {
        foreach (var sample in samples)
        {
            w.Write(true, asByte: true);
            w.Write(sample.Type);
            w.Write(sample.Time);
            w.WriteData(sample.Data);
        }
        w.Write(false, asByte: true);
    }

    private static void WriteEntRecordDeltas(GbxWriter w, List<EntRecordDelta> samples)
    {
        foreach (var sample in samples)
        {
            w.Write(true, asByte: true);
            w.Write(sample.Time);
            sample.Write();
            w.WriteData(sample.Data);
        }
        w.Write(false, asByte: true);
    }

    private static IEnumerable<NoticeRecordListElem> ReadBulkNoticeList(GbxReader r)
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

    private void WriteBulkNoticeList(GbxWriter w)
    {
        foreach (var elem in bulkNoticeList)
        {
            w.Write(true, asByte: true);
            w.Write(elem.Type);
            w.Write(elem.Time);
            w.WriteData(elem.Data);
        }
        w.Write(false, asByte: true);
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

        var classId = version >= 8 ? r.ReadUInt32() : default(uint?);
        var period = version >= 10 ? r.ReadInt32() : default(int?);

        return new CustomModulesDeltaList
        {
            Deltas = deltas,
            ClassId = classId,
            Period = period
        };
    }

    private void WriteCustomModulesDeltaLists(GbxWriter w, int version)
    {
        var deltaListCount = version >= 8 ? customModulesDeltaLists.Count : (customModulesDeltaLists.Count > 0 ? 1 : 0);

        if (version >= 8)
        {
            w.Write(deltaListCount);
        }

        if (deltaListCount == 0)
        {
            return;
        }

        if (version >= 7)
        {
            var count = version >= 8 ? deltaListCount : 1;
            for (var i = 0; i < count; i++)
            {
                WriteCustomModulesDeltaList(w, customModulesDeltaLists[i], version);
            }
        }
    }

    private static void WriteCustomModulesDeltaList(GbxWriter w, CustomModulesDeltaList deltaList, int version)
    {
        foreach (var delta in deltaList.Deltas)
        {
            w.Write(true, asByte: true);
            w.Write(delta.U01);
            w.WriteData(delta.Data);

            if (version >= 9)
            {
                w.WriteData(delta.U02);
            }
        }
        w.Write(false, asByte: true);

        if (version >= 8)
        {
            w.Write(deltaList.ClassId ?? uint.MaxValue);
        }

        if (version >= 10)
        {
            w.Write(deltaList.Period ?? 0);
        }
    }

    public partial class EntRecordDesc
    {
        [Obsolete("Use StateSize instead.")]
        public int U01 { get => StateSize; set => StateSize = value; }

        [Obsolete("Use SceneUIdOffset instead.")]
        public int U02 { get => SceneUIdOffset; set => SceneUIdOffset = value; }

        [Obsolete("Use StateArchiveVersion instead.")]
        public int U03 { get => StateArchiveVersion; set => StateArchiveVersion = value; }

        [Obsolete("Use InitialState instead.")]
        public byte[]? U04 { get => InitialState; set => InitialState = value; }

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
        [Obsolete("Use NoticeArchiveVersion instead.")]
        public int U01 { get => NoticeArchiveVersion; set => NoticeArchiveVersion = value; }

        [Obsolete("Use NoticeType instead.")]
        public int U02 { get => NoticeType; set => NoticeType = value; }

        public override string ToString()
        {
            if (classId is null)
            {
                return $"type {NoticeType}, version {NoticeArchiveVersion}";
            }

            var name = ClassManager.GetName(classId.Value);
            return string.IsNullOrEmpty(name)
                ? $"0x{classId:X8} type {NoticeType}, version {NoticeArchiveVersion}"
                : $"{name} (0x{classId:X8}) type {NoticeType}, version {NoticeArchiveVersion}";
        }
    }

    public sealed class EntRecordListElem
    {
        public int Type { get; set; }
        public int SceneUId { get; set; }
        public TimeInt32 BeginTime { get; set; }
        public TimeInt32 EndTime { get; set; }
        public int U04 { get; set; }

        [Obsolete("Use SceneUId instead.")]
        public int U01 { get => SceneUId; set => SceneUId = value; }

        [Obsolete("Use BeginTime instead.")]
        public int U02 { get => BeginTime.TotalMilliseconds; set => BeginTime = new(value); }

        [Obsolete("Use EndTime instead.")]
        public int U03 { get => EndTime.TotalMilliseconds; set => EndTime = new(value); }

        public List<EntRecordDelta> Samples { get; set; } = [];
        public List<EntRecordDelta2> Samples2 { get; set; } = [];
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

        internal virtual void Read(MemoryStream ms) { }
        internal virtual void Write() { }

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
        public int Type { get; set; }
        public TimeInt32 Time { get; set; }
        public byte[] Data { get; set; } = [];

        [Obsolete("Use Type instead.")]
        public int U01 { get => Type; set => Type = value; }

        [Obsolete("Use Time instead.")]
        public int U02 { get => Time.TotalMilliseconds; set => Time = new(value); }
    }

    public class CustomModulesDeltaList
    {
        public List<CustomModulesDelta> Deltas { get; set; } = [];
        public uint? ClassId { get; set; }
        public int? Period { get; set; }
    }

    public class CustomModulesDelta
    {
        public int U01 { get; set; }
        public byte[] Data { get; set; } = [];
        public byte[] U02 { get; set; } = [];
    }
}
