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
- [0x00F (old block data)](#0x00F-old-block-data)
- [0x011](#0x011)
- [0x012](#0x012)
- [0x013 (legacy block data)](#0x013-legacy-block-data)
- [0x014 - skippable (legacy password)](#0x014---skippable-legacy-password)
- [0x016 - skippable](#0x016---skippable)
- [0x017 - skippable (checkpoints)](#0x017---skippable-checkpoints)
- [0x019 - skippable (mod)](#0x019---skippable-mod)
- [0x01C - skippable (play mode)](#0x01C---skippable-play-mode)
- [0x01F (block data)](#0x01F-block-data)
- [0x021 (legacy mediatracker)](#0x021-legacy-mediatracker)
- [0x022](#0x022)
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
- 0x042 - skippable
- 0x043 - skippable
- 0x044 - skippable (metadata)
- 0x048 - skippable
- 0x049 (mediatracker)
- 0x050 - skippable
- 0x051 - skippable
- 0x052 - skippable
- 0x053 - skippable
- 0x054 - skippable (embedded items)
- 0x055 - skippable
- 0x056 - skippable
- 0x057 - skippable
- 0x058 - skippable
- 0x059 - skippable

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

	bool a = r.ReadBoolean();
	int b = r.ReadInt32();

	if (version < 1)
		byte c = r.ReadByte();

	byte d = r.ReadByte();

	if (version < 9)
		BoatName boatName = (BoatName)r.ReadByte();

	if (version >= 9)
		LookbackString boat = r.ReadLookbackString();

	if (version >= 12)
		LookbackString boatAuthor = r.ReadLookbackString();

	RaceMode raceMode = (RaceMode)r.ReadByte();
	byte e = r.ReadByte();
	WindDirection windDirection = (WindDirection)r.ReadByte();
	byte windStrength = r.ReadByte();
	Weather weather = (Weather)r.ReadByte();
	byte f = r.ReadByte();
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
					byte g = r.ReadByte();

					if (version == 6 || version == 7)
					{
						bool h = r.ReadBoolean();
						string i = r.ReadString();
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

Variable | Map 1 | Map 2 | Map 3 | Map 4 | Map 5
- | - | - | - | - | - 
bool a | false | true | true | false | true
int b | 0 | 1 | 5 | 0 | 1
byte c | 0 | 1 | 5 | 0 | 1
byte d | 0 | 1 | 5 | 0 | 1
byte e | 0 | 1 | 5 | 0 | 1
byte f | 0 | 1 | 5 | 0 | 1
byte g | 0 | 1 | 5 | 0 | 1
bool h | true | false | true | true | true
string i | "" | "" | "" | "" | ""

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

	int a = r.ReadInt32();

	if (version >= 1)
	{
		int bronzeTime = r.ReadInt32();
		int silverTime = r.ReadInt32();
		int goldTime = r.ReadInt32();
		int authorTime = r.ReadInt32();

		if (version == 2)
			byte b = r.ReadByte();

		if (version >= 4)
		{
			int cost = r.ReadInt32();

			if (version >= 5)
			{
				bool multilap = r.ReadBoolean();

				if (version == 6)
					int c = r.ReadInt32();

				if (version >= 7)
				{
					int trackType = r.ReadInt32();

					if (version >= 9)
					{
						int d = r.ReadInt32();

						if (version >= 10)
						{
							int authorScore = r.ReadInt32();

							if (version >= 11)
							{
								int editorMode = r.ReadInt32(); // bit 0: advanced/simple editor, bit 1: has ghost blocks

								if (version >= 12)
								{
									int e = r.ReadInt32();

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

Variable |
- |
int a |
byte b |
int c |
int d |
int e |

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
						byte[] a = r.ReadBytes(16);

						if (version >= 6)
						{
							string mapType = r.ReadString();
							string mapStyle = r.ReadString();

							if (version <= 8)
								bool b = r.ReadBoolean();
							
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

Variable |
- |
byte[] a |
bool b |

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

### 0x00F (old block data)

```cs
void Read(GameBoxReader r)
{
	Meta mapInfo = r.ReadMeta();
	Int3 size = r.ReadInt3();

	int numBlocks = r.ReadInt32();
	for (var i = 0; i < numBlocks; i++)
		CGameCtnBlock block = r.ReadNodeRef<CGameCtnBlock>();

	int a = r.ReadInt32();
	int b = r.ReadMeta();
}
```

### 0x011

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

### 0x012

```cs
void Read(GameBoxReader r)
{
	string a = r.ReadString();
}
```

#### Unknown variables

Variable |
- |
string a |

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
	int a = r.ReadInt32();
	string password = r.ReadString();
}
```

#### Unknown variables

Variable |
- |
int a |

### 0x016 - skippable

```cs
void Read(GameBoxReader r)
{
	int a = r.ReadInt32();
}
```

#### Unknown variables

Variable |
- |
int a |

### 0x017 - skippable (checkpoints)

Checkpoint positions in coords. Available up to TMUF.

```cs
void Read(GameBoxReader r)
{
	int numCheckpoints = r.ReadInt32();
	for(var i = 0; i < numCheckpoints; i++)
		(int, int, int) checkpointCoord = r.ReadInt3();
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
		(byte, byte, byte) coord = r.ReadByte3();
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
	bool a = r.ReadBoolean();
}
```

#### Unknown variables

Variable |
- |
bool a |

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
	(float, float) mapCoordOrigin = r.ReadVec2();
	(float, float) mapCoordTarget = r.ReadVec2();
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
		byte a = r.ReadByte();
		(float, float, float) b = r.ReadVec3();
		(float, float, float) c = r.ReadVec3();
		(float, float, float) d = r.ReadVec3();
		(float, float, float) e = r.ReadVec3();
		float f = r.ReadSingle();
		float g = r.ReadSingle();
		float h = r.ReadSingle();
	}
}
```

#### Unknown variables

Variable |
- |
byte a |
(float, float, float) b |
(float, float, float) c |
(float, float, float) d |
(float, float, float) e |
float f |
float g |
float h |

### 0x028 (comments)

```cs
void Read(GameBoxReader r)
{
	Chunk0x027.Read(r);
	string comments = r.ReadString();
}
```

### 0x029 - skippable (password)

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
	bool a = r.ReadBoolean();
}
```

#### Unknown variables

Variable |
- |
bool a |

### 0x034 - skippable

### 0x036 - skippable (realtime thumbnail)

```cs
void Read(GameBoxReader r)
{
	(float, float, float) thumbnailPosition = r.ReadVec3();
	(float, float, float) thumbnailPitchYawRoll = r.ReadVec3(); // in radians
	float thumbnailFOV = r.ReadSingle();

	float a = r.ReadSingle();
	float b = r.ReadSingle();
	float c = r.ReadSingle();
	float d = r.ReadSingle();
	float e = r.ReadSingle();
}
```

#### Unknown variables

Variable |
- |
float a |
float b |
float c |
float d |
float e |

### 0x038 - skippable

### 0x03D - skippable (lightmaps)

```cs
void Read(GameBoxReader r)
{
	bool a = reader.ReadBoolean();
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
			byte[] data = r.ReadBytes(compressedSize); // ZLIB compressed data

			CHmsLightMapCache lightmapCache = DecompressZLIB(data);
		}
	}
}
```

#### Unknown variables

Variable |
- |
bool a |

### 0x03E - skippable
### 0x042 - skippable
### 0x043 - skippable
### 0x044 - skippable (metadata)
### 0x048 - skippable
### 0x049 (mediatracker)
### 0x050 - skippable
### 0x051 - skippable
### 0x052 - skippable
### 0x053 - skippable
### 0x054 - skippable (embedded items)
### 0x055 - skippable
### 0x056 - skippable
### 0x057 - skippable
### 0x058 - skippable
### 0x059 - skippable
### 0x05A - skippable [TM®️]