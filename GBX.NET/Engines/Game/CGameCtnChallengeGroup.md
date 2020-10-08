# CGameCtnChallengeGroup (0x0308F000)

## Chunks

- [0x002](#0x002)
- [0x00B](#0x00B)

### 0x002

```cs
void Read(GameBoxReader r)
{
    string default = r.ReadString();
}
```

### 0x00B

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();

    int numMaps = r.ReadInt32();
    for(var i = 0; i < numMaps; i++)
    {
        Meta mapInfo = r.ReadMeta();
        string filePath = r.ReadString();
    }
}
```