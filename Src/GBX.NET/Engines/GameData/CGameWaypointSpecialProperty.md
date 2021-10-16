# CGameWaypointSpecialProperty (0x2E009000)

## Chunks

- [0x000](#0x000)
- [0x001 - skippable](#0x001---skippable)

### 0x000

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    if (version == 1)
    {
        int spawn = r.ReadInt32();
        int order = r.ReadInt32();
    }
    else if (version == 2)
    {
        string tag = r.ReadString();
        int order = r.ReadInt32();
    }
}
```

### 0x001 - skippable

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();
    int u02 = r.ReadInt32();
}
```
