# CGameCtnCampaign (0x03090000)

## Chunks

- [0x000](#0x000)
- [0x006](#0x006)
- [0x009 - skippable](#0x009---skippable)
- [0x00B - skippable](#0x00B---skippable)
- [0x00C - skippable](#0x00C---skippable)
- [0x00D](#0x00D)
- [0x00E](#0x00E)
- [0x00F - skippable](#0x00F---skippable)
- [0x010](#0x010)
- [0x012 - skippable](#0x012---skippable)

### 0x000

```cs
void Read(GameBoxReader r)
{
	int version = r.ReadInt32();

	int numMapGroups = r.ReadInt32();
	for(var i = 0; i < numMapGroups; i++)
		CGameCtnChallengeGroup mapGroup = r.ReadNodeRef<CGameCtnChallengeGroup>();
}
```

### 0x006

```cs
void Read(GameBoxReader r)
{
	LookbackString campaignID = r.ReadLookbackString();
}
```

### 0x009 - skippable

```cs
void Read(GameBoxReader r)
{
	byte a = r.ReadByte();
	int b = r.ReadInt32();
	int c = r.ReadInt32();
}
```

### 0x00B - skippable

```cs
void Read(GameBoxReader r)
{
	int a = r.ReadInt32();
	int b = r.ReadInt32();
}
```

### 0x00C - skippable

```cs
void Read(GameBoxReader r)
{
	int a = r.ReadInt32();
}
```

### 0x00D

```cs
void Read(GameBoxReader r)
{
	int a = r.ReadInt32();
}
```

### 0x00E

```cs
void Read(GameBoxReader r)
{
	int a = r.ReadInt32();
}
```

### 0x00F - skippable

```cs
void Read(GameBoxReader r)
{
	string name = r.ReadString();
	int type = r.ReadInt32();
	int unlockType = r.ReadInt32();
}
```

### 0x010

```cs
void Read(GameBoxReader r)
{
	int a = r.ReadInt32();
	int b = r.ReadInt32();
}
```

### 0x012 - skippable

```cs
void Read(GameBoxReader r)
{
	int a = r.ReadInt32();
	int b = r.ReadInt32();
	byte c = r.ReadByte();
	int d = r.ReadInt32();
}
```