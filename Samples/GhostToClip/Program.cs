using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GBX.NET;
using GBX.NET.Engines.Game;
using Microsoft.Extensions.Logging;

var fileName = args.FirstOrDefault();

if (fileName is null)
{
    return;
}

var logger = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = true;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    });
    builder.SetMinimumLevel(LogLevel.Debug);
}).CreateLogger<Program>();

var node = GameBox.ParseNode(fileName);

if (node is not CGameCtnGhost ghost)
{
    return;
}

var ghostMediaBlock = CGameCtnMediaBlockGhost.Create(ghost)
    .ForTMUF()
    .EndingAt(ghost.RaceTime.GetValueOrDefault(TimeSpan.FromSeconds(3)) + TimeSpan.FromSeconds(3))
    .Build();

var track = CGameCtnMediaTrack.Create()
    .WithName(ghost.GhostNickname ?? "Unnamed")
    .WithBlocks(ghostMediaBlock)
    .ForTMUF()
    .Build();

var clip = CGameCtnMediaClip.Create()
    .WithTracks(track)
    .ForTMUF()
    .Build();

clip.Save(Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(fileName)) + ".Clip.Gbx");