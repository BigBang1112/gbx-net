using GBX.NET;
using GBX.NET.Engines.Game;
using System;
using System.Linq;

namespace EmbedExtractor;

class Program
{
    static void Main(string[] args)
    {
        var fileName = args.FirstOrDefault();
        if (fileName == null) return;

        var node = GameBox.ParseNode(fileName);

        if (node is CGameCtnChallenge map)
            map.ExtractOriginalEmbedZip(map.GBX.FileName + ".zip");
    }
}
