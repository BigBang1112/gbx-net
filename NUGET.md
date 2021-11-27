**Since GBX.NET 0.11.0, the base of parsing has been simplified.**

To parse a GBX with a known type:

```cs
using GBX.NET;
using GBX.NET.Engines.Game;

var map = GameBox.ParseNode<CGameCtnChallenge>("MyMap.Map.Gbx");
```

To parse a GBX with an unknown type:

```cs
using GBX.NET;
using GBX.NET.Engines.Game;

var node = GameBox.ParseNode("MyMap.Map.Gbx");

if (node is CGameCtnChallenge map)
{
    // Node data is available in map
}
else if (node is CGameCtnReplayRecord replay)
{
    // Node data is available in replay
}

// C# 7+

switch (node)
{
    case CGameCtnChallenge map:
        // Node data is available in map
        break;
    case CGameCtnReplayRecord replay:
        // Node data is available in replay
        break;
}
```

To get GBX metadata or header chunks, use the `node.GBX` property.

To save changes of the parsed GBX file:

```cs
using GBX.NET;
using GBX.NET.Engines.Game;

var node = GameBox.ParseNode("MyMap.Map.Gbx");

if (node is CGameCtnChallenge map)
{
    // Do changes with CGameCtnChallenge

    map.Save("MyMap.Map.Gbx");
}
else if (node is CGameCtnGhost ghost)
{
    // Do changes with CGameCtnGhost

    ghost.Save("MyGhost.Ghost.Gbx");
}
```

To save any supported `Node` to a GBX file:

```cs
using GBX.NET;
using GBX.NET.Engines.Game;

var replay = GameBox.ParseNode<CGameCtnReplayRecord>("MyReplay.Replay.Gbx");

foreach (CGameCtnGhost ghost in replay.Ghosts)
{
    ghost.Save("MyExtractedGhost.Ghost.Gbx");
}
```