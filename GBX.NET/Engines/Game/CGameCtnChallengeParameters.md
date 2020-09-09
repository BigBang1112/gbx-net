# CGameCtnChallengeParameters (0x0305B000)

## Chunks

- [0x000](#0x000)
- [0x001](#0x001)
- [0x002](#0x002)
- [0x003](#0x003)
- [0x004](#0x004)
- [0x005](#0x005)
- [0x006](#0x006)
- [0x007](#0x007)
- [0x008 (stunts)](#0x008-stunts)
- [0x00A - skippable](#0x00A---skippable)
- [0x00D](#0x00D)
- [0x00E - skippable (map type)](#0x00E---skippable-map-type)

### 0x000

```cs
void Read(GameBoxReader r)
{
    int a = r.ReadInt32();
    int b = r.ReadInt32();
    int c = r.ReadInt32();
    int d = r.ReadInt32();

    int e = r.ReadInt32();
    int f = r.ReadInt32();
    int g = r.ReadInt32();

    int h = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int a | ~ | ~ | ~ | ~ | ~
| int b | ~ | ~ | ~ | ~ | ~
| int c | ~ | ~ | ~ | ~ | ~
| int d | ~ | ~ | ~ | ~ | ~
| int e | ~ | ~ | ~ | ~ | ~
| int f | ~ | ~ | ~ | ~ | ~
| int g | ~ | ~ | ~ | ~ | ~
| int h | ~ | ~ | ~ | ~ | ~

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
    int a = r.ReadInt32();
    int b = r.ReadInt32();
    int c = r.ReadInt32();

    float d = r.ReadSingle();
    float e = r.ReadSingle();
    float f = r.ReadSingle();

    int g = r.ReadInt32();
    int h = r.ReadInt32();
    int i = r.ReadInt32();
    int j = r.ReadInt32();
    int k = r.ReadInt32();
    int l = r.ReadInt32();
    int m = r.ReadInt32();
    int n = r.ReadInt32();
    int o = r.ReadInt32();
    int p = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int a | ~ | ~ | ~ | ~ | ~
| int b | ~ | ~ | ~ | ~ | ~
| int c | ~ | ~ | ~ | ~ | ~
| float d | ~ | ~ | ~ | ~ | ~
| float e | ~ | ~ | ~ | ~ | ~
| float f | ~ | ~ | ~ | ~ | ~
| int g | ~ | ~ | ~ | ~ | ~
| int h | ~ | ~ | ~ | ~ | ~
| int i | ~ | ~ | ~ | ~ | ~
| int j | ~ | ~ | ~ | ~ | ~
| int k | ~ | ~ | ~ | ~ | ~
| int l | ~ | ~ | ~ | ~ | ~
| int m | ~ | ~ | ~ | ~ | ~
| int n | ~ | ~ | ~ | ~ | ~
| int o | ~ | ~ | ~ | ~ | ~
| int p | ~ | ~ | ~ | ~ | ~

### 0x003

```cs
void Read(GameBoxReader r)
{
    int a = r.ReadInt32();
    float b = r.ReadSingle();

    int c = r.ReadInt32();
    int d = r.ReadInt32();
    int e = r.ReadInt32();

    int f = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| int a | ~ | ~ | ~ | ~ | ~
| float b | ~ | ~ | ~ | ~ | ~
| int c | ~ | ~ | ~ | ~ | ~
| int d | ~ | ~ | ~ | ~ | ~
| int e | ~ | ~ | ~ | ~ | ~
| int f | ~ | ~ | ~ | ~ | ~

### 0x004

```cs
void Read(GameBoxReader r)
{
    int bronzeTime = r.ReadInt32();
    int silverTime = r.ReadInt32();
    int goldTime = r.ReadInt32();
    int authorTime = r.ReadInt32();

    int a = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| int a | ~ | ~ | ~ | ~ | ~

### 0x005

```cs
void Read(GameBoxReader r)
{
    int a = r.ReadInt32();
    int b = r.ReadInt32();
    int c = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| int a | ~ | ~ | ~ | ~ | ~
| int b | ~ | ~ | ~ | ~ | ~
| int c | ~ | ~ | ~ | ~ | ~

### 0x006

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
    uint a = r.ReadUInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| uint a | ~ | ~ | ~ | ~ | ~

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
    int a = r.ReadInt32();

    int bronzeTime = r.ReadInt32();
    int silverTime = r.ReadInt32();
    int goldTime = r.ReadInt32();
    int authorTime = r.ReadInt32();
    int timeLimit = r.ReadInt32();
    int authorScore = r.ReadInt32();
}
```

### 0x00D

```cs
void Read(GameBoxReader r)
{
    int a = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| int a | ~ | ~ | ~ | ~ | ~

### 0x00E - skippable (map type)

```cs
void Read(GameBoxReader r)
{
    string mapType = r.ReadString();
    string mapStyle = r.ReadString();
    int a = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| int a | ~ | ~ | ~ | ~ | ~