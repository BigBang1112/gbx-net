using GBX.NET.Engines.GameData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public CGameCtnBlock[] Blocks { get; set; }

        public CGameCtnAnchoredObject[] AnchoredObjects { get; set; }

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
                n.Blocks = r.ReadArray(() =>
                {
                    Int3? coord = null;
                    Direction? dir = null;
                    Vec3? position = null;
                    Vec3? pitchYawRoll = null;

                    var ver = r.ReadInt32();
                    var ident = r.ReadIdent();
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
                        if (r.ReadNodeRef() != null)
                            throw new NotImplementedException();

                    if (ver >= 4)
                        if (r.ReadNodeRef() != null)
                            throw new NotImplementedException();

                    var correctFlags = flags & 15;

                    if ((flags & 0x20000) != 0) // Fixes inner pillars of some blocks
                        correctFlags |= 0x400000;

                    if ((flags & 0x10000) != 0) // Fixes ghost blocks
                        correctFlags |= 0x10000000;

                    var block = new CGameCtnBlock()
                    {
                        BlockInfo = ident,
                        Coord = coord.GetValueOrDefault(),
                        Direction = dir.GetValueOrDefault(),
                        Flags = correctFlags
                    };

                    if ((flags & (1 << 26)) != 0)
                    {
                        block.IsFree = true;
                        block.AbsolutePositionInMap += position.GetValueOrDefault();
                        block.PitchYawRoll += pitchYawRoll.GetValueOrDefault();
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
                var unknown = r.ReadArray(() =>
                {
                    var version = r.ReadInt32();
                    if(r.ReadNodeRef() != null)
                        throw new NotImplementedException();

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
                var unknown = r.ReadArray(() =>
                {
                    return new object[]
                    {
                        r.ReadInt32(),
                        r.ReadArray(() => r.ReadIdent()),

                        r.ReadInt32(),
                        r.ReadInt32(),
                        r.ReadInt32()
                    };
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
                var nodrefs = r.ReadArray(() => r.ReadNodeRef());
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

                n.AnchoredObjects = r.ReadArray(() =>
                {
                    var v = r.ReadInt32();

                    var itemModel = r.ReadIdent();

                    Vec3 pitchYawRoll = default;
                    Vec3 pivotPosition = default;
                    float scale = 1;

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
                    var lookback = r.ReadId();
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

                    return new CGameCtnAnchoredObject()
                    {
                        ItemModel = itemModel,
                        PitchYawRoll = pitchYawRoll,
                        BlockUnitCoord = (Byte3)blockCoord,
                        AbsolutePositionInMap = pos,
                        PivotPosition = pivotPosition,
                        Scale = scale,
                    };
                });

                var num = r.ReadInt32();
                if (num == 1)
                {
                    r.ReadInt32();
                    r.ReadInt32();
                }
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
