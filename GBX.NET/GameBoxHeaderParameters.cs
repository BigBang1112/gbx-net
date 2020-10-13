using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET
{
    public class GameBoxHeaderParameters
    {
        public GameBox GBX { get; }
        public byte[] UserData { get; set; }

        public GameBoxHeaderParameters(GameBox gbx)
        {
            GBX = gbx;
        }

        public bool Read(GameBoxReader r)
        {
            if (r.ReadString(3) == "GBX")
                Log.Write("GBX recognized!", ConsoleColor.Green);
            else
            {
                Log.Write("GBX magic missing! Corrupted file or not a GBX file.", ConsoleColor.Red);
                return false;
            }

            GBX.Version = r.ReadInt16();
            Log.Write($"- Version: {GBX.Version}");

            if (GBX.Version >= 3)
            {
                GBX.ByteFormat = (char)r.ReadByte();
                Log.Write($"- Byte format: {GBX.ByteFormat}");

                GBX.RefTableCompression = (char)r.ReadByte();
                Log.Write($"- Ref. table compression: {GBX.RefTableCompression}");

                GBX.BodyCompression = (char)r.ReadByte();
                Log.Write($"- Body compression: {GBX.RefTableCompression}");

                if (GBX.Version >= 4)
                {
                    GBX.UnknownByte = (char)r.ReadByte();
                    Log.Write($"- Unknown byte: {GBX.UnknownByte}");
                }

                GBX.ClassID = r.ReadUInt32();
                Log.Write($"- Class ID: 0x{GBX.ClassID:X8}");

                if (GBX.Version >= 6)
                {
                    var userDataSize = r.ReadInt32();
                    Log.Write($"- User data size: {userDataSize/1024f} kB");

                    if (userDataSize > 0)
                        UserData = r.ReadBytes(userDataSize);
                }

                GBX.NumNodes = r.ReadInt32();
                Log.Write($"- Number of nodes: {GBX.NumNodes}");
            }

            Log.Write("Header completed!", ConsoleColor.Green);

            return true;
        }
    }
}
