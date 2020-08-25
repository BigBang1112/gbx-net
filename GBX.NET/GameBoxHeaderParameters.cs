using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET
{
    public class GameBoxHeaderParameters
    {
        public short Version { get; set; }
        public char? ByteFormat { get; set; }
        public char? RefTableCompression { get; set; }
        public char? BodyCompression { get; set; }
        public char? UnknownByte { get; set; }
        public uint? ClassID { get; set; }
        public byte[] UserData { get; set; }
        public int? NumNodes { get; set; }

        public bool Read(GameBoxReader r)
        {
            if (r.ReadString(3) == "GBX")
                Log.Write("GBX recognized!", ConsoleColor.Green);
            else
            {
                Log.Write("GBX magic missing! Corrupted file or not a GBX file.", ConsoleColor.Red);
                return false;
            }

            Version = r.ReadInt16();
            Log.Write($"- Version: {Version}");

            if (Version >= 3)
            {
                ByteFormat = (char)r.ReadByte();
                Log.Write($"- Byte format: {ByteFormat}");

                RefTableCompression = (char)r.ReadByte();
                Log.Write($"- Ref. table compression: {RefTableCompression}");

                BodyCompression = (char)r.ReadByte();
                Log.Write($"- Body compression: {RefTableCompression}");

                if (Version >= 4)
                {
                    UnknownByte = (char)r.ReadByte();
                    Log.Write($"- Unknown byte: {UnknownByte}");
                }

                ClassID = r.ReadUInt32();
                Log.Write($"- Class ID: 0x{ClassID:x8}");

                if (Version >= 6)
                {
                    var userDataSize = r.ReadInt32();
                    Log.Write($"- User data size: {userDataSize/1024f} kB");

                    if (userDataSize > 0)
                        UserData = r.ReadBytes(userDataSize);
                }

                NumNodes = r.ReadInt32();
                Log.Write($"- Number of nodes: {NumNodes}");
            }

            Log.Write("Header completed!", ConsoleColor.Green);

            return true;
        }
    }
}
