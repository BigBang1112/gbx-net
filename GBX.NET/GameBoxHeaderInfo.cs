using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using GBX.NET.Engines.MwFoundations;

namespace GBX.NET
{
    public class GameBoxHeaderInfo
    {
        public short Version { get; set; }
        public char? ByteFormat { get; set; }
        public char? RefTableCompression { get; set; }
        public char? BodyCompression { get; set; }
        public char? UnknownByte { get; set; }
        public uint? ID { get; internal set; }
        public byte[] UserData { get; protected set; }
        public int NumNodes { get; protected set; }

        public GameBoxHeaderInfo(uint id)
        {
            Version = 6;
            ByteFormat = 'B';
            RefTableCompression = 'U';
            BodyCompression = 'C';
            UnknownByte = 'R';
            ID = id;
            UserData = new byte[0];
        }

        public GameBoxHeaderInfo(GameBoxReader reader)
        {
            Read(reader);
        }

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

                if (ByteFormat == 'T') throw new NotSupportedException("Text-formatted GBX files are not supported.");

                RefTableCompression = (char)reader.ReadByte();
                Log.Write($"- Ref. table compression: {RefTableCompression}");

                BodyCompression = (char)reader.ReadByte();
                Log.Write($"- Body compression: {BodyCompression}");

                if (Version >= 4)
                {
                    UnknownByte = (char)reader.ReadByte();
                    Log.Write($"- Unknown byte: {UnknownByte}");
                }

                ID = CMwNod.Remap(reader.ReadUInt32());
                Log.Write($"- Class ID: 0x{ID:X8}");

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
