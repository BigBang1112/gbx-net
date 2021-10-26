# CGameCtnChallengeGroup (0x0308F000)

## Chunks

- [0x002 (default)](#0x002-default)
- [0x00B (map infos)](#0x00B-map-infos)

### 0x002 (default)

```cs
void Read(GameBoxReader r)
{
    string default = r.ReadString();
}
```

### 0x00B (map infos)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();

    int numMaps = r.ReadInt32();
    for(var i = 0; i < numMaps; i++)
    {
        Ident mapInfo = r.ReadIdent();
        string filePath = r.ReadString();
    }
}
```