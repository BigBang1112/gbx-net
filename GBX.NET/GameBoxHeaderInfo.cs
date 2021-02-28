using System;

namespace GBX.NET
{
    public class GameBoxHeaderInfo
    {
        public short Version { get; set; } = 6;
        public char? ByteFormat { get; set; } = 'B';
        public char? RefTableCompression { get; set; } = 'U';
        public char? BodyCompression { get; set; } = 'C';
        public char? UnknownByte { get; set; } = 'R';
        public uint? ClassID { get; internal set; }
        public byte[] UserData { get; private set; } = new byte[0];
        public int NumNodes { get; private set; }

        public bool Read(GameBoxReader reader)
        {
            if (reader.HasMagic(GameBox.Magic))
                Log.Write("GBX recognized!", ConsoleColor.Green);
            else
            {
                Log.Write("GBX magic missing! Corrupted file or not a GBX file.", ConsoleColor.Red);
                return false;
            }

            Version = reader.ReadInt16();
            Log.Write($"- Version: {Version}");

            if (Version >= 3)
            {
                ByteFormat = (char)reader.ReadByte();
                Log.Write($"- Byte format: {ByteFormat}");

                RefTableCompression = (char)reader.ReadByte();
                Log.Write($"- Ref. table compression: {RefTableCompression}");

                BodyCompression = (char)reader.ReadByte();
                Log.Write($"- Body compression: {BodyCompression}");

                if (Version >= 4)
                {
                    UnknownByte = (char)reader.ReadByte();
                    Log.Write($"- Unknown byte: {UnknownByte}");
                }

                ClassID = Node.Remap(reader.ReadUInt32());
                Log.Write($"- Class ID: 0x{ClassID:X8}");

                if (Version >= 6)
                {
                    var userDataSize = reader.ReadInt32();
                    Log.Write($"- User data size: {userDataSize / 1024f} kB");

                    if (userDataSize > 0)
                        UserData = reader.ReadBytes(userDataSize);
                }

                NumNodes = reader.ReadInt32();
                Log.Write($"- Number of nodes: {NumNodes}");
            }

            Log.Write("Header completed!", ConsoleColor.Green);

            return true;
        }
    }
}
