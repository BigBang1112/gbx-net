# Currently supported Gbx file types

Disclaimer: The list may be incomplete. Also, watch the last modified date.

Older extensions | Latest extension | Class | Read (whole) | Write | Read (header)
| --- | --- | --- | --- | --- | ---
| Challenge.Gbx | Map.Gbx | [CGameCtnChallenge](Src/GBX.NET/Engines/Game/CGameCtnChallenge.chunkl) | Yes | Yes | Yes
| | Replay.Gbx | [CGameCtnReplayRecord](Src/GBX.NET/Engines/Game/CGameCtnReplayRecord.chunkl) | Yes | No<sup>1</sup> | Yes
| | Ghost.Gbx | [CGameCtnGhost](Src/GBX.NET/Engines/Game/CGameCtnGhost.chunkl) | Yes | Yes
| | Clip.Gbx | [CGameCtnMediaClip](Src/GBX.NET/Engines/Game/CGameCtnMediaClip.chunkl) | Yes | Yes
| | Item.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.chunkl) | Yes | Yes | Yes
| | Block.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.chunkl) | Yes | Yes | Yes
| | Mat.Gbx | [CPlugMaterialUserInst](Src/GBX.NET/Engines/Plug/CPlugMaterialUserInst.chunkl) | Yes | Yes
| Solid2.Gbx | Mesh.Gbx | [CPlugSolid2Model](Src/GBX.NET/Engines/Plug/CPlugSolid2Model.chunkl) | Yes | Yes
| | Shape.Gbx | [CPlugSurface](Src/GBX.NET/Engines/Plug/CPlugSurface.chunkl) | Yes | Yes
| | Macroblock.Gbx | [CGameCtnMacroBlockInfo](Src/GBX.NET/Engines/Game/CGameCtnMacroBlockInfo.chunkl) | Yes | Yes | Yes
| | SystemConfig.Gbx | [CSystemConfig](Src/GBX.NET/Engines/System/CSystemConfig.chunkl) | Yes | Yes
| | FidCache.Gbx | [CMwRefBuffer](Src/GBX.NET/Engines/MwFoundations/CMwRefBuffer.chunkl) | Yes | Yes
| | Profile.Gbx | [CGamePlayerProfile](Src/GBX.NET/Engines/Game/CGamePlayerProfile.chunkl) | Up to TMF | Up to TMF | Yes
| | Spawn.Gbx | [CGameSpawnModel](Src/GBX.NET/Engines/GameData/CGameSpawnModel.chunkl) | Yes | Yes
| ConstructionCampaign.Gbx | Campaign.Gbx | [CGameCtnCampaign](Src/GBX.NET/Engines/Game/CGameCtnCampaign.chunkl) | Yes | Yes
| TMCollection.Gbx | Collection.Gbx | [CGameCtnCollection](Src/GBX.NET/Engines/Game/CGameCtnCollection.chunkl) | Yes | Yes | Yes
| TMDecoration.Gbx | Decoration.Gbx | [CGameCtnDecoration](Src/GBX.NET/Engines/Game/CGameCtnDecoration.chunkl) | Yes | Yes | Yes
| TMDecorationSize.Gbx | DecorationSize.Gbx | [CGameCtnDecorationSize](Src/GBX.NET/Engines/Game/CGameCtnDecorationSize.chunkl) | Yes | Yes | Yes
| TMDecorationMood.Gbx | DecorationMood.Gbx | [CGameCtnDecorationMood](Src/GBX.NET/Engines/Game/CGameCtnDecorationMood.chunkl) | Yes | Yes | Yes
| TMDecorationAudio.Gbx | DecorationAudio.Gbx | [CGameCtnDecorationAudio](Src/GBX.NET/Engines/Game/CGameCtnDecorationAudio.chunkl) | Yes | Yes | Yes
| TMEDClassic.Gbx | EDClassic.Gbx | [CGameCtnBlockInfoClassic](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoClassic.chunkl) | Yes | Yes | Yes
| TMEDClip.Gbx | EDClip.Gbx | [CGameCtnBlockInfoClip](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoClip.chunkl) | Yes | Yes | Yes
| TMEDFlat.Gbx | EDFlat.Gbx | [CGameCtnBlockInfoFlat](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoFlat.chunkl) | Yes | Yes | Yes
| TMEDFrontier.Gbx | EDFrontier.Gbx | [CGameCtnBlockInfoFrontier](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoFrontier.chunkl) | Yes | Yes | Yes
| TMEDPylon.Gbx | EDPylon.Gbx | [CGameCtnBlockInfoPylon](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoPylon.chunkl) | Yes | Yes | Yes
| TMEDRectAsym.Gbx | EDRectAsym.Gbx | [CGameCtnBlockInfoRectAsym](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoRectAsym.chunkl) | Yes | Yes | Yes
| TMEDRoad.Gbx | EDRoad.Gbx | [CGameCtnBlockInfoRoad](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoRoad.chunkl) | Yes | Yes | Yes
| | EDTransition.Gbx | [CGameCtnBlockInfoTransition](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoTransition.chunkl) | Yes | Yes | Yes
| TMZoneFlat.Gbx | ZoneFlat.Gbx | [CGameCtnZoneFlat](Src/GBX.NET/Engines/Game/CGameCtnZoneFlat.chunkl) | Yes | Yes
| TMZoneFrontier.Gbx | ZoneFrontier.Gbx | [CGameCtnZoneFrontier](Src/GBX.NET/Engines/Game/CGameCtnZoneFrontier.chunkl) | Yes | Yes
| | FuncShader.Gbx | [CFuncShaderLayerUV](Src/GBX.NET/Engines/Func/CFuncShaderLayerUV.chunkl) | Yes | Yes
| | TMTerrainModifier.Gbx | [CGameCtnDecorationTerrainModifier](Src/GBX.NET/Engines/Game/CGameCtnDecorationTerrainModifier.chunkl) | Yes | Yes
| | MotionManagerWeathers.Gbx | [CPlugWeatherModel](Src/GBX.NET/Engines/Plug/CPlugWeatherModel.chunkl) | Up to TMUF | Yes
| | RallyLeafManager.Gbx | [CMotionManagerLeaves](Src/GBX.NET/Engines/Motion/CMotionManagerLeaves.chunkl) | Yes | Yes
| | GameSkin.Gbx | [CPlugGameSkin](Src/GBX.NET/Engines/Plug/CPlugGameSkin.chunkl) | Yes | Yes
| | VehicleTunings.Gbx | [CPlugVehiclePhyTunings](Src/GBX.NET/Engines/Plug/CPlugVehiclePhyTunings.chunkl) | Up to TM2 | Up to TM2 | Yes
| | ObjectInfo.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.chunkl) | Yes | Yes | Yes
| | Mobil.Gbx | [CSceneMobil](Src/GBX.NET/Engines/Scene/CSceneMobil.chunkl) | Yes | Yes
| | Solid.Gbx | [CPlugSolid](Src/GBX.NET/Engines/Plug/CPlugSolid.cs) | Yes | Yes
| | Material.Gbx | [CPlugMaterial](Src/GBX.NET/Engines/Plug/CPlugMaterial.cs) | Up to TM2 | No
| | Texture.Gbx | [CPlugBitmap](Src/GBX.NET/Engines/Plug/CPlugBitmap.cs) | Up to TMUF | No
| | Light.Gbx | [CPlugLight](Src/GBX.NET/Engines/Plug/CPlugLight.cs) | Yes | Yes
| | Prefab.Gbx | [CPlugPrefab](Src/GBX.NET/Engines/Plug/CPlugPrefab.cs) | Yes | Yes

- <sup>1</sup>Safety reasons. Consider extracting `CGameCtnGhost` from `CGameCtnReplayRecord`, transfer it over to `CGameCtnMediaBlockGhost`, add it to `CGameCtnMediaClip`, and save it as `.Clip.Gbx`, which you can then import in MediaTracker.