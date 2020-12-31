# CGameCtnChallengeParameters (0x0305B000)

## Chunks

- [0x000](#0x000)
- [0x001](#0x001)
- [0x002](#0x002)
- [0x003](#0x003)
- [0x004](#0x004)
- [0x005](#0x005)
- [0x006 (items)](#0x006-items)
- [0x007](#0x007)
- [0x008 (stunts)](#0x008-stunts)
- [0x00A - skippable](#0x00A---skippable)
- [0x00D (race validate ghost)](#0x00D-race-validate-ghost)
- [0x00E - skippable (map type)](#0x00E---skippable-map-type)

### 0x000

```cs
void Read(GameBoxReader r)
{
    int u01 = r.ReadInt32();
    int u02 = r.ReadInt32();
    int u03 = r.ReadInt32();
    int u04 = r.ReadInt32();

    int u05 = r.ReadInt32();
    int u06 = r.ReadInt32();
    int u07 = r.ReadInt32();

    int u08 = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int u01 | ~ | ~ | ~ | ~ | ~
| int u02 | ~ | ~ | ~ | ~ | ~
| int u03 | ~ | ~ | ~ | ~ | ~
| int u04 | ~ | ~ | ~ | ~ | ~
| int u05 | ~ | ~ | ~ | ~ | ~
| int u06 | ~ | ~ | ~ | ~ | ~
| int u07 | ~ | ~ | ~ | ~ | ~
| int u08 | ~ | ~ | ~ | ~ | ~

### 0x001

```cs
void Read(GameBoxReader r)
{
    string tip1 = r.ReadString();
    string tip2 = r.ReadString();
    string tip3 = r.ReadString();
    string tip4 = r.ReadString();
}
```

### 0x002

```cs
void Read(GameBoxReader r)
{
    int u01 = r.ReadInt32();
    int u02 = r.ReadInt32();
    int u03 = r.ReadInt32();

    float u04 = r.ReadSingle();
    float u05 = r.ReadSingle();
    float u06 = r.ReadSingle();

    int u07 = r.ReadInt32();
    int u08 = r.ReadInt32();
    int u09 = r.ReadInt32();
    int u10 = r.ReadInt32();
    int u11 = r.ReadInt32();
    int u12 = r.ReadInt32();
    int u13 = r.ReadInt32();
    int u14 = r.ReadInt32();
    int u15 = r.ReadInt32();
    int u16 = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int u01 | ~ | ~ | ~ | ~ | ~
| int u02 | ~ | ~ | ~ | ~ | ~
| int u03 | ~ | ~ | ~ | ~ | ~
| float u04 | ~ | ~ | ~ | ~ | ~
| float u05 | ~ | ~ | ~ | ~ | ~
| float u06 | ~ | ~ | ~ | ~ | ~
| int u07 | ~ | ~ | ~ | ~ | ~
| int u08 | ~ | ~ | ~ | ~ | ~
| int u09 | ~ | ~ | ~ | ~ | ~
| int u10 | ~ | ~ | ~ | ~ | ~
| int u11 | ~ | ~ | ~ | ~ | ~
| int u12 | ~ | ~ | ~ | ~ | ~
| int u13 | ~ | ~ | ~ | ~ | ~
| int u14 | ~ | ~ | ~ | ~ | ~
| int u15 | ~ | ~ | ~ | ~ | ~
| int u16 | ~ | ~ | ~ | ~ | ~

### 0x003

```cs
void Read(GameBoxReader r)
{
    int u01 = r.ReadInt32();
    float u02 = r.ReadSingle();

    int u03 = r.ReadInt32();
    int u04 = r.ReadInt32();
    int u05 = r.ReadInt32();

    int u06 = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| int u01 | ~ | ~ | ~ | ~ | ~
| float u02 | ~ | ~ | ~ | ~ | ~
| int u03 | ~ | ~ | ~ | ~ | ~
| int u04 | ~ | ~ | ~ | ~ | ~
| int u05 | ~ | ~ | ~ | ~ | ~
| int u06 | ~ | ~ | ~ | ~ | ~

### 0x004

```cs
void Read(GameBoxReader r)
{
    int bronzeTime = r.ReadInt32();
    int silverTime = r.ReadInt32();
    int goldTime = r.ReadInt32();
    int authorTime = r.ReadInt32();

    int u01 = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| int u01 | ~ | ~ | ~ | ~ | ~

### 0x005

```cs
void Read(GameBoxReader r)
{
    int u01 = r.ReadInt32();
    int u02 = r.ReadInt32();
    int u03 = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| int u01 | ~ | ~ | ~ | ~ | ~
| int u02 | ~ | ~ | ~ | ~ | ~
| int u03 | ~ | ~ | ~ | ~ | ~

### 0x006 (items)

This chunk causes "Couldn't load map" in ManiaPlanet.

```cs
void Read(GameBoxReader r)
{
    int itemCount = r.ReadInt32();
    for(var i = 0; i < itemCount; i++)
        int item = r.ReadUInt32();
}
```

### 0x007

```cs
void Read(GameBoxReader r)
{
    uint u01 = r.ReadUInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| uint u01 | ~ | ~ | ~ | ~ | ~

### 0x008 (stunts)

```cs
void Read(GameBoxReader r)
{
    int timeLimit = r.ReadInt32();
    int authorScore = r.ReadInt32();
}
```

### 0x00A - skippable

```cs
void Read(GameBoxReader r)
{
    int u01 = r.ReadInt32();

    int bronzeTime = r.ReadInt32();
    int silverTime = r.ReadInt32();
    int goldTime = r.ReadInt32();
    int authorTime = r.ReadInt32();
    int timeLimit = r.ReadInt32();
    int authorScore = r.ReadInt32();
}
```

### 0x00D (race validate ghost)

```cs
void Read(GameBoxReader r)
{
    CGameCtnGhost raceValidateGhost = r.ReadNodeRef<CGameCtnGhost>();
}
```

### 0x00E - skippable (map type)

```cs
void Read(GameBoxReader r)
{
    string mapType = r.ReadString();
    string mapStyle = r.ReadString();
    int u01 = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| int u01 | ~ | ~ | ~ | ~ | ~