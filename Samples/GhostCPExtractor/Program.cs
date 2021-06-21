using GBX.NET;
using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GhostCPExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var fileName in args)
            {
                var node = GameBox.ParseNode(fileName);

                var ghosts = new List<CGameCtnGhost>();

                var isStuntsMap = false;

                if (node is CGameCtnReplayRecord replay)
                {
                    foreach (var g in replay.GetGhosts())
                    {
                        ghosts.Add(g);
                    }

                    if (replay.Challenge.Result.Mode == CGameCtnChallenge.PlayMode.Stunts)
                        isStuntsMap = true;
                }
                else if (node is CGameCtnGhost ghost)
                {
                    ghosts.Add(ghost);
                }
                else return;

                foreach (var ghost in ghosts)
                {
                    var ghostName = $"{ghost.RaceTime.ToStringTM()} by {Formatter.Deformat(ghost.GhostNickname)}";
                    Console.Write(ghostName);
                    Console.WriteLine($" ({Path.GetFileName(fileName)})");
                    Console.WriteLine(new string('-', ghostName.Length));

                    for (var i = 0; i < ghost.Checkpoints.Length - 1; i++)
                    {
                        var cp = ghost.Checkpoints[i];

                        if (isStuntsMap)
                            Console.WriteLine($"CP{i + 1}: {cp.Time.ToStringTM()} | {cp.StuntsScore}pts");
                        else
                            Console.WriteLine($"CP{i + 1}: {cp.Time.ToStringTM()}");
                    }

                    Console.WriteLine();
                }
            }

            Console.ReadKey(true);
        }
    }
}
