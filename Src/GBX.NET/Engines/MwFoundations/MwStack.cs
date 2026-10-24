using GBX.NET.Serialization;

namespace GBX.NET.Engines.MwFoundations;

public sealed class MwStack : IReadableWritable, IReadable, IWritable
{
    public Value[] Values { get; set; } = [];

    public void Read(GbxReader r, int v = 0) => ReadWrite(new GbxReaderWriter(r), v);

    public void Write(GbxWriter w, int v = 0) => ReadWrite(new GbxReaderWriter(w), v);

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        var types = rw.Array(Values.Select(value => value.Type).ToArray()) ?? [];

        if (rw.Reader is not null)
        {
            Values = types.Select(type => new Value { Type = type }).ToArray();
        }

        foreach (var value in Values)
        {
            switch (value.Type)
            {
                case 0:
                case 1:
                    value.Number = rw.Int32(value.Number);
                    break;
                case 2:
                    value.IsLocalName = rw.Boolean(value.IsLocalName);
                    value.Name = rw.String(value.Name);
                    break;
                default:
                    // Runtime reference types have no persistent value payload.
                    break;
            }
        }
    }

    public sealed class Value
    {
        public int Type { get; set; }
        public int Number { get; set; }
        public bool IsLocalName { get; set; } = true;
        public string? Name { get; set; }
    }
}
