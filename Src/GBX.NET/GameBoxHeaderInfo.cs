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
    public GameBoxHeaderInfo(GameBoxReader reader, ILogger? logger)
    {
        UserData = Array.Empty<byte>();
        Read(reader, logger);
    }

    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    internal bool Read(GameBoxReader reader, ILogger? logger)
    {
        if (!reader.HasMagic(GameBox.Magic))
        {
            logger?.LogError("GBX magic missing! Corrupted file or not a GBX file.");
            return false;
        }

        logger?.LogDebug("GBX recognized!");

        Version = reader.ReadInt16();
        logger?.LogDebug("Version: {version}", Version);

        if (Version >= 3)
        {
            ByteFormat = (GameBoxByteFormat)reader.ReadByte();
            logger?.LogDebug("Byte format: {format}", ByteFormat);

            if (ByteFormat == GameBoxByteFormat.Text)
                throw new TextFormatNotSupportedException();

            CompressionOfRefTable = (GameBoxCompression)reader.ReadByte();
            logger?.LogDebug("Ref. table compression: {compression}", CompressionOfRefTable);

            CompressionOfBody = (GameBoxCompression)reader.ReadByte();
            logger?.LogDebug("Body compression: {compression}", CompressionOfBody);

            if (Version >= 4)
            {
                UnknownByte = (char)reader.ReadByte();
                logger?.LogDebug("Unknown byte: {byte}", UnknownByte);
            }

            ID = CMwNod.Remap(reader.ReadUInt32());
            logger?.LogDebug("Class ID: 0x{classId}", ID.Value, "X8");

            if (Version >= 6)
            {
                var userDataSize = reader.ReadInt32();
                logger?.LogDebug("User data size: {size} kB", userDataSize / 1024f);

                if (userDataSize > 0)
                    UserData = reader.ReadBytes(userDataSize);
            }

            NumNodes = reader.ReadInt32();
            logger?.LogDebug("Number of nodes: {numNodes}", NumNodes);
        }

        logger?.LogDebug("Header completed!");

        return true;
    }
}
