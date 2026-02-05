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
| Solid2.Gbx | Mesh.Gbx | [CPlugSolid2Model](Src/GBX.NET/Engines/Plug/CPlugSolid2Model.chunkl) | Yes | Yes | Yes
| | Shape.Gbx | [CPlugSurface](Src/GBX.NET/Engines/Plug/CPlugSurface.chunkl) | Yes | Yes
| | Macroblock.Gbx | [CGameCtnMacroBlockInfo](Src/GBX.NET/Engines/Game/CGameCtnMacroBlockInfo.chunkl) | Yes | Yes | Yes
| | SystemConfig.Gbx | [CSystemConfig](Src/GBX.NET/Engines/System/CSystemConfig.chunkl) | Yes | Yes
| RefBuffer.Gbx | FidCache.Gbx | [CMwRefBuffer](Src/GBX.NET/Engines/MwFoundations/CMwRefBuffer.chunkl) | Yes | Yes
| | Profile.Gbx | [CGamePlayerProfile](Src/GBX.NET/Engines/Game/CGamePlayerProfile.chunkl) | Up to TMF | No | Yes
| | Spawn.Gbx | [CGameSpawnModel](Src/GBX.NET/Engines/GameData/CGameSpawnModel.chunkl) | Yes | Yes
| ConstructionCampaign.Gbx | Campaign.Gbx | [CGameCtnCampaign](Src/GBX.NET/Engines/Game/CGameCtnCampaign.chunkl) | Yes | Yes
| TMCollection.Gbx | Collection.Gbx | [CGameCtnCollection](Src/GBX.NET/Engines/Game/CGameCtnCollection.chunkl) | Yes | Yes | Yes
| TMDecoration.Gbx | Decoration.Gbx | [CGameCtnDecoration](Src/GBX.NET/Engines/Game/CGameCtnDecoration.chunkl) | Yes | Yes | Yes
| TMDecorationSize.Gbx | DecorationSize.Gbx | [CGameCtnDecorationSize](Src/GBX.NET/Engines/Game/CGameCtnDecorationSize.chunkl) | Yes | Yes | Yes
| TMDecorationMood.Gbx | DecorationMood.Gbx | [CGameCtnDecorationMood](Src/GBX.NET/Engines/Game/CGameCtnDecorationMood.chunkl) | Yes | Yes | Yes
| TMDecorationAudio.Gbx | DecorationAudio.Gbx | [CGameCtnDecorationAudio](Src/GBX.NET/Engines/Game/CGameCtnDecorationAudio.chunkl) | Yes | Yes | Yes
| | Scene3d.Gbx | [CSceneLayout](Src/GBX.NET/Engines/Scene/CSceneLayout.chunkl) | From TMF | From TMF
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
| | MotionManagerWeathers.Gbx | [CPlugWeatherModel](Src/GBX.NET/Engines/Plug/CPlugWeatherModel.chunkl) | Up to TM2 | Up to TM2
| | RallyLeafManager.Gbx | [CMotionManagerLeaves](Src/GBX.NET/Engines/Motion/CMotionManagerLeaves.chunkl) | Yes | Yes
| | GameSkin.Gbx | [CPlugGameSkin](Src/GBX.NET/Engines/Plug/CPlugGameSkin.chunkl) | Yes | Yes
| | VehicleTunings.Gbx | [CPlugVehiclePhyTunings](Src/GBX.NET/Engines/Plug/CPlugVehiclePhyTunings.chunkl) | Up to TM2 | Up to TM2
| | CtrlCam.Gbx | [CGameControlCamera](Src/GBX.NET/Engines/Game/CGameControlCamera.chunkl) | Yes | Yes
| | CtrlCamTarget.Gbx | [CGameControlCameraTarget](Src/GBX.NET/Engines/Game/CGameControlCameraTarget.chunkl) | Yes | Yes
| | CtrlCamOrbital3d.Gbx | [CGameControlCameraOrbital3d](Src/GBX.NET/Engines/Game/CGameControlCameraOrbital3d.chunkl) | Yes | Yes
| | CtrlCamTmRace.Gbx | [CGameControlCameraTrackManiaRace](Src/GBX.NET/Engines/TrackMania/CGameControlCameraTrackManiaRace.chunkl) | Yes | Yes
| | CtrlCamTmRace2.Gbx | [CGameControlCameraTrackManiaRace2](Src/GBX.NET/Engines/TrackMania/CGameControlCameraTrackManiaRace2.chunkl) | Yes | Yes
| | CtrlCamTmRace3.Gbx | [CGameControlCameraTrackManiaRace3](Src/GBX.NET/Engines/TrackMania/CGameControlCameraTrackManiaRace3.chunkl) | Yes | Yes
| | DecoSolid.Gbx | [CPlugDecoratorSolid](Src/GBX.NET/Engines/CPlug/CPlugDecoratorSolid.chunkl) | Yes | Yes
| | FuncKeysReals.Gbx | [CFuncKeysReal](Src/GBX.NET/Engines/Func/CFuncKeysReal.chunkl) | Yes | Yes
| | TrackManiaVehicle.Gbx | [CSceneVehicleCar](Src/GBX.NET/Engines/Scene/CSceneVehicleCar.chunkl) | Only TMUF | Only TMUF
| TMVehicle.Gbx | ConstructionVehicle.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.chunkl) | Yes | Yes | Yes
| | ParticleModel.Gbx | [CPlugParticleEmitterModel](Src/GBX.NET/Engines/CPlug/CPlugParticleEmitterModel.chunkl) | Up to TMUF | Up to TMUF
| | VehicleStruct.Gbx | [CPlugVehicleVisModelShared](Src/GBX.NET/Engines/GameData/CPlugVehicleVisModelShared.chunkl) | Yes | Yes
| | FuncClouds.Gbx | [CPlugClouds](Src/GBX.NET/Engines/Plug/CPlugClouds.chunkl) | Yes | Yes
| | FuncShader.Gbx | [CFuncShaderLayerUV](Src/GBX.NET/Engines/Func/CFuncShaderLayerUV.chunkl) | Yes | Yes
| | AmbientOcc.Gbx | [CHmsAmbientOcc](Src/GBX.NET/Engines/Hms/CHmsAmbientOcc.chunkl) | Yes | Yes
| | ObjectInfo.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.chunkl) | Yes | Yes | Yes
| | Mobil.Gbx | [CSceneMobil](Src/GBX.NET/Engines/Scene/CSceneMobil.chunkl) | Yes | Yes
| | Sound.Gbx | [CPlugSound](Src/GBX.NET/Engines/Plug/CPlugSound.chunkl) | Yes | Yes
| | Solid.Gbx | [CPlugSolid](Src/GBX.NET/Engines/Plug/CPlugSolid.chunkl) | Yes | Yes
| | AudioEnvironment.Gbx | [CPlugAudioEnvironment](Src/GBX.NET/Engines/Plug/CPlugAudioEnvironment.chunkl) | Yes | Yes
| | Material.Gbx | [CPlugMaterial](Src/GBX.NET/Engines/Plug/CPlugMaterial.chunkl) | Up to TM2 | Up to TM2
| | Shader.Gbx | [CPlugShader](Src/GBX.NET/Engines/Plug/CPlugShader.chunkl) | Up to TM2 | Up to TM2
| | Texture.Gbx | [CPlugBitmap](Src/GBX.NET/Engines/Plug/CPlugBitmap.chunkl) | Up to TM2 | Up to TM2
| | Light.Gbx | [CPlugLight](Src/GBX.NET/Engines/Plug/CPlugLight.chunkl) | Yes | Yes
| | Prefab.Gbx | [CPlugPrefab](Src/GBX.NET/Engines/Plug/CPlugPrefab.chunkl) | Yes | Yes
| | Wagon.Gbx | [CPlugTrainWagonModel](Src/GBX.NET/Engines/Plug/CPlugTrainWagonModel.chunkl) | Yes | Yes
| | Armor.Gbx | [CGameArmorModel](Src/GBX.NET/Engines/GameData/CGameArmorModel.chunkl) | Yes | Yes
| | Gate.Gbx | [CGameGateModel](Src/GBX.NET/Engines/GameData/CGameGateModel.chunkl) | Yes | Yes
| | GameAdvertisement.Gbx | [CGameAdvertising](Src/GBX.NET/Engines/Game/CGameAdvertising.chunkl) | Yes | Yes

- <sup>1</sup>Safety reasons. Consider extracting `CGameCtnGhost` from `CGameCtnReplayRecord`, transfer it over to `CGameCtnMediaBlockGhost`, add it to `CGameCtnMediaClip`, and save it as `.Clip.Gbx`, which you can then import in MediaTracker.