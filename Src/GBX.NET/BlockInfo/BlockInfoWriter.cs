namespace GBX.NET.BlockInfo;

public sealed class BlockInfoWriter : BinaryWriter
{
    private readonly Dictionary<string, ushort> strings = new();

    public BlockInfoWriter(Stream output) : base(output)
    {
    }

    public override void Write(string value)
    {
        if (strings.ContainsKey(value))
        {
            Write(strings[value]);
            return;
        }

        Write((ushort)0);
        base.Write(value);

        strings.Add(value, (ushort)(strings.Count + 1));
    }
    
    public static void Write(string biFile, Dictionary<string, BlockModel> blockModels)
    {
        using var stream = File.Create(biFile);
        Write(stream, blockModels);
    }

    public static void Write(Stream biStream, Dictionary<string, BlockModel> blockModels)
    {
        using var writer = new BlockInfoWriter(biStream);

        writer.Write(new byte[] { (byte)'B', (byte)'L', (byte)'O', (byte)'C', (byte)'K', (byte)'I', (byte)'N', (byte)'F', (byte)'O' });

        writer.Write((byte)0); // version

        writer.Write(blockModels.Count);

        foreach (var pair in blockModels)
        {
            var blockModel = pair.Value;

            writer.Write(pair.Key);

            writer.Write((byte)((blockModel.Air is null ? 0 : 1) + (blockModel.Ground is null ? 0 : 1)));

            WriteVariant(writer, blockModel.Air, "Air");
            WriteVariant(writer, blockModel.Ground, "Ground");
        }
    }

    private static void WriteVariant(BlockInfoWriter writer, BlockUnit[]? units, string name)
    {
        if (units is null)
        {
            return;
        }

        writer.Write(name);

        writer.Write((byte)units.Length);

        foreach (var unit in units)
        {
            writer.Write((byte)unit.Coord.X);
            writer.Write((byte)unit.Coord.Y);
            writer.Write((byte)unit.Coord.Z);

            var clipAvailabilityByte = (unit.NorthClips?.Length > 0 ? 1 : 0)
                | (unit.EastClips?.Length > 0 ? 2 : 0)
                | (unit.SouthClips?.Length > 0 ? 4 : 0)
                | (unit.WestClips?.Length > 0 ? 8 : 0)
                | (unit.TopClips?.Length > 0 ? 16 : 0)
                | (unit.BottomClips?.Length > 0 ? 32 : 0);

            writer.Write((byte)clipAvailabilityByte);

            WriteClips(writer, unit.NorthClips);
            WriteClips(writer, unit.EastClips);
            WriteClips(writer, unit.SouthClips);
            WriteClips(writer, unit.WestClips);
            WriteClips(writer, unit.TopClips);
            WriteClips(writer, unit.BottomClips);
        }
    }

    private static void WriteClips(BlockInfoWriter writer, string[]? clips)
    {
        if (clips is null || clips.Length == 0)
        {
            return;
        }

        writer.Write((byte)clips.Length);

        foreach (var clip in clips)
        {
            writer.Write(clip);
        }
    }
}
