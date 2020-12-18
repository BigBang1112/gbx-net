# CGameCtnMediaClip (0x03079000)

## Chunks

- [0x003](#0x003)
- [0x004](#0x004)
- [0x005](#0x005)
- [0x007](#0x007)
- [0x008](#0x008)
- [0x009](#0x009)
- [0x00D](#0x00d)

### 0x003

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    int numTracks = r.ReadInt32();
    for (var i = 0; i < numTracks; i++)
        CGameCtnMediaTrack track = r.ReadNodeRef<CGameCtnMediaTrack>();

	string name = r.ReadString();
}
```

### 0x004

```cs
void Read (GameBoxReader r)
{
    object u01 = r.ReadNodeRef();
}
```

### 0x005

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();

    int numTracks = r.ReadInt32();
    for (var i = 0; i < numTracks; i++)
        CGameCtnMediaTrack track = r.ReadNodeRef<CGameCtnMediaTrack>();

	string name = r.ReadString();
}
```

### 0x007

```cs
void Read (GameBoxReader r)
{
    int u01 = r.ReadInt32();
}
```

### 0x008

```cs
void Read (GameBoxReader r)
{
    float u01 = r.ReadSingle();
}
```

### 0x009

```cs
void Read (GameBoxReader r)
{
    int u01 = r.ReadInt32();
}
```

### 0x00D

```cs
void Read (GameBoxReader r)
{
    int u01 = r.ReadInt32();
	int version = r.ReadInt32();

    int numTracks = r.ReadInt32();
    for (var i = 0; i < numTracks; i++)
        CGameCtnMediaTrack track = r.ReadNodeRef<CGameCtnMediaTrack>();

	string name = r.ReadString();

	int u02 = r.ReadInt32();
	int u03 = r.ReadInt32();
	int u04 = r.ReadInt32();
	int u05 = r.ReadInt32();
	float u06 = r.ReadSingle();
	int u07 = r.ReadInt32(); // -1
}
```
