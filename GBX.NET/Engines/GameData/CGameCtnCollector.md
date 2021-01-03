# CGameCtnCollector (0x2E001000)

## Chunks

- [0x003 - header chunk](#0x003---header-chunk)
- [0x004 - header chunk](#0x004---header-chunk)
- [0x006 - header chunk](#0x006---header-chunk)
- [0x006](#0x006)
- [0x007](#0x007)
- [0x008](#0x008)
- [0x009](#0x009)
- [0x00A](#0x00a)
- [0x00B](#0x00b)
- [0x00C](#0x00c)
- [0x00D](#0x00d)
- [0x00E](#0x00e)
- [0x010](#0x010)
- [0x011](#0x011)

### 0x003 - header chunk

```cs
void Read (GameBoxReader r)
{
    Meta meta = r.ReadMeta();
    int version = r.ReadInt32();
    string pageName = r.ReadString();

    if (version == 5)
        int u01 = r.ReadInt32();
    if (Version >= 4)
        int u02 = r.ReadInt32();

    if (version >= 3)
    {
        int u03 = r.ReadInt32();
        short catalogPosition = r.ReadInt16();
    }

    if (version >= 7)
        string name = r.ReadString();
}
```

### 0x004 - header chunk

Every collector icon (item, custom or official block, macroblock) is 64x64 size RGBA uncompressed and always takes 16kB of the GBX binary.

```cs
void Read (GameBoxReader r)
{
    int width = r.ReadInt16();
    int height = r.ReadInt16();

    byte[] iconData = r.ReadBytes(width * height * 4);
}
```

### 0x006 - header chunk

```cs
void Read (GameBoxReader r)
{
    long fileTime = r.ReadInt64();
}
```

### 0x006

```cs
void Read (GameBoxReader r)
{
    int u01 = r.ReadInt32();
}
```

### 0x007

```cs
void Read (GameBoxReader r)
{
    int u01 = r.ReadInt32();
    int u02 = r.ReadInt32();
    int u03 = r.ReadInt32();
    int u04 = r.ReadInt32();
    int u05 = r.ReadInt32();
    int u06 = r.ReadInt32();
}
```

### 0x008

```cs
void Read (GameBoxReader r)
{
    byte u01 = r.ReadByte(); // 0
    string skinFile = r.ReadString();
}
```

### 0x009

```cs
void Read (GameBoxReader r)
{
    string pageName = r.ReadString();

    bool hasIconFid = r.ReadBoolean();
    if (hasIconFid)
        Node iconFid = r.ReadNodeRef();

    string u01 = r.ReadLookbackString();
}
```

### 0x00A

```cs
void Read (GameBoxReader r)
{
    Node u01 = r.ReadNodeRef();
}
```

### 0x00B

```cs
void Read (GameBoxReader r)
{
    Meta meta = r.ReadMeta();
}
```

### 0x00C

```cs
void Read (GameBoxReader r)
{
    string collectorName = r.ReadString();
}
```

### 0x00D

```cs
void Read (GameBoxReader r)
{
    string description = r.ReadString();
}
```

### 0x00E

```cs
void Read (GameBoxReader r)
{
    bool iconUseAutoRender = r.ReadBoolean();
    int iconQuarterRotationY = r.ReadInt32();
}
```

### 0x010

```cs
void Read (GameBoxReader r)
{
    int u01 = r.ReadInt32(); // 2
    int u02 = r.ReadInt32(); // -1
    int u03 = r.ReadInt32(); // 0
    int u04 = r.ReadInt32(); // -1
}
```

### 0x011

```cs
void Read (GameBoxReader r)
{
    byte u01 = r.ReadByte();
    int u02 = r.ReadInt32();
    int u03 = r.ReadInt32();
    int u04 = r.ReadInt32();
    int u05 = r.ReadInt32();
}
```
