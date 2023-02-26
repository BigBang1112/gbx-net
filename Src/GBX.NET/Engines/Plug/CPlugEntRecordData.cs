using System.Collections.ObjectModel;
using System.IO.Compression;

namespace GBX.NET.Engines.Plug;

/// <summary>
/// Entity data in a timeline.
/// </summary>
/// <remarks>ID: 0x0911F000</remarks>
[Node(0x0911F000)]
public class CPlugEntRecordData : CMwNod
{
    private ObservableCollection<Sample>? samples;

    public byte[]? Data { get; set; }

    [NodeMember]
    [AppliedWithChunk<Chunk0911F000>]
    public ObservableCollection<Sample>? Samples
    {
        get
        {
            if (samples is null)
            {
                var chunkVersion = GetChunk<Chunk0911F000>()?.Version;

                if (chunkVersion.HasValue)
                {
                    samples = ReadSamples(chunkVersion.Value);
                }
            }

            return samples;
        }
    }

    internal CPlugEntRecordData()
    {
        
    }

    private ObservableCollection<Sample> ReadSamples(int version)
    {
        if (Data is null)
        {
            throw new Exception();
        }

        var samples = new ObservableCollection<Sample>();

        using var ms = new MemoryStream(Data);
        using var cs = new CompressedStream(ms, CompressionMode.Decompress);
        using var r = new GameBoxReader(cs);

        var u01 = r.ReadInt32();
        var ghostLength = r.ReadInt32(); // milliseconds
        var objects = r.ReadArray<object>(r =>
        {
            var nodeId = r.ReadUInt32();
            var nodeName = NodeManager.GetName(nodeId);

            return new
            {
                nodeId,
                nodeName,
                obj_u01 = r.ReadInt32(),
                obj_u02 = r.ReadInt32(),
                obj_u03 = r.ReadInt32(),
                mwbuffer = r.ReadInt32(),
                obj_u05 = r.ReadInt32()
            };
        });

        if (version >= 2)
        {
            var objcts2 = r.ReadArray<object>(r =>
            {
                var u02 = r.ReadInt32();
                var u03 = r.ReadInt32();

                uint? clas = null;
                string? clasName = null;

                if (version >= 4)
                {
                    clas = r.ReadUInt32();
                    clasName = NodeManager.GetName(clas.Value);
                }

                return new
                {
                    u02,
                    u03,
                    clas,
                    clasName
                };
            });
        }

        var u04 = r.ReadByte();
        while (u04 != 0)
        {
            var bufferType = r.ReadInt32();
            var u06 = r.ReadInt32();
            var u07 = r.ReadInt32();
            var ghostLengthFinish = r.ReadInt32(); // ms

            if (version < 6)
            {
                // temp_79f24995b2b->field_0x28 = temp_79f24995b2b->field_0xc
            }
            else
            {
                var u08 = r.ReadInt32();
            }

            // Reads byte on every loop until the byte is 0, should be 1 otherwise
            for (byte x; (x = r.ReadByte()) != 0;)
            {
                var timestamp = r.ReadInt32();
                var buffer = r.ReadBytes(); // MwBuffer

                if (buffer.Length == 0)
                {
                    continue;
                }

                using var bufferMs = new MemoryStream(buffer);
                using var bufferR = new GameBoxReader(bufferMs);

                var sampleProgress = (int)bufferMs.Position;

                var sample = new Sample(buffer)
                {
                    BufferType = (byte)bufferType
                };

                switch (bufferType)
                {
                    case 0:
                        break;
                    case 2:
                        {
                            bufferMs.Position = 5;

                            var (position, rotation, speed, velocity) = bufferR.ReadTransform(); // Only position matches

                            sample.Timestamp = TimeInt32.FromMilliseconds(timestamp);
                            sample.Position = position;
                            sample.Rotation = rotation;
                            sample.Speed = speed * 3.6f;
                            sample.Velocity = velocity;

                            break;
                        }
                    case 4:
                    case 6:
                        {
                            bufferMs.Position = 5;
                            var rpmByte = bufferR.ReadByte();

                            bufferMs.Position = 14;
                            var steerByte = bufferR.ReadByte();
                            var steer = ((steerByte / 255f) - 0.5f) * 2;

                            bufferMs.Position = 91;
                            var gearByte = bufferR.ReadByte();
                            var gear = gearByte / 5f;

                            sample.Gear = gear;
                            sample.RPM = rpmByte;
                            sample.Steer = steer;

                            bufferMs.Position = 15;
                            var u15 = bufferR.ReadByte();

                            bufferMs.Position = 18;
                            var brakeByte = bufferR.ReadByte();
                            var brake = brakeByte / 255f;
                            var gas = u15 / 255f + brake;

                            sample.Brake = brake;
                            sample.Gas = gas;

                            bufferMs.Position = 47;

                            var (position, rotation, speed, velocity) = bufferR.ReadTransform();

                            sample.Timestamp = TimeInt32.FromMilliseconds(timestamp);
                            sample.Position = position;
                            sample.Rotation = rotation;
                            sample.Speed = speed * 3.6f;
                            sample.Velocity = velocity;

                            // ICE
                            bufferMs.Position = 81;
                            var FLIceByte = bufferR.ReadByte();
                            bufferMs.Position = 82;
                            var FRIceByte = bufferR.ReadByte();
                            bufferMs.Position = 83;
                            var RRIceByte = bufferR.ReadByte();
                            bufferMs.Position = 84;
                            var RLIceByte = bufferR.ReadByte();

                            sample.FLIcing = FLIceByte / 255f;
                            sample.FRIcing = FRIceByte / 255f;
                            sample.RRIcing = RRIceByte / 255f;
                            sample.RLIcing = RLIceByte / 255f;

                            // DIRT
                            bufferMs.Position = 93;
                            var FLDirtByte = bufferR.ReadByte();
                            bufferMs.Position = 95;
                            var FRDirtByte = bufferR.ReadByte();
                            bufferMs.Position = 97;
                            var RRDirtByte = bufferR.ReadByte();
                            bufferMs.Position = 99;
                            var RLDirtByte = bufferR.ReadByte();

                            sample.FLDirt = FLDirtByte / 255f;
                            sample.FRDirt = FRDirtByte / 255f;
                            sample.RRDirt = RRDirtByte / 255f;
                            sample.RLDirt = RLDirtByte / 255f;

                            // GroundContactMaterial
                            bufferMs.Position = 24;
                            var FLGroundContactMaterialByte = bufferR.ReadByte();
                            bufferMs.Position = 26;
                            var FRGroundContactMaterialByte = bufferR.ReadByte();
                            bufferMs.Position = 28;
                            var RRGroundContactMaterialByte = bufferR.ReadByte();
                            bufferMs.Position = 30;
                            var RLGroundContactMaterialByte = bufferR.ReadByte();

                            sample.FLGroundContactMaterial = (EPlugSurfaceMaterialId)FLGroundContactMaterialByte;
                            sample.FRGroundContactMaterial = (EPlugSurfaceMaterialId)FRGroundContactMaterialByte;
                            sample.RRGroundContactMaterial = (EPlugSurfaceMaterialId)RRGroundContactMaterialByte;
                            sample.RLGroundContactMaterial = (EPlugSurfaceMaterialId)RLGroundContactMaterialByte;

                            // DampenLen
                            bufferMs.Position = 23;
                            var FLDampenLenByte = bufferR.ReadByte();
                            bufferMs.Position = 25;
                            var FRDampenLenByte = bufferR.ReadByte();
                            bufferMs.Position = 27;
                            var RRDampenLenByte = bufferR.ReadByte();
                            bufferMs.Position = 29;
                            var RLDampenLenByte = bufferR.ReadByte();

                            // Multiply by 4 instead of 2 as it matches value given by openplanet CSceneVehicleVisState
                            sample.FLDampenLen = ((FLDampenLenByte / 255f) - 0.5f) * 4;
                            sample.FRDampenLen = ((FRDampenLenByte / 255f) - 0.5f) * 4;
                            sample.RRDampenLen = ((RRDampenLenByte / 255f) - 0.5f) * 4;
                            sample.RLDampenLen = ((RLDampenLenByte / 255f) - 0.5f) * 4;

                            // SlipCoef
                            bufferMs.Position = 32;
                            var SlipCoefByte1 = bufferR.ReadByte();
                            bufferMs.Position = 33;
                            var SlipCoefByte2 = bufferR.ReadByte();

                            byte maskFR = 0x1;  // 00000001
                            byte maskRR = 0x4;  // 00000100
                            byte maskRL = 0x10; // 00010000
                            byte maskFL = 0x40; // 01000000

                            // Nadeo uses two bytes for some reason
                            // FLSlip is unique in that it is located at the 7th bit of SlipCoefByte1.
                            // The 7th bit of SlipCoefByte2 is used too, however it appears to not be correlated with anything.
                            sample.FLSlipCoef = (SlipCoefByte1 & maskFL) != 0;
                            sample.FRSlipCoef = (SlipCoefByte2 & maskFR) != 0;
                            sample.RRSlipCoef = (SlipCoefByte2 & maskRR) != 0;
                            sample.RLSlipCoef = (SlipCoefByte2 & maskRL) != 0;

                            // WheelRotation
                            bufferMs.Position = 6;
                            var FLWheelRotationByte = bufferR.ReadByte();
                            bufferMs.Position = 7;
                            var FLWheelRotationCountByte = bufferR.ReadByte();
                            bufferMs.Position = 8;
                            var FRWheelRotationByte = bufferR.ReadByte();
                            bufferMs.Position = 9;
                            var FRWheelRotationCountByte = bufferR.ReadByte();
                            bufferMs.Position = 10;
                            var RRWheelRotationByte = bufferR.ReadByte();
                            bufferMs.Position = 11;
                            var RRWheelRotationCountByte = bufferR.ReadByte();
                            bufferMs.Position = 12;
                            var RLWheelRotationByte = bufferR.ReadByte();
                            bufferMs.Position = 13;
                            var RLWheelRotationCountByte = bufferR.ReadByte();

                            var tau = (float)Math.PI *2;
                            sample.FLWheelRot = (FLWheelRotationByte / 255f * tau) + (FLWheelRotationCountByte * tau);
                            sample.FRWheelRot = (FRWheelRotationByte / 255f * tau) + (FRWheelRotationCountByte * tau);
                            sample.RRWheelRot = (RRWheelRotationByte / 255f * tau) + (RRWheelRotationCountByte * tau);
                            sample.RLWheelRot = (RLWheelRotationByte / 255f * tau) + (RLWheelRotationCountByte * tau);

                            // Water
                            bufferMs.Position = 101;
                            var waterByte = bufferR.ReadByte();

                            sample.WetnessValue = waterByte / 255f;

                            // IsGroundContact, IsReactorGroundMode, ReactorState
                            bufferMs.Position = 89;
                            var groundModeByte = bufferR.ReadByte();

                            var maskIsGroundMode = 0x1;         // 00000001
                            var maskIsReactorGroundMode = 0x4;  // 00000010
                            var maskIsReactorUp = 0x8;          // 00001000
                            var maskIsReactorDown = 0x10;       // 00010000
                            var maskReactorLvl1 = 0x20;         // 00100000
                            var maskReactorLvl2 = 0x40;         // 01000000 
                            //var maskSlowMo = 0x80;              // 10000000 // Can use SimulationTimeCoef instead

                            sample.IsGroundContact = (groundModeByte & maskIsGroundMode) != 0;
                            sample.IsReactorGroundMode = (groundModeByte & maskIsReactorGroundMode) != 0;

                            var isReactorUp = (groundModeByte & maskIsReactorUp) != 0;
                            var isReactorDown = (groundModeByte & maskIsReactorDown) != 0;

                            var isReactorLvl1 = (groundModeByte & maskReactorLvl1) != 0;
                            var isReactorLvl2 = (groundModeByte & maskReactorLvl2) != 0;

                            //var isSlowMo = (groundModeByte & maskSlowMo) != 0;

                            if (isReactorLvl1)
                                sample.ReactorBoostLvl = ESceneVehicleVisReactorBoostLvl.Lvl1;
                            else if (isReactorLvl2)
                                sample.ReactorBoostLvl = ESceneVehicleVisReactorBoostLvl.Lvl2;
                            else
                                sample.ReactorBoostLvl = ESceneVehicleVisReactorBoostLvl.None;

                            if (isReactorUp && isReactorDown)
                                sample.ReactorBoostType = ESceneVehicleVisReactorBoostType.UpAndDown;
                            else if (isReactorUp)
                                sample.ReactorBoostType = ESceneVehicleVisReactorBoostType.Up;
                            else if (isReactorDown)
                                sample.ReactorBoostType = ESceneVehicleVisReactorBoostType.Down;
                            else
                                sample.ReactorBoostType = ESceneVehicleVisReactorBoostType.None;

                            // Turbo
                            bufferMs.Position = 31;
                            var isTurboByte = bufferR.ReadByte();
                            bufferMs.Position = 21;
                            var turboTimeByte = bufferR.ReadByte();

                            var maskIsTurbo = 0x82; // 10000010

                            sample.IsTurbo = (isTurboByte & maskIsTurbo) != 0;
                            sample.TurboTime = turboTimeByte / 255f;

                            // SimulationTimeCoef (SlowMo)
                            bufferMs.Position = 102;
                            var SimulationTimeCoefByte = bufferR.ReadByte();

                            sample.SimulationTimeCoef = SimulationTimeCoefByte / 255f;

                            // ReactorAirControl
                            // [1,0,-1] = (Accell,None,Brake), (Left,None,Right)
                            bufferMs.Position = 90;
                            var boosterAirControlByte = bufferR.ReadByte();

                            var maskPedalNone = 0x10;   // 00010000
                            var maskPedalAccel = 0x20;  // 00100000
                            var maskSteerNone = 0x40;   // 01000000
                            var maskSteerLeft = 0x80;   // 10000000

                            var isAirControlPedalAccel = (boosterAirControlByte & maskPedalAccel) != 0;
                            var isAirControlPedalNone = (boosterAirControlByte & maskPedalNone) != 0;

                            sample.ReactorAirControlPedal = isAirControlPedalAccel ? 1 : isAirControlPedalNone ? 0 : -1;

                            var isAirControlSteerLeft = (boosterAirControlByte & maskSteerLeft) != 0;
                            var isAirControlSteerNone = (boosterAirControlByte & maskSteerNone) != 0;

                            sample.ReactorAirControlSteer = isAirControlSteerLeft ? 1 : isAirControlSteerNone ? 0 : -1;

                            // IsTopContact
                            bufferMs.Position = 76;
                            var vechicleStateByte = bufferR.ReadByte();

                            var maskIsTopContact = 0x20;
                            //var maskIsReactor = 0x10; 

                            sample.IsTopContact = (vechicleStateByte & maskIsTopContact) != 0;

                            // SideSpeed
                            bufferMs.Position = 2;
                            float sideSpeedInt = bufferR.ReadUInt16();
                            sample.SideSpeed = (float)((float)((sideSpeedInt / 65536.0) - 0.5) * 2000.0);

                            break;
                        }
                    case 10:
                        break;
                    default:
                        break;
                }

                samples.Add(sample);
            }

            u04 = r.ReadByte();

            if (version >= 2)
            {
                while (r.ReadByte() != 0)
                {
                    var type = r.ReadInt32();
                    var timestamp = r.ReadInt32();
                    var buffer = r.ReadBytes(); // MwBuffer
                }
            }
        }

        if (version >= 3)
        {
            while (r.ReadByte() != 0)
            {
                var u19 = r.ReadInt32();
                var u20 = r.ReadInt32();
                var u21 = r.ReadBytes(); // MwBuffer
            }

            if (version == 7)
            {
                while (r.ReadByte() != 0)
                {
                    var u23 = r.ReadInt32();
                    var u24 = r.ReadBytes(); // MwBuffer
                }
            }

            if (version >= 8)
            {
                var u23 = r.ReadInt32();

                if (u23 == 0)
                {
                    return samples;
                }

                if (version == 8)
                {
                    while (r.ReadByte() != 0)
                    {
                        var u25 = r.ReadInt32();
                        var u26 = r.ReadBytes(); // MwBuffer
                    }
                }
                else
                {
                    while (r.ReadByte() != 0)
                    {
                        var u28 = r.ReadInt32();
                        var u29 = r.ReadBytes(); // MwBuffer
                        var u30 = r.ReadBytes(); // MwBuffer
                    }

                    if (version >= 10)
                    {
                        var period = r.ReadInt32();
                    }
                }
            }
        }

        return samples;
    }

    /// <summary>
    /// CPlugEntRecordData 0x000 chunk
    /// </summary>
    [Chunk(0x0911F000)]
    public class Chunk0911F000 : Chunk<CPlugEntRecordData>, IVersionable
    {       
        public int Version { get; set; }

        public int CompressedSize { get; private set; }
        public int UncompressedSize { get; private set; }

        public override void Read(CPlugEntRecordData n, GameBoxReader r)
        {
            Version = r.ReadInt32(); // 10
            UncompressedSize = r.ReadInt32();
            CompressedSize = r.ReadInt32();
            n.Data = r.ReadBytes(CompressedSize);
        }

        public override void Write(CPlugEntRecordData n, GameBoxWriter w)
        {
            w.Write(Version);
            w.Write(UncompressedSize);
            w.Write(CompressedSize);
            w.Write(n.Data);
        }
    }
}
