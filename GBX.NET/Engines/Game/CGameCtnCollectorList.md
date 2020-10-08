# CGameCtnCollectorList (0x0301B000)

## Chunks

- [0x000](#0x000)

### 0x000

```cs
void Read(GameBoxReader r)
{
    int numCollectors = r.ReadInt32();

    for(var i = 0; i < num; i++)
        Meta collector = r.ReadMeta();
}
```