using GBX.NET.Engines.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Macroblock (0x0310D000)
    /// </summary>
    [Node(0x0310D000)]
    public class CGameCtnMacroBlockInfo : CGameCtnCollector
    {
        public Block[] Blocks { get; set; }

        public Item[] Items { get; set; }

        public List<FreeBlock> FreeBlocks { get; set; } = new List<FreeBlock>();

        public CGameCtnMacroBlockInfo(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        #region Chunks

        #region 0x000 chunk (blocks)

        /// <summary>
        /// CGameCtnMacroBlockInfo 0x000 chunk (blocks)
        /// </summary>
        [Chunk(0x0310D000, "blocks")]
        public class Chunk0310D000 : Chunk<CGameCtnMacroBlockInfo>
        {
            public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Blocks = r.ReadArray(i =>
                {
                    Int3? coord = null;
                    Direction? dir = null;
                    Vec3? position = null;
                    Vec3? pitchYawRoll = null;

                    var ver = r.ReadInt32();
                    var meta = r.ReadMeta();
                    int flags = 0;

                    if(ver >= 2)
                    {
                        if(ver < 5)
                        {

                        }

                        flags = r.ReadInt32();

                        if (ver >= 4)
                        {
                            if ((flags & (1 << 26)) != 0) // free block
                            {
                                position = r.ReadVec3();
                                pitchYawRoll = r.ReadVec3();
                            }
                            else
                            {
                                coord = (Int3)r.ReadByte3();
                                dir = (Direction)r.ReadByte();
                            }
                        }
                    }

                    if (ver >= 3)
                        r.ReadNodeRef();

                    if (ver >= 4)
                        r.ReadNodeRef();

                    var block = new Block() { Meta = meta, Coord = coord.GetValueOrDefault(), Direction = dir.GetValueOrDefault(), Flags = flags };

                    if (position.HasValue && pitchYawRoll.HasValue)
                    {
                        n.FreeBlocks.Add(new FreeBlock(block) { Position = position.Value, PitchYawRoll = pitchYawRoll.Value });
                        return null;
                    }

                    return block;
                }).Where(x => x != null).ToArray();
            }
        }

        #endregion

        #region 0x001 chunk

        /// <summary>
        /// CGameCtnMacroBlockInfo 0x001 chunk
        /// </summary>
        [Chunk(0x0310D001)]
        public class Chunk0310D001 : Chunk<CGameCtnMacroBlockInfo>
        {
            public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var unknown = r.ReadArray(i =>
                {
                    var version = r.ReadInt32();
                    r.ReadNodeRef();

                    if(version == 0)
                    {
                        r.ReadInt32();
                        r.ReadInt32();
                        r.ReadInt32();
                    }

                    r.ReadInt32();

                    return new object();
                });
            }
        }

        #endregion

        #region 0x002 chunk

        /// <summary>
        /// CGameCtnMacroBlockInfo 0x002 chunk
        /// </summary>
        [Chunk(0x0310D002)]
        public class Chunk0310D002 : Chunk<CGameCtnMacroBlockInfo>
        {
            public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var unknown = r.ReadArray(i =>
                {
                    r.ReadInt32();
                    r.ReadArray(j => r.ReadMeta());

                    r.ReadInt32();
                    r.ReadInt32();
                    r.ReadInt32();

                    return new object();
                });
            }
        }

        #endregion

        #region 0x006 chunk

        /// <summary>
        /// CGameCtnMacroBlockInfo 0x006 chunk
        /// </summary>
        [Chunk(0x0310D006)]
        public class Chunk0310D006 : Chunk<CGameCtnMacroBlockInfo>
        {
            public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var version = r.ReadInt32();
                var arrayLength = r.ReadInt32();
            }
        }

        #endregion

        #region 0x008 chunk

        /// <summary>
        /// CGameCtnMacroBlockInfo 0x008 chunk
        /// </summary>
        [Chunk(0x0310D008)]
        public class Chunk0310D008 : Chunk<CGameCtnMacroBlockInfo>
        {
            public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var version = r.ReadInt32();
                var nodrefs = r.ReadArray(i => r.ReadNodeRef());
                r.ReadArray<int>(2);
            }
        }

        #endregion

        #region 0x00E chunk (items)

        /// <summary>
        /// CGameCtnMacroBlockInfo 0x00E chunk (items)
        /// </summary>
        [Chunk(0x0310D00E)]
        public class Chunk0310D00E : Chunk<CGameCtnMacroBlockInfo>
        {
            public int Version { get; set; }

            public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                n.Items = r.ReadArray(i =>
                {
                    var v = r.ReadInt32();

                    var meta = r.ReadMeta();

                    Vec3? pitchYawRoll = null;
                    Vec3? pivotPosition = null;
                    float? scale = null;

                    if (v < 3)
                    {
                        var quarterY = r.ReadByte();

                        if (v != 0)
                        {
                            var additionalDir = r.ReadByte();
                        }
                    }
                    else
                    {
                        pitchYawRoll = r.ReadVec3();
                    }

                    var blockCoord = r.ReadInt3();
                    r.ReadLookbackString();
                    var pos = r.ReadVec3();

                    if (v < 5)
                        r.ReadInt32();
                    if (v < 6)
                        r.ReadInt32();
                    if (v >= 6)
                        r.ReadInt16(); // 0
                    if (v >= 7)
                        pivotPosition = r.ReadVec3();
                    if (v >= 8)
                        r.ReadNodeRef(); // probably waypoint
                    if (v >= 9)
                        scale = r.ReadSingle(); // 1
                    if (v >= 10)
                        r.ReadArray<int>(3); // 0 1 -1

                    return new Item()
                    {
                        Meta = meta,
                        PitchYawRoll = pitchYawRoll,
                        BlockCoord = blockCoord,
                        Position = pos,
                        PivotPosition = pivotPosition,
                        Scale = scale,
                    };
                });

                r.ReadInt32();
            }
        }

        #endregion

        #region 0x00F chunk

        /// <summary>
        /// CGameCtnMacroBlockInfo 0x00F chunk
        /// </summary>
        [Chunk(0x0310D00F)]
        public class Chunk0310D00F : Chunk<CGameCtnMacroBlockInfo>
        {
            public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var version = r.ReadInt32();
                _ = r.ReadArray<int>(7);
            }
        }

        #endregion

        #endregion
    }
}
