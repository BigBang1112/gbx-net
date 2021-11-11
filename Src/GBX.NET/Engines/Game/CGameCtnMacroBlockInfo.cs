using System;
using System.Linq;

using GBX.NET.Engines.GameData;

namespace GBX.NET.Engines.Game;

/// <summary>
/// Macroblock (0x0310D000)
/// </summary>
[Node(0x0310D000), WritingNotSupported]
public sealed class CGameCtnMacroBlockInfo : CGameCtnCollector
{
    public CGameCtnBlock[]? Blocks { get; set; }

    public CGameCtnAnchoredObject[]? AnchoredObjects { get; set; }

    #region Constructors

    private CGameCtnMacroBlockInfo()
    {

    }

    #endregion

    #region Chunks

    #region 0x000 chunk (blocks)

    /// <summary>
    /// CGameCtnMacroBlockInfo 0x000 chunk (blocks)
    /// </summary>
    [Chunk(0x0310D000, "blocks")]
    public class Chunk0310D000 : Chunk<CGameCtnMacroBlockInfo>
    {
        public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r)
        {
            n.Blocks = r.ReadArray(r1 =>
            {
                Int3? coord = null;
                Direction? dir = null;
                Vec3? position = null;
                Vec3? pitchYawRoll = null;

                var ver = r1.ReadInt32();
                var blockModel = r1.ReadIdent();
                int flags = 0;

                if (ver >= 2)
                {
                    if (ver < 5)
                    {

                    }

                    flags = r1.ReadInt32();

                    if (ver >= 4)
                    {
                        if ((flags & (1 << 26)) != 0) // free block
                            {
                            position = r1.ReadVec3();
                            pitchYawRoll = r1.ReadVec3();
                        }
                        else
                        {
                            coord = (Int3)r1.ReadByte3();
                            dir = (Direction)r1.ReadByte();
                        }
                    }
                }

                if (ver >= 3)
                    if (r1.ReadNodeRef() != null)
                        throw new NotImplementedException();

                if (ver >= 4)
                    if (r1.ReadNodeRef() != null)
                        throw new NotImplementedException();

                var correctFlags = flags & 15;

                if ((flags & 0x20000) != 0) // Fixes inner pillars of some blocks
                        correctFlags |= 0x400000;

                if ((flags & 0x10000) != 0) // Fixes ghost blocks
                        correctFlags |= 0x10000000;

                var block = new CGameCtnBlock(blockModel, dir.GetValueOrDefault(), coord.GetValueOrDefault(), correctFlags);

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
        public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r)
        {
            var unknown = r.ReadArray(r1 =>
            {
                var version = r1.ReadInt32();
                if (r1.ReadNodeRef() != null)
                    throw new NotImplementedException();

                if (version == 0)
                {
                    r1.ReadInt32();
                    r1.ReadInt32();
                    r1.ReadInt32();
                }

                r1.ReadInt32();

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
        public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r)
        {
            var unknown = r.ReadArray(r1 =>
            {
                return new object[]
                {
                        r1.ReadInt32(),
                        r1.ReadArray(r2 => r2.ReadIdent()),

                        r1.ReadInt32(),
                        r1.ReadInt32(),
                        r1.ReadInt32()
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
        public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r)
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
        public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r)
        {
            var version = r.ReadInt32();
            var nodrefs = r.ReadArray(r1 => r1.ReadNodeRef());
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

        public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            n.AnchoredObjects = r.ReadArray(r1 =>
            {
                var v = r1.ReadInt32();

                var itemModel = r1.ReadIdent();

                Vec3 pitchYawRoll = default;
                Vec3 pivotPosition = default;
                float scale = 1;

                if (v < 3)
                {
                    var quarterY = r1.ReadByte();

                    if (v != 0)
                    {
                        var additionalDir = r1.ReadByte();
                    }
                }
                else
                {
                    pitchYawRoll = r1.ReadVec3();
                }

                var blockCoord = r1.ReadInt3();
                var lookback = r1.ReadId();
                var pos = r1.ReadVec3();

                if (v < 5)
                    r1.ReadInt32();
                if (v < 6)
                    r1.ReadInt32();
                if (v >= 6)
                    r1.ReadInt16(); // 0
                    if (v >= 7)
                    pivotPosition = r1.ReadVec3();
                if (v >= 8)
                    r1.ReadNodeRef(); // probably waypoint
                    if (v >= 9)
                    scale = r1.ReadSingle(); // 1
                    if (v >= 10)
                    r1.ReadArray<int>(3); // 0 1 -1

                    return new CGameCtnAnchoredObject(itemModel, pos, pitchYawRoll, pivotPosition,
                    scale: scale, blockUnitCoord: (Byte3)blockCoord);
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
        public override void Read(CGameCtnMacroBlockInfo n, GameBoxReader r)
        {
            var version = r.ReadInt32();
            _ = r.ReadArray<int>(7);
        }
    }

    #endregion

    #endregion
}
