using GBX.NET;
using GBX.NET.Engines.Game;
using System;
using System.IO;

foreach (var fileName in args)
{
    var node = GameBox.ParseNode(fileName);

    if (node is CGameCtnChallenge map)
    {
        map.CrackPassword();

        var savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(fileName));
        map.Save(savePath);
    }
}