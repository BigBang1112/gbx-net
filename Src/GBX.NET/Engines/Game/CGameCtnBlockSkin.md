# CGameCtnBlockSkin (0x03059000)

## Chunks

- [0x000](#0x000)
- [0x001](#0x001)
- [0x002](#0x002)
- [0x003 (TM®)](#0x003-tm®)

### 0x000

```cs
void Read(GameBoxReader r)
{
    string text = r.ReadString();
    string ignored = r.ReadString();
}
```

### 0x001

```cs
void Read(GameBoxReader r)
{
    string text = r.ReadString();
    FileRef packDesc = r.ReadFileRef();
}
```

### 0x002

```cs
void Read(GameBoxReader r)
{
    string text = r.ReadString();
    FileRef packDesc = r.ReadFileRef();
    FileRef parentPackDesc = r.ReadFileRef();
}
```

### 0x003 (TM®)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    FileRef secondaryPackDesc = r.ReadFileRef();
}
```