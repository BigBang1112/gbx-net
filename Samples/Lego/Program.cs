using GBX.NET;
using GBX.NET.Engines.Game;
using System;
using System.IO;

namespace Lego
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();

            foreach (var fileName in args)
            {
                var node = GameBox.ParseNode(fileName);

                if (node is CGameCtnChallenge ch)
                {
                    var colors = Enum.GetValues<DifficultyColor>();

                    foreach (var block in ch.Blocks)
                    {
                        var randomColor = colors[random.Next(colors.Length)];
                        block.Color = randomColor;
                    }

                    foreach (var item in ch.AnchoredObjects)
                    {
                        var randomColor = colors[random.Next(colors.Length)];
                        item.Color = randomColor;
                    }

                    ch.CreateChunk<CGameCtnChallenge.Chunk03043062>();

                    ch.Save(Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileName)) + "-LEGO.Map.Gbx");
                }
            }
        }
    }
}
