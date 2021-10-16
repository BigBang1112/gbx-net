# CGameCtnZoneGenealogy (0x0311D000)

## Chunks

- [0x001](#0x001)
- [0x002](#0x002)

### 0x001

```cs
void Read (GameBoxReader r)
{
    int currentIndex = r.ReadInt32();
    int dir = r.ReadInt32();
}
```

### 0x002

```cs
void Read (GameBoxReader r)
{
    int numZoneIds = r.ReadInt32();
    for (var i = 0; i < numZoneIds; i++)
        string zoneId = r.ReadLookbackString();
    int dir = r.ReadInt32();
    string currentZoneId = r.ReadLookbackString();
}
```
