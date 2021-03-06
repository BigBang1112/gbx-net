﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET.Engines.Game
{
    [Node(0x0303F000)]
    public class CGameGhost : Node
    {
        private Action<Task<CGameGhostData>> dataExceptionHandle;

        public bool IsReplaying { get; set; }

        public Task<CGameGhostData> Data { get; set; }

        public CGameGhost()
        {
            dataExceptionHandle = task =>
            {
                if (task.IsFaulted)
                {
                    Log.Write($"\nExceptions while reading ghost data: ({task.Exception.InnerExceptions.Count})", ConsoleColor.Yellow);
                    foreach (var ex in task.Exception.InnerExceptions)
                        Log.Write(ex.ToString());
                }
            };
        }

        #region Chunks

        #region 0x003 chunk

        [Chunk(0x0303F003)]
        public class Chunk0303F003 : Chunk<CGameGhost>
        {
            public byte[] Data { get; set; }
            public int[] Samples { get; set; }
            public int SamplePeriod { get; set; }

            public override void ReadWrite(CGameGhost n, GameBoxReaderWriter rw)
            {
                Data = rw.Bytes(Data);
                Samples = rw.Array(Samples);

                rw.Int32(Unknown);
                rw.Boolean(Unknown);
                SamplePeriod = rw.Int32(SamplePeriod);
                rw.Int32(Unknown);

                n.Data = Task.Run(() =>
                {
                    var ghostData = new CGameGhostData
                    {
                        SamplePeriod = TimeSpan.FromMilliseconds(SamplePeriod)
                    };

                    using (var ms = new MemoryStream(Data))
                    {
                        ghostData.ReadSamples(ms, Samples.Length, 56);
                    }

                    return ghostData;
                });

                n.Data.ContinueWith(n.dataExceptionHandle);
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

                if (rw.Mode == GameBoxReaderWriterMode.Read)
                {
                    n.Data = Task.Run(() =>
                    {
                        var ghostData = new CGameGhostData();
                        using (var ms = new MemoryStream(Data))
                            ghostData.Read(ms, true);
                        return ghostData;
                    });

                    n.Data.ContinueWith(n.dataExceptionHandle);
                }
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
