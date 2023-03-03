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
            0x0A018000 => new CSceneVehicleVisEntRecordDelta(),
            _ => null
        };

        if (delta is null)
        {
            return new()
            {
                Time = time,
                Data = data
            };
        }

        delta.Time = time;
        delta.Data = data;

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

            n.uncompressedSize = r.ReadInt32();
            n.data = r.ReadBytes();

            using var ms = new MemoryStream(n.Data);
            using var cs = new CompressedStream(ms, CompressionMode.Decompress);
            using var rData = new GameBoxReader(cs);

            var rw = new GameBoxReaderWriter(rData);

            n.ReadWriteData(rw, Version);
        }

        public override void Write(CPlugEntRecordData n, GameBoxWriter w)
        {
            w.Write(Version);
            w.Write(n.uncompressedSize);
            w.WriteByteArray(n.Data);
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
        public TimeInt32 Time { get; set; }
        public byte[] Data { get; set; } = Array.Empty<byte>();

        public override string ToString()
        {
            return $"{Time}, {Data.Length} bytes";
        }

        public virtual void Read(MemoryStream ms, GameBoxReader r)
        {
            
        }
    }

    public class CSceneVehicleVisEntRecordDelta : EntRecordDelta
    {
        public Vec3 Position { get; set; }
        public Quat Rotation { get; set; }
        public Vec3 PitchYawRoll => Rotation.ToPitchYawRoll();
        public float Speed { get; set; }
        public Vec3 Velocity { get; set; }
        public float? Gear { get; set; }
        public byte? RPM { get; set; }
        public float? Steer { get; set; }
        public float? Brake { get; set; }
        public float? Gas { get; set; }

        public float FLIcing { get; set; }
        public float FRIcing { get; set; }
        public float RLIcing { get; set; }
        public float RRIcing { get; set; }

        public float FLDirt { get; set; }
        public float FRDirt { get; set; }
        public float RLDirt { get; set; }
        public float RRDirt { get; set; }

        public CPlugSurface.MaterialId FLGroundContactMaterial { get; set; }
        public CPlugSurface.MaterialId FRGroundContactMaterial { get; set; }
        public CPlugSurface.MaterialId RLGroundContactMaterial { get; set; }
        public CPlugSurface.MaterialId RRGroundContactMaterial { get; set; }

        public float FLDampenLen { get; set; }
        public float FRDampenLen { get; set; }
        public float RLDampenLen { get; set; }
        public float RRDampenLen { get; set; }

        public bool FLSlipCoef { get; set; }
        public bool FRSlipCoef { get; set; }
        public bool RLSlipCoef { get; set; }
        public bool RRSlipCoef { get; set; }

        public float FLWheelRot { get; set; }
        public float FRWheelRot { get; set; }
        public float RLWheelRot { get; set; }
        public float RRWheelRot { get; set; }

        public float WetnessValue { get; set; }
        public bool IsGroundContact { get; set; }
        public bool IsReactorGroundMode { get; set; }

        public CSceneVehicleVis.ReactorBoostLvl ReactorBoostLvl { get; set; }
        public CSceneVehicleVis.ReactorBoostType ReactorBoostType { get; set; }

        public int ReactorAirControlSteer { get; set; }
        public int ReactorAirControlPedal { get; set; }

        public bool IsTurbo { get; set; }
        public float TurboTime { get; set; }

        public float SimulationTimeCoef { get; set; }

        public float SideSpeed { get; set; }
        public bool IsTopContact { get; set; }

        public override void Read(MemoryStream ms, GameBoxReader r)
        {
            ms.Position = 5;
            var rpmByte = r.ReadByte();

            ms.Position = 14;
            var steerByte = r.ReadByte();
            var steer = ((steerByte / 255f) - 0.5f) * 2;

            ms.Position = 91;
            var gearByte = r.ReadByte();
            var gear = gearByte / 5f;

            Gear = gear;
            RPM = rpmByte;
            Steer = steer;

            ms.Position = 15;
            var u15 = r.ReadByte();

            ms.Position = 18;
            var brakeByte = r.ReadByte();
            var brake = brakeByte / 255f;
            var gas = u15 / 255f + brake;

            Brake = brake;
            Gas = gas;

            ms.Position = 47;

            var (position, rotation, speed, velocity) = r.ReadTransform();

            Position = position;
            Rotation = rotation;
            Speed = speed * 3.6f;
            Velocity = velocity;

            // ICE
            ms.Position = 81;
            var FLIceByte = r.ReadByte();
            ms.Position = 82;
            var FRIceByte = r.ReadByte();
            ms.Position = 83;
            var RRIceByte = r.ReadByte();
            ms.Position = 84;
            var RLIceByte = r.ReadByte();

            FLIcing = FLIceByte / 255f;
            FRIcing = FRIceByte / 255f;
            RRIcing = RRIceByte / 255f;
            RLIcing = RLIceByte / 255f;

            // DIRT
            ms.Position = 93;
            var FLDirtByte = r.ReadByte();
            ms.Position = 95;
            var FRDirtByte = r.ReadByte();
            ms.Position = 97;
            var RRDirtByte = r.ReadByte();
            ms.Position = 99;
            var RLDirtByte = r.ReadByte();

            FLDirt = FLDirtByte / 255f;
            FRDirt = FRDirtByte / 255f;
            RRDirt = RRDirtByte / 255f;
            RLDirt = RLDirtByte / 255f;

            // GroundContactMaterial
            ms.Position = 24;
            var FLGroundContactMaterialByte = r.ReadByte();
            ms.Position = 26;
            var FRGroundContactMaterialByte = r.ReadByte();
            ms.Position = 28;
            var RRGroundContactMaterialByte = r.ReadByte();
            ms.Position = 30;
            var RLGroundContactMaterialByte = r.ReadByte();

            FLGroundContactMaterial = (CPlugSurface.MaterialId)FLGroundContactMaterialByte;
            FRGroundContactMaterial = (CPlugSurface.MaterialId)FRGroundContactMaterialByte;
            RRGroundContactMaterial = (CPlugSurface.MaterialId)RRGroundContactMaterialByte;
            RLGroundContactMaterial = (CPlugSurface.MaterialId)RLGroundContactMaterialByte;

            // DampenLen
            ms.Position = 23;
            var FLDampenLenByte = r.ReadByte();
            ms.Position = 25;
            var FRDampenLenByte = r.ReadByte();
            ms.Position = 27;
            var RRDampenLenByte = r.ReadByte();
            ms.Position = 29;
            var RLDampenLenByte = r.ReadByte();

            // Multiply by 4 instead of 2 as it matches value given by openplanet CSceneVehicleVisState
            FLDampenLen = ((FLDampenLenByte / 255f) - 0.5f) * 4;
            FRDampenLen = ((FRDampenLenByte / 255f) - 0.5f) * 4;
            RRDampenLen = ((RRDampenLenByte / 255f) - 0.5f) * 4;
            RLDampenLen = ((RLDampenLenByte / 255f) - 0.5f) * 4;

            // SlipCoef
            ms.Position = 32;
            var SlipCoefByte1 = r.ReadByte();
            ms.Position = 33;
            var SlipCoefByte2 = r.ReadByte();

            byte maskFR = 0x1;  // 00000001
            byte maskRR = 0x4;  // 00000100
            byte maskRL = 0x10; // 00010000
            byte maskFL = 0x40; // 01000000

            // Nadeo uses two bytes for some reason
            // FLSlip is unique in that it is located at the 7th bit of SlipCoefByte1.
            // The 7th bit of SlipCoefByte2 is used too, however it appears to not be correlated with anything.
            FLSlipCoef = (SlipCoefByte1 & maskFL) != 0;
            FRSlipCoef = (SlipCoefByte2 & maskFR) != 0;
            RRSlipCoef = (SlipCoefByte2 & maskRR) != 0;
            RLSlipCoef = (SlipCoefByte2 & maskRL) != 0;

            // WheelRotation
            ms.Position = 6;
            var FLWheelRotationByte = r.ReadByte();
            ms.Position = 7;
            var FLWheelRotationCountByte = r.ReadByte();
            ms.Position = 8;
            var FRWheelRotationByte = r.ReadByte();
            ms.Position = 9;
            var FRWheelRotationCountByte = r.ReadByte();
            ms.Position = 10;
            var RRWheelRotationByte = r.ReadByte();
            ms.Position = 11;
            var RRWheelRotationCountByte = r.ReadByte();
            ms.Position = 12;
            var RLWheelRotationByte = r.ReadByte();
            ms.Position = 13;
            var RLWheelRotationCountByte = r.ReadByte();

            var tau = (float)Math.PI * 2;
            FLWheelRot = (FLWheelRotationByte / 255f * tau) + (FLWheelRotationCountByte * tau);
            FRWheelRot = (FRWheelRotationByte / 255f * tau) + (FRWheelRotationCountByte * tau);
            RRWheelRot = (RRWheelRotationByte / 255f * tau) + (RRWheelRotationCountByte * tau);
            RLWheelRot = (RLWheelRotationByte / 255f * tau) + (RLWheelRotationCountByte * tau);

            // Water
            ms.Position = 101;
            var waterByte = r.ReadByte();

            WetnessValue = waterByte / 255f;

            // IsGroundContact, IsReactorGroundMode, ReactorState
            ms.Position = 89;
            var groundModeByte = r.ReadByte();

            var maskIsGroundMode = 0x1;         // 00000001
            var maskIsReactorGroundMode = 0x4;  // 00000010
            var maskIsReactorUp = 0x8;          // 00001000
            var maskIsReactorDown = 0x10;       // 00010000
            var maskReactorLvl1 = 0x20;         // 00100000
            var maskReactorLvl2 = 0x40;         // 01000000 
                                                //var maskSlowMo = 0x80;              // 10000000 // Can use SimulationTimeCoef instead

            IsGroundContact = (groundModeByte & maskIsGroundMode) != 0;
            IsReactorGroundMode = (groundModeByte & maskIsReactorGroundMode) != 0;

            var isReactorUp = (groundModeByte & maskIsReactorUp) != 0;
            var isReactorDown = (groundModeByte & maskIsReactorDown) != 0;

            var isReactorLvl1 = (groundModeByte & maskReactorLvl1) != 0;
            var isReactorLvl2 = (groundModeByte & maskReactorLvl2) != 0;

            //var isSlowMo = (groundModeByte & maskSlowMo) != 0;

            if (isReactorLvl1)
                ReactorBoostLvl = CSceneVehicleVis.ReactorBoostLvl.Lvl1;
            else if (isReactorLvl2)
                ReactorBoostLvl = CSceneVehicleVis.ReactorBoostLvl.Lvl2;
            else
                ReactorBoostLvl = CSceneVehicleVis.ReactorBoostLvl.None;

            if (isReactorUp && isReactorDown)
                ReactorBoostType = CSceneVehicleVis.ReactorBoostType.UpAndDown;
            else if (isReactorUp)
                ReactorBoostType = CSceneVehicleVis.ReactorBoostType.Up;
            else if (isReactorDown)
                ReactorBoostType = CSceneVehicleVis.ReactorBoostType.Down;
            else
                ReactorBoostType = CSceneVehicleVis.ReactorBoostType.None;

            // Turbo
            ms.Position = 31;
            var isTurboByte = r.ReadByte();
            ms.Position = 21;
            var turboTimeByte = r.ReadByte();

            var maskIsTurbo = 0x82; // 10000010

            IsTurbo = (isTurboByte & maskIsTurbo) != 0;
            TurboTime = turboTimeByte / 255f;

            // SimulationTimeCoef (SlowMo)
            ms.Position = 102;
            var SimulationTimeCoefByte = r.ReadByte();

            SimulationTimeCoef = SimulationTimeCoefByte / 255f;

            // ReactorAirControl
            // [1,0,-1] = (Accell,None,Brake), (Left,None,Right)
            ms.Position = 90;
            var boosterAirControlByte = r.ReadByte();

            var maskPedalNone = 0x10;   // 00010000
            var maskPedalAccel = 0x20;  // 00100000
            var maskSteerNone = 0x40;   // 01000000
            var maskSteerLeft = 0x80;   // 10000000

            var isAirControlPedalAccel = (boosterAirControlByte & maskPedalAccel) != 0;
            var isAirControlPedalNone = (boosterAirControlByte & maskPedalNone) != 0;

            ReactorAirControlPedal = isAirControlPedalAccel ? 1 : isAirControlPedalNone ? 0 : -1;

            var isAirControlSteerLeft = (boosterAirControlByte & maskSteerLeft) != 0;
            var isAirControlSteerNone = (boosterAirControlByte & maskSteerNone) != 0;

            ReactorAirControlSteer = isAirControlSteerLeft ? 1 : isAirControlSteerNone ? 0 : -1;

            // IsTopContact
            ms.Position = 76;
            var vechicleStateByte = r.ReadByte();

            var maskIsTopContact = 0x20;
            //var maskIsReactor = 0x10; 

            IsTopContact = (vechicleStateByte & maskIsTopContact) != 0;

            // SideSpeed
            ms.Position = 2;
            float sideSpeedInt = r.ReadUInt16();
            SideSpeed = (float)((float)((sideSpeedInt / 65536.0) - 0.5) * 2000.0);
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
