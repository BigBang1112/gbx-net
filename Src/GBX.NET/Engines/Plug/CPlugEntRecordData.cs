using System.Collections.ObjectModel;
using System.IO.Compression;

namespace GBX.NET.Engines.Plug;

[Node(0x0911F000)]
public sealed class CPlugEntRecordData : CMwNod
{
    private Task<ObservableCollection<Sample>> samples;

    public ObservableCollection<Sample> Samples
    {
        get => samples.Result;
    }

    private CPlugEntRecordData()
    {
        samples = null!;
    }

    public async Task<ObservableCollection<Sample>> GetSamplesAsync()
    {
        return await samples;
    }

    [Chunk(0x0911F000)]
    public class Chunk0911F000 : Chunk<CPlugEntRecordData>, IVersionable
    {
        private byte[]? data;

        public int Version { get; set; } = 10;
        public int CompressedSize { get; private set; }
        public int UncompressedSize { get; private set; }

        public override void Read(CPlugEntRecordData n, GameBoxReader r)
        {
            Version = r.ReadInt32(); // 10
            UncompressedSize = r.ReadInt32();
            CompressedSize = r.ReadInt32();
            data = r.ReadBytes(CompressedSize);

            n.samples = Task.Run(() =>
            {
                var samples = new ObservableCollection<Sample>();

                using var ms = new MemoryStream(data);
                using var cs = new CompressedStream(ms, CompressionMode.Decompress);
                using var r = new GameBoxReader(cs);

                var u01 = r.ReadInt32();
                var ghostLength = r.ReadInt32(); // milliseconds
                var objects = r.ReadArray<object>(r =>
                {
                    var nodeId = r.ReadUInt32();
                    NodeCacheManager.Names.TryGetValue(nodeId, out string? nodeName);

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

                if (Version >= 2)
                {
                    var objcts2 = r.ReadArray<object>(r =>
                    {
                        var u02 = r.ReadInt32();
                        var u03 = r.ReadInt32();

                        uint? clas = null;
                        string? clasName = null;

                        if (Version >= 4)
                        {
                            clas = r.ReadUInt32();
                            NodeCacheManager.Names.TryGetValue(clas.Value, out clasName);
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

                    if (Version < 6)
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

                        if (buffer.Length > 0)
                        {
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

                                        sample.Timestamp = TimeSpan.FromMilliseconds(timestamp);
                                        sample.Position = position;
                                        sample.Rotation = rotation;
                                        sample.Speed = speed * 3.6f;
                                        sample.Velocity = velocity;

                                        break;
                                    }
                                case 4:
                                    {
                                        bufferMs.Position = 5;
                                        var gearByte = bufferR.ReadByte();
                                        var gear = gearByte / 5f;
                                        var rpmByte = bufferR.ReadByte();
                                        var steer = ((rpmByte / 255f) - 0.5f) * 2;

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

                                        sample.Timestamp = TimeSpan.FromMilliseconds(timestamp);
                                        sample.Position = position;
                                        sample.Rotation = rotation;
                                        sample.Speed = speed * 3.6f;
                                        sample.Velocity = velocity;

                                        break;
                                    }
                                case 10:
                                    break;
                                default:
                                    break;
                            }

                            samples.Add(sample);
                        }
                    }

                    u04 = r.ReadByte();

                    if (Version >= 2)
                    {
                        for (byte x; (x = r.ReadByte()) != 0;)
                        {
                            var type = r.ReadInt32();
                            var timestamp = r.ReadInt32();
                            var buffer = r.ReadBytes(); // MwBuffer
                        }
                    }
                }

                if (Version >= 3)
                {
                    for (byte x; (x = r.ReadByte()) != 0;)
                    {
                        var u19 = r.ReadInt32();
                        var u20 = r.ReadInt32();
                        var u21 = r.ReadBytes(); // MwBuffer
                    }

                    if (Version == 7)
                    {
                        for (byte x; (x = r.ReadByte()) != 0;)
                        {
                            var u23 = r.ReadInt32();
                            var u24 = r.ReadBytes(); // MwBuffer
                        }
                    }

                    if (Version >= 8)
                    {
                        var u23 = r.ReadInt32();

                        if (u23 != 0)
                        {
                            if (Version == 8)
                            {
                                for (byte x; (x = r.ReadByte()) != 0;)
                                {
                                    var u25 = r.ReadInt32();
                                    var u26 = r.ReadBytes(); // MwBuffer
                                }
                            }
                            else
                            {
                                for (byte x; (x = r.ReadByte()) != 0;)
                                {
                                    var u28 = r.ReadInt32();
                                    var u29 = r.ReadBytes(); // MwBuffer
                                    var u30 = r.ReadBytes(); // MwBuffer
                                }

                                if (Version >= 10)
                                {
                                    var period = r.ReadInt32();
                                }
                            }
                        }
                    }
                }

                return samples;
            });
        }

        public override void Write(CPlugEntRecordData n, GameBoxWriter w)
        {
            w.Write(Version);
            w.Write(UncompressedSize);
            w.Write(CompressedSize);
            w.WriteBytes(data);
        }
    }
}
