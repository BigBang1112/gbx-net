# CGameCtnBlock (0x03057000)

## Chunks

- [0x002](#0x002)

### 0x002

```cs
void Read(GameBoxReader r)
{
    Ident blockInfo = r.ReadIdent();
    Direction dir = (Direction)r.ReadByte();
    Byte3 coord = r.Byte3();
    int flags = r.Int32();
}
```

#### Enums

```cs
public enum Direction : byte
{
    North,
    East,
    South,
    West 
}
```