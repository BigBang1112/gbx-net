using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GBX.NET;
using GBX.NET.Engines.Game;

namespace GhostToClip
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = args.FirstOrDefault();
            if (fileName == null) return;

            Log.OnLogEvent += Log_OnLogEvent;

            var node = GameBox.ParseNode(fileName);

            if (node is CGameCtnGhost ghost)
            {
                var clip = new CGameCtnMediaClip();
                clip.CreateChunk<CGameCtnMediaClip.Chunk0307900D>();

                var track = new CGameCtnMediaTrack()
                {
                    Name = ghost.GhostNickname
                };
                track.CreateChunk<CGameCtnMediaTrack.Chunk03078001>();
                track.CreateChunk<CGameCtnMediaTrack.Chunk03078005>();

                clip.Tracks.Add(track);

                var ghostBlock = new CGameCtnMediaBlockGhost()
                {
                    GhostModel = ghost,
                    Keys = new List<CGameCtnMediaBlockGhost.Key>
                    {
                        new CGameCtnMediaBlockGhost.Key(),
                        new CGameCtnMediaBlockGhost.Key()
                        {
                            Time = ghost.RaceTime.GetValueOrDefault(TimeSpan.FromSeconds(3))
                        }
                    }
                };

                var chunk002 = ghostBlock.CreateChunk<CGameCtnMediaBlockGhost.Chunk030E5002>();
                chunk002.Version = 3;

                track.Blocks.Add(ghostBlock);

                clip.Save(Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(node.GBX.FileName)) + ".Clip.Gbx");
            }
        }

        private static void Log_OnLogEvent(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
