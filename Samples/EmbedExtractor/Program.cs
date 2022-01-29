using GBX.NET;
using GBX.NET.Engines.Game;
using System.IO;
using System.Linq;

var fileName = args.FirstOrDefault();
if (fileName is null) return;

var node = GameBox.ParseNode(fileName);

if (node is CGameCtnChallenge map)
{
    map.ExtractOriginalEmbedZip(Path.GetFileName(fileName) + ".zip");
}