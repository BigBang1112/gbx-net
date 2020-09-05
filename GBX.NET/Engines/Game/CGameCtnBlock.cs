using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03057000)]
    public class CGameCtnBlock : Node
    {
        public Meta BlockInfo
        {
            get => GetValue<Chunk002>(x => x.BlockInfo) as Meta;
            set => SetValue<Chunk002>(x => x.BlockInfo = value);
        }

        public Direction? Direction
        {
            get => GetValue<Chunk002>(x => x.Direction) as Direction?;
            set => SetValue<Chunk002>(x => x.Direction = value.GetValueOrDefault());
        }

        public Byte3? Coord
        {
            get => GetValue<Chunk002>(x => x.Coord) as Byte3?;
            set => SetValue<Chunk002>(x => x.Coord = value.GetValueOrDefault());
        }

        public int? Flags
        {
            get => GetValue<Chunk002>(x => x.Flags) as int?;
            set => SetValue<Chunk002>(x => x.Flags = value.GetValueOrDefault());
        }

        public CGameCtnBlock(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        public CGameCtnBlock(ILookbackable lookbackable) : base(lookbackable, 0x03057000)
        {

        }

        public override string ToString()
        {
            return $"{BlockInfo?.ID} {Coord}";
        }

        [Chunk(0x03057002)]
        public class Chunk002 : Chunk
        {
            public Meta BlockInfo { get; set; }
            public Direction Direction { get; set; }
            public Byte3 Coord { get; set; }
            public int Flags { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                BlockInfo = rw.Meta(BlockInfo);
                Direction = (Direction)rw.Byte((byte)Direction);
                Coord = rw.Byte3(Coord);
                Flags = rw.Int32(Flags);
            }
        }
    }
}
