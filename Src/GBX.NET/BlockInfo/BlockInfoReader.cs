namespace GBX.NET.BlockInfo;

public sealed class BlockInfoReader : BinaryReader
{
    private readonly Dictionary<ushort, string> strings = new();

    public BlockInfoReader(Stream input) : base(input)
    {
    }

    public override string ReadString()
    {
        var index = ReadUInt16();

        if (index != 0 && strings.TryGetValue(index, out string str))
        {
            return str;
        }

        str = base.ReadString();

        strings.Add((ushort)(strings.Count + 1), str);

        return str;
    }

    public static Dictionary<string, BlockModel> Read(string biFile)
    {
        using var stream = File.OpenRead(biFile);
        return Read(stream);
    }

    public static Dictionary<string, BlockModel> Read(Stream biStream)
    {
        using var reader = new BlockInfoReader(biStream);

        if (!new byte[] { (byte)'B', (byte)'L', (byte)'O', (byte)'C', (byte)'K', (byte)'I', (byte)'N', (byte)'F', (byte)'O' }.SequenceEqual(reader.ReadBytes(9)))
        {
            throw new Exception("Invalid BLOCKINFO file.");
        }

        var version = reader.ReadByte();

        var blockModelCount = reader.ReadInt32();

        var dict = new Dictionary<string, BlockModel>(blockModelCount);

        for (var i = 0; i < blockModelCount; i++)
        {
            var name = reader.ReadString();
            dict.Add(name, ReadBlockModel(reader));
        }

        return dict;
    }

    private static BlockModel ReadBlockModel(BlockInfoReader reader)
    {
        var air = default(BlockUnit[]);
        var ground = default(BlockUnit[]);

        var variantCount = reader.ReadByte();
        
        for (var i = 0; i < variantCount; i++)
        {
            var variantName = reader.ReadString();

            switch (variantName)
            {
                case "Air":
                    air = ReadBlockUnits(reader);
                    break;
                case "Ground":
                    ground = ReadBlockUnits(reader);
                    break;
                default:
                    throw new NotSupportedException("Block variant not supported");
            }
        }

        return new BlockModel { Air = air, Ground = ground };
    }

    private static BlockUnit[] ReadBlockUnits(BlockInfoReader reader)
    {
        var unitCount = reader.ReadByte();

        var blockUnits = new BlockUnit[unitCount];

        for (var i = 0; i < unitCount; i++)
        {
            var x = reader.ReadByte();
            var y = reader.ReadByte();
            var z = reader.ReadByte();

            var unit = new BlockUnit { Coord = new(x, y, z) };

            var clipAvailabilityByte = reader.ReadByte();

            if ((clipAvailabilityByte & 1) != 0) unit.NorthClips = ReadClips(reader);
            if ((clipAvailabilityByte & 2) != 0) unit.EastClips = ReadClips(reader);
            if ((clipAvailabilityByte & 4) != 0) unit.SouthClips = ReadClips(reader);
            if ((clipAvailabilityByte & 8) != 0) unit.WestClips = ReadClips(reader);
            if ((clipAvailabilityByte & 16) != 0) unit.TopClips = ReadClips(reader);
            if ((clipAvailabilityByte & 32) != 0) unit.BottomClips = ReadClips(reader);
            
            blockUnits[i] = unit;
        }

        return blockUnits;
    }

    private static string[] ReadClips(BlockInfoReader reader)
    {
        var northClipCount = reader.ReadByte();
        var clips = new string[northClipCount];

        for (var j = 0; j < northClipCount; j++)
        {
            clips[j] = reader.ReadString();
        }

        return clips;
    }
}
