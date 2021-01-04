# CGameCtnChallenge (0x03043000 / 0x24003000)

The class behind every single map made in Trackmania.

## Chunks

- [0x001 (Virtual Skipper)](#0x001-virtual-skipper)
- [0x002 (map info)](#0x002-map-info)
- [0x003 (common)](#0x003-common)
- [0x004 (version)](#0x004-version)
- [0x005 (XML)](#0x005-xml)
- [0x007 (thumbnail)](#0x007-thumbnail)
- [0x008 (author)](#0x008-author)
- [0x00D (vehicle)](#0x00D-vehicle)
- [0x00F (TM1.0 block data)](#0x00F-tm1.0-block-data)
- [0x011 (parameters)](#0x011-parameters)
- [0x012 (TM1.0 map name)](#0x012-tm1.0-map-name)
- [0x013 (legacy block data)](#0x013-legacy-block-data)
- [0x014 - skippable (legacy password)](#0x014---skippable-legacy-password)
- [0x016 - skippable](#0x016---skippable)
- [0x017 - skippable (checkpoints)](#0x017---skippable-checkpoints)
- [0x019 - skippable (mod)](#0x019---skippable-mod)
- [0x01C - skippable (play mode)](#0x01C---skippable-play-mode)
- [0x01F (block data)](#0x01F-block-data)
- [0x021 (legacy mediatracker)](#0x021-legacy-mediatracker)
- [0x022](#0x022)
- [0x023](#0x023)
- [0x024 (music)](#0x024-music)
- [0x025](#0x025)
- [0x026](#0x026)
- [0x027](#0x027)
- [0x028 (comments)](#0x028-comments)
- [0x029 - skippable (password)](#0x029---skippable-password)
- [0x02A](#0x02A)
- 0x034 - skippable
- [0x036 - skippable (realtime thumbnail)](#0x036---skippable-realtime-thumbnail)
- 0x038 - skippable
- [0x03D - skippable (lightmaps)](#0x03D---skippable-lightmaps)
- 0x03E - skippable
- [0x040 - skippable (items)](#0x040---skippable-items)
- [0x042 - skippable (author)](#0x042---skippable-author)
- 0x043 - skippable
- [0x044 - skippable (metadata)](#0x044---skippable-metadata)
- [0x048 - skippable (baked blocks)](#0x048---skippable-baked-blocks)
- [0x049 (mediatracker)](#0x049-mediatracker)
- [0x04B - skippable (objectives)](#0x04B---skippable-objectives)
- 0x050 - skippable
- [0x051 - skippable (title info)](#0x051---skippable-title-info)
- 0x052 - skippable
- 0x053 - skippable
- 0x054 - skippable
- 0x055 - skippable
- 0x056 - skippable
- 0x057 - skippable
- 0x058 - skippable
- [0x059 - skippable](#0x059---skippable)
- 0x05A - skippable [TM2020]
- [0x05F - skippable (free blocks) [TM2020]](#0x05F---skippable-free-blocks-tm2020)

### 0x001 (Virtual Skipper)

```cs
void Read(GameBoxReader r)
{
    byte version = r.ReadByte();

    if (version < 1)
    {
        Meta mapInfo = r.ReadMeta();
        string mapName = r.ReadString();
    }

    bool u01 = r.ReadBoolean();
    int u02 = r.ReadInt32();

    if (version < 1)
        byte u03 = r.ReadByte();

    byte u04 = r.ReadByte();

    if (version < 9)
        BoatName boatName = (BoatName)r.ReadByte();

    if (version >= 9)
        LookbackString boat = r.ReadLookbackString();

    if (version >= 12)
        LookbackString boatAuthor = r.ReadLookbackString();

    RaceMode raceMode = (RaceMode)r.ReadByte();
    byte u05 = r.ReadByte();
    WindDirection windDirection = (WindDirection)r.ReadByte();
    byte windStrength = r.ReadByte();
    Weather weather = (Weather)r.ReadByte();
    byte u06 = r.ReadByte();
    StartDelay startDelay = (StartDelay)r.ReadByte();
    int startTime = r.ReadInt32();

    if (version >= 2)
    {
        int timeLimit = r.ReadInt32();
        bool noPenalty = r.ReadBoolean();
        bool inflPenalty = r.ReadBoolean();
        bool finishFirst = r.ReadBoolean();

        if (version >= 3)
        {
            byte nbAIs = r.ReadByte();

            if (version >= 4)
            {
                float courseLength = r.ReadSingle();

                if (version >= 5)
                {
                    int windShiftAngle = r.ReadInt32();
                    byte u07 = r.ReadByte();

                    if (version == 6 || version == 7)
                    {
                        bool u08 = r.ReadBoolean();
                        string u09 = r.ReadString();
                    }

                    if (version >= 7)
                    {
                        bool exactWind = !r.ReadBoolean(); // an exact wind is inverted in chunk representation

                        if (version >= 10)
                        {
                            int spawnPoints = r.ReadInt32();

                            if (version >= 11)
                            {
                                AILevel aILevel = (AILevel)r.ReadByte();

                                if (version >= 13)
                                {
                                    bool smallShifts = r.ReadBoolean();

                                    if (version >= 14)
                                    {
                                        bool noRules = r.ReadBoolean();
                                        bool startSailUp = r.ReadBoolean();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
```

#### Enums

```cs
public enum BoatName : byte
{
    Acc,
    Multi,
    Melges,
    OffShore
}

public enum RaceMode : byte
{
    FleetRace,
    MatchRace,
    TeamRace
}

public enum WindDirection : byte
{
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest
}

public enum Weather : byte
{
    Sunny,
    Cloudy,
    Rainy,
    Stormy
}

public enum StartDelay : byte
{
    Immediate,
    OneMin,
    TwoMin,
    FiveMin,
    EightMin
}

public enum AILevel : byte
{
    Easy,
    Intermediate,
    Expert,
    Pro
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| bool u01 | ~ | ~ | ~ | ~ | ~
| int u02 | ~ | ~ | ~ | ~ | ~
| byte u03 | ~ | ~ | ~ | ~ | ~
| byte u04 | ~ | ~ | ~ | ~ | ~
| byte u05 | ~ | ~ | ~ | ~ | ~
| byte u06 | ~ | ~ | ~ | ~ | ~
| byte u07 | ~ | ~ | ~ | ~ | ~
| bool u08 | ~ | ~ | ~ | ~ | ~
| string u09 | ~ | ~ | ~ | ~ | ~

### 0x002 (map info)

```cs
void Read(GameBoxReader r)
{
    byte version = r.ReadByte();

    if (version < 3)
    {
        Meta mapInfo = r.ReadMeta();
        string mapName = r.ReadString();
    }

    int u01 = r.ReadInt32();

    if (version >= 1)
    {
        int bronzeTime = r.ReadInt32();
        int silverTime = r.ReadInt32();
        int goldTime = r.ReadInt32();
        int authorTime = r.ReadInt32();

        if (version == 2)
            byte u02 = r.ReadByte();

        if (version >= 4)
        {
            int cost = r.ReadInt32();

            if (version >= 5)
            {
                bool multilap = r.ReadBoolean();

                if (version == 6)
                    int u03 = r.ReadInt32();

                if (version >= 7)
                {
                    int trackType = r.ReadInt32();

                    if (version >= 9)
                    {
                        int u04 = r.ReadInt32();

                        if (version >= 10)
                        {
                            int authorScore = r.ReadInt32();

                            if (version >= 11)
                            {
                                int editorMode = r.ReadInt32(); // bit 0: advanced/simple editor, bit 1: has ghost blocks

                                if (version >= 12)
                                {
                                    int u05 = r.ReadInt32();

                                    if (version >= 13)
                                    {
                                        int nbCheckpoints = r.ReadInt32();
                                        int nbLaps = r.ReadInt32();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| int u01 | ~ | ~ | ~ | ~ | ~
| byte u02 | ~ | ~ | ~ | ~ | ~
| int u03 | ~ | ~ | ~ | ~ | ~
| int u04 | ~ | ~ | ~ | ~ | ~
| int u05 | ~ | ~ | ~ | ~ | ~

### 0x003 (common)

```cs
void Read(GameBoxReader r)
{
    byte version = r.ReadByte();
    Meta mapInfo = r.ReadMeta();
    string mapName = r.ReadString();
    TrackKind kind = (TrackKind)r.ReadByte();

    if (version >= 1)
    {
        uint locked = r.ReadUInt32(); // Gives a big integer sometimes, can't confirm to be always boolean
        string password = r.ReadString();

        if (version >= 2)
        {
            Meta decoration = r.ReadMeta();

            if (version >= 3)
            {
                Vec2 mapOrigin = r.ReadVec2();

                if (version >= 4)
                {
                    Vec2 mapTarget = r.ReadVec2();

                    if (version >= 5)
                    {
                        byte[] u01 = r.ReadBytes(16);

                        if (version >= 6)
                        {
                            string mapType = r.ReadString();
                            string mapStyle = r.ReadString();

                            if (version <= 8)
                                bool u02 = r.ReadBoolean();
                            
                            if (version >= 8)
                            {
                                ulong lightmapCacheUID = r.ReadUInt64();

                                if (version >= 9)
                                {
                                    byte lightmapVersion = r.ReadByte();

                                    if (version >= 11)
                                        LookbackString titleUID = r.ReadLookbackString();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
```

#### Enums

```cs
public enum TrackKind : byte
{
    EndMarker,
    Campaign,
    Puzzle,
    Retro,
    TimeAttack,
    Rounds,
    InProgress,
    Campaign_7,
    Multi,
    Solo,
    Site,
    SoloNadeo,
    MultiNadeo
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| byte[] u01 | ~ | ~ | ~ | ~ | ~
| bool u02 | ~ | ~ | ~ | ~ | ~

### 0x004 (version)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
}
```

### 0x005 (XML)

```cs
void Read(GameBoxReader r)
{
    string xml = r.ReadString();
}

```

### 0x007 (thumbnail)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();

    if(version != 0)
    {
        int thumbnailSize = r.ReadInt32();
        r.ReadBytes("<Thumbnail.jpg>".Length);
        byte[] thumbnailJpeg = r.ReadBytes(thumbnailSize);
        r.ReadBytes("</Thumbnail.jpg>".Length);
        r.ReadBytes("<Comments.jpg>".Length);
        string comments = r.ReadString();
        r.ReadBytes("</Comments.jpg>".Length);
    }
}
```

### 0x008 (author)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    int authorVersion = r.ReadInt32();
    string authorLogin = r.ReadString();
    string authorNick = r.ReadString();
    string authorZone = r.ReadString();
    string authorExtraInfo = r.ReadString();
}
```

### 0x00D (vehicle)

```cs
void Read(GameBoxReader r)
{
    Meta vehicle = r.ReadMeta();
}
```

### 0x00F (TM1.0 block data)

```cs
void Read(GameBoxReader r)
{
    Meta mapInfo = r.ReadMeta();
    Int3 size = r.ReadInt3();
    int u01 = r.ReadInt32();

    int numBlocks = r.ReadInt32();
    for (var i = 0; i < numBlocks; i++)
        CGameCtnBlock block = r.ReadNodeRef<CGameCtnBlock>();

    int u02 = r.ReadInt32();
    int u03 = r.ReadMeta();
}
```

### 0x011 (parameters)

```cs
void Read(GameBoxReader r)
{
    CGameCtnCollectorList collectorList = r.ReadNodeRef<CGameCtnCollectorList>();
    CGameCtnChallengeParameters challengeParameters = r.ReadNodeRef<CGameCtnChallengeParameters>();
    TrackKind kind = (TrackKind)r.ReadInt32();
}
```

#### Enums

```cs
public enum TrackKind : int
{
    EndMarker,
    Campaign,
    Puzzle,
    Retro,
    TimeAttack,
    Rounds,
    InProgress,
    Campaign_7,
    Multi,
    Solo,
    Site,
    SoloNadeo,
    MultiNadeo
}
```

### 0x012 (TM1.0 map name)

```cs
void Read(GameBoxReader r)
{
    string mapName = r.ReadString();
}
```

### 0x013 (legacy block data)

```cs
void Read(GameBoxReader r)
{
    Chunk0x01F.Read(r);
}
```

### 0x014 - skippable (legacy password)

```cs
void Read(GameBoxReader r)
{
    int u01 = r.ReadInt32();
    string password = r.ReadString();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int u01 | ~ | ~ | ~ | ~ | ~

### 0x016 - skippable

```cs
void Read(GameBoxReader r)
{
    int u01 = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | ---
| int u01 | ~ | ~ | ~ | ~ | ~

### 0x017 - skippable (checkpoints)

Checkpoint positions in coords. Available up to TMUF.

```cs
void Read(GameBoxReader r)
{
    int numCheckpoints = r.ReadInt32();
    for(var i = 0; i < numCheckpoints; i++)
        Int3 checkpointCoord = r.ReadInt3();
}
```

### 0x019 - skippable (mod)

```cs
void Read(GameBoxReader r)
{
    FileRef modPackDesc = r.ReadFileRef();
}
```

### 0x01C - skippable (play mode)

```cs
void Read(GameBoxReader r)
{
    PlayMode playMode = (PlayMode)r.ReadInt32();
}
```

#### Enums

```cs
public enum PlayMode : int
{
    Race,
    Platform,
    Puzzle,
    Crazy,
    Shortcut,
    Stunts
}
```

### 0x01F (block data)

```cs
void Read(GameBoxReader r)
{
    Meta mapInfo = r.ReadMeta();
    string mapName = r.ReadString();
    Meta decoration = r.ReadMeta();
    Int3 size = r.ReadInt3();
    bool needUnlock = r.ReadBoolean();

    if ((chunk.ID & 0xFFF) != 0x013) // If this chunk is not 0x013
        int version = r.ReadInt32();

    int numBlocks = r.ReadInt32(); // Amount of blocks that aren't flag -1

    while ((r.PeekUInt32() & 0xC0000000) > 0)
    {
        LookbackString blockName = r.ReadLookbackString();
        Direction dir = (Direction)r.ReadByte();
        Byte3 coord = r.ReadByte3();
        int flags = 0;

        if (version == null)
            flags = r.ReadInt16();
        else if (version > 0)
            flags = r.ReadInt32();
        
        if (flags == -1)
        {
            i--;
            continue;
        }

        if ((flags & 0x8000) != 0) // custom block
        {
            LookbackString author = r.ReadLookbackString();
            CGameCtnBlockSkin skin = r.ReadNodeRef<CGameCtnBlockSkin>();
        }

        if ((flags & 0x100000) != 0)
            CGameWaypointSpecialProperty parameters = r.ReadNodeRef<CGameWaypointSpecialProperty>();
    }
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

### 0x021 (legacy mediatracker)

```cs
void Read(GameBoxReader r)
{
    CGameCtnMediaClip clipIntro = r.ReadNodeRef<CGameCtnMediaClip>();
    CGameCtnMediaClipGroup clipGroupInGame = r.ReadNodeRef<CGameCtnMediaClipGroup>();
    CGameCtnMediaClipGroup clipGroupEndRace = r.ReadNodeRef<CGameCtnMediaClipGroup>();
}
```

### 0x022

```cs
void Read(GameBoxReader r)
{
    bool u01 = r.ReadBoolean();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| bool u01 | ~ | ~ | ~ | ~ | ~

### 0x023

```cs
void Read(GameBoxReader r)
{
    int u01 = r.ReadInt32();
    int u02 = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int u01 | ~ | ~ | ~ | ~ | ~
| int u01 | ~ | ~ | ~ | ~ | ~

### 0x024 (music)

```cs
void Read(GameBoxReader r)
{
    FileRef customMusicPackDesc = r.ReadFileRef();
}
```

### 0x025

```cs
void Read(GameBoxReader r)
{
    Vec2 mapCoordOrigin = r.ReadVec2();
    Vec2 mapCoordTarget = r.ReadVec2();
}
```

### 0x026

```cs
void Read(GameBoxReader r)
{
    NodeRef clipGlobal = r.ReadNodeRef();
}
```

### 0x027

```cs
void Read(GameBoxReader r)
{
    bool archiveGmCamVal = r.ReadBoolean();

    if (archiveGmCamVal)
    {
        byte u01 = r.ReadByte();
        Vec3 u02 = r.ReadVec3();
        Vec3 u03 = r.ReadVec3();
        Vec3 u04 = r.ReadVec3();
        Vec3 u05 = r.ReadVec3();
        float u06 = r.ReadSingle();
        float u07 = r.ReadSingle();
        float u08 = r.ReadSingle();
    }
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| byte u01 | ~ | ~ | ~ | ~ | ~
| Vec3 u02 | ~ | ~ | ~ | ~ | ~
| Vec3 u03 | ~ | ~ | ~ | ~ | ~
| Vec3 u04 | ~ | ~ | ~ | ~ | ~
| Vec3 u05 | ~ | ~ | ~ | ~ | ~
| float u06 | ~ | ~ | ~ | ~ | ~
| float u07 | ~ | ~ | ~ | ~ | ~
| float u08 | ~ | ~ | ~ | ~ | ~

### 0x028 (comments)

```cs
void Read(GameBoxReader r)
{
    Chunk0x027.Read(r);
    string comments = r.ReadString();
}
```

### 0x029 - skippable (password)

If you want to remove a password from a map, you can just remove this chunk.

```cs
void Read(GameBoxReader r)
{
    byte[] passwordHashMD5 = r.ReadBytes(16);
    uint crc32 = r.ReadUInt32();
}
```

### 0x02A

```cs
void Read(GameBoxReader r)
{
    bool u01 = r.ReadBoolean();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| bool u01 | ~ | ~ | ~ | ~ | ~

### 0x034 - skippable

Undiscovered.

### 0x036 - skippable (realtime thumbnail)

```cs
void Read(GameBoxReader r)
{
    Vec3 thumbnailPosition = r.ReadVec3();
    Vec3 thumbnailPitchYawRoll = r.ReadVec3(); // in radians
    float thumbnailFOV = r.ReadSingle();

    // + 31 more unknown bytes
}
```

### 0x038 - skippable

Undiscovered.

### 0x03D - skippable (lightmaps)

```cs
void Read(GameBoxReader r)
{
    bool u01 = reader.ReadBoolean(); // maybe if shadows are calculated
    int version = reader.ReadInt();

    int frames = 1; // Default value if version is below 5
    if (version >= 5)
        int frames = reader.ReadInt(); // Read normally

    if (version >= 2)
    {
        int size = 0;

        for (var i = 0; i < frames; i++)
        {
            int size = r.ReadInt32();
            byte[] image = r.ReadBytes(size);

            if (version >= 3)
            {
                int size = r.ReadInt32();
                byte[] image = r.ReadBytes(size);
            }

            if (version >= 6)
            {
                int size = r.ReadInt32();
                byte[] image = r.ReadBytes(size);
            }
        }

        if (size != 0)
        {
            int uncompressedSize = r.ReadInt32();
            int compressedSize = r.ReadInt32();
            byte[] lightmapCacheData = r.ReadBytes(compressedSize); // ZLIB compressed data

            using (var ms = new MemoryStream(data))
            using (var zlib = new InflaterInputStream(ms))
            using (var gbxr = new GameBoxReader(zlib))
                CHmsLightMapCache lightmapCache = Parse(gbxr);
        }
    }
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| bool u01 | ~ | ~ | ~ | ~ | ~

### 0x03E - skippable

Undiscovered.

### 0x040 - skippable (items)

**Note: This chunk has it's own lookback.**

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();

    if (Version != 0)
    {
        int a = r.ReadInt32();
        int size = r.ReadInt32();
        int b = r.ReadInt32();
        
        int numItems = r.ReadInt32();

        for (var i = 0; i < numItems; i++)
            CGameCtnAnchoredObject item = Parse<CGameCtnAnchoredObject>(r);

        int c = r.ReadInt32();
    }
}
```

### 0x042 - skippable (author)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    int authorVersion = r.ReadInt32();
    string authorLogin = r.ReadString();
    string authorNickname = r.ReadString();
    string authorZone = r.ReadString();
    string authorExtraInfo = r.ReadString();
}
```

### 0x043 - skippable (genealogy)

**Note: This chunk has it's own lookback.**

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    int size = r.ReadInt32();
    byte[] data = r.ReadBytes(size);

    using(var ms = new MemoryStream(data));
    using(var r2 = new GameBoxReader(ms, this));

    int numGenealogies = r2.ReadInt32();

    for(var i = 0; i < numGenealogies; i++)
        CGameCtnZoneGenealogy zoneGenealogy = Parse<CGameCtnZoneGenealogy>(r2);
}
```

Note: There's usually a LOT of zone genealogy classes which with a the current parser of GBX.NET can take over 3 seconds to read all.
You can take the `byte[] data` to process on a seperate thread, or just at a time it's needed.

### 0x044 - skippable (metadata)

Note: Purely theoretical read of ManiaPlanet 4.1+ metadata

```cs
void Read(GameBoxReader r)
{
    int unknown = r.ReadInt32();
    int size = r.ReadInt32();
    uint classID = r.ReadUInt32(); // CScriptTraitsMetadata
    int version = r.ReadInt32();

    byte typeCount = r.ReadByte();
    Type[] types = new Types[typeCount];

    for (var i = 0; i < typeCount; i++)
    {
        byte varType;

        switch (varType)
        {
            case 7: // Array
                ReadScriptArray();
                break;
            case 15: // Struct
                ReadScriptStruct();
                break;
        }

        types[i] = ConstructType();
    }

    byte varCount = r.ReadByte();

    for (var i = 0; i < varCount; i++)
    {
        string metadataVarNam = r.ReadString(); // If smaller than 255, length must be read as byte, not integer!
        byte typeIndex = r.ReadByte();

        ReadType(types[typeIndex]);
    }

    void ReadScriptArray()
    {
        ScriptVariable indexVar;

        byte indexType = r.ReadByte(); // Array index type
        if (indexType == 15) // Struct
            ReadScriptStruct(); // Haven't tested this case, might bug out, but structs can be apparently used as an index

        byte arrayType = r.ReadByte(); // Array value type
        if (arrayType == 7) // Array
            ReadScriptArray();
        else if (arrayType == 15) // Struct
            ReadScriptStruct();

        ReadUntilByteIsNotZero(); // Sometimes the amount of zero bytes is 1 when struct array, could be a struct read issue, usually 0
    }

    ScriptStruct ReadScriptStruct(out int defaultLength)
    {
        byte numMembers = r.ReadByte();
        string structName = r.ReadString();

        for (var i = 0; i < numMembers; i++)
        {
            string memberName = r.ReadString();
            byte memberType = r.ReadByte();

            switch (memberType)
            {
                case 7: // Array
                    ReadScriptArray();
                    break;
                case 15: // Struct
                    ReadScriptStruct();
                    break;
            }

            switch (memberType)
            {
                case ScriptType.Integer:
                    int default = r.ReadInt32();
                    break;
                case ScriptType.Real:
                    float default = r.ReadString();
                    break;
                case ScriptType.Vec2:
                    Vec2 default = r.ReadVec2();
                    break;
                case ScriptType.Vec3:
                    Vec3 default = r.ReadVec3();
                    break;
                case ScriptType.Int3:
                    Int3 default = r.ReadInt3();
                    break;
                case ScriptType.Int2:
                    Int2 default = r.ReadInt2();
                    break;
                case ScriptType.Array:
                    break;
                case ScriptType.Struct:
                    break;
                default:
                    byte default = r.ReadByte();
                    break;
            }
        }

        ReadUntilByteIsNotZero();
    }

    Type ReadType(Type type)
    {
        switch (type.Type)
        {
            case ScriptType.Boolean:
                byte boolean = r.ReadByte();
                break;
            case ScriptType.Integer:
                int integer = r.ReadInt32();
                break;
            case ScriptType.Real:
                float real = r.ReadSingle();
                break;
            case ScriptType.Text:
                string str = r.ReadString(); // If smaller than 255, length must be read as byte, not integer!
                break;
            case ScriptType.Vec2:
                Vec2 vec2 = r.ReadVec2();
                break;
            case ScriptType.Vec3:
                Vec3 vec3 = r.ReadVec3();
                break;
            case ScriptType.Int3:
                Int3 int3 = r.ReadInt3();
                break;
            case ScriptType.Int2:
                Int2 int2 = r.ReadInt2();
                break;
            case ScriptType.Array:
                byte numElements = r.ReadByte();

                if (numElements > 0)
                {
                    if (type.Key == ScriptType.Void)
                    {
                        for (var i = 0; i < numElements; i++)
                            ReadType(type.Value);
                    }
                    else
                    {
                        ReadType(type.Key);
                        for (var i = 0; i < numElements; i++)
                            ReadType(type.Key);
                    }
                }
                break;
            case ScriptType.Struct:
                for (var i = 0; i < type.Members.Length; i++)
                    type.Members[i] = ReadType(type.Members[i]);
                break;
        }
    }
}
```

#### Enums

```cs
public enum ScriptType
{
    Void,
    Boolean,
    Integer,
    Real,
    Class, // Not allowed for metadata
    Text,
    Enum,
    Array,
    ParamArray,
    Vec2,
    Vec3,
    Int3,
    Iso4, // Not allowed for metadata
    Ident, // Not allowed for metadata
    Int2,
    Struct
}
```

### 0x048 - skippable (baked blocks)

```cs
void Read(GameBoxReader r)
{
    int u01 = r.ReadInt32();
    int u02 = r.ReadInt32();

    int numBakedBlocks = r.ReadInt32();
    for (var i = 0; i < numBakedBlocks; i++)
    {
        LookbackString blockName = r.ReadLookbackString();
        Direction dir = (Direction)r.ReadByte();
        Byte3 coord = r.ReadByte3();
        int flags = r.ReadInt32();
    }

    int u03 = r.ReadInt32();
    int u04 = r.ReadInt32();
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int u01 | ~ | ~ | ~ | ~ | ~
| int u02 | ~ | ~ | ~ | ~ | ~
| int u03 | ~ | ~ | ~ | ~ | ~
| int u04 | ~ | ~ | ~ | ~ | ~

### 0x049 (mediatracker)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();

    CGameCtnMediaClip clipIntro = r.ReadNodeRef<CGameCtnMediaClip>();
    CGameCtnMediaClip clipPodium = r.ReadNodeRef<CGameCtnMediaClip>();
    CGameCtnMediaClipGroup clipGroupInGame = r.ReadNodeRef<CGameCtnMediaClipGroup>();
    CGameCtnMediaClipGroup clipGroupEndRace = r.ReadNodeRef<CGameCtnMediaClipGroup>();

    if(version >= 2)
    {
        CGameCtnMediaClip clipAmbiance = r.ReadNodeRef<CGameCtnMediaClip>();

        int u01 = r.ReadInt32();
        int u02 = r.ReadInt32();
        int u03 = r.ReadInt32();
    }
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| int u01 | ~ | ~ | ~ | ~ | ~
| int u02 | ~ | ~ | ~ | ~ | ~
| int u03 | ~ | ~ | ~ | ~ | ~

### 0x04B - skippable (objectives)

```cs
void Read(GameBoxReader r)
{
    string objectiveTextAuthor = r.ReadString();
    string objectiveTextGold = r.ReadString();
    string objectiveTextSilver = r.ReadString();
    string objectiveTextBronze = r.ReadString();
}
```

### 0x050 - skippable

Undiscovered.

### 0x051 - skippable (title info)

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    string titleID = r.ReadLookbackString();
    string buildVersion = r.ReadString();
}
```

### 0x052 - skippable

Undiscovered.

### 0x053 - skippable

Undiscovered.

### 0x054 - skippable

Undiscovered.

### 0x055 - skippable

Undiscovered.

### 0x056 - skippable

Undiscovered.

### 0x057 - skippable

Undiscovered.

### 0x058 - skippable

Undiscovered.

### 0x059 - skippable

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32(); // 3

    Vec3 u01 = r.ReadVec3();

    if (version != 0)
    {
        bool u02 = r.ReadBoolean();

        if (Version >= 3)
        {
            float u03 = r.ReadSingle();
            float u04 = r.ReadSingle();
        }
    }
}
```

#### Unknown variables

| Variable | ~ | ~ | ~ | ~ | ~
| --- | --- | --- | --- | --- | --- 
| Vec3 u01 | ~ | ~ | ~ | ~ | ~
| bool u02 | ~ | ~ | ~ | ~ | ~
| float u03 | ~ | ~ | ~ | ~ | ~
| float u04 | ~ | ~ | ~ | ~ | ~

### 0x05A - skippable [TM2020]

Undiscovered.

### 0x05F - skippable (free blocks) [TM2020]

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();

    List<Vec3> vectors = new List<Vec3>();
    while (r.BaseStream.Position < r.BaseStream.Length) // read the skippable chunk to the end basically
        vectors.Add(r.ReadVec3());
}
```

This chunk data strictly relies on the 0x01F chunk data.

To understand the vectors, the amount of them is undefined in this chunk. The structure of the vector list looks something like this:
- 1st free block in 0x01F
    - Vec3 absolutePositionInMap
    - Vec3 pitchYawRoll
    - for each clip of that block
        - Vec3 clipPosition
        - Vec3 clipPitchYawRoll
- 2nd free block in 0x01F
    - Vec3 absolutePositionInMap
    - Vec3 pitchYawRoll
    - for each clip of that block
        - Vec3 clipPosition
        - Vec3 clipPitchYawRoll
- ...

You can't tell the amount of free blocks without the chunk 0x01F (because of the clips). Free block in the block flags is defined by the bit 29. If the bit is set, the block is a free block. Free blocks also have a coordinate (0, 0, 0).

You also can't tell the amount of free blocks without knowing the amount of clip the block model has. It is unsure where this information is available, but probably in the CGameCtnBlockInfo nodes which are available in the PAK files.

Therefore, to read the chunk with a known `CGameCtnBlock` and `CGameCtnBlockInfo`:

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();

    foreach (var block in Blocks.Where(x => x.IsFree))
    {
        Vec3 absolutePositionInMap = r.ReadVec3();
        Vec3 pitchYawRoll = r.ReadVec3();

        foreach (var clip in block.BlockInfo.Clips)
        {
            Vec3 clipPosition = r.ReadVec3();
            Vec3 clipPointPitchYawRoll = r.ReadVec3();
        }
    }
}
```
