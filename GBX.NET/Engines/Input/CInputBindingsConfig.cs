using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GBX.NET.Engines.Input
{
    [Node(0x13006000)]
    public class CInputBindingsConfig : Node
    {
        public ReadOnlyCollection<string> Devices { get; private set; }
        public List<Binding> Bindings { get; private set; }

        [Chunk(0x13006000)]
        public class Chunk13006000 : Chunk<CInputBindingsConfig>
        {
            public override void Read(CInputBindingsConfig n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var race = r.ReadString();
                n.Bindings = r.ReadArray<Binding>(i =>
                {
                    if (i == 0) r.ReadInt32();

                    return new Binding
                    {
                        KeyCode = r.ReadInt32(),
                        DeviceGuid = r.ReadId(),
                        U01 = r.ReadInt32(),
                        U02 = r.ReadInt32(),
                        Action = r.ReadString()
                    };
                }).ToList();
            }
        }

        [Chunk(0x13006001)]
        public class Chunk13006001 : Chunk<CInputBindingsConfig>
        {
            public override void Read(CInputBindingsConfig n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Devices = new ReadOnlyCollection<string>(
                    r.ReadArray(i => r.ReadId()).Select(x => x.ToString()).ToList()
                );
            }
        }

        public class Binding
        {
            public int KeyCode { get; set; }
            public string DeviceGuid { get; set; }
            public int U01 { get; set; }
            public int U02 { get; set; }
            public string Action { get; set; }

            public override string ToString()
            {
                return $"[{DeviceGuid}] {Action}: {KeyCode}";
            }
        }
    }
}
