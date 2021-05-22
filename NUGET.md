To parse a GBX with a known type:

```cs
var gbx = GameBox.Parse<CGameCtnChallenge>("MyMap.Map.Gbx");
// Node data is available in gbx.MainNode
```

To parse a GBX with an unknown type (method 1):

```cs
var gbx = GameBox.Parse("MyMap.Map.Gbx");

if (gbx is GameBox<CGameCtnChallenge> gbxMap)
{
    var map = gbxMap.MainNode;
    
    // Node data is available in map
}
else if (gbx is GameBox<CGameCtnReplayRecord> gbxReplay)
{
    var replay = gbxReplay.MainNode;
    
    // Node data is available in replay
}
```

To parse a GBX with an unknown type (method 2):

```cs
var gbx = GameBox.Parse("MyMap.Map.Gbx");

if (gbx.TryNode(out CGameCtnChallenge map))
{
    // Node data is available in map
}
else if (gbx.TryNode(out CGameCtnReplayRecord replay))
{
    // Node data is available in replay
}
```

To save changes of the parsed GBX file:

```cs
var gbx = GameBox.Parse("MyMap.Map.Gbx");

if (gbx is GameBox<CGameCtnChallenge> gbxMap)
{
    // Do changes with CGameCtnChallenge

    gbxMap.Save("MyMap.Map.Gbx"); // Can be also a new file
}
else if (gbx is GameBox<CGameCtnGhost> gbxGhost)
{
    // Do changes with CGameCtnGhost

    gbxGhost.Save("MyGhost.Ghost.Gbx"); // Can be also a new file
}

gbx.Save(); // will throw an error
// GameBox with unspecified/unknown type can't be currently written back
```

To save any supported `Node` to a GBX file:

```cs
var gbxReplay = GameBox.Parse<CGameCtnReplayRecord>("MyReplay.Replay.Gbx");
CGameCtnReplayRecord replay = gbxReplay.MainNode;

foreach (CGameCtnGhost ghost in replay.Ghosts)
{
    var gbxGhost = new GameBox<CGameCtnGhost>(ghost); // Create a GameBox<T> with the Node object
    gbxGhost.Save("MyExtractedGhost.Ghost.Gbx"); // Save the new GameBox object to a GBX file
}
```