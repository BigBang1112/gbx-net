To parse a GBX with a known type:

```cs
var gbx = GameBox.Parse<CGameCtnChallenge>("MyMap.Map.Gbx");
// Node data is available in gbx.MainNode
```

To parse a GBX with an unknown type:

```cs
var gbx = GameBox.Parse("MyMap.Map.Gbx");

if (gbx is GameBox<CGameCtnChallenge> gbxMap)
{
    // Node data is available in gbxMap.MainNode
}
else if (gbx is GameBox<CGameCtnReplayRecord> gbxReplay)
{
    // Node data is available in gbxReplay.MainNode
}
```
