# CGameCtnMediaBlockInterface (0x03195000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x000](#0x000)

### 0x000

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    float start = r.ReadSingle();
    float end = r.ReadSingle();
    bool showInterface = r.ReadBoolean();
    string manialink = r.String();
}
```
