using System.Collections.ObjectModel;

namespace GBX.NET.Engines.Input;

/// <summary>
/// Input bindings configuration.
/// </summary>
/// <remarks>ID: 0x13006000</remarks>
[Node(0x13006000)]
public class CInputBindingsConfig : CMwNod
{
    public ReadOnlyCollection<string> Devices { get; private set; }
    public IList<Binding> Bindings { get; private set; }

    protected CInputBindingsConfig()
    {
        Devices = null!;
        Bindings = null!;
    }

    /// <summary>
    /// CInputBindingsConfig 0x000 chunk
    /// </summary>
    [Chunk(0x13006000)]
    public class Chunk13006000 : Chunk<CInputBindingsConfig>
    {
        public override void Read(CInputBindingsConfig n, GameBoxReader r)
        {
            var race = r.ReadString();
            n.Bindings = r.ReadList<Binding>((i, r1) =>
            {
                //if (i == 0) r1.ReadInt32();

                return new Binding
                {
                    KeyCode = r1.ReadInt32(),
                    DeviceGuid = r1.ReadId(),
                    U01 = r1.ReadInt32(),
                    U02 = r1.ReadInt32(),
                    Action = r1.ReadString()
                };
            });
        }
    }

    /// <summary>
    /// CInputBindingsConfig 0x001 chunk
    /// </summary>
    [Chunk(0x13006001)]
    public class Chunk13006001 : Chunk<CInputBindingsConfig>
    {
        public override void Read(CInputBindingsConfig n, GameBoxReader r)
        {
            n.Devices = new ReadOnlyCollection<string>(
                r.ReadArray(r1 => r1.ReadId()).Select(x => x.ToString()).ToList()
            );
        }
    }

    public class Binding
    {
        public int U01;
        public int U02;

        public int KeyCode { get; set; }
        public string? DeviceGuid { get; set; }
        public string? Action { get; set; }

        public override string ToString()
        {
            return $"[{DeviceGuid ?? "unknown"}] {Action ?? "unknown"}: {KeyCode}";
        }
    }
}
