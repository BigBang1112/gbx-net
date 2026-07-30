# Changelog

## [GBX.NET 2.4.4](https://github.com/BigBang1112/gbx-net/releases/tag/v2.4.4) - 2026-07-30

- Added `CPlugVisualQuads2D`, `CControlStyle`, `CControlIconIndex`, `CFuncEnum`
- `CGameCtnChallenge`: Added `0x01E`
- `CHmsZone`: Renamed `0x003` `U01` to `MRIsForced` (by marosis)
- Better `NPlugTrigger_SSpecial` support
- Fixed `CPlugSurface` write issue
- Fixed rare TM2020 input state bugs
- Fixed some `CCtnMediaBlockEventTrackMania` ToStrings
- Updated some `CPlugVehicleCarPhyTuning` types from `int` to `float`
- Made `GbxRefTable.GetFilePath` public
- More external nodes

GBX.NET.PAK
- Fixed `BlowfishStream` to read 8-byte blocks guaranteed

GBX.NET.Crypto
- Added support for MD5 in WASM (browser)

## [GBX.NET 1.12.14](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.14) - 2026-07-24

- Fixed rare TM2020 input state bugs

## [GBX.NET 2.4.3](https://github.com/BigBang1112/gbx-net/releases/tag/v2.4.3) - 2026-06-25

- Added support for `CampaignScoresManager.Manager.Gbx` and `LeaguesManager.Manager.Gbx`
- Added `CHmsLightMapMood` and `CControlEffectMaster` (by marosis)
- Added `GhostVersion` `SecurityKey`, `SecurityKey128`, and `GhostUid128` to `CGameCtnGhost`
- Added `Checksum128` and `Checksum256`
- Added more external file options on various classes
- Implemented various ManiaPlanet 3 chunks
- Reworked handling of unsupported writing
- Fixed `CPlugSurface` for MP3 and TM2020
- Fixed some TMUnlimiter data being lost upon write
- Fixed TMUnlimiter version 6 resaved in 2.0
- Fixed TM2020 ghost sample gear value calculation
- Fixed sample data driven in TM2 ghost recorder
- Small optimizations

GBX.NET.PAK 2.4.3
- Added `Pak.ComputeKeyFromMasterServer`
- `CheckFileIsGbxAsync` now returns `false` on empty files instead of exception
- `BruteforceFileHashesAsync` now also looks for script file references
- Reworked parsing strategy into 3 categoric keys
- Fixed memory leak when reading LZ4-compressed Paks
- Fixed trick to be able to read ManiaPlanet Pak files before version 18

## [GBX.NET 2.4.2](https://github.com/BigBang1112/gbx-net/releases/tag/v2.4.2) - 2026-04-11

- Fixed write of LightmapCache when discovered
- Fixed TMUnlimiter write in vanilla mode
- Improved various `ToString`s
- Added checks for the number of nodes
- Added more attributes to various properties
- `CGameCtnChallenge.Checkpoints` is now `List`

## [GBX.NET 2.4.1](https://github.com/BigBang1112/gbx-net/releases/tag/v2.4.1) - 2026-04-04

- Fixed sample reading of TMF and older versions
- Added `TryCreateChunk`
- Implemented `CControlList` `0x007`
- Slightly reduced allocation size of `BoundedStream`
- Fixed `CCtnMediaBlockUiTMSimpleEvtsDisplay` to inherit `CGameCtnMediaBlockUi`

## [GBX.NET 2.4.0](https://github.com/BigBang1112/gbx-net/releases/tag/v2.4.0) - 2026-04-04

- **Added support for modifying ghost samples** (`CPlugEntRecordData` and `CGameGhost.Data` classes)
  - Individual sample data is now stored as integers and bytes to avoid possible floating point issues
  - Previous values are directly mapped from those integers and bytes in real time (small performance cost of retrieving them)
  - To have an interface reaching the raw data, cast to `CSceneVehicleCar.ISampleRawData` or `CSceneVehicleVis.IEntRecordDeltaRawData`
  - Support ranges from roughly TMO to TM2020
- **Added TMUnlimiter support for all of its versions**
  - The object tree is not 100% ideal yet, but it is working as expected
  - Some older TMUnlimiter tracks reexported in 2.0 may not work yet, that's expected and to be resolved later
  - Any tracks containing AngelScript bytecode cannot be parsed properly due to a mistake in the TMUnlimiter serializer not providing a length prefix, slim chances this is gonna get resolved
- **Added full Profile.Gbx support for ManiaPlanet and TM2020**
  - `CGamePlayerProfile` recognizes all profile chunks
  - `CGameUserProfile` parses successfully for the latest TM2020 Gbx, but misses most of the namings
- **Added full `CHmsLightMapCache` support**
  - In `CGameCtnChallenge`, it is thread-safe lazy-loaded upon accessing most of the lightmap properties
- Removed all special explicit parse features (it is just a cast now)
  - This change reduces code size and makes `CPlugFileGen` parse possible for the cost of less trimming capabilities and a small performance cost
- **Implemented support of `VehicleTunings.Gbx` for TM2020**
- Refactored `CompressedData` to `ZlibData`
  - Newly has `Exception` property to track errors that aren't disastrous to the parse
  - Also includes `Parsed`, which is used to handle lazy-loading features
- Added `RawData`, which works similarly to `ZlibData` but without compression tracking
  - Some raw encapsulated data is available: `CGameCtnChallenge.ZoneGenealogyData`, `CGameCtnReplayRecord.ChallengeGbxData`
- `CGameCtnChallenge.ZoneGenealogy` is now lazy-loaded - improves initial parse speed for TM2+
- Inputs from TMS, TMNESWC, and TMO have been moved from `CGameCtnReplayRecord.Inputs` to the first ghost's `CGameCtnGhost.Inputs`
  - `CGameCtnReplayRecord.Inputs` had to stay due to TM1.0 storing the whole binding configuration with it before the ghost is parsed
- Reworked `ChunkSet` system to be more consistent and reliable
  - Internally, there's no sorting applied
  - Each added chunk is stored in multiple hashmaps
  - On external chunk addition, a comparer is applied, and the chunk is inserted at a binary-searched index
  - The comparer now considers base class IDs as it should
  - Many more methods have been added to `CMwNod` for interaction as it was in v1
- **Implemented data boundaries system**
  - Data regions that include a length prefix (userdata, skippable chunks, larger byte arrays) are now wrapped into a special `BoundedStream`
  - It is designed to disallow reading past the defined length of the expected buffer without allocating larger byte arrays when possible
  - If the data can be sought, it simply calculates allowed positions and disallows reading further. If seeking is not allowed, then it behaves like a `MemoryStream`, reading all the bytes beforehand and working within this allocated buffer
  - This replaces the old wonky "limiter" that existed (only) in Gbx header reading
  - GBX.NET is now much safer to use on the web backend thanks to this
- **Added full support for `Scores.Gbx`** (also including write)
- Added `CScriptTraitsPersistent` support (ScriptCache.Gbx)
- Added `CGameCtnChallengeGroup` `0x00A`
- Added `CGameCtnBlockInfoClipHorizontal` and `CGameCtnBlockInfoClipVertical` (by Zai)
- Added weak support for `CPlugFileGen`
- Added `CGameCtnChallenge.ThumbnailRotationMatrix`
- Added `Quat.Identity` and `Vec3.One`
- Added Gbx file extensions (as many as I could figure out)
- Improved input parsing performance
- Support for TMCP inputs v3 (by Tomashu)
- **Fixed many instances of improper stream reading, often appearing in network streaming or during zipping**
- Fixed various class ID remapping issues during write in main node, header chunks, and node references
- Fixed `CGameCtnCollection` not writing icon file references properly
- Fixed `CGameCtnCampaign.CollectionId` to be `Id` instead of `string`
- Fixed `CPlugBitmapAtlas` not handling many Lagoon textures properly
- Fixed `CPlugSurface` v5
- Fixed `CPlugSkel` v20
- Fixed `systemtime` type to read/write correct datetimes
- Fixed `Quat` multiplication
- Fixed `CSystemConfig` `0x057`
- Fixed `CGameCtnZone` decryption issue in TM2+ paks
- Fixed `CPlugVehiclePhyModel` for TM2020
- Fixed inheritance issues for adding `AppliedWithChunk` attributes
- Class names now prefer the last provided name instead of the first one in `ClassId.txt`
- Improved some `ToString`s
- Updated TmEssentials to 2.6.2

GBX.NET.PAK 2.4.2
- Added `Pak.CheckIsGbx`

GBX.NET.Crypto 1.2.1
- Added more guard clauses

GBX.NET.LZO 2.1.5
- Added more decompression checks

GBX.NET.Templates 0.2.1
- Added 404.html to GbxBlazorWasmApp
- Removed CRC32 features from templates

Gbx Explorer
- Game version is automatically determined from the remembered Gbx parameters (checkbox for older games is no longer available)
  - This has some rare issues at times, for example, when exporting the `Challenge` node from older TM replay, but other than that, it's very reliable
- All `byte[]` now include the `Export` button, which saves all the bytes into a file (can be used on `ChallengeGbxData.Data` for example)
- Added support for trace logs (shows chunk parse speeds)
- Brought back file extensions with a few improvements
- Fixed `[Flags]` to display individual values - improves `GameVersion` display
- Fixed a crash when modifying an immutable object that doesn't apply readonly

## [GBX.NET 1.2.13](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.13) - 2026-03-18

- Competition Patch 2 updates by Tomashu:
  - Support TMCP 2.0 inputs format v3
  - Do not output inputs after finish
- Fixed `CPlugEntRecordData` v11 to use correct sample times

## [GBX.NET 2.3.4](https://github.com/BigBang1112/gbx-net/releases/tag/v2.3.4) - 2026-02-19

- Expanded support for `SystemConfig.Gbx` to all Nadeo games
- Added `Solid` and `SolidRef` to `CGameObjectVisModel`
- CRC32 implementation is now directly part of the main GBX.NET library and enabled by default
  - GBX.NET.Hashing will soon be deprecated
- Fixed compressed Gbx reading in networking/zipping scenarios

GBX.NET.PAK 2.4.1
- Made Pak6 public
- Disabled automatic processing of file tree for MP3 title packs to avoid a key-less crash
- Fixed `GbxHeadersStart` to be `uint`

GBX.NET.Crypto 1.2.0
- Added `Blowfish.Encrypt` with IV and `Blowfish.EncryptHexa`
- Completed `RSAExtensions` with stable `PublicDecrypt` and `PrivateEncrypt`

## [GBX.NET 2.3.3](https://github.com/BigBang1112/gbx-net/releases/tag/v2.3.3) - 2026-02-06

- Added `Gbx.IsGbxAsync`
- Fixed `IsGbx` throwing when it shouldn't
- Fixed TM2020 macroblocks that included skinnable items
- Fixed TM2020 `CScriptTraitsMetadata` for arrays inside structs

GBX.NET.Crypto 1.1.0
- Added custom implementation of Blowfish (little-endian, little-endian Pak 18, and big-endian tricks)
- Added `DecryptHexa` replica
- Added an extension `RSA.PublicDecrypt` (experimental)

GBX.NET.PAK 2.4.0
- Added `computeKey` which runs the key hashing trick
  - If true, base key is expected, if false, the final key is expected
  - `PakList` reflects that and returns the base keys
  - All current Pak solutions need to be updated to use base keys or opt out of `computeKey`
- Switched to use the Blowfish implementation from GBX.NET.Crypto
- Title packs can now display folders and files without providing a key (data still needs the key)
- **The library is now MIT licensed**

GBX.NET.Templates 0.2.0
- Added GBX.NET Blazor WebAssembly App template
- Updated dependencies

## [GBX.NET 2.3.2](https://github.com/BigBang1112/gbx-net/releases/tag/v2.3.2) - 2026-01-31

- Added `Gbx.IsGbx` for quick `Gbx` checking without throwing exceptions
- `CGameItemModel`:
  - Added `VFX` and `MaterialModifier`
  - Fixed `EntityModelEdition` if it included a file
- Added `CPlugBitmapDecals.MatterId`
- Implemented `CPlugImageArray`
- Attempt to implement `CPlugCurveSimpleNod`
- Fixed `CPlugEntRecordData` v11 to use correct sample times
- Fixed `CPlugVisual` `0x00F`
- Fixed `CSceneLayout` `0x01C`

GBX.NET.PAK 2.3.0
- Transformed the parsing API to require only one key (either PakList-resolved key or the ManiaPlanet base key)
- Made the parsing fully support async (except for individual files) to allow efficient client-side parsing on WebAssembly
- Slightly improved bruteforcing to catch some parent folders
- Moved some `Pak6` parts to the `Pak` class

## [GBX.NET 2.3.1](https://github.com/BigBang1112/gbx-net/releases/tag/v2.3.1) - 2026-01-28

- Added new collection IDs: RedIsland, GreenCoast, BlueBay, WhiteShore
- `CGameCtnChallenge`:
  - Added read-only `ExpectedEmbeddedItemModels` for verifying packaged content (it doesn't have to match the modified zip)
  - Added `HasClones` and updated `CreateHeaderXml` to reflect this
- `CGameCtnChallengeParameters`: Added support for the new `RaceValidateGhost` in TM2020
- `CGameCtnReplayRecord`:
  - Added `ChallengeParameters` (wtf? it's in the map file already)
  - Implemented chunks `0x026`, `0x027`, `0x028`
- `CGameCtnCollection`:
  - Added `VehicleTransform_...`, `BlockSkins_Default_...`, and `GlobalMaterialModifier` members
  - Fixed various chunks
- Fixed `CPlugEntRecordData` v11 that now uses new delta encoding
  - The sample times might be off still, this will be resolved soon in a later update
- Fixed `CGameItemModel` `0x01F`
- Fixed `CGameCtnGhost.GetDisplayableInputs` to handle TM2 and TMT properly

GBX.NET.LZO 2.1.4
- Updated to NativeSharpLzo 0.3.5
- Improved locking mechanism for parallelism

GBX.NET.PAK 2.2.2
- Updated to NativeSharpLzo 0.3.5
- Updated to NativeSharpZlib 0.2.9

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v2.3.0...v2.3.1

## [GBX.NET 1.2.12](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.12) - 2026-01-28

- Fixed `CPlugEntRecordData` v11 that now uses new delta encoding
  - The sample times might be off still, this will be resolved soon **only** on v2 in a later update

## [GBX.NET 1.2.11](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.11) - 2026-01-12

- Fixed `CGameCtnGhost.GetDisplayableInputs` to handle TM2 and TMT properly and added a missing version constraint to it.

This is a rare GBX.NET v1 update as Clip Input still runs on v1.

## [GBX.NET 2.3.0](https://github.com/BigBang1112/gbx-net/releases/tag/v2.3.0) - 2026-01-06

- `CGameCtnChallenge`: Added `ExtractEmbeddedZipData`
- Added more member names to `CSceneMobilLeaves`, `CGameCtnDecorationMood`, `CSceneMobilLeaves`, `CPlugBitmapRenderLightFromMap`, `CPlugBitmapRenderLightOcc`, `CFuncTreeRotate`, `CMotionEmitterLeaves`, `CMotionTrackMobilPitchin` (by marosis)
- `CGamePlayerProfile`:
  - Added more member names
  - Added `TagId`, `IsVisibleByOtherPlayers` (by LinuxCat)
  - Perhaps fixed the write problem
- `CInputBindingsConfig.Binding`:  Added `DeviceId`, `IsAnalog`, `Name` (by LinuxCat)
- `CPlugTree`:
  - Added more members from flags (special thanks to marosis and greffmaster)
  - Added `Material` (shortcut of `Shader as CPlugMaterial`, as that's what the game does too)
- `CPlugShader`: Added more members from flags (special thanks to marosis and greffmaster)
- `CPlugBitmap`: Added `Usage` and `PixelUpdate`, added `0x014`, fixed `0x015`
- Added `CGameAdvertising` and `CGameNod` (by Zai)
- `CPlugSolid2Model`:
  - Added `FileImageBytes`, `LodMaxDistAtFov90`, `Boxes`, `Joints`
  - Renamed `ShadedGeom` `Lod` to `LodMask` as named by the game (not breaking, just `Lod` is obsolete now)
  - Fixed `0x002`
- `CHmsSoundSource`: Added `0x001`
- `CGameCtnGhost`: Added `GetDisplayableInputs` for Competition Patch 2 support (thanks Tomashu for communicating it with me)
  - This introduces a miniature breaking change for `FakeFinishLine` and `FakeIsRaceRunning` input structs
- Highlands patches:
  - Fixed case-insensitive reference table file check for case-sensitive systems
  - `CGameCtnBlockInfo`: Fixed  `0x005` `Ident.Id` part
  - `CPlugSurface`: Fixed `OctreeVersion` version 1
  - `CPlugMaterial`: Added `0x004`
- `CPlugSurface`: Improved compatibility of various surf types
- `CPlugCrystal`: Fixed lightmap coord counting and `IsEnabled` not reacting to it
- Fixed `ObjExporter` not counting root tree in `CPlugSolid`
- Fixed `CGameCtnDecorationMood` `ShadowScene` which should've been `SolidLightAreSkinned`
- Fixed `ReadRawBody` mode not working properly
- Updated to .NET 10, removed .NET 6 support

Gbx Explorer
- Added C# editor (by achepta)

## [GBX.NET 1.2.10](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.10) - 2025-12-16

- Added `CGameCtnGhost.GetDisplayableInputs` to handle Competition Patch 2 changes
- Breaking change: `FakeIsRaceRunning` and `FakeFinishLine` are now `IInput` instead of `IInputState` and store the whole `Data` as `uint`

This is a rare GBX.NET v1 update as Clip Input still runs on v1.

## [GBX.NET 2.2.2](https://github.com/BigBang1112/gbx-net/releases/tag/v2.2.2) - 2025-09-12

- Added `Validate_ValidationSeed`,  `Validate_TitleChecksum`, `WalltimeStartTimestamp`, `WalltimeEndTimestamp` to `CGameCtnGhost`
- Implemented ignored `CGameCtnGhost` chunks
- Added `unixtime` type (32bit timestamp, nando..)
- Added formatting attributes to `GhostNickname` and `GhostClubTag`
- Corrected `CGameCtnReplayRecord` versioning (fixes Bench replay in TMF)
- Fixed `CGameCtnCampaign`
- Fixed `CGameCtnDecorationAudio` write
- Fixed evaluation of `GameVersion` when unknown skippable chunks are present

Gbx Explorer
- Fixed version display on master branch

> [!WARNING]
> From 2.3.0, the support for .NET 6 will drop as some dependencies started to error out because of it. There should be a fair reason nowadays to be on .NET 8 at least.

## [GBX.NET 2.2.1](https://github.com/BigBang1112/gbx-net/releases/tag/v2.2.1) - 2025-07-18

- Added `HelperSolidFid` and `FacultativeHelperSolidFid` to `CGameCtnBlockInfoVariant`
- Added per-wheel `OnGround` property to ghost samples
- Improved support for `CGameCtnChallenge.CreateHeaderXml`
- Fixed `CGameCtnMediaBlockCameraOrbital` for TMF (by @SuperKulPerson)
- Fixed `CPlugTree.GetAllChildren` not properly scrolling mips
- Fixed various transformation issues when flattening `CPlugTree`
- Fixed `CGameCtnCollection` and `CGameCtnBlockInfo` for TM2020
- Further mesh and material support for TM2020

## [GBX.NET 2.2.0](https://github.com/BigBang1112/gbx-net/releases/tag/v2.2.0) - 2025-06-28

Biggest v2 update yet. Likely followed by a TM2020 Summer update bugfixes afterwards.
- Added support for 30+ classes
- Added a lot of new member names (TMF/TM2)
- Implemented a lot of chunks for TM2 support
- Added implicit/explicit operators for matrix/vector/quaternion structs
- Added `Direction` to `Int3` explicit operator
- Added support for network ghost sample data (version 13)
- Added/fixed newly discovered ghost sample values
- Changed `CGameCtnCollection.Collection` to be `Id` (TM2/020 issues)
- Fixed time values for ghost samples with non-fixed time step
- Fixed default of `CGameCtnChallenge.ClipTriggerSize` to be `(1, 1, 1)`
- Fixed writing of normals and colors in `CPlugVisual3D`
- Fixed `CPlugVertexStream` normals (`Vec3_10b`) and made them available as `Normals`
- Fixed `CPlugSolid` `0x017` versions 1-2
- Fixed `CPlugTree.GetAllChildrenWithLocation` not using the parent `Location`
- `GbxWriter.WriteArrayExternalNodeRef` now better tolerates `null` values
- `ObjExporter`: Fixed vertex streams for `CPlugSolid`
- `ObjExporter`: Fixed rotation/skewing issues including normals
- `ObjExporter`: Fixed root tree location not counted in calculation
- Tweaked a few limbo `GameVersion` cases

**GBX.NET.PAK 2.2.0**
- Added full Pak v6+ support (ManiaPlanet+TM2020 with various decryption tricks), by Auris

**New package GBX.NET.Templates!**

## [GBX.NET 2.1.2](https://github.com/BigBang1112/gbx-net/releases/tag/v2.1.2) - 2025-05-11

- `CGameCtnBlockInfo`:
  - Added `GroundHelperMobil`, `AirHelperMobil`, and `ConstructionModeHelperMobil`
  - Added `IsReplacement`
- Added `FuncShader` and `Passes` to `CPlugShader`
- Added some members to `CPlugBitmap`
- Added `GbxPath.ChangeExtension` to easily change double extensions
- Added `Vec3`/`int` and `Vec3`/`float` implicit operator support
- **Missing ZLIB module will now throw exceptions on properties where it's needed**
- Validation info from replay is now passed to the first ghost (except inputs, which will also move there eventually)
- Changed `CollectionIconFid` to `IconSmallFid`, removed the `Has`-prefixed properties
- Changed and improved some properties in `CGameCtnCollection`
- Changed `PitchYawRoll` to `YawPitchRoll` in `CGameCtnBlock` and `CGameCtnAnchoredObject` (they behave the same, but `PitchYawRoll` is now obsolete)
- Made `CPlugTree.GetAllChildrenWithLocation` public
- Fixed `WriteSmallLen` writing corrupted data for 128+ elements (thanks Yhomas for reminding me about this issue)
- Fixed `ObjExporter` non-UV visuals in vertex streams
- Fixed `ObjExporter` pick of LOD in some cases
- Fixed default value reading in ChunkL for auto properties - issue just in `CGameCtnBlock` decal things
- The `GameBox` class is now obsolete

## [GBX.NET 2.1.1](https://github.com/BigBang1112/gbx-net/releases/tag/v2.1.1) - 2025-01-22

- Added support for `Shader.Gbx`, `CtrlCam.Gbx`, `CtrlCamTarget.Gbx`, `CtrlCamOrbital3d.Gbx`, `CtrlCamTmRace.Gbx`, `CtrlCamTmRace2.Gbx`, `CtrlCamTmRace3.Gbx`, `DecoSolid.Gbx`, `TrackManiaVehicle.Gbx`, `TMVehicle.Gbx`/`ConstructionVehicle.Gbx`, `VehicleStruct.Gbx`, `FuncClouds.Gbx`, `FuncShader.Gbx`, `AmbientOcc.Gbx`, `ParticleModel.Gbx` (only TMUF), `Scene3d.Gbx` (only TMUF), and `RefBuffer.Gbx`.
- Added support for reading external nodes directly inside Pak files
  - `GbxRefTable` `ExternalData` is now `ExternalNodes`
- Fixed header chunks for `CGameCtnCollection` and `CGameCtnDecoration`
- Recognized many member names from various classes (mostly TMUF, but some TMS too)
- Named some TMUnlimiter classes
- Fixed many classes for TMUF Pak export

**GBX.NET.PAK 2.1.0**
- Added v6+ support for unencrypted parts (decryption coming soon!)
  - `Pak6` class inherits `Pak`, but `Pak.Parse` can recognize the version. To view Pak v6 specifics, gotta cast to `Pak6` class.
- Added possibility to not pass the key to read the unencrypted parts
- `OpenGbxFile` now fills `GbxRefTable.ExternalNodes` when `importExternalNodesFromRefTable=true`

**Gbx Explorer** (yo updates to Explorer?)
- Chunk IDs in Value Explorer are no longer masked to the last 3 digits
- Hovering over class grouping shows the namespace of the class
- Fixed some CSS on unknown class names

## [GBX.NET 2.1.0](https://github.com/BigBang1112/gbx-net/releases/tag/v2.1.0) - 2024-12-31

- Added experimental Gbx text format support (it hasn't been tested for a long time, please let me know about any issues)
- Added `DataUInt32`, binary scope, and other internal things to help with text format Gbx files
- Added lightmap UV support to `CPlugCrystal`
- Added color palette support (TM2020)
- Added `EncryptionInitializer` to `GbxReadSettings` for handling ivXor tricks in Pak files
- Implemented `CPlugDecoratorSolid`
- Improved `Normal` and `Color` support for `CPlugVisual3D`
- Filled `CPlugVehicleCarPhyTuning` with many new member names (by @GreffMASTER )
- **Changed all generated `IList` to `List`**, which provides better options for optimizatoin
- Changed `CPlugTree.Flags` from `int` to `ulong`
- `CGameCtnBlock` `Variant` and `SubVariant` are no longer nullable
- Moved `CGamePodiumInfo` to `Game` namespace
- Various block info read/write improvements for TM2 Gbxs
- `CMwNod.IsGameVersion` is now `virtual` to specify concrete game version cases manually
  - `CGameCtnGhost` can now distinguish MP4 from TM2020 with this function
- Extended `CGameCtnZone` with TM2 features
- Fixed map embedding reporting missing items (even when they were embedded)
- Fixed consistency issues with `CGameCtnChallenge` `NbLaps`
- Fixed `CGameCtnChallenge` `AuthorLogin` being set to empty string accidentally
- Fixed `CGameCtnBlockInfoVariant` `Mobils` property not being public
- Fixed `CPlugParticleEmitterSubModel` for TMUF
- Fixed `CGameBlockItem` `ArchetypeBlockInfoCollectionId` issues with numeric collections - failing to modify custom blocks from TM2020
- Added normals support to `ObjExporter` `CPlugSolid`
- Various `ObjExporter` fixes related to LOD
- Various chunk fixes and improvements
- Updated to .NET 9 (.NET 8/6 and Standard 2 is still kept)

Various updates to other sub-libraries due to .NET 9 update.

**New package GBX.NET.PAK!**

## [GBX.NET 1.2.9](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.9) - 2024-11-07

- Fixed constraint of map item chunk version being 8 instead of 9

## [GBX.NET 1.2.8](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.8) - 2024-11-07

- Fixed map item chunk of Fall 2024 update

## [GBX.NET 2.0.9](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.9) - 2024-10-01

- Added `Gbx.IsCompressed` and `Gbx.IsUncompressed` (throws on non-Gbx data)
- Fixed map item chunk read/write as Nadeo might have changed it a little

Tool framework:
- Refactored tool execution to be possible to generate compile-time in the future
  - Warning can now be eliminated with methods suffixed with `Statically`
- Added stdin support

## [GBX.NET 2.0.8](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.8) - 2024-09-29

An update released earlier before the Platform update to reduce chaos with UOTD and Envimix bots.
- `CPlugCrystal`: Added `LightLayer`
- `CPlugCrystal`: Added message for unsupported layers
- `CPlugCrystal`: Fixed `Materials.Count == 0` being handled incorrectly
- `CGameCtnChallenge`: Added item chunk version 8 support (leak from TOTD map made in alpha)
- `CGameCtnChallenge`: Linked `MapType` and `MapStyle` with `ChallengeParameters`
- `CGameCtnChallenge`: Fixed no lightmap frames writing compressed data
- `CGameCtnBlockInfo`: Added `SpawnLocAir` and `SpawnLocGround`
- `CPlugMaterialUserInst`: Changed types of `SurfacePhysicId` and `SurfaceGameplayId`
- `CGameGhost`: Fixed chunk `0x003` (very old TM games)
- `CPlugShader`: Implemented missing `0x01F` chunk 
- Added `CPlugVisualQuads`
- Fixed optimized ints when the length is exactly 255 or 65535
- Fixed face writing in `ObjExporter`

Tool framework
- Added `IComplexConfig` to avoid loading the whole complex configuration into memory on each tool instance
- Added more YML features

## [GBX.NET 2.0.7](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.7) - 2024-08-16

- Added `CPlugSolid2Model.ExportToObj()`
- Added missing `DefaultZone` to `CGameCtnCollection`
- Added missing `CGameUserFileList` from v1
- Added `CPlugWeatherModel`/`CMotionManagerWeathers` support
- Added missing `IGbx` interface members
- Added concrete exception for text format Gbxs
- Added `GetValidFileName()`, `InvalidFileNameChars`, and `InvalidFileNameCharSearchValues` to `GbxPath`
- Implemented `CFuncKeysSkel` `0x001`
- Implemented missing `CGameCtnGhost.Checkpoint` `ToString` from v1
- Made `CGameCtnReplayRecord.InterfaceScriptInfos` nullable (fixes JSON serialization)
- Changed `CPlugSurface.SurfMaterial.SurfaceId` from `short` to `CPlugSurface.MaterialId?`
- Fixed `CPlugTree` all children methods to properly handle mips
- Fixed `CPlugMaterial` for TMF
- Fixed `CPlugCrystal.SpawnPositionLayer` being wrongly de/serialized
- Fixed various serialization problems (often meshes and metadata) when values were between 255-65534
- Fixed some class IDs not being mapped properly

New package GBX.NET.Crypto!

GBX.NET.Tool.CLI 0.2.0:
- Added YML support
- Added `HidePath` to `ConsoleSettings`
- Added `JsonOptions` to `ToolConsoleOptions`
- Renamed `JsonSerializerContext` to `JsonContext` in `ToolConsoleOptions`
- Enhanced logging

GBX.NET.LZO 2.1.1:
- Made `Lzo.Compress` thread-safe due to unknown parallel issues

## [GBX.NET 1.2.7](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.7) - 2024-08-12

- Fixed `CGameCtnMediaBlockCameraCustom` of Summer 2024 update

## [GBX.NET 2.0.6](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.6) - 2024-07-23

- **Enhanced `CGameCtnBlock` properties (by @tomek0055)**
  - `Unassigned1` blocks have been discovered as **decal properties** of the previous block: `DecalId`, `DecalIntensity`, `DecalVariant`
  - Added `IsPillar` and `IsReplacement`
  - Added `PhyCharSpecialProperty` (class `CScenePhyCharSpecialProperty`)
  - Added `SquareCardEventIds`
  - Some `CGameCtnChallenge` method signatures have been updated to address the change *(breaking change)*
  - `HasFlags` and `Bit17` are now obsolete
- **Added full `CPlugPrefab` support**
- Added `CPlugDynaObjectModel` support (moving items in TM2020)
- **Block's X and Z coordinates are now subtracted by 1 in TM2 and TM2020 maps *(breaking change)***
- Reworked `CPlugTreeVisualMip` *(breaking change)*
  - `IDictionary<float, CPlugTree> Levels` is now `IList<CPlugTreeVisualMip.Level>`
  - `Level` has `float FarZ` and `CPlugTree Tree`
- Enabled lightmap support properly in `CGameCtnChallenge`
- Added new read setting `SafeSkippableChunks`
  - Reads skippable chunks more expensively (similarly to GBX.NET 1) while allowing to ignore skippable chunk data
  - Saving Gbx files with these errors can lead to unpredictable issues
- Added `exceptions = true` to external node properties
- Updated `GetChallenge` methods in `CGameCtnReplayRecord` with asyncs and parse configuration
- Implemented some missing `CPlugMaterial` and `CPlugShader` chunks from v1
- Added `CGameCtnChallenge.GenerateMapUid()`
- Added `Color.ToRgba()`
- `Gbx.Compress` now attempts to recompress, which can result in better or worse compression
- `Gbx.Compress` and `Gbx.Decompress` no longer return `bool` *(breaking change)*
- Removed `Gbx.Recompress` to reduce confusion *(breaking change)*
- Fixed `Gbx.Compress` incorrectly copying over
- Fixed map metadata writing string lengths to write byte lengths instead of character lengths
- Fixed `CPlugSolid` `0x000` version `15-28` not being properly written
- Fixed `CPlugVehicleCarPhyTuning` for ManiaPlanet
- Fixed `CGameCtnCollection` for ManiaPlanet
- Attempts to fix `CPlugSurface` for TM2020
- Updated TmEssentials to 2.5.0
- Updated some attributes

**GBX.NET.LZO 2.1.0 now uses native LZO implementation with 999 compression by default.** This compression has been tested on all Trackmania games with success.

GBX.NET.Imaging.SkiaSharp 1.1.0:
- Added `ImportIcon` to `CGameCtnCollector` extensions
- Fixed incorrect colors on WebAssembly build

GBX.NET.NewtonsoftJson 1.0.1:
- Added static `GbxJson` class and trimming annotations

New package GBX.NET.Imaging.ImageSharp, GBX.NET.Tool and GBX.NET.Tool.CLI!

## [GBX.NET 2.0.6-beta3](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.6-beta3) - 2024-07-20
**[PRE-RELEASE]**

https://github.com/BigBang1112/gbx-net/pull/118

## [GBX.NET 2.0.6-beta2](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.6-beta2) - 2024-07-16
**[PRE-RELEASE]**

https://github.com/BigBang1112/gbx-net/pull/118

## [GBX.NET 2.0.6-beta1](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.6-beta1) - 2024-07-16
**[PRE-RELEASE]**

https://github.com/BigBang1112/gbx-net/pull/118

## [GBX.NET 2.0.5](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.5) - 2024-07-01

- Added `ExportToObj` to `CPlugCrystal` and `CPlugSolid`
- Added `CPlugTree.GetAllChildrenWithLocation()` *(don't mind the weird naming)*
- Added `CSceneSoundSource.SoundSource`
- Added missing `IVersionable` on `CGameCtnChallenge` `0x040` chunk
- Added `Gbx<T>(T node)` constructor - easier and safe way to make typed `Gbx` other than `CMwNod.ToGbx()`
- Renamed `CPlugTree` `Translation` to `Location` *(breaking change, better late than never)*
- Implemented some `CGameCtnChallenge` chunks
- Implemented more guard clauses
- Fixed `CGameCtnMediaBlockCameraCustom` of Summer 2024 update
- Fixed write issue with analog input values in TM2 and older games
- Fixed MediaTracker not being written properly in `CGameCtnMacroBlockInfo`
- Fixed some read/write issues of `CPlugSurfaceGeom`
- Fixed various null issues
- Fixed various Gbx Explorer issues

New package!
- `GBX.NET.NewtonsoftJson` 1.0.0
  - Current replacement of `GBX.NET.Json`, properly working now
  - Upcoming `GBX.NET.Json` v2 will use `System.Text.Json`
  - You can dump any `Gbx` or `CMwNod` type with `ToJson`, explore everything in JSON, and maybe even compare changes.

## [GBX.NET 2.0.4](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.4) - 2024-06-12

- Added `ScriptMetadata` to `CGameWaypointSpecialProperty`
- Added `CGameItemModel.ItemGroupElements`
- Added more description to `CPlugCrystal`
- Added `CPlugSolid2Model` `0x000` header chunk
- Added classes related to leaves in TMUF
- Improved `CSceneObjectLink` (TMUF triggers can be easily linked now)
- Published hidden `CPlugSolid2Model` properties
- Fixed `SBadge` read/write for TMTurbo ghosts
- Fixed `Blocks` for TMUnlimiter 2

## [GBX.NET 2.0.3](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.3) - 2024-05-31

- Added partial `Action.Gbx` support with a bunch of classes with it
  - It is not yet complete, make sure to use `IgnoreExceptionsInBody` for now
- Added `CGameCtnBlockInfoRectAsym`
- Added `UpgradeIconToWebP` to `GBX.NET.Imaging.SkiaSharp`
- `CPlugTree`: Added `includeVisualMipLevels = false` to `GetAllChildren`
- `CGameCtnChallenge`: Fixed `AuthorLogin` not being linked to `MapInfo`
- `CGameCtnChallenge`: Fixed macroblock instances when all objects of it are removed
- `CPlugTree`: Fixed `GetAllChildren` not working properly
- Published `CollectionManager.Collections`
- Updated some attributes

Gbx Explorer:
- Added Gbx Explorer support for v2
- Enabled `IgnoreExceptionsInBody` by default (node will return with partial data if Gbx body fails)
- Some features were stripped off, let me know if they are important to you
- Server supports OpenTelemetry Protocol

Breaking changes:
- `CGameCtnCollector` `0x009` `Version` has been removed
- `CGameCtnChallenge` `TMObjective_IsLapRace` and `IsLapRace` have been merged to `IsLapRace`
- `CGameCtnChallenge` `TMObjective_NbLaps` and `NbLaps` have been merged to `NbLaps` (could cause inexact behavior, report to me in case)

## [GBX.NET 2.0.3-beta1](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.3-beta1) - 2024-05-30
**[PRE-RELEASE]**

## [GBX.NET 2.0.2](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.2) - 2024-05-04

- Added `CGamePlayerProfile` (Profile.Gbx) support for TMF (MP4 also does not crash)
- Added more ways to construct `Ident` (could be breaking change)
- Added `Gbx.ParseClassId` to improve Gbx type scans
- Added more guard clauses in `Gbx`
- Added `Gbx.StrictIdIndices` to help with corrupted `Id`s in Profile.Gbx
- Fixed `CGameCtnGhost` `0x025` on TM2 ghosts with no inputs
- Fixed bugged return of `Gbx.Recompress`
- Renamed `ClassManager.GetClassId` to `GetId` (breaking change)

`GBX.NET.Imaging.SkiaSharp` now supported.

## [GBX.NET 2.0.1](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.1) - 2024-04-29

Largest minor update of mine I think. xd I didnt want to jump straight to 2.1 due to convenience, even though I probably should have for correctness.
- Added `CSystemConfigDisplay` and missing `CSystemConfig` chunks
- Added missing `CGameCtnMediaBlockColoringBase`
- Added many simple methods to `CGameCtnChallenge`
- Added `Gbx.Recompress` to switch Gbx compression instead of forcing kind *(upcoming project soon)*
- Added `RemoveAll` as an extension to `IList`
- Added a few Game Version Interfaces manually (right now just `IGameCtnChallenge`, `IGameCtnBlock`, and their game variants)
- **Reworked** and fixed `CGameBlockItem` *(breaking change)*
  - `CustomizedVariants` is now `IList<CGameBlockItem.Mobil>` instead of `Dictionary<int, CPlugCrystal>`
  - Fixed bugs of TM2020 custom blocks
- Connected `CGameCtnChallenge` medal times with `CGameCtnChallengeParameters` medal times (no desync)
- Allowed `CGameCtnChallenge` `MapUid` and `HashedPassword` to be modified without importing CRC32
- Logs now show if skippable chunk is unknown or not
- Fixed `CGameCtnAnchoredObject.SnappedOnBlock` when the block is removed from the map
- Fixed write of `CGameItemModel.VisModelCustom`
- Published some internal attributes
- Minor fixes and nullability tweaks

## [GBX.NET 2.0.0](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.0) - 2024-04-26

ITS REAAAAL!

## [GBX.NET 2.0.0-rc1](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.0-rc1) - 2024-04-23
**[PRE-RELEASE]**

- Added `CGameCtnMediaBlock.IHasKeys` and `CGameCtnMediaBlock.IHasTwoKeys` to connect MT block behavior together
- Added `Gbx` to `CMwNod` and `Gbx<T>` to `T` implicit operators
- Added `CPlugTree.GetAllChildren()` to recursively get tree nodes
- Changed `LeaveOpen` to `CloseStream`, default is `false`
- Fixed `CPlugTree.Name` in how it is saved
- Fixed `TrackMania` namespace MT blocks not inheriting `CGameCtnMediaBlock`

## [GBX.NET 2.0.0-beta2](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.0-beta2) - 2024-04-21
**[PRE-RELEASE]**

- Fixed read/write of `CGameCtnMediaBlockFxBlurDepth.Key`
- Added possibility to skip skippable chunks using `SkipChunkIds` as a replacement of the "discover" feature
  - Does not work for unskippable chunks just yet (in that case it would throw an exception if it is found)
- Added trace level logs to measure how long certain chunks took to process

## [GBX.NET 2.0.0-beta1](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.0-beta1) - 2024-04-20
**[PRE-RELEASE]**

- Added several unused MediaTracker blocks
- Implemented the rest of the MediaTracker blocks
- Added class ID remap functionality (stored in `Gbx`, otherwise on serialization specify `ClassIdRemapMode`)
  - Modes: `Latest` (TM2+ and mostly TMUF), `Id2008` (same as `Latest` except for `CGameCtnCollector`, TMUF), `Id2006` (TMU and lower)
- Added reference table overview (`Files` and `Resources`) to `GbxRefTable`
  - During serialization, data from this will be picked only if raw body is used (`ReadRawBody` setting)
- Added `TransQuat`
- Fixed `ClassManager.GetName` picking the oldest name instead of the latest
- Fixed empty UserData being written as 4 bytes with 0 header chunks
- General chunk fixes and `(external)` additions

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v2.0.0-alpha3...v2.0.0-beta1

## [GBX.NET 2.0.0-alpha3](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.0-alpha3) - 2024-04-18
**[PRE-RELEASE]**

- Added 70+ classes from GBX.NET v1
- Implemented many unimplemented MediaTracker blocks
- Added `GbxWriteSettings` `PackDescVersion`
- Added `Int3` math operators
- Fixed `GbxReader` state not passed correctly after reading compressed body

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v2.0.0-alpha2...v2.0.0-alpha3

## [GBX.NET 2.0.0-alpha2](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.0-alpha2) - 2024-04-14
**[PRE-RELEASE]**

- [`GBX.NET.ZLib`](https://www.nuget.org/packages/GBX.NET.ZLib/1.0.0-alpha2) is now public
- Added missing members of `CPlugEntRecordData` and `CGameGhost` (samples)
- Added `GameVersion.VSK5`
- Added automatic generation of `AppliedWithChunkAttribute`
- Fixed `CGameCtnGhost` `0x025` for TM2020
- Fixed `prevChunkId` for explicit parse
- Updated NuGet package properties

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v2.0.0-alpha1...v2.0.0-alpha2

## [GBX.NET 2.0.0-alpha1](https://github.com/BigBang1112/gbx-net/releases/tag/v2.0.0-alpha1) - 2024-04-12
**[PRE-RELEASE]**

GBX.NET 2 begins!

## [GBX.NET 1.2.6](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.6) - 2024-02-19

- Fixed `CGameCtnGhost` `0x000` `v9` causing parse errors in autosave replays https://github.com/BigBang1112/gbx-net/pull/90/commits/04b09d30e92dfa3f2e558e54fe30ec31f8aac4ac
- Added `CGameCtnBlockUnitInfo` `0x00D` https://github.com/BigBang1112/gbx-net/pull/90/commits/7c162b4d52893387cbb2307255e453acceb43db1
- Added `CPlugVehicleMaterial` `0x004` https://github.com/BigBang1112/gbx-net/pull/90/commits/63a53d5c797c54a91d794c20504725b472b93628 https://github.com/BigBang1112/gbx-net/pull/90/commits/d2d1dcf5eec84f2332de661d73fa8ad41522340b https://github.com/BigBang1112/gbx-net/pull/90/commits/15d09cb1267cd4fd8fefefa743aec7535a405cbb
- Implemented `MoveShape` and `CarVsCarShapeBox` to `CPlugVehicleCarPhyShape` https://github.com/BigBang1112/gbx-net/pull/90/commits/0e7625244da6f10323ca0ccf579995dae61f94ab
- Fixed `CPlugIndexBuffer` for new meshes https://github.com/BigBang1112/gbx-net/pull/90/commits/1ff7b9d0d1f35787165e01f6083eda9bfec309ec
- Fixed `Int4.Zero` https://github.com/BigBang1112/gbx-net/pull/90/commits/5cb3db7153304c8684467c45d2e4660753df3eb9

* Chunk 0x090F1004 implementation by @Mystixor in https://github.com/BigBang1112/gbx-net/pull/86

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v1.2.5...v1.2.6

## [GBX.NET 1.2.5](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.5) - 2023-12-31

- Implemented and fixed `CGameCtnMediaBlockCameraOrbital` [648c0ba](https://github.com/BigBang1112/gbx-net/pull/85/commits/648c0ba1468776c1989f5e12cee8826043dc3021)
- Implemented `CPlugFileGen` [22db2f4](https://github.com/BigBang1112/gbx-net/pull/85/commits/22db2f44654865b2c32b8ed7abaffce19e3c5ba7)
- Improved handling of CRC32 calculation [0179bdf](https://github.com/BigBang1112/gbx-net/pull/85/commits/0179bdf81d90c7be539c2fae219573eb868fc0a5)
- Fixed `CGameCtnMediaBlockEntity` `0x000` `U14` to be `SkinOptions` [8fc3b5b](https://github.com/BigBang1112/gbx-net/pull/85/commits/8fc3b5b75fa9d655d9375de7e775450f66cfb11c) [d9b28da](https://github.com/BigBang1112/gbx-net/pull/85/commits/d9b28da9c04e3267847ba3f02096d54d6f9426ac)
- Removed `Version` from `Chunk2E002009` [52eb086](https://github.com/BigBang1112/gbx-net/pull/85/commits/52eb08620d9ced58f94b0cfbdb12cb060ad34241)
- Updated TmEssentials to 2.4.0 [ee2ddd9](https://github.com/BigBang1112/gbx-net/pull/85/commits/ee2ddd9bf0bb6f5c55a8cbf5de95ca700a948660)

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v1.2.4...v1.2.5

## [GBX.NET 1.2.4](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.4) - 2023-11-26

- **Fixed TM2020 input reading bug when car transformation is used** https://github.com/BigBang1112/gbx-net/pull/84/commits/6e92668dd21bdfb9a4715ad460cf77ccae011d32 https://github.com/BigBang1112/gbx-net/pull/84/commits/5eb5ccdcfd110cda3d6e3c55b4bdad1f2db1a813
- Added CRC32 calculation on map save. **No more random password prompts!** https://github.com/BigBang1112/gbx-net/commit/3a5c16c7ec0fcacd32d9c7502fd9bd5b292824cf
- Added `CPlugTree` `0x017` https://github.com/BigBang1112/gbx-net/pull/84/commits/480163f0e9ef7335efc30b739cb18601d6704f5d
- Fixed `CGameCommonItemEntityModel` and `CPlugStaticObjectModel` (thx schadocalex)
- Fixed `CPlugSurface` `0x003` v4
- Removed warning from `CGameItemPlacementParam` `0x005` https://github.com/BigBang1112/gbx-net/pull/84/commits/50e8787b509e7bae61e7948d8bea5ca139156b19
- Connected `CGameCtnChallenge` `0x06B` with dynamic daytime https://github.com/BigBang1112/gbx-net/pull/84/commits/7990cee9a9ebe9d0ab191a216e2b2ebfab7f40ed

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v1.2.3...v1.2.4

## [GBX.NET 1.2.3](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.3) - 2023-11-24

- Added support for .NET 8 and C# 12
- Added `CScriptTraitsMetadata.Create()`
- Added `CGameCtnGhost` `0x000` version 9 support https://github.com/BigBang1112/gbx-net/pull/80/commits/445f59746bca28975e06ad0ae0479bdb45fd9e17
- Added `CGameCtnMediaBlockEntity` `0x000` version 11 support https://github.com/BigBang1112/gbx-net/pull/80/commits/de258948e507fa1a664bd283baa910817d231c90
- Added `CGameCommonItemEntityModelEdition` `0x000` version 8 support https://github.com/BigBang1112/gbx-net/pull/80/commits/26b1c4bd2263e2d91dd064361413334f35883862
- Added `CGameCtnChallenge` `0x06B` without knowing its content yet https://github.com/BigBang1112/gbx-net/pull/80/commits/479a8588e59fc34f9bd4c6b182df2f5b5f590dd5
- Fixed `CGameBlockItem` `0x000` version 1 (still incomplete) https://github.com/BigBang1112/gbx-net/pull/80/commits/4b87694125dd778eb0d87018b8a5a98a80d8f27a
- Fixed parse state after `CGameCtnGhost` `0x025` causing weird ghost errors https://github.com/BigBang1112/gbx-net/pull/80/commits/6c39f55c2d422e6eca8c0c2c7b6593426af8df69
- Fixed missing `RequiresUnreferencedCode`
- Tweaked [Gbx Explorer](https://gbxexplorer.net) look (Ctrl+F5)

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v1.2.2...v1.2.3

## [GBX.NET 1.2.2](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.2) - 2023-07-08

- Added `CGameCtnChallenge.CreatedWithGamepadEditor` https://github.com/BigBang1112/gbx-net/pull/76/commits/ae57f625ab05c1c49c0fb09b3d13a6658b95f1a2
- Added `IInput.WithTime()` https://github.com/BigBang1112/gbx-net/pull/76/commits/0e800b257af7f2f51ccd4b764284f80b8184becc
- Fixed cases with `CGameObjectVisModel` https://github.com/BigBang1112/gbx-net/pull/76/commits/b7df83b94b06f505b2978404d40ae773bd4c5396

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v1.2.1...v1.2.2

## [GBX.NET 1.2.1](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.1) - 2023-06-07

- **Breaking:** Enhanced `CPlugVertexStream` (from now on, breaking changes will be reported in this class)
- [Added `CGameCtnZone.Height`](https://github.com/BigBang1112/gbx-net/pull/74/commits/2a69ce04127d8b006066e2167b1e324bccbd660d)
- [Added `CPlugMediaClipList`](https://github.com/BigBang1112/gbx-net/pull/74/commits/e4930bed996460d012fcd7202a20f6f224c8b9c8)
- [Added `CGameCtnGhost.SteeringWheelSensitivity`](https://github.com/BigBang1112/gbx-net/pull/74/commits/b69affff3d18875130db2bfbb640c10bfd54bbc5)
- [Added `CPlugVisual.VisualSkinData`](https://github.com/BigBang1112/gbx-net/pull/74/commits/dcdead248e6d791df149fd57e6fc9e026bcef6a7)
- Improved node export speed from O(n) to O(1)
- **Breaking:** [Moved `CPlugVehiclePhyTuning` from `Scene` to `Plug` namespace](https://github.com/BigBang1112/gbx-net/pull/74/commits/b20645fe3782b0c83d5d4d6ba0ecec7002204aed)
- Fixed `CScriptTraitsMetadata` version 6, added protection against newer versions
- Added trimming annotations (regarding LZO auto-detection)

### Gbx Explorer

- Fixed possible XSS

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v1.2.0...v1.2.1

## [GBX.NET 1.2.0](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.0) - 2023-05-03

A lot of updates with ghost samples and vehicle tunings.

Breaking changes will be described soon.

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v1.1.3...v1.2.0

## Pull requests
* Add GetCrossProduct by @Mystixor in https://github.com/BigBang1112/gbx-net/pull/67
* Fix back to back formatting not working properly by @thaumictom in https://github.com/BigBang1112/gbx-net/pull/68
* Extend vehicle state parsing tm2020 by @dtuit in https://github.com/BigBang1112/gbx-net/pull/69

## New Contributors
* @Mystixor made their first contribution in https://github.com/BigBang1112/gbx-net/pull/67
* @dtuit made their first contribution in https://github.com/BigBang1112/gbx-net/pull/69

## [GBX.NET 1.2.0-beta2](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.0-beta2) - 2023-04-30
**[PRE-RELEASE]**

## [GBX.NET 1.2.0-beta1](https://github.com/BigBang1112/gbx-net/releases/tag/v1.2.0-beta1) - 2023-04-25
**[PRE-RELEASE]**

A lot of updates with ghost samples and vehicle tunings!

## [GBX.NET 1.1.3](https://github.com/BigBang1112/gbx-net/releases/tag/v1.1.3) - 2023-02-01

- **New Input API** (updated documentation soon)
  - `ControlEntry[] ControlEntries` is replaced with `IReadOnlyCollection<IInput> Inputs`
    - **`ControlEntries` is now obsolete and will be removed in 1.3.0**
    - Range -65536 to 65536 is now preferred instead of -1 to 1 (which can be retrieved with `GetValue()` instead).
  - `IList<CGameCtnGhost.PlayerInputData.IInputChange> InputChanges` with `IReadOnlyCollection<IInput> Inputs`
    - Removal of `InputChanges` will be decided once the whole input behavior from TM2020 and Shootmania is figured out
- Fixed a lot of the input behavior in TM2020, mainly with Horn and Character inputs
- Fixed more cases with `CGameCtnMediaBlockEntity`
- Fixed script metadata with lengths above 127
- Improved reading of large arrays
- Updated some collections in `CGameCtnReplayRecord`
- `CGameCtnReplayRecord.ChallengeData` is now public

Breaking changes include only cases with array modification that is not useful.

## [GBX.NET 1.1.2](https://github.com/BigBang1112/gbx-net/releases/tag/v1.1.2) - 2023-01-21

- Added TM2020 action keys, look back, and secondary respawn input support
  - Horn is currently unsupported
- Added `CGameItemModel` `0x022`, fixed `0x01E`
- Added `CGameCtnMediaBlockSpectators`
- Enhanced `CPlugSpriteParam`
- Enhanced `CPlugVisualSprite`
- Enhanced `CGameCtnMediaBlockEntity` with a lot of properties
- Enhanced `CGameCtnReplayRecord` with `PlaygroundScript`, `InterfaceScriptInfos`, and some scenery things
- Updated `CPlugSolid2Model.ShadedGeom` and `CPlugVisual.TexCoord` with property names
- **Fixed issue with skinnable blocks in `BakedBlocks` (causing many issues with recent maps)**
- Fixed issues with sprite elements in TM2 solids
- Fixed `AppliedWithChunkAttribute` on `CGameCtnChallenge.MapName`
- Fixed 0 header chunk still writing to header data
- Fixed `CGameCtnCollector` `0x008`
- Fixed `CGameObjectVisModel` `0x001`
- `Node.WriteAsync` is now `internal`

### GBX.NET Explorer
- Fixed issues with certain collections not being expandable

### [Breaking changes](https://github.com/BigBang1112/gbx-net/wiki/Breaking-changes#112)

## [GBX.NET 1.1.1](https://github.com/BigBang1112/gbx-net/releases/tag/v1.1.1) - 2022-12-30

### See GBX.NET Explorer for simple preview: https://gbxexplorer.net/ (verify if you're on v1.1.1.0)

- Added TM2020 and Shootmania input reading support
  - Accessible through `CGameCtnGhost.PlayerInputs.InputChanges`
  - `InputChange` was transformed into `TrackmaniaInputChange`
  - `ShootmaniaInputChange` was added for Shootmania inputs
  - Similarities are packed under `IInputChange`
- Added `CGameCtnMediaBlockTriangles3D.Create()`
- Added `CGameCtnDecorationSize.Size`
- Added `CSceneObject.Motion`
- Added `CPlugSolid` `SolidPreLightGen` and `FileWriteTime`
- Added `Mat3`
- Changed `CGameCtnCollector` `Author` to `Ident`
- Changed `CGameCtnGhost.Data` `NodeID` to `SavedMobilClassId`
- Fixed `CControlEffectSimi` on old maps (usually problematic with Randomizer TMF)
- Fixed NodeRef writing of external nodes
- Fixed many cases of `CPlugVisual3D`
- Fixed `CPlugSurface.Box` read/write
- Fixed other `CPlugSurface` problems
- Fixed `Mat4.Zero`

### GBX.NET Explorer

- Fixed GitHub link branch on the `master` build

### GBX.NET.PAK 1.1.2

- Added `NadeoPakFile.GetDirectoryName()`

### [Breaking changes](https://github.com/BigBang1112/gbx-net/wiki/Breaking-changes#111)

## [GBX.NET 1.1.0](https://github.com/BigBang1112/gbx-net/releases/tag/v1.1.0) - 2022-12-11

Too many things to mention again.

Breaking changes will be described soon.

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v1.0.1...v1.1.0

## [GBX.NET 1.1.0-alpha2](https://github.com/BigBang1112/gbx-net/releases/tag/v1.1.0-alpha2) - 2022-11-11
**[PRE-RELEASE]**

This release was made to test the async methods in practice. It also includes .NET 7 build, and support for Mesh.Gbx, Shape.Gbx, and extended Item.Gbx and Solid.Gbx support.

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v1.1.0-alpha1...v1.1.0-alpha2

## [GBX.NET 1.1.0-alpha1](https://github.com/BigBang1112/gbx-net/releases/tag/v1.1.0-alpha1) - 2022-10-25
**[PRE-RELEASE]**

This release has been made to test the new source-generated way to determine the classes from IDs. Full patch notes are coming in the final release.

**Automatically generated changelog**: https://github.com/BigBang1112/gbx-net/compare/v1.0.1...v1.1.0-alpha1

## [GBX.NET 1.0.1](https://github.com/BigBang1112/gbx-net/releases/tag/v1.0.1) - 2022-09-10

- Fixed embedded object writing (thx nbert)
- Fixed `PlayerInputs` not properly loading
- Resolved `CGameCtnGhost` `0x015`
- `CGameCtnChallenge` `0x008` version now defaults to 1

**GBX.NET Explorer**
- Fixed **Export selected node to Gbx** not allowing to change extension

## [GBX.NET 1.0.0](https://github.com/BigBang1112/gbx-net/releases/tag/v1.0.0) - 2022-09-09

- Added `Translation` and `FuncTree` to `CPlugTree`
- Added and fixed more attributes
- Added more "decorative" chunks
- Improve crystal materialization when exporting to .obj (thx frolad)
- Added optional parameter `alsoInClips = true` to `GetGhosts()` of `CGameCtnReplayRecord`
- Made block positions consistent between `Blocks` and `BakedBlocks` in `CGameCtnChallenge`
- `GameBox` `Compress`/`Decompress` now return bool if already compressed or not (explained in summary)
- `EventsDuration` is now of type `TimeInt32`
- Extended flexibility of ObjFileExporter
- Fixed external node writing
- Improved logging

## [GBX.NET 0.16.6](https://github.com/BigBang1112/gbx-net/releases/tag/v0.16.6) - 2022-08-24

- Refactored state store to fix general save errors, mostly related to maps
- Added new properties to `CGameCtnAnchoredObject`:
  - `SnappedOnBlock` - on which deleted block should also the item be deleted
  - `SnappedOnItem` - currently has no real effects (in tm2020)
  - `SnappedOnGroup` - about 0-5 value range, groups the item deletion into groups
  - `PlacedOnItem` - similar idea to `SnappedOnItem` but for MP4 and special TM2020 cases
  - `AnimPhaseOffset` - animation phase offset (delay) of those cool Royal moving items
  - `ForegroundPackDesc` - second layer item skin
  - `LightmapQuality` - some new form to define specific lightmap calculation quality on an item
  - `MacroblockReference` - reference to the macroblock instance that placed this item
- Added new properties to `CGameCtnBlock`
  - `LightmapQuality`
  - `MacroblockReference`
- Added `MacroblockInstances` to `CGameCtnChallenge` - simple macroblock infos placed on TM2020 maps
- Added `PlayerInputs` to `CGameCtnGhost` just to troll everyone
- Added more chunks to recognize
- Fixed `CGameCtnChallenge` `0x040` chunk for all versions 4+, and all general saving problems in TM2020
- Fixed `TimeOfDay` read/write consistency (thx frolad for help)
- Fixed `GameBoxReaderWriter.ArrayId()`
- Fixed `CGameCtnGhost.Checkpoint` `ToString()`

## [GBX.NET 0.16.5](https://github.com/BigBang1112/gbx-net/releases/tag/v0.16.5) - 2022-08-21

The library has reached 1MB!
- Added a lot more attributes
- Added `AsHeader` to every class implementing `INodeHeader`
- Added a bit more versioning protection to certain chunks (still not enough though)
- Added `CGameCtnChallenge.BakedClipsAdditionalData` and `CGameCtnChallenge.NbBakedBlocks`
- Added cam-related members to `CGameCtnMediaBlockCameraGame`
- Fixed `CGameCtnChallenge.BakedBlocks` in TM2
- Fixed old collector icon being horizontally flipped
- Fixed `KindInHeader` not pointing at the correct field
- Improved map chunks related to real-time thumbnail cameras
- Attempt to optimize block parse

## [GBX.NET 0.16.4](https://github.com/BigBang1112/gbx-net/releases/tag/v0.16.4) - 2022-08-19

- Separated `CGameCtnChallenge.Kind` into `KindInHeader` and `Kind`, as they seem to have different values without external modification.
- Added warning log to a case of a weird Id version
- Fixed `CGameCtnGhost` `0x000` chunk version 8
- Fixed `CGameCtnChallenge` `0x048` (baked blocks) chunk `Id` handling

## [GBX.NET 0.16.3](https://github.com/BigBang1112/gbx-net/releases/tag/v0.16.3) - 2022-08-18

- Added high-level header chunk support to more classes
- Added more attributes to members
- Added `CGameCtnDecoration` `0x001` header chunk
- Added `CPlugVisual` `0x00C` chunk that fixes ESWC solids
- Added `CPlugShaderApply` `0x006`
- Fixed macroblocks for TM2020
- Fixed `CGameCtnAnchoredObject` from ManiaPlanet 3
- Fixed `CGameCtnChallenge` `0x040` item chunk for all kinds of versions (without proper write support above version 4)
- Fixed naming of certain `CPlugTree`s
- Fixed `AuthorNickname`, `AuthorZone`, and `AuthorExtraInfo` being improperly set in ManiaPlanet and TM2020
- Removed `Dependencies` from `CMwNod`

## [GBX.NET 0.16.2](https://github.com/BigBang1112/gbx-net/releases/tag/v0.16.2) - 2022-08-11

- Improved `CSceneMobil` support
- Added more attributes
- Added nullability support to `GBX.NET.Imaging`
- Recognized a few `CGameCtnGhost` chunks
- Fixed some obj export thingies
- Fixed `CGamePlayerProfile` header chunk

## [GBX.NET 0.16.1](https://github.com/BigBang1112/gbx-net/releases/tag/v0.16.1) - 2022-07-22

This is one of the few upcoming revisions for 0.16 during the summer.

- Fixed `AuthorLogin` not showing on TMUF maps
- Fixed *.Mat.Gbx parse on older versions
- Fixed async parse
- Expanded `AppliedWithChunkAttribute` usabilities for future use

## [GBX.NET 0.16.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.16.0) - 2022-07-16

Way too much stuff to cover.

### What's Changed
* GBX.NET 0.16.0 by @BigBang1112 in https://github.com/BigBang1112/gbx-net/pull/48

### New Contributors
* @MKuijpers made their first contribution in https://github.com/BigBang1112/gbx-net/pull/49

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v0.15.1...v0.16.0

## [GBX.NET 0.15.1](https://github.com/BigBang1112/gbx-net/releases/tag/v0.15.1) - 2022-04-04

Pull request: https://github.com/BigBang1112/gbx-net/pull/43

- Added `CGameCtnMediaBlockOpponentVisibility`
- Added WebP support for `CGameCtnCollector` icons
- Added support for `bufferType = 6` in `CPlugEntRecordData`, fixing transform sample parse for newly generated ghost files
- Partially fixed the item embedding bug

**Full changelog**: https://github.com/BigBang1112/gbx-net/compare/v0.15.0...v0.15.1

## [GBX.NET 0.15.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.15.0) - 2022-02-23

* GBX.NET 0.15.0-rc by @BigBang1112 in https://github.com/BigBang1112/gbx-net/pull/37
* GBX.NET 0.15.0-rc2 by @BigBang1112 in https://github.com/BigBang1112/gbx-net/pull/38
* GBX.NET 0.15.0 by @BigBang1112 in https://github.com/BigBang1112/gbx-net/pull/39

**Full Changelog**: https://github.com/BigBang1112/gbx-net/compare/v0.14.3...v0.15.0

## [GBX.NET 0.15.0-rc2](https://github.com/BigBang1112/gbx-net/releases/tag/v0.15.0-rc2) - 2022-02-19
**[PRE-RELEASE]**

https://github.com/BigBang1112/gbx-net/pull/38

## [GBX.NET 0.15.0-rc](https://github.com/BigBang1112/gbx-net/releases/tag/v0.15.0-rc) - 2022-02-01
**[PRE-RELEASE]**

Too many changes to write down, see https://github.com/BigBang1112/gbx-net/pull/37.

## [GBX.NET 0.14.3](https://github.com/BigBang1112/gbx-net/releases/tag/v0.14.3) - 2021-12-19

- Temporarily ignore data about free blocks (the project is not too far to figure these out finally)
- Removed `AbsolutePositionInMap` and `PitchYawRoll` from `CGameCtnBlock`
- Removed `PlaceFreeBlock()` from `CGameCtnChallenge`
- Made IsFree unsettable
- Byte3/Int2/Int3/Vec2/Vec3/Vec4's `ToString()` is formatted into `<x, y, z>` instead of `(x, y, z)`
- `Discover()` method wont work on ignored chunk
- Added weird `Id` parsing methods
- Added deconstructors to Byte3, Int2, Int3, Vec2, Vec3, Vec4, Quaternion and Ident

## [GBX.NET 0.14.2](https://github.com/BigBang1112/gbx-net/releases/tag/v0.14.2) - 2021-12-12

- Experimentally removed a check of `AvailableInheritanceClasses` when parsing chunks to solve an issue related to `CGameCtnMediaBlockTimeSpeed`, also speeding up the parsing by a little bit
- Fixed a bug when writing compressed body after parsing only a header with `readRawBody: true`
- Fixed `GameBoxReader.ReadUntilNextChunk()`

## [GBX.NET 0.14.1](https://github.com/BigBang1112/gbx-net/releases/tag/v0.14.1) - 2021-12-03

- Added `readRawBody` parameter to `GameBox.ParseHeader` and `GameBox.ParseNodeHeader` allowing to modify header values without reading the full GBX file
  - In this mode, changing the compression type is forbidden and `HeaderOnlyParseLimitationException` can be thrown by `set;`
- Changed `X`, `Y`, `ScaleX` and `ScaleY` in `CControlEffectSimi` to `Position` and `Scale` by using `Vec2`
- Added `Centered()`, `WithColorBlendMode()`, `ContinousEffect()` and `Interpolated()` methods to `CControlEffectSimi` builders
- **Replaced `File.OpenWrite()` with `File.Create()`**
- Updated defaults for `CGameCtnMediaBlockSoundBuilder`
- `GameBoxReader.ReadToEnd()` now prefers non-seeking solution by default
- Split some generic classes into files with `OfT.cs` suffix
- Fixed `CGameCtnMediaBlockTextBuilder` not applying the color
- Fixed `Chunk.Remap()` fatal bugs related to downgrading (down-mapping)
  - More GBX files should be now saveable in TMU and lower versions
- Fixed `GameBox.TryNode()`

## [GBX.NET 0.14.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.14.0) - 2021-11-27

- **Library support has been shifted up to .NET 6. Support for .NET Framework 4.5 was dropped and replaced with .NET Framework 4.8.** It is still possible for the framework support to be widened in the future.
- **Added 5 new node builders**
  - `CGameCtnMediaClip.Create()`
  - `CGameCtnMediaTrack.Create()`
  - `CGameCtnMediaBlockText.Create()`
  - `CGameCtnMediaBlockSound.Create()`
  - `CControlEffectSimi.Create()`
- Added some ghost sample data findings to `CPlugEntRecordData` by TheMrMiku and the TMDojo team
- Expanded `CGamePlayerProfile` (Profile.Gbx)
- Added `CGameNetOnlineMessage`
- Added `CGameLeague`
- Added support for `CGameCtnMediaBlockTimeSpeed` and `CGameCtnMediaBlockCameraOrbital`
- Added `GameBox.Decompress()` static method that turns a GBX file into an uncompressed version while still being compatible with the game
  - GBX Decompressor tool is available in the Tools folder
- Reduced overloads across the whole project more towards default parameters
- **Returning values of `ParseNode` and `ParseNodeHeader` are now marked as nullable (they can be null if the node is not recognized by the library)**
- All `GameBoxReaderWriter` methods now won't change the returned value to its written default after writing
- All `CMwNod.Parse()` methods are now internal
- Enhanced documentation across `GameBoxReader`, `GameBoxWriter`, `GameBoxReaderWriter`, but it still isn't complete because it takes ages xd
- Generalogies in `CGameCtnChallenge` are back
- Chunk's `ToString` now includes the version of the chunk
- Added `NodeCacheManager.GetNodeInstance()`
- Added `GameBoxReader.ReadBytesUntilNextChunk()`
- Added `GameBoxReaderWriter.Uri()`
- Lists reads/writes have been rewritten more towards iteration behavior
- Time-related methods in `GameBoxReaderWriter` are now preferring truncating over rounding
- Added `AppliedWithChunkAttribute` that will be used later in the updates to improve the reference/documentation of chunks availabilities across different Trackmania versions
- Moved attributes to `GBX.NET.Attributes` and exceptions to `GBX.NET.Exceptions`
- `GameBox` constructor is no longer exposed to guide the beginners towards static methods
- `Formatter` has been replaced with the one from `TmEssentials`
- `CGameCtnBlock.AbsolutePositionInMap` is now a nullable property
- Ghost sample data now expose full data buffers instead of only showing the unknown buffers
- `CGameCtnGhost.SampleData` will be `null` if its node ID is `-1`
- **Fixed LZO methods not being statically defined**
- Fixed bugs related to `GameBoxReaderWriter.EnumInt32()`, possibly fixing the camera game bug
- **Fixed the Y block coordinate on ManiaPlanet maps**
- Fixed `CGameCtnGhost` chunk `0x015` modifying the `PlayerModel` property even though it shouldn't
- Code now fully uses file-scoped namespaces and implicit usings

The library has also started to add hundreds of unit and integration tests and this process is expected to expand on for the next years.

## [GBX.NET 0.13.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.13.0) - 2021-10-26

- Use TmEssentials library
- Use C# 9 (C# 10 soon)
- Use nullable reference types
- Add more summaries
- Add specialized exceptions
- Improved parse speed of map blocks
- Parse methods should not return null (this case might still exist) but throw exceptions
- Replace `Task<CGameCtnChallenge> Challenge` with `CGameCtnChallenge Challenge` and `GetChallengeAsync`
- Replace `Task<CGameCtnGenealogy> Genealogies` to `CGameCtnGenealogy Genealogies`
- Fixed `CGameCtnMediaBlockCameraEffectScript` chunk version 0
- Fixed reading of `CGameCtnMediaBlockFxBlurDepth`
- Added some Plug engine progress
- Fixed exception on item's and block's property `Color`
- Fixed exception caused by ManiaPlanet 3 metadata chunk
- Preparing new debugging methods, currently only available in Debug configuration
- **Remove public constructors from nodes** (alternative solution coming in 0.14)

## [GBX.NET 0.12.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.12.0) - 2021-08-26

- Added PAK3 file parsing
- Fixed reference table writing
- Decreased the whole parse time by almost 3 times

## [GBX.NET 0.11.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.11.0) - 2021-06-25

- Added new `GameBox.ParseNode` methods, the new conventional way to parse GBX with GBX.NET
- `Node` has been transferred to `CMwNod`
- First parse of GBX has been sped by more than 60%
- All MediaTracker block `Key[]` have been changed to `IList<Key>`
- Added `CMwNod.Save` method to simplify GBX saving
- Added support for TM2020 block colors
- Added `IVersionable` to group versionable chunks
- Added `IsValidatedForScriptModes` to `CGameCtnChallengeParameters`
- Added more samples and updated current samples
- Added `Byte(int)` to reader-writer
- Added `CGameCtnReplayRecord.GetGhosts`
- Fixed map metadata parse
- Fixed replay ghost validation chunk writing
- Fixed ghost checkpoints to be nullable
- Removed more obsoletes

Renames:
- Renamed `GameBox<T>.MainNode` to `GameBox<T>.Node`

## [GBX.NET 0.10.2](https://github.com/BigBang1112/gbx-net/releases/tag/v0.10.2) - 2021-06-06

- Fixed exception related to `EventsDuration == 0`
- Fixed problems with CGameCtnMediaBlockTime

## [GBX.NET 0.10.1](https://github.com/BigBang1112/gbx-net/releases/tag/v0.10.1) - 2021-06-05

- Added `GBX.NET.IO`
- Added null check on `GameBox.AssignBodyToNode`
- Fixed parse of `CGameCtnMediaTrack` `0x004`
- Other fixes

## [GBX.NET 0.10.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.10.0) - 2021-06-01

Version 0.10.0 is finally out together with GBX.NET.LZO and GBX.NET.Imaging.

The library successfully drops System.Drawing.Common from its needed supports, making the library package-independent in .NET Standard 2.0.

- **The library is now separated into GBX.NET and GBX.NET.LZO**
  - GBX.NET, GBX.NET.Imaging, and GBX.NET.Json are now distributed under MIT
  - GBX.NET.LZO stays GNU GPL 3.0, which means you have to use GNU GPL 3.0 after referencing
  - More details available in [README](https://github.com/BigBang1112/gbx-net/blob/master/README.md)
- **Added support for TM2020 ghost sample data reading**
  - `CPlugEntRecordData` has been successfully reverse-engineered
  - The ghost now provides the same data as a usual TM ghost (position, rotation, velocity, speed)
  - Shootmania ghost samples are also retrievable, but the only position that is accurate
- `CSystemConfig` (.SystemConfig.Gbx) is now officially supported
- **Added support for input reading ranging TM1.0 - TM2**
  - **Getting TM2020 to work is [likely impossible](https://github.com/BigBang1112/gbx-net/issues/25)**
- Added experimental `readUncompressedBodyDirectly` parameter recommended for parsing internal GBX files from the internet
- Added support for reading nodes in streams with `CanSeek: false`
- **Added map lightmap read/write support**
  - Methods currently missing in GBX.NET.Imaging
- Map thumbnail is now presented as a byte array
  - GBX.NET.Imaging has extensions that convert them into Bitmap
- Collector icons are now presented as 2D arrays of colors
  - GBX.NET.Imaging has extensions that convert them into Bitmap
- Added `INodeHeader` to `CGameCtnChallenge` and `CGameCtnReplayRecord`
  - An interface that shows only the properties that can be read from the header
- Added `CGameCtnMediaBlockTimeSpeed`
- Added `CCtnMediaBlockUiTMSimpleEvtsDisplay`
- Added `GameBox.TryNode` method
- `DiscoverAll` method has now parallel and serial methods
- Enhanced `CGamePlayerProfile`
- `GameBoxReader.ReadArray` method now has an optional `length` parameter
- Fixed ghost parse for TM Turbo
- Fixed ghost checkpoints for TMUF and lower ghosts
- Fixed light trail color for TM2 not showing
- Fixed BakedBlocks property not discovering its chunks
- Fixed name collisions with reference table classes
- Removed more obsoletes
- Class reorganizations
- Tons of more minor changes

Renames:
- Renamed `CGameGhost.Data` to `CGameGhost.SampleData`
- Renamed `MediaBlockKey` to `CGameCtnMediaBlock.Key`
- Renamed `MapOrigin` and `MapTarget` to `MapCoordOrigin` and `MapCoordTarget`
- Renamed `Embeds` to `EmbeddedObjects`
- Renamed `SecondaryPackDesc` to `ForegroundPackDesc`

## [GBX.NET 0.9.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.9.0) - 2021-03-04

- **Project now supports AnyCPU and x64 for reading purposes** 
- New parsing strategy so that GBX streams with `CanSeek` set to `false` can be parsed
- **Improved ghost's control entries to support analog input** with `ControlEntryAnalog` class
- Added a new contrustor `GameBox<T>(T node)` to make saving `Node` to GBX format easier
- Added `CGameCtnGhostData.GetSampleLerp` for sample data interpolation
- Added `CGameCtnChallenge` chunks `0x01A`, `0x01B`, `0x01D`
- Added `CGameCtnReplayRecord` chunk `0x018`
- Added `CGameCtnGhost` chunk `0x025`, fixed parse of `0x000`
- Added `AnchorTreeId` to `CGameCtnAnchoredObject `
- Added `TriggerSize` to offzones
- Added `CompressedStream` to reduce repetitiveness
- Added better handle of ghost data exceptions
- Added more classes to ClassID.txt
- Added `GameBoxReader.HasMagic` method
- Added `TimeSpan.ToStringTM` method for a string representation similar to Trackmania's
- Added `Lerp` methods for `Vec2`, `Vec3` and `Quaternion`
- `DiscoverAll` methods now discovers chunk in parallel
- Block coords now substract by 1,1,1 only if chunk version >= 6
- Changed `FileRef` `LocatorUrl` to `Uri` type
- Removed DebugViews on nodes
- Removed more obsolete features

## [GBX.NET 0.9.0-rc](https://github.com/BigBang1112/gbx-net/releases/tag/v0.9.0-rc) - 2021-02-28
**[PRE-RELEASE]**

Full changelog will come on full release.

## [GBX.NET 0.8.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.8.0) - 2021-01-29

- **Item.Gbx and Block.Gbx parsing has been expanded**
  - Added CGameCommonItemEntityModelEdition
  - **Added support for mesh reading (+ ToOBJ method converting the mesh to .obj) through CPlugCrystal**
  - CGameItemPlacementParam now reflects map editor placement values
  - New properties for CGameItemModel and CGameBlockItem
  - Added CPlugVehiclePhyModelCustom for custom vehicle item reading
- Data type classes rename
  - `LookbackString` to `Id` and `Meta` to `Ident`, including renames in `GameBoxReader`, `GameBoxWriter` and `GameBoxReaderWriter`
- Wrongly parsed unskippable chunk now throws an exception
- Added new chunk support:
  - CGameCtnChallenge: 0x050 (offzones), 0x052 (deco height), 0x053 (bot paths), 0x056 (light settings)
  - CGameCtnMediaBlockCameraGame: 0x005
  - CGameCtnMediaBlockBloomHdr: 0x001
- Fixed chunks:
  - CGameCtnMediaBlockCameraPath: 0x003
  - CGameCtnMediaBlockGhost: 0x002
  - CGameCtnReplayRecord: 0x003
- Added `TriggerSize` property for CGameCtnChallenge 0x049 (MediaTracker).
- Added a new clean way of reading+writing chunks through `ref` keyword
- Added new methods for `GameBoxReader`, `GameBoxWriter` and `GameBoxReaderWriter` to reduce parse code repetition.
- Added vector magnitude methods
- Added `AdditionalMath.Lerp()` method
- Prefer km/h over m/s in ghost speed measures
- Support ghost sample reading from CGameGhost 0x003 chunk
- GBX saving can be now controlled with other streams other than `MemoryStream`
  - However there might be actions involving seeking, so check that your stream supports seeking!

This update also reveals a road to the x64 platform library support. A new check has been added for `GameBox.Save()` to throw an exception if the library is running in x64 (as the bug is with the LZO compressing). In this version though, the library is still x86 only. x64 platform support for reading-only is planned for 0.9.0.

### Island Converter 1.0.0

- **Program now runs on .NET Framework 4.5.2, included natively in every Windows since Win7**
  - **Fixes problems with opening the exe file.** If Nations Converter did open to you, Island Converter will also open.
- **Added clip support for all blocks**
- Added Platform gamemode conversion support
- Added official port of some TMS sign skins
- **Fixed TONS of blocks**
- Fixed problems with 32x32 (+ 31x31) base
  - Converting multiple maps at once won't break the maps
  - Fixed MediaTracker trigger and camera alignment
  - Fixed invisible water underground in 32x32 base
  - Fixed centering being off by 1 unit
  - Fixed item height for 31x31
- New water-removing system that properly handles water cleanup under terrain
- Fixed crash when selecting a map without thumbnail
- **GBX.NET has been updated to 0.8.0**
  - Better performance, minor conversion fixes, MediaTracker parsing fixes

#### How to install Island Converter
1. Download the IslandConverter.zip
2. Extract it anywhere (on Desktop for example)
3. Open the folder
4. Open IslandConverter.exe

## [Island Converter 1.0.0-rc2](https://github.com/BigBang1112/gbx-net/releases/tag/v1.0.0ic-rc2) - 2021-01-23
**[PRE-RELEASE]**

- Added clip system with clips currently available only on highway road start and finish
- Added Platform gamemode conversion support
- Added official port of some TMS skins
- Fixed bugged water blocks in 32x32
- Fixed invisible water underground in 32x32 base
- Fixed item height for 31x31
- Fixed crash when selecting a map without thumbnail
- **Many block fixes (very close to all-fixed).** Look around for remaining bugs!

### How to install Island Converter
1. Download the IslandConverter-rc2.zip
2. Extract it anywhere (on Desktop for example)
3. Open the folder
4. Open IslandConverter.exe

## [GBX.NET 0.8.0-rc2](https://github.com/BigBang1112/gbx-net/releases/tag/v0.8.0gbx-rc2) - 2021-01-20
**[PRE-RELEASE]**

- Fixed parse of `CGameCtnMediaClip` chunk 0x005 
- Improved logging for `WriteNodes()` method
- Added `TriggerSize` property to 0x049
- Added logging Push event intended for moving log report display cursor up

### Island Converter 1.0.0-rc1

- **Program now runs on .NET Framework 4.5.2, included natively in every Windows since Win7**
  - Fixes problems with opening the exe file. If Nations Converter did open to you, Island Converter will also open.
- GBX.NET has been updated to 0.8.0-rc2
  - Better performance, minor conversion fixes, MediaTracker parsing fixes
- Fixed problems with 32x32 (+ 31x31) base
  - Converting multiple maps at once won't break the maps
  - MediaTracker triggers and cameras are correctly aligned
  - Fixed centering being off by 1 unit
- New water-removing system which should improve with less water in tunnels

Please report load-related, conversion-related and block-related issues. A full release is planned at the end of the week.

#### How to install Island Converter
1. Download the IslandConverter-rc1.zip
2. Extract it anywhere (on Desktop for example)
3. Open the folder
4. Open IslandConverter.exe

## [GBX.NET 0.8.0-rc](https://github.com/BigBang1112/gbx-net/releases/tag/v0.8.0gbx-rc) - 2021-01-19
**[PRE-RELEASE]**

## [GBX.NET 0.7.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.7.0gbx) - 2021-01-04

- Added the new `CGameGhost.Data` member providing `CGameGhostData`, which contains the most basic sample data of a ghost at the moment
- Added some of the missing chunks related to MediaTracker, maps, and replays
- Added `Quaternion` struct and more `AdditionalMath` features
- Fixed some improper time values, added `GameBoxReader.ReadTimeSpan()` and `GameBoxWriter.Write(TimeSpan? timeSpan)`
- Provided a less wordy method of parsing arrays in ReaderWriter methods

Doc Generator has been also added to the repository which helps with partially automating the chunk navigation in markdown.

## [GBX.NET 0.6.3](https://github.com/BigBang1112/gbx-net/releases/tag/v0.6.3gbx) - 2021-01-01

- Added `CGameCtnMediaBlockTriangles3D` readability
- Added `0x002` chunk of `CControlEffectSimi`
- Added map name to body of TM1.0

## [GBX.NET 0.6.2](https://github.com/BigBang1112/gbx-net/releases/tag/v0.6.2gbx) - 2020-12-22

- Fixed ClipGroup trigger coord writing

## [GBX.NET 0.6.1](https://github.com/BigBang1112/gbx-net/releases/tag/v0.6.1gbx) - 2020-12-21

- Added new `CGameCtnMediaClipGroup` clip system
- Added `StopWhenRespawn` and `StopWhenLeave` to `CGameCtnMediaClip`
- Added `GameBoxWriter.Write()` params `List<T>` and `IEnumerable<T>`
- `TransferMediaTrackerTo049()` now won't delete some MediaTracker blocks but will remove FxBloom
- `ClipPodium` has been moved from chunk `0x049` to node
- Fixed CameraGame chunk 0x007

## [GBX.NET 0.6.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.6.0gbx) - 2020-12-15

- Added `AutoReadWriteChunkAttribute`, `GameBoxReaderWriter.TillFacade` and `GameBoxWriter.WriteBytes`
- Added `IsKeepPlaying`, `IsCycling`, `TransferMediaTrackTo005` to `CGameCtnMediaTrack`
- Added acceptance of `CGameCtnMediaBlockEntity`, `CGameCtnMediaBlockTriangles`
- Full support of `CGameCtnMediaBlockFxColors` and `CGameCtnMediaBlockFxBloom`
- `InvalidCastException` on `Parse<T>` is now properly thrown
- `IgnoreChunkAttribute` now has an effect on unskippable chunks - exception is thrown on find
- Fixed `AuthorLogin` on TMUF and lower
- Fixed `CrackPassword` for old tracks
- Fixed documentation

## [GBX.NET 0.5.4](https://github.com/BigBang1112/gbx-net/releases/tag/v0.5.4gbx) - 2020-12-10

Fix camerapath writing and macroblock item placing

## [GBX.NET 0.5.3](https://github.com/BigBang1112/gbx-net/releases/tag/v0.5.3gbx) - 2020-11-22

Fix Unassigned1 writing, fix deflate stream reading

## [GBX.NET 0.5.2](https://github.com/BigBang1112/gbx-net/releases/tag/v0.5.2gbx) - 2020-11-19

.NET Framework 4.5 support, macroblock fixes, bugfix when setting skippable properties

## [GBX.NET 0.5.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.5.0gbx) - 2020-11-18

One of the last releases before 1.0.

- Added embed management to `CGameCtnChallenge` using native zipping from `System.IO.Compression`
- Added gradual parsing ability, making it possible to track progress and **use earlier data before the whole parse is finished**
- Added `CGameCtnChallenge.BakedBlocks`
- Added `GetBlock()` and `GetBlocks()` to `CGameCtnChallenge`
- Added `BlockInfoManager` for managing units and clips without having to read official block GBX
- Added logging to GBX writing/saving
- Added `AdditionalMath` for few handy math functions
- Increased write speed by avoiding `dynamic`
- `GameBox.ParseHeader` now also reads reference table
- Removed `Microsoft.CSharp` and `SharpZipLib.NETStandard` dependencies
- Minor fixes

## [GBX.NET 0.4.1](https://github.com/BigBang1112/gbx-net/releases/tag/v0.4.1gbx) - 2020-11-11

Fixed null exception when reading very old Trackmania tracks.

## [GBX.NET 0.4.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.4.0gbx) - 2020-10-29

- Moved GBX header back to the Node class
- Flags while debugging are now displayed in series of 0 and 1 for easier visualization
- Bunch of minor fixes

## [GBX.NET 0.3.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.3.0gbx) - 2020-10-13

This one took way too long, but it's out either way!

Tons of changes, major ones:

- CGameCtnBlockInfoClassic (EDClassic.Gbx) parsing added
- Introduced embed reading in CGameCtnChallenge
- Merged Block.cs with CGameCtnBlock.cs
- Added RaceValidateGhost to CGameCtnChallengeParameters
- Added more implicit operators to data types used in GBX
- Added a new Collection class to distinguish types of collection storage in Meta.
- Slightly simplified header access (full system will be coming soon)
- Improved node debug views using DebuggerTypeProxy
- Enhanced the documentation

## [GBX.NET 0.2.1](https://github.com/BigBang1112/gbx-net/releases/tag/v0.2.1gbx) - 2020-09-19

This release mainly fixes a major issue with reading Items from maps.

- Remove `body` parameter from `Node.Parse<T>()`
- Fixed `null` errors related to chunks
- Fixed header writing with no user data
- Experimental error avoidance of reading node reference
- Fixed thumbnail write if `Thumbnail` is `null`
- Custom exception if the node is missing

## [GBX.NET 0.2.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.2.0gbx) - 2020-09-19

A big step closer to 1.0.0!
- **Now compatible with .NET Standard 2.0**
- Improved Node parsing algorithm, mainly by eliminating dynamic declaration while reading
  - Up to **35% improvement** in reading speed!
  - No requirement to have an inherited constructor
- Improved GBX header parsing, small boost in there as well
- Added `CCtnMediaBlockEventTrackMania`, tracking stunt timestamps from TMS, lovely node that got deprecated in TMUF
- Widen `CGameCtnReplayRecord` support
- Avoided `CGameCtnMediaBlockTriangles` and `CGameCtnMediaBlockFxColors` crash when reading TMUF maps
  - Proper read not supported yet
- Enhanced data types
- Changed `Vector2` and `Vector3` to new structs `Vec2` and `Vec3`
- Moved body chunks from `GameBoxBody` to `Node` for consistency with aux nodes
- Added `ReadToEnd()`, `ReadStringTillFacade()` and `ReadArrayTillFacade()`
- Renamed ChunkList to ChunkSet

Includes a release GBX.NET.Json 0.1.1 which very simply just downgrades the required framework to .NET Standard 2.0.

You can also test the Island Converter port to **.NET Framework 4.6.1** by cloning the project.

## [GBX.NET 0.1.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.1.0gbx) - 2020-09-12

This release rewrites the entire system of chunk data storing. Now available on NuGet. This release still works under .NET Standard 2.1.

- Added `GameBox.Parse` to simplify the GBX parsing. `GameBox.Load()` is now obsolete.
- Introduced a chunk system of `HeaderChunk<T> : SkippableChunk<T> : Chunk<T> : Chunk`
  - Necessary rework to make the simplest possible chunk read/write mechanism - method arguing the specific node class
  - `SkippableChunk<T>` additionally implements `ISkippableChunk` and `HeaderChunk<T>` implements `IHeaderChunk` for simplified non-type specific actions
- **Header chunks are not part of the `Node` properties anymore (obsoletes weren't included so if you have worked with the library already, you may experience errors)**
  - At the moment, you have to read them through `GameBoxHeader`
- Added wider expand of chunk management
- Added a new class `ChunkList` for easier list management
- Added `ReadBytes()` and `ReadArray<T>()` which read the number of elements first
- Added `ReadTill(uint)` and `ReadTillFacade()` which read bytes until hitting the number, or `0xFACADE01`
- Modernized the `ChunkAttribute`
- Added tons of MediaTracker blocks:
  - CGameCtnMediaBlockVehicleLight
  - CGameCtnMediaBlock3dStereo
  - CGameCtnMediaBlockBloomHdr
  - CGameCtnMediaBlockTime
  - ... many more ...
- Expanded `CGameCtnReplayRecord` support
- Fixed `CGameCtnGhost` for TM®
- Added `GhostTrigram` property to `CGameCtnGhost`, available in TM®
- Added `CGameCtnMediaClip` chunks `0x008` and `0x009`
- Expanded the documentation
- Tons of minor changes

The new approach right now for the chunk reading is:
- Inherit generic `Chunk<T>`/`SkippableChunk<T>` where the `T` is the `Node` the class is nested in
- Override the Read/Write/ReadWrite with the `T node` included
- Implement known values to the desired `Node` class and set them through `T n`
- Implement unknown values in the chunk or through the `Unknown` stream, just as before
- No need to implement the constructor, `Chunk` now has a parameterless constructor

A new subcomponent has been added called GBX.NET.Json:
- Library for simplified JSON serialization of GBX, useful for comparing data for example
- `GameBox` and `Node` classes are extended by a new method `ToJson()` after including

Another version 0.2.0 is gonna be coming out next week that will expand the library compatibility to .NET Standard 2.0.

## [GBX.NET 0.0.3](https://github.com/BigBang1112/gbx-net/releases/tag/v0.0.3gbx) - 2020-08-31

This release adds an early testing custom block reading support, includes mesh data like vertices or UV maps.

- Added Block.Gbx support (custom blocks, not macroblocks - these are already supported)
- Added special exception if you miss a ChunkAttribute
- Moved IsHeavy to SkippableChunk

This is the last update before the rewrite of the chunk data storing system. Chunk data will become stored in node classes instead of chunk classes. This will fix a lot of recent confusing issues. Also, additional features like UnknownStream (for easier viewing of unknown chunk data) or HeaderChunk inheriting SkippableChunk will be coming too.
New branch will be made called chunk-data-storing-rewrite for this mini-project. After it's done, the library is going to move to 0.1.0. Can't wait for this to happen.

## [Island Converter 0.3.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.3.0ic) - 2020-08-30

This release fixes some major minor issues.

- Fixed block height in 32x32 base
- Fixed more Island blocks
- Fixed platform T and cross corners to not have holes
- Fixed Change Maps folder menu item

This version still uses GBX.NET version 0.0.2.

If you can't open the exe, you might have an outdated .NET runtime. You need .NET Core 3.1 Runtime x86: https://dotnet.microsoft.com/download

## [Island Converter 0.2.0](https://github.com/BigBang1112/gbx-net/releases/tag/v0.2.0ic) - 2020-08-27

This release fixes problems with Sunrise maps and makes passworded maps convertible.

- Added map password cracking
- Fixed few seaway roads/pillars and more blocks
- Fixed a Graphics API crash if no thumbnail
- Fixed UI crash after loading a map without a thumbnail
- Attempted to fix envimix transfer

If you can't open the exe, you might have an outdated .NET runtime. You need .NET Core 3.1 Runtime x86: https://dotnet.microsoft.com/download

## [GBX.NET 0.0.2](https://github.com/BigBang1112/gbx-net/releases/tag/v0.0.2gbx) - 2020-08-27

This update fixes a few Sunrise map related things.

- Fixed CGameCtnChallenge 0x016 to be skippable
- Changed how CrackPassword works

## [GBX.NET 0.0.1](https://github.com/BigBang1112/gbx-net/releases/tag/v0.0.1) - 2020-08-26

The very first release of the library and the Island Converter.

Known issues of the Island Converter:
- Clips are missing
- Several pillars are bugged
- Most of the time the top pillar part is missing
- CGameCtnMediaBlockFxColors and CGameCtnMediaBlockTriangles2D aren't readable at the moment

To use Island Converter, you only need to download IslandConverter.zip. If you can't open the exe, you might have an outdated .NET runtime. You need .NET Core 3.1 Runtime x86: https://dotnet.microsoft.com/download

Feel free to report any issues in Issues.
