using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0303F000)]
    public class CGameGhost : Node
    {
        public bool IsReplaying { get; set; }

        public CGameGhost(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x003 chunk

        [Chunk(0x0303F003)]
        public class Chunk0303F003 : Chunk<CGameGhost>
        {
            public byte[] Data { get; set; }
            public int[] Samples { get; set; }

            public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
            {
                Data = rw.Bytes(Data);
                Samples = rw.Array(Samples);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x004 chunk

        [Chunk(0x0303F004)]
        public class Chunk0303F004 : Chunk<CGameGhost>
        {
            public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
            {
                rw.Reader.ReadInt32(); // 0x0A103000
            }
        }

        #endregion

        #region 0x005 chunk

        [Chunk(0x0303F005)]
        public class Chunk0303F005 : Chunk<CGameGhost>
        {
            public int UncompressedSize { get; set; }
            public int CompressedSize { get; set; }
            public byte[] Data { get; set; }

            public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
            { 
                UncompressedSize = rw.Int32(UncompressedSize);
                CompressedSize = rw.Int32(CompressedSize);
                Data = rw.Bytes(Data, CompressedSize);

#if DEBUG

                using (var ms = new MemoryStream(Data))
                using (var zlib = new InflaterInputStream(ms))
                using (var gbxr = new GameBoxReader(zlib))
                {
                    var classID = gbxr.ReadInt32(); // CSceneVehicleCar
                    if (classID != -1)
                    {
                        var bSkipList2 = gbxr.ReadBoolean();
                        gbxr.ReadInt32();
                        var samplePeriod = gbxr.ReadInt32();
                        gbxr.ReadInt32();

                        var size = gbxr.ReadInt32();
                        var sampleData = gbxr.ReadBytes(size);

                        var numSamples = gbxr.ReadInt32();
                        if (numSamples > 0)
                        {
                            var firstSampleOffset = gbxr.ReadInt32();
                            if (numSamples > 1)
                            {
                                var sizePerSample = gbxr.ReadInt32();
                                if (sizePerSample == -1)
                                {
                                    var sampleSizes = gbxr.ReadArray<int>(numSamples - 1);
                                }
                            }
                        }

                        if (!bSkipList2)
                        {
                            var num = gbxr.ReadInt32();
                            var sampleTimes = gbxr.ReadArray<int>(num);
                        }

                        using (var msSampleData = new MemoryStream(sampleData))
                        using (var gbxrSampleData = new GameBoxReader(msSampleData))
                        {
                            for (var i = 0; i < numSamples; i++)
                            {
                                var pos = gbxrSampleData.ReadVec3();
                                var angle = gbxrSampleData.ReadInt16();
                                var axisHeading = gbxrSampleData.ReadInt16();
                                var axisPitch = gbxrSampleData.ReadInt16();
                                var speed = (float)Math.Exp(gbxrSampleData.ReadInt16() / 1000);
                                var velocityHeading = gbxrSampleData.ReadByte();
                                var velocityPitch = gbxrSampleData.ReadByte();
                                gbxrSampleData.ReadArray<short>(12);
                            }
                        }
                    }
                }

                #endif
            }
        }

        #endregion

        #region 0x006 chunk

        [Chunk(0x0303F006)]
        public class Chunk0303F006 : Chunk<CGameGhost>
        {
            public Chunk0303F005 Chunk005 { get; } = new Chunk0303F005();

            public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
            {
                n.IsReplaying = rw.Boolean(n.IsReplaying);
                Chunk005.ReadWrite(n, rw);
            }
        }

        #endregion

        #endregion
    }
}
