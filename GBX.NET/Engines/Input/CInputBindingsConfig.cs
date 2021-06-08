using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Input
{
    [Node(0x13006000)]
    public class CInputBindingsConfig : CMwNod
    {
        public ReadOnlyCollection<string> Devices { get; private set; }
        public List<Binding> Bindings { get; private set; }

        [Chunk(0x13006000)]
        public class Chunk13006000 : Chunk<CInputBindingsConfig>
        {
            public override void Read(CInputBindingsConfig n, GameBoxReader r, GameBoxWriter unknownW)
            {
                var race = r.ReadString();
                n.Bindings = r.ReadArray<Binding>((i, r1) =>
                {
                    if (i == 0) r1.ReadInt32();

                    return new Binding
                    {
                        KeyCode = r1.ReadInt32(),
                        DeviceGuid = r1.ReadId(),
                        U01 = r1.ReadInt32(),
                        U02 = r1.ReadInt32(),
                        Action = r1.ReadString()
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
                    r.ReadArray(r1 => r1.ReadId()).Select(x => x.ToString()).ToList()
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
