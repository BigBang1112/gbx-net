namespace GBX.NET;

public class GameBoxHeaderInfo
{
    public short Version { get; set; }
    public GameBoxByteFormat ByteFormat { get; set; }
    public GameBoxCompression CompressionOfRefTable { get; set; }
    public GameBoxCompression CompressionOfBody { get; internal set; }
    public char? UnknownByte { get; set; }
    public uint? ID { get; internal set; }
    public byte[] UserData { get; protected set; }
    public int NumNodes { get; protected set; }

    public GameBoxHeaderInfo(uint id)
    {
        Version = 6;
        ByteFormat = GameBoxByteFormat.Byte;
        CompressionOfRefTable = GameBoxCompression.Uncompressed;
        CompressionOfBody = GameBoxCompression.Compressed;
        UnknownByte = 'R';
        ID = id;
        UserData = Array.Empty<byte>();
    }

    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public GameBoxHeaderInfo(GameBoxReader reader)
    {
        UserData = Array.Empty<byte>();
        Read(reader);
    }

    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    internal bool Read(GameBoxReader reader)
    {
        if (!reader.HasMagic(GameBox.Magic))
        {
            Log.Write("GBX magic missing! Corrupted file or not a GBX file.", ConsoleColor.Red);
            return false;
        }

        Log.Write("GBX recognized!", ConsoleColor.Green);

        Version = reader.ReadInt16();
        Log.Write("- Version: " + Version.ToString());

        if (Version >= 3)
        {
            ByteFormat = (GameBoxByteFormat)reader.ReadByte();
            Log.Write("- Byte format: " + ByteFormat.ToString());

            if (ByteFormat == GameBoxByteFormat.Text)
                throw new TextFormatNotSupportedException();

            CompressionOfRefTable = (GameBoxCompression)reader.ReadByte();
            Log.Write("- Ref. table compression: " + CompressionOfRefTable.ToString());

            CompressionOfBody = (GameBoxCompression)reader.ReadByte();
            Log.Write("- Body compression: " + CompressionOfBody.ToString());

            if (Version >= 4)
            {
                UnknownByte = (char)reader.ReadByte();
                Log.Write("- Unknown byte: " + UnknownByte.ToString());
            }

            ID = CMwNod.Remap(reader.ReadUInt32());
            Log.Write("- Class ID: 0x" + ID.Value.ToString("X8"));

            if (Version >= 6)
            {
                var userDataSize = reader.ReadInt32();
                Log.Write($"- User data size: {(userDataSize / 1024f).ToString()} kB");

                if (userDataSize > 0)
                    UserData = reader.ReadBytes(userDataSize);
            }

            NumNodes = reader.ReadInt32();
            Log.Write("- Number of nodes: " + NumNodes.ToString());
        }

        Log.Write("Header completed!", ConsoleColor.Green);

        return true;
    }
}
