# CGameCtnMediaBlockManialink (0x0312A000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x001](#0x001)

### 0x001

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    float start = r.ReadSingle();
    float end = r.ReadSingle();
    string manialinkURL = r.ReadString();
}
```
