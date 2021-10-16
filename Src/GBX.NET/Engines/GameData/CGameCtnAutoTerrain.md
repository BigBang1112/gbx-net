# CGameCtnAutoTerrain (0x03120000)

## Chunks

- [0x001](#0x001)

### 0x001

```cs
void Read (GameBoxReader r)
{
    Int3 offset = r.ReadInt3();
    CGameCtnZoneGenealogy genealogy = r.ReadNodeRef<CGameCtnZoneGenealogy>();
}
```
