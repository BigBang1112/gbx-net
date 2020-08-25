using GBX.NET.Engines.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0310D000)]
    public class CGameCtnMacroBlockInfo : CGameCtnCollector
    {
        public Block[] Blocks
        {
            get => GetValue<Chunk000>(x => x.Blocks) as Block[];
            set => SetValue<Chunk000>(x => x.Blocks = value);
        }

        public Item[] Items
        {
            get => GetValue<Chunk00E_2>(x => x.Items) as Item[];
            set => SetValue<Chunk00E_2>(x => x.Items = value);
        }

        public List<FreeBlock> FreeBlocks
        {
            get => GetValue<Chunk000>(x => x.FreeBlocks) as List<FreeBlock>;
        }

        public CGameCtnMacroBlockInfo(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x0310D000)]
        public class Chunk000 : Chunk
        {
            public Block[] Blocks { get; set; }
            public List<FreeBlock> FreeBlocks { get; set; }

            public Chunk000(CGameCtnMacroBlockInfo node) : base(node)
            {
                FreeBlocks = new List<FreeBlock>();
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Blocks = r.ReadArray(i =>
                {
                    Int3? coord = null;
                    Direction? dir = null;
                    Vector3? position = null;
                    Vector3? pitchYawRoll = null;

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
                        FreeBlocks.Add(new FreeBlock(block) { Position = position.Value, PitchYawRoll = pitchYawRoll.Value });
                        return null;
                    }

                    return block;
                }).Where(x => x != null).ToArray();
            }
        }

        [Chunk(0x0310D001)]
        public class Chunk001 : Chunk
        {
            public Chunk001(CGameCtnMacroBlockInfo node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                var gsdg = r.ReadArray(i =>
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

        [Chunk(0x0310D002)]
        public class Chunk002 : Chunk
        {
            public Chunk002(CGameCtnMacroBlockInfo node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                var gsdg = r.ReadArray(i =>
                {
                    r.ReadInt32();
                    var dsgds = r.ReadArray(i => r.ReadMeta());

                    r.ReadInt32();
                    r.ReadInt32();
                    r.ReadInt32();

                    return new object();
                });
            }
        }

        [Chunk(0x0310D006)]
        public class Chunk006_2 : Chunk
        {
            public Chunk006_2(CGameCtnMacroBlockInfo node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                var version = r.ReadInt32();
                var arrayLength = r.ReadInt32();
            }
        }

        [Chunk(0x0310D008)]
        public class Chunk008_2 : Chunk
        {
            public Chunk008_2(CGameCtnMacroBlockInfo node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                var version = r.ReadInt32();
                var nodrefs = r.ReadArray(i => r.ReadNodeRef());
                var idk = r.ReadArray<int>(2);
            }
        }

        /// <summary>
        /// CGameCtnMacroBlockInfo 0x00E chunk (items)
        /// </summary>
        [Chunk(0x0310D00E)]
        public class Chunk00E_2 : Chunk
        {
            public int Version { get; set; }
            public Item[] Items { get; set; }

            public Chunk00E_2(CGameCtnMacroBlockInfo node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                Items = r.ReadArray(i =>
                {
                    var v = r.ReadInt32();

                    var meta = r.ReadMeta();

                    Vector3? pitchYawRoll = null;
                    Vector3? pivotPosition = null;
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

        [Chunk(0x0310D00F)]
        public class Chunk00F : Chunk
        {
            public Chunk00F(CGameCtnMacroBlockInfo node) : base(node)
            {

            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                var version = r.ReadInt32();
                _ = r.ReadArray<int>(7);
            }
        }
    }
}
