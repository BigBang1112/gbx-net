# Supported Gbx file types

GBX.NET can read and write the file types below using the implemented chunk and archive versions. This list was reviewed against the layouts and extension registry on 2026-10-20.

Support varies by game, format version, and referenced classes. **Yes does not mean every version of a file type is supported.** Unknown skippable chunks can be preserved without exposing their fields. Classes with only placeholder layouts are omitted.

Extensions are naming conventions. GBX.NET identifies a class from the Gbx class ID, and multiple classes can share an extension. The class links point to their layouts. See [Resources/Extensions.txt](Resources/Extensions.txt) for the extension registry, which also includes unsupported types.

**Header metadata** indicates implemented header chunks, including inherited chunks. `N/A` means the class has no implemented header chunks with metadata. The general Gbx header can still be read without parsing the body.

- [Common file types](#common-file-types)
- [Game data, environments and blocks](#game-data-environments-and-blocks)
- [Vehicles, cameras and animation](#vehicles-cameras-and-animation)
- [Geometry, materials and effects](#geometry-materials-and-effects)
- [Audio](#audio)
- [Controls and menus](#controls-and-menus)
- [MediaTracker](#mediatracker)
- [Functions](#functions)
- [Scenes, lighting and other data](#scenes-lighting-and-other-data)
- [Limitations](#limitations)

## Common file types

| Extensions | Class | Read | Write | Header metadata |
| --- | --- | --- | --- | --- |
| Map.Gbx, Challenge.Gbx | [CGameCtnChallenge](Src/GBX.NET/Engines/Game/CGameCtnChallenge.chunkl) | Yes | Yes | Yes |
| Replay.Gbx, TMReplay.Gbx | [CGameCtnReplayRecord](Src/GBX.NET/Engines/Game/CGameCtnReplayRecord.chunkl) | Yes | No<sup>1</sup> | Yes |
| Ghost.Gbx | [CGameCtnGhost](Src/GBX.NET/Engines/Game/CGameCtnGhost.chunkl) | Yes | Yes | N/A |
| Ghost.Gbx | [CGameGhost](Src/GBX.NET/Engines/Game/CGameGhost.chunkl) | Yes | Yes | N/A |
| Clip.Gbx | [CGameCtnMediaClip](Src/GBX.NET/Engines/Game/CGameCtnMediaClip.chunkl) | Yes | Yes | N/A |
| Item.Gbx, ObjectInfo.Gbx, Block.Gbx, TMVehicle.Gbx, TrackManiaVehicle.Gbx, ConstructionVehicle.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.chunkl) | Yes | Yes | Yes |
| Mat.Gbx | [CPlugMaterialUserInst](Src/GBX.NET/Engines/Plug/CPlugMaterialUserInst.chunkl) | Yes | Yes | N/A |
| Mesh.Gbx, Solid2.Gbx | [CPlugSolid2Model](Src/GBX.NET/Engines/Plug/CPlugSolid2Model.chunkl) | Yes | Yes | Yes |
| Shape.Gbx, HitShape.Gbx | [CPlugSurface](Src/GBX.NET/Engines/Plug/CPlugSurface.chunkl) | Yes | Yes | N/A |
| Macroblock.Gbx | [CGameCtnMacroBlockInfo](Src/GBX.NET/Engines/Game/CGameCtnMacroBlockInfo.chunkl) | Yes | Yes | Yes |
| Profile.Gbx | [CGamePlayerProfile](Src/GBX.NET/Engines/Game/CGamePlayerProfile.chunkl) | Yes | Yes | Yes |
| Profile.Gbx | [CGameUserProfile](Src/GBX.NET/Engines/Game/CGameUserProfile.chunkl) | Yes | Yes | N/A |
| SystemConfig.Gbx | [CSystemConfig](Src/GBX.NET/Engines/System/CSystemConfig.chunkl) | Yes | Yes | N/A |
| ScriptCache.Gbx | [CScriptTraitsPersistent](Src/GBX.NET/Engines/Script/CScriptTraitsPersistent.chunkl) | Yes | Yes | N/A |
| Scores.Gbx | [CGamePlayerScore](Src/GBX.NET/Engines/Game/CGamePlayerScore.chunkl) | Yes | Yes | N/A |

## Game data, environments and blocks

| Extensions | Class | Read | Write | Header metadata |
| --- | --- | --- | --- | --- |
| Action.Gbx | [CGameActionModel](Src/GBX.NET/Engines/GameData/CGameActionModel.chunkl) | Yes | Yes | N/A |
| Armor.Gbx | [CGameArmorModel](Src/GBX.NET/Engines/GameData/CGameArmorModel.chunkl) | Yes | Yes | N/A |
| BlockItem.Gbx | [CGameBlockItem](Src/GBX.NET/Engines/GameData/CGameBlockItem.chunkl) | Yes | Yes | N/A |
| Collection.Gbx, TMCollection.Gbx, TMElementColl.Gbx | [CGameCtnCollection](Src/GBX.NET/Engines/Game/CGameCtnCollection.chunkl) | Yes | Yes | Yes |
| Decoration.Gbx, TMDecoration.Gbx | [CGameCtnDecoration](Src/GBX.NET/Engines/Game/CGameCtnDecoration.chunkl) | Yes | Yes | Yes |
| DecorationAudio.Gbx, TMDecorationAudio.Gbx | [CGameCtnDecorationAudio](Src/GBX.NET/Engines/Game/CGameCtnDecorationAudio.chunkl) | Yes | Yes | N/A |
| DecorationMood.Gbx, TMDecorationMood.Gbx | [CGameCtnDecorationMood](Src/GBX.NET/Engines/Game/CGameCtnDecorationMood.chunkl) | Yes | Yes | N/A |
| DecorationSize.Gbx, TMDecorationSize.Gbx | [CGameCtnDecorationSize](Src/GBX.NET/Engines/Game/CGameCtnDecorationSize.chunkl) | Yes | Yes | N/A |
| EDClassic.Gbx, TMEDClassic.Gbx | [CGameCtnBlockInfoClassic](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoClassic.chunkl) | Yes | Yes | Yes |
| EDClip.Gbx, TMEDClip.Gbx | [CGameCtnBlockInfoClip](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoClip.chunkl) | Yes | Yes | Yes |
| EDFlat.Gbx, TMEDFlat.Gbx | [CGameCtnBlockInfoFlat](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoFlat.chunkl) | Yes | Yes | Yes |
| EDFrontier.Gbx, TMEDFrontier.Gbx | [CGameCtnBlockInfoFrontier](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoFrontier.chunkl) | Yes | Yes | Yes |
| EDHorizontalClip.Gbx | [CGameCtnBlockInfoClipHorizontal](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoClipHorizontal.chunkl) | Yes | Yes | Yes |
| Editor.Gbx | [CGameEditorModel](Src/GBX.NET/Engines/GameData/CGameEditorModel.chunkl) | Yes | Yes | N/A |
| EDPylon.Gbx, TMEDPylon.Gbx | [CGameCtnBlockInfoPylon](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoPylon.chunkl) | Yes | Yes | Yes |
| EDRectAsym.Gbx, TMEDRectAsym.Gbx | [CGameCtnBlockInfoRectAsym](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoRectAsym.chunkl) | Yes | Yes | Yes |
| EDRoad.Gbx, TMEDRoad.Gbx | [CGameCtnBlockInfoRoad](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoRoad.chunkl) | Yes | Yes | Yes |
| EDSlope.Gbx, TMEDSlope.Gbx | [CGameCtnBlockInfoSlope](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoSlope.chunkl) | Yes | Yes | Yes |
| EDTransition.Gbx | [CGameCtnBlockInfoTransition](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoTransition.chunkl) | Yes | Yes | Yes |
| EDVerticalClip.Gbx | [CGameCtnBlockInfoClipVertical](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoClipVertical.chunkl) | Yes | Yes | Yes |
| GameActionFxResources.Gbx | [CGameActionFxResources](Src/GBX.NET/Engines/GameData/CGameActionFxResources.chunkl) | Yes | Yes | N/A |
| GameAdvertisement.Gbx | [CGameAdvertising](Src/GBX.NET/Engines/Game/CGameAdvertising.chunkl) | Yes | Yes | N/A |
| GameCoverFlow.Gbx | [CGameCoverFlowDesc](Src/GBX.NET/Engines/Game/CGameCoverFlowDesc.chunkl) | Partial<sup>4</sup> | Partial<sup>4</sup> | N/A |
| GameCtnApp.Gbx | [CGameCtnApp](Src/GBX.NET/Engines/Game/CGameCtnApp.chunkl) | Yes | Yes | N/A |
| GameCtnCampaign.Gbx, ConstructionCampaign.Gbx, Campaign.Gbx | [CGameCtnCampaign](Src/GBX.NET/Engines/Game/CGameCtnCampaign.chunkl) | Yes | Yes | N/A |
| GameObjectPhyModel.Gbx | [CGameObjectPhyModel](Src/GBX.NET/Engines/GameData/CGameObjectPhyModel.chunkl) | Yes | Yes | N/A |
| GameObjectVisModel.Gbx | [CGameObjectVisModel](Src/GBX.NET/Engines/GameData/CGameObjectVisModel.chunkl) | Yes | Yes | N/A |
| GamePodiumInfo.Gbx | [CGamePodiumInfo](Src/GBX.NET/Engines/Game/CGamePodiumInfo.chunkl) | Yes | Yes | N/A |
| GameResources.Gbx, TMInterface.Gbx | [CGameResources](Src/GBX.NET/Engines/Game/CGameResources.chunkl) | Yes | Yes | N/A |
| GameSkinnedNod.Gbx | [CGameSkinnedNod](Src/GBX.NET/Engines/Game/CGameSkinnedNod.chunkl) | Yes | Yes | N/A |
| Gate.Gbx | [CGameGateModel](Src/GBX.NET/Engines/GameData/CGameGateModel.chunkl) | Yes | Yes | N/A |
| League.Gbx | [CGameLeague](Src/GBX.NET/Engines/Game/CGameLeague.chunkl) | Yes | Yes | N/A |
| Manager.Gbx | [CGameCampaignsScoresManager](Src/GBX.NET/Engines/Game/CGameCampaignsScoresManager.chunkl) | Yes | Yes | N/A |
| Manager.Gbx | [CGameLeagueManager](Src/GBX.NET/Engines/Game/CGameLeagueManager.chunkl) | Yes | Yes | N/A |
| ManiaPlanet.Gbx | [CGameManiaPlanet](Src/GBX.NET/Engines/Game/CGameManiaPlanet.chunkl) | Yes | Yes | N/A |
| ManiaPlanet.Gbx, TrackMania.Gbx | [CTrackMania](Src/GBX.NET/Engines/TrackMania/CTrackMania.chunkl) | Yes | Yes | N/A |
| Module.Gbx | [CGameModulePlaygroundAltimeterModel](Src/GBX.NET/Engines/GameData/CGameModulePlaygroundAltimeterModel.chunkl) | Yes | Yes | N/A |
| Module.Gbx | [CGameModulePlaygroundChronoModel](Src/GBX.NET/Engines/GameData/CGameModulePlaygroundChronoModel.chunkl) | Yes | Yes | N/A |
| Module.Gbx | [CGameModulePlaygroundHudModel](Src/GBX.NET/Engines/GameData/CGameModulePlaygroundHudModel.chunkl) | Yes | Yes | N/A |
| Module.Gbx | [CGameModulePlaygroundInventoryModel](Src/GBX.NET/Engines/GameData/CGameModulePlaygroundInventoryModel.chunkl) | Yes | Yes | N/A |
| Module.Gbx | [CGameModulePlaygroundPlayerStateModel](Src/GBX.NET/Engines/GameData/CGameModulePlaygroundPlayerStateModel.chunkl) | Yes | Yes | N/A |
| Module.Gbx | [CGameModulePlaygroundSpeedMeterModel](Src/GBX.NET/Engines/GameData/CGameModulePlaygroundSpeedMeterModel.chunkl) | Yes | Yes | N/A |
| Module.Gbx | [CGameModulePlaygroundStoreModel](Src/GBX.NET/Engines/GameData/CGameModulePlaygroundStoreModel.chunkl) | Yes | Yes | N/A |
| Module.Gbx | [CGameModulePlaygroundTeamStateModel](Src/GBX.NET/Engines/GameData/CGameModulePlaygroundTeamStateModel.chunkl) | Yes | Yes | N/A |
| Module.Gbx | [CGameModulePlaygroundThrottleModel](Src/GBX.NET/Engines/GameData/CGameModulePlaygroundThrottleModel.chunkl) | Yes | Yes | N/A |
| PainterSetting.Gbx | [CGameCtnPainterSetting](Src/GBX.NET/Engines/Game/CGameCtnPainterSetting.chunkl) | Yes | Yes | N/A |
| PlaceParam.Gbx | [CGameItemPlacementParam](Src/GBX.NET/Engines/GameData/CGameItemPlacementParam.chunkl) | Partial<sup>5</sup> | Partial<sup>5</sup> | N/A |
| SmArenaResource.Gbx | [CSmArenaResource](Src/GBX.NET/Engines/ShootMania/CSmArenaResource.chunkl) | Yes | Yes | N/A |
| Spawn.Gbx | [CGameSpawnModel](Src/GBX.NET/Engines/GameData/CGameSpawnModel.chunkl) | Yes | Yes | N/A |
| StickerSlots.Gbx | [CGameBadgeStickerSlots](Src/GBX.NET/Engines/Game/CGameBadgeStickerSlots.chunkl) | Yes | Yes | N/A |
| Teleporter.Gbx | [CGameTeleporterModel](Src/GBX.NET/Engines/GameData/CGameTeleporterModel.chunkl) | Yes | Yes | N/A |
| Title.Gbx | [CGameManiaTitle](Src/GBX.NET/Engines/Game/CGameManiaTitle.chunkl) | Yes | Yes | N/A |
| TitleCore.Gbx | [CShootMania](Src/GBX.NET/Engines/ShootMania/CShootMania.chunkl) | Yes | Yes | N/A |
| TitleCore.Gbx | [CTrackManiaCore](Src/GBX.NET/Engines/TrackMania/CTrackManiaCore.chunkl) | Yes | Yes | N/A |
| TMRace.Gbx | [CTrackManiaRace1P](Src/GBX.NET/Engines/TrackMania/CTrackManiaRace1P.chunkl) | Yes | Yes | N/A |
| TMRace.Gbx | [CTrackManiaRace1PCrazy](Src/GBX.NET/Engines/TrackMania/CTrackManiaRace1PCrazy.chunkl) | Yes | Yes | N/A |
| TMRace.Gbx | [CTrackManiaRace1PGhosts](Src/GBX.NET/Engines/TrackMania/CTrackManiaRace1PGhosts.chunkl) | Yes | Yes | N/A |
| TMRace.Gbx | [CTrackManiaRace1PSpecial](Src/GBX.NET/Engines/TrackMania/CTrackManiaRace1PSpecial.chunkl) | Yes | Yes | N/A |
| TMRace.Gbx | [CTrackManiaRace2PTurnBased](Src/GBX.NET/Engines/TrackMania/CTrackManiaRace2PTurnBased.chunkl) | Yes | Yes | N/A |
| TMRace.Gbx | [CTrackManiaRaceNetLaps](Src/GBX.NET/Engines/TrackMania/CTrackManiaRaceNetLaps.chunkl) | Yes | Yes | N/A |
| TMRace.Gbx | [CTrackManiaRaceNetRounds](Src/GBX.NET/Engines/TrackMania/CTrackManiaRaceNetRounds.chunkl) | Yes | Yes | N/A |
| TMRace.Gbx | [CTrackManiaRaceNetTimeAttack](Src/GBX.NET/Engines/TrackMania/CTrackManiaRaceNetTimeAttack.chunkl) | Yes | Yes | N/A |
| TMTerrainModifier.Gbx | [CGameCtnDecorationTerrainModifier](Src/GBX.NET/Engines/Game/CGameCtnDecorationTerrainModifier.chunkl) | Yes | Yes | N/A |
| TMZone.Gbx | [CGameCtnZone](Src/GBX.NET/Engines/Game/CGameCtnZone.chunkl) | Yes | Yes | N/A |
| TrackManiaResource.Gbx | [CTrackManiaResource](Src/GBX.NET/Engines/TrackMania/CTrackManiaResource.chunkl) | Yes | Yes | N/A |
| Turbine.Gbx | [CGameTurbineModel](Src/GBX.NET/Engines/GameData/CGameTurbineModel.chunkl) | Yes | Yes | N/A |
| ZoneFlat.Gbx, TMZoneFlat.Gbx | [CGameCtnZoneFlat](Src/GBX.NET/Engines/Game/CGameCtnZoneFlat.chunkl) | Yes | Yes | N/A |
| ZoneFrontier.Gbx, TMZoneFrontier.Gbx | [CGameCtnZoneFrontier](Src/GBX.NET/Engines/Game/CGameCtnZoneFrontier.chunkl) | Yes | Yes | N/A |
| ZoneGenealogy.Gbx | [CGameCtnZoneGenealogy](Src/GBX.NET/Engines/Game/CGameCtnZoneGenealogy.chunkl) | Yes | Yes | N/A |
| ZoneTransition.Gbx | [CGameCtnZoneTransition](Src/GBX.NET/Engines/Game/CGameCtnZoneTransition.chunkl) | Yes | Yes | N/A |

## Vehicles, cameras and animation

| Extensions | Class | Read | Write | Header metadata |
| --- | --- | --- | --- | --- |
| Anim.Gbx | [CPlugAnimFile](Src/GBX.NET/Engines/Plug/CPlugAnimFile.chunkl) | Partial<sup>3</sup> | Partial<sup>3</sup> | N/A |
| AnimLocSimple.Gbx | [CPlugAnimLocSimple](Src/GBX.NET/Engines/Plug/CPlugAnimLocSimple.chunkl) | Yes | Yes | N/A |
| CamShakeModel.Gbx | [CPlugCamShakeModel](Src/GBX.NET/Engines/Plug/CPlugCamShakeModel.chunkl) | Yes | Yes | N/A |
| CarMarksModel.Gbx | [CSceneVehicleCarMarksModel](Src/GBX.NET/Engines/Scene/CSceneVehicleCarMarksModel.chunkl) | Yes | Yes | N/A |
| CarMarksModelSub.Gbx | [CSceneVehicleCarMarksModelSub](Src/GBX.NET/Engines/Scene/CSceneVehicleCarMarksModelSub.chunkl) | Yes | Yes | N/A |
| CtrlCam.Gbx | [CGameControlCamera](Src/GBX.NET/Engines/Game/CGameControlCamera.chunkl) | Yes | Yes | N/A |
| CtrlCamOrbital3d.Gbx | [CGameControlCameraOrbital3d](Src/GBX.NET/Engines/Game/CGameControlCameraOrbital3d.chunkl) | Yes | Yes | N/A |
| CtrlCamTarget.Gbx | [CGameControlCameraTarget](Src/GBX.NET/Engines/Game/CGameControlCameraTarget.chunkl) | Yes | Yes | N/A |
| CtrlCamTmRace.Gbx | [CGameControlCameraTrackManiaRace](Src/GBX.NET/Engines/TrackMania/CGameControlCameraTrackManiaRace.chunkl) | Yes | Yes | N/A |
| CtrlCamTmRace2.Gbx | [CGameControlCameraTrackManiaRace2](Src/GBX.NET/Engines/TrackMania/CGameControlCameraTrackManiaRace2.chunkl) | Yes | Yes | N/A |
| CtrlCamTmRace3.Gbx | [CGameControlCameraTrackManiaRace3](Src/GBX.NET/Engines/TrackMania/CGameControlCameraTrackManiaRace3.chunkl) | Yes | Yes | N/A |
| Skel.Gbx | [CPlugSkel](Src/GBX.NET/Engines/Plug/CPlugSkel.chunkl) | Yes | Yes | N/A |
| TrackManiaVehicle.Gbx | [CSceneToyVehicleTrackMania](Src/GBX.NET/Engines/Scene/CSceneToyVehicleTrackMania.chunkl) | Yes | Yes | N/A |
| Vehicle.Gbx, TrackManiaVehicle.Gbx | [CSceneVehicleCar](Src/GBX.NET/Engines/Scene/CSceneVehicleCar.chunkl) | Yes | Yes | N/A |
| VehicleCameraInternalModel.Gbx, CtrlCam.Gbx, Cam.Gbx | [CPlugVehicleCameraInternalModel](Src/GBX.NET/Engines/Plug/CPlugVehicleCameraInternalModel.chunkl) | Yes | Yes | N/A |
| VehicleCameraRace2Model.Gbx, Cam.Gbx | [CPlugVehicleCameraRace2Model](Src/GBX.NET/Engines/Plug/CPlugVehicleCameraRace2Model.chunkl) | Yes | Yes | N/A |
| VehicleCameraRace3Model.Gbx, Cam.Gbx | [CPlugVehicleCameraRace3Model](Src/GBX.NET/Engines/Plug/CPlugVehicleCameraRace3Model.chunkl) | Yes | Yes | N/A |
| VehicleCameraRaceModel.Gbx | [CPlugVehicleCameraRaceModel](Src/GBX.NET/Engines/Plug/CPlugVehicleCameraRaceModel.chunkl) | Yes | Yes | N/A |
| VehicleGearBox.Gbx | [CPlugVehicleGearBox](Src/GBX.NET/Engines/Plug/CPlugVehicleGearBox.chunkl) | Yes | Yes | N/A |
| VehicleMaterial.Gbx | [CPlugVehicleMaterial](Src/GBX.NET/Engines/Plug/CPlugVehicleMaterial.chunkl) | Yes | Yes | N/A |
| VehicleTuning.Gbx | [CPlugVehicleCarPhyTuning](Src/GBX.NET/Engines/Plug/CPlugVehicleCarPhyTuning.chunkl) | Yes | Yes | N/A |
| VehicleTunings.Gbx | [CPlugVehiclePhyTunings](Src/GBX.NET/Engines/Plug/CPlugVehiclePhyTunings.chunkl) | Yes | Yes | N/A |
| VehicleVisModel.Gbx | [CPlugVehicleVisModel](Src/GBX.NET/Engines/Plug/CPlugVehicleVisModel.chunkl) | Yes | Yes | N/A |
| VehicleVisModel.Gbx, VehicleStruct.Gbx | [CPlugVehicleVisModelShared](Src/GBX.NET/Engines/Plug/CPlugVehicleVisModelShared.chunkl) | Yes | Yes | N/A |

## Geometry, materials and effects

| Extensions | Class | Read | Write | Header metadata |
| --- | --- | --- | --- | --- |
| BeamModel.Gbx | [CPlugBeamEmitterModel](Src/GBX.NET/Engines/Plug/CPlugBeamEmitterModel.chunkl) | Yes | Yes | N/A |
| BeamModel.Gbx | [CPlugBeamEmitterSubModel](Src/GBX.NET/Engines/Plug/CPlugBeamEmitterSubModel.chunkl) | Yes | Yes | N/A |
| BitmapDecals.Gbx | [CPlugBitmapDecals](Src/GBX.NET/Engines/Plug/CPlugBitmapDecals.chunkl) | Yes | Yes | N/A |
| Bullet.Gbx | [CPlugBulletModel](Src/GBX.NET/Engines/Plug/CPlugBulletModel.chunkl) | Yes | Yes | N/A |
| Crystal.Gbx | [CPlugCrystal](Src/GBX.NET/Engines/Plug/CPlugCrystal.chunkl) | Yes | Yes | N/A |
| Decal.Gbx | [CPlugDecalModel](Src/GBX.NET/Engines/Plug/CPlugDecalModel.chunkl) | Yes | Yes | N/A |
| DecoSolid.Gbx | [CPlugDecoratorSolid](Src/GBX.NET/Engines/Plug/CPlugDecoratorSolid.chunkl) | Yes | Yes | N/A |
| DecoTree.Gbx | [CPlugDecoratorTree](Src/GBX.NET/Engines/Plug/CPlugDecoratorTree.chunkl) | Yes | Yes | N/A |
| DynaObject.Gbx | [CPlugDynaObjectModel](Src/GBX.NET/Engines/Plug/CPlugDynaObjectModel.chunkl) | Yes | Yes | N/A |
| FogBox.Gbx | [CPlugFogVolumeBox](Src/GBX.NET/Engines/Plug/CPlugFogVolumeBox.chunkl) | Yes | Yes | N/A |
| FogMatter.Gbx | [CPlugFogMatter](Src/GBX.NET/Engines/Plug/CPlugFogMatter.chunkl) | Yes | Yes | N/A |
| Font.Gbx | [CPlugFontBitmap](Src/GBX.NET/Engines/Plug/CPlugFontBitmap.chunkl) | Yes | Yes | N/A |
| FxHdrScales_Tech3.Gbx | [CPlugFxHdrScales_Tech3](Src/GBX.NET/Engines/Plug/CPlugFxHdrScales_Tech3.chunkl) | Yes | Yes | N/A |
| FxLensFlareArray.Gbx | [CPlugFxLensFlareArray](Src/GBX.NET/Engines/Plug/CPlugFxLensFlareArray.chunkl) | Yes | Yes | N/A |
| FxLightning.Gbx | [CPlugFxLightning](Src/GBX.NET/Engines/Plug/CPlugFxLightning.chunkl) | Yes | Yes | N/A |
| FxWindOnDecal.Gbx | [CPlugFxWindOnDecal](Src/GBX.NET/Engines/Plug/CPlugFxWindOnDecal.chunkl) | Yes | Yes | N/A |
| GameSkin.Gbx | [CPlugGameSkin](Src/GBX.NET/Engines/Plug/CPlugGameSkin.chunkl) | Yes | Yes | Yes |
| GpuCache.Gbx | [CPlugGpuCompileCache](Src/GBX.NET/Engines/Plug/CPlugGpuCompileCache.chunkl) | Yes | Yes | N/A |
| ImageGen.Gbx | [CPlugFileGen](Src/GBX.NET/Engines/Plug/CPlugFileGen.chunkl) | Yes | Yes | N/A |
| ItemPlacementClass.Gbx | [NPlugItemPlacement_SClass](Src/GBX.NET/Engines/Plug/NPlugItemPlacement_SClass.chunkl) | Yes | Yes | N/A |
| Light.Gbx | [CPlugLight](Src/GBX.NET/Engines/Plug/CPlugLight.chunkl) | Yes | Yes | N/A |
| Material.Gbx | [CPlugMaterial](Src/GBX.NET/Engines/Plug/CPlugMaterial.chunkl) | Yes | Yes | N/A |
| ParticleEmitterSubModel.Gbx | [CPlugParticleEmitterSubModel](Src/GBX.NET/Engines/Plug/CPlugParticleEmitterSubModel.chunkl) | Yes | Yes | N/A |
| ParticleMaterialImpactModel.Gbx | [CPlugParticleMaterialImpactModel](Src/GBX.NET/Engines/Plug/CPlugParticleMaterialImpactModel.chunkl) | Yes | Yes | N/A |
| ParticleModel.Gbx | [CPlugParticleEmitterModel](Src/GBX.NET/Engines/Plug/CPlugParticleEmitterModel.chunkl) | Yes | Yes | N/A |
| Path.Gbx | [CPlugPath](Src/GBX.NET/Engines/Plug/CPlugPath.chunkl) | Yes | Yes | N/A |
| PlacementPatch.Gbx | [CPlugPlacementPatch](Src/GBX.NET/Engines/Plug/CPlugPlacementPatch.chunkl) | Yes | Yes | N/A |
| PlugCharVisModel.Gbx | [CPlugCharVisModel](Src/GBX.NET/Engines/Plug/CPlugCharVisModel.chunkl) | Yes | Yes | N/A |
| PlugFlockModel.Gbx, MotionEmitterFlockModel.Gbx | [CPlugFlockModel](Src/GBX.NET/Engines/Plug/CPlugFlockModel.chunkl) | Yes | Yes | N/A |
| PlugModelShading.Gbx | [CPlugModelShading](Src/GBX.NET/Engines/Plug/CPlugModelShading.chunkl) | Yes | Yes | N/A |
| PlugProbe.Gbx | [CPlugProbe](Src/GBX.NET/Engines/Plug/CPlugProbe.chunkl) | Yes | Yes | N/A |
| PointsInSphere.Gbx | [CPlugPointsInSphereOpt](Src/GBX.NET/Engines/Plug/CPlugPointsInSphereOpt.chunkl) | Yes | Yes | N/A |
| Prefab.Gbx | [CPlugPrefab](Src/GBX.NET/Engines/Plug/CPlugPrefab.chunkl) | Yes | Yes | N/A |
| SamplerArray.Gbx | [CPlugBitmapApplyArray](Src/GBX.NET/Engines/Plug/CPlugBitmapApplyArray.chunkl) | Yes | Yes | N/A |
| Shader.Gbx | [CPlugShader](Src/GBX.NET/Engines/Plug/CPlugShader.chunkl) | Yes | Yes | N/A |
| Shader.Gbx, ShaderMaterial.Gbx, ShaderApply.Gbx | [CPlugShaderApply](Src/GBX.NET/Engines/Plug/CPlugShaderApply.chunkl) | Yes | Yes | N/A |
| Shield.Gbx | [CPlugShieldModel](Src/GBX.NET/Engines/Plug/CPlugShieldModel.chunkl) | Yes | Yes | N/A |
| ShieldModel.Gbx | [CPlugShieldEmitterModel](Src/GBX.NET/Engines/Plug/CPlugShieldEmitterModel.chunkl) | Yes | Yes | N/A |
| Solid.Gbx | [CPlugSolid](Src/GBX.NET/Engines/Plug/CPlugSolid.chunkl) | Yes | Yes | N/A |
| StaticObject.Gbx | [CPlugStaticObjectModel](Src/GBX.NET/Engines/Plug/CPlugStaticObjectModel.chunkl) | Yes | Yes | N/A |
| Texture.Gbx | [CPlugBitmap](Src/GBX.NET/Engines/Plug/CPlugBitmap.chunkl) | Yes | Yes | N/A |
| TextureArray.Gbx | [CPlugBitmapArray](Src/GBX.NET/Engines/Plug/CPlugBitmapArray.chunkl) | Yes | Yes | N/A |
| Turret.Gbx | [CPlugTurret](Src/GBX.NET/Engines/Plug/CPlugTurret.chunkl) | Yes | Yes | N/A |
| VisEntFx.Gbx | [CPlugVisEntFxModel](Src/GBX.NET/Engines/Plug/CPlugVisEntFxModel.chunkl) | Yes | Yes | N/A |
| Visual.Gbx | [CPlugVisual](Src/GBX.NET/Engines/Plug/CPlugVisual.chunkl) | Yes | Yes | N/A |
| Wagon.Gbx | [CPlugTrainWagonModel](Src/GBX.NET/Engines/Plug/CPlugTrainWagonModel.chunkl) | Yes | Yes | N/A |
| WeatherModel.Gbx, MotionManagerWeathers.Gbx | [CPlugWeatherModel](Src/GBX.NET/Engines/Plug/CPlugWeatherModel.chunkl) | Yes | Yes | N/A |

## Audio

| Extensions | Class | Read | Write | Header metadata |
| --- | --- | --- | --- | --- |
| AudioBalance.Gbx | [CPlugAudioBalance](Src/GBX.NET/Engines/Plug/CPlugAudioBalance.chunkl) | Yes | Yes | N/A |
| AudioEnvironment.Gbx | [CPlugAudioEnvironment](Src/GBX.NET/Engines/Plug/CPlugAudioEnvironment.chunkl) | Yes | Yes | N/A |
| Music.Gbx | [CPlugMusic](Src/GBX.NET/Engines/Plug/CPlugMusic.chunkl) | Yes | Yes | N/A |
| Sound.Gbx, Music.Gbx, SoundMulti.Gbx | [CPlugSound](Src/GBX.NET/Engines/Plug/CPlugSound.chunkl) | Yes | Yes | N/A |
| Sound.Gbx, SoundMulti.Gbx | [CPlugSoundMulti](Src/GBX.NET/Engines/Plug/CPlugSoundMulti.chunkl) | Yes | Yes | N/A |
| SoundEngine.Gbx, Sound.Gbx | [CPlugSoundEngine](Src/GBX.NET/Engines/Plug/CPlugSoundEngine.chunkl) | Yes | Yes | N/A |
| SoundEngine.Gbx | [CPlugSoundEngine2](Src/GBX.NET/Engines/Plug/CPlugSoundEngine2.chunkl) | Yes | Yes | N/A |
| SoundGauge.Gbx | [CPlugSoundGauge](Src/GBX.NET/Engines/Plug/CPlugSoundGauge.chunkl) | Yes | Yes | N/A |
| SoundMood.Gbx, Sound.Gbx | [CPlugSoundMood](Src/GBX.NET/Engines/Plug/CPlugSoundMood.chunkl) | Yes | Yes | N/A |
| SoundSurface.Gbx | [CPlugSoundSurface](Src/GBX.NET/Engines/Plug/CPlugSoundSurface.chunkl) | Yes | Yes | N/A |

## Controls and menus

| Extensions | Class | Read | Write | Header metadata |
| --- | --- | --- | --- | --- |
| ColorEffect.Gbx | [CGameMenuColorEffect](Src/GBX.NET/Engines/Game/CGameMenuColorEffect.chunkl) | Yes | Yes | N/A |
| ControlEffect.Gbx | [CControlEffectSimi](Src/GBX.NET/Engines/Control/CControlEffectSimi.chunkl) | Yes | Yes | N/A |
| ControlEffectMaster.Gbx | [CControlEffectMaster](Src/GBX.NET/Engines/Control/CControlEffectMaster.chunkl) | Yes | Yes | N/A |
| ControlEffectMoveFrame.Gbx | [CControlEffectMoveFrame](Src/GBX.NET/Engines/Control/CControlEffectMoveFrame.chunkl) | Yes | Yes | N/A |
| ControlLayout.Gbx | [CControlLayout](Src/GBX.NET/Engines/Control/CControlLayout.chunkl) | Yes | Yes | N/A |
| ControlStyle.Gbx | [CControlStyle](Src/GBX.NET/Engines/Control/CControlStyle.chunkl) | Yes | Yes | N/A |
| ControlUrlLinks.Gbx, Toy.Gbx | [CControlUrlLinks](Src/GBX.NET/Engines/Control/CControlUrlLinks.chunkl) | Yes | Yes | N/A |
| CtnCtlCardMng.Gbx | [CGameControlCardManager](Src/GBX.NET/Engines/Game/CGameControlCardManager.chunkl) | Yes | Yes | N/A |
| CtnDispType.Gbx | [CGameControlDataType](Src/GBX.NET/Engines/Game/CGameControlDataType.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CControlFrame](Src/GBX.NET/Engines/Control/CControlFrame.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCard](Src/GBX.NET/Engines/Game/CGameControlCard.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCardBuddy](Src/GBX.NET/Engines/Game/CGameControlCardBuddy.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCardCtnArticle](Src/GBX.NET/Engines/Game/CGameControlCardCtnArticle.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCardCtnChallengeInfo](Src/GBX.NET/Engines/Game/CGameControlCardCtnChallengeInfo.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCardCtnChapter](Src/GBX.NET/Engines/Game/CGameControlCardCtnChapter.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCardCtnGhostInfo](Src/GBX.NET/Engines/Game/CGameControlCardCtnGhostInfo.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCardCtnNetServerInfo](Src/GBX.NET/Engines/Game/CGameControlCardCtnNetServerInfo.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCardCtnReplayRecordInfo](Src/GBX.NET/Engines/Game/CGameControlCardCtnReplayRecordInfo.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCardCtnVehicle](Src/GBX.NET/Engines/Game/CGameControlCardCtnVehicle.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCardGeneric](Src/GBX.NET/Engines/Game/CGameControlCardGeneric.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCardLadderRanking](Src/GBX.NET/Engines/Game/CGameControlCardLadderRanking.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCardLeague](Src/GBX.NET/Engines/Game/CGameControlCardLeague.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCardMessage](Src/GBX.NET/Engines/Game/CGameControlCardMessage.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameControlCardProfile](Src/GBX.NET/Engines/Game/CGameControlCardProfile.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGameMenuFrame](Src/GBX.NET/Engines/Game/CGameMenuFrame.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGamePlaygroundControlMessages](Src/GBX.NET/Engines/Game/CGamePlaygroundControlMessages.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CGamePlaygroundControlScores](Src/GBX.NET/Engines/Game/CGamePlaygroundControlScores.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CTrackManiaControlCard](Src/GBX.NET/Engines/TrackMania/CTrackManiaControlCard.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CTrackManiaControlCheckPointList](Src/GBX.NET/Engines/TrackMania/CTrackManiaControlCheckPointList.chunkl) | Yes | Yes | N/A |
| Frame.Gbx | [CTrackManiaControlMatchSettingsCard](Src/GBX.NET/Engines/TrackMania/CTrackManiaControlMatchSettingsCard.chunkl) | Yes | Yes | N/A |
| FrameAnimated.Gbx | [CControlFrameAnimated](Src/GBX.NET/Engines/Control/CControlFrameAnimated.chunkl) | Yes | Yes | N/A |
| FrameStyled.Gbx | [CControlFrameStyled](Src/GBX.NET/Engines/Control/CControlFrameStyled.chunkl) | Yes | Yes | N/A |
| GameGridCards.Gbx | [CGameControlGridCard](Src/GBX.NET/Engines/Game/CGameControlGridCard.chunkl) | Yes | Yes | N/A |
| Menu.Gbx | [CGameMenu](Src/GBX.NET/Engines/Game/CGameMenu.chunkl) | Yes | Yes | N/A |
| Pager.Gbx | [CControlPager](Src/GBX.NET/Engines/Control/CControlPager.chunkl) | Yes | Yes | N/A |
| StyleSheet.Gbx | [CControlStyleSheet](Src/GBX.NET/Engines/Control/CControlStyleSheet.chunkl) | Yes | Yes | N/A |
| Toy.Gbx | [CControlLabel](Src/GBX.NET/Engines/Control/CControlLabel.chunkl) | Yes | Yes | N/A |
| UiElement.Gbx | [CControlUiElement](Src/GBX.NET/Engines/Control/CControlUiElement.chunkl) | Yes | Yes | N/A |

## MediaTracker

| Extensions | Class | Read | Write | Header metadata |
| --- | --- | --- | --- | --- |
| CtnMediaBlockCamCustom.Gbx | [CGameCtnMediaBlockCameraCustom](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockCameraCustom.chunkl) | Yes | Yes | N/A |
| CtnMediaBlockCamFxShake.Gbx | [CGameCtnMediaBlockCameraEffectShake](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockCameraEffectShake.chunkl) | Yes | Yes | N/A |
| CtnMediaBlockCamPath.Gbx | [CGameCtnMediaBlockCameraPath](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockCameraPath.chunkl) | Yes | Yes | N/A |
| CtnMediaBlockMusicFx.Gbx | [CGameCtnMediaBlockMusicEffect](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockMusicEffect.chunkl) | Yes | Yes | N/A |
| CtnMediaBlockTransFade.Gbx | [CGameCtnMediaBlockTransitionFade](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockTransitionFade.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlock3dStereo.Gbx | [CGameCtnMediaBlock3dStereo](Src/GBX.NET/Engines/Game/CGameCtnMediaBlock3dStereo.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockBloomHdr.Gbx | [CGameCtnMediaBlockBloomHdr](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockBloomHdr.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockCamGame.Gbx | [CGameCtnMediaBlockCameraGame](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockCameraGame.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockColorGrading.Gbx | [CGameCtnMediaBlockColorGrading](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockColorGrading.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockColoringBase.Gbx | [CGameCtnMediaBlockColoringBase](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockColoringBase.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockColoringCapturable.Gbx | [CGameCtnMediaBlockColoringCapturable](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockColoringCapturable.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockDecal2d.Gbx | [CGameCtnMediaBlockDecal2d](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockDecal2d.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockDirtyLens.Gbx | [CGameCtnMediaBlockDirtyLens](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockDirtyLens.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockDOF.Gbx | [CGameCtnMediaBlockDOF](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockDOF.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockFog.Gbx | [CGameCtnMediaBlockFog](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockFog.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockFxBloom.Gbx | [CGameCtnMediaBlockFxBloom](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockFxBloom.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockFxBlurDepth.Gbx | [CGameCtnMediaBlockFxBlurDepth](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockFxBlurDepth.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockFxBlurMotion.Gbx | [CGameCtnMediaBlockFxBlurMotion](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockFxBlurMotion.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockFxCameraBlend.Gbx | [CGameCtnMediaBlockFxCameraBlend](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockFxCameraBlend.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockFxCameraMap.Gbx | [CGameCtnMediaBlockFxCameraMap](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockFxCameraMap.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockFxColors.Gbx | [CGameCtnMediaBlockFxColors](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockFxColors.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockGhost.Gbx | [CGameCtnMediaBlockGhost](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockGhost.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockImage.Gbx | [CGameCtnMediaBlockImage](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockImage.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockInterface.Gbx | [CGameCtnMediaBlockInterface](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockInterface.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockManialink.Gbx | [CGameCtnMediaBlockManialink](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockManialink.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockObject.Gbx | [CGameCtnMediaBlockObject](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockObject.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockSound.Gbx | [CGameCtnMediaBlockSound](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockSound.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockText.Gbx | [CGameCtnMediaBlockText](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockText.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockToneMapping.Gbx | [CGameCtnMediaBlockToneMapping](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockToneMapping.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockTrails.Gbx | [CGameCtnMediaBlockTrails](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockTrails.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockTriangles.Gbx | [CGameCtnMediaBlockTriangles](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockTriangles.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockTriangles2D.Gbx | [CGameCtnMediaBlockTriangles2D](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockTriangles2D.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockTriangles3D.Gbx | [CGameCtnMediaBlockTriangles3D](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockTriangles3D.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockUi.Gbx | [CGameCtnMediaBlockUi](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockUi.chunkl) | Yes | Yes | N/A |
| GameCtnMediaBlockVehicleLight.Gbx | [CGameCtnMediaBlockVehicleLight](Src/GBX.NET/Engines/Game/CGameCtnMediaBlockVehicleLight.chunkl) | Yes | Yes | N/A |
| GameCtnMediaClipGroup.Gbx | [CGameCtnMediaClipGroup](Src/GBX.NET/Engines/Game/CGameCtnMediaClipGroup.chunkl) | Yes | Yes | N/A |
| GameCtnMediaTrack.Gbx | [CGameCtnMediaTrack](Src/GBX.NET/Engines/Game/CGameCtnMediaTrack.chunkl) | Yes | Yes | N/A |

## Functions

| Extensions | Class | Read | Write | Header metadata |
| --- | --- | --- | --- | --- |
| FuncClouds.Gbx | [CPlugClouds](Src/GBX.NET/Engines/Plug/CPlugClouds.chunkl) | Yes | Yes | N/A |
| FuncCloudsParam.Gbx, PlugCloudsParam.Gbx | [CPlugCloudsParam](Src/GBX.NET/Engines/Plug/CPlugCloudsParam.chunkl) | Yes | Yes | N/A |
| FuncCloudsSolids.Gbx | [CPlugCloudsSolids](Src/GBX.NET/Engines/Plug/CPlugCloudsSolids.chunkl) | Yes | Yes | N/A |
| FuncColorGradient.Gbx | [CFuncColorGradient](Src/GBX.NET/Engines/Function/CFuncColorGradient.chunkl) | Yes | Yes | N/A |
| FuncDayTime.Gbx | [CPlugDayTime](Src/GBX.NET/Engines/Plug/CPlugDayTime.chunkl) | Yes | Yes | N/A |
| FuncEnum.Gbx | [CFuncEnum](Src/GBX.NET/Engines/Function/CFuncEnum.chunkl) | Yes | Yes | N/A |
| FuncFullColorGradient.Gbx | [CFuncFullColorGradient](Src/GBX.NET/Engines/Function/CFuncFullColorGradient.chunkl) | Yes | Yes | N/A |
| FuncKey.Gbx | [CFuncKeys](Src/GBX.NET/Engines/Function/CFuncKeys.chunkl) | Yes | Yes | N/A |
| FuncKeysReals.Gbx | [CFuncKeysReal](Src/GBX.NET/Engines/Function/CFuncKeysReal.chunkl) | Yes | Yes | N/A |
| FuncKeysReals.Gbx | [CPlugCurveSimpleNod](Src/GBX.NET/Engines/Plug/CPlugCurveSimpleNod.chunkl) | Yes | Yes | N/A |
| FuncShader.Gbx | [CFuncShaderLayerUV](Src/GBX.NET/Engines/Function/CFuncShaderLayerUV.chunkl) | Yes | Yes | N/A |
| FuncShader.Gbx | [CFuncShaders](Src/GBX.NET/Engines/Function/CFuncShaders.chunkl) | Yes | Yes | N/A |
| FuncTree.Gbx | [CFuncTreeRotate](Src/GBX.NET/Engines/Function/CFuncTreeRotate.chunkl) | Yes | Yes | N/A |

## Scenes, lighting and other data

| Extensions | Class | Read | Write | Header metadata |
| --- | --- | --- | --- | --- |
| AmbientOcc.Gbx | [CHmsAmbientOcc](Src/GBX.NET/Engines/Hms/CHmsAmbientOcc.chunkl) | Yes | Yes | N/A |
| BufferSelect.Gbx | [CMwRefBufferSelect](Src/GBX.NET/Engines/MwFoundations/CMwRefBufferSelect.chunkl) | Yes | Yes | N/A |
| Character.Gbx | [CSceneMobilCharVis](Src/GBX.NET/Engines/Scene/CSceneMobilCharVis.chunkl) | Yes | Yes | N/A |
| FogBlender.Gbx | [GxFogBlender](Src/GBX.NET/Engines/Graphic/GxFogBlender.chunkl) | Yes | Yes | N/A |
| InputsReplay.Gbx, InputReplay.Gbx | [CInputReplay](Src/GBX.NET/Engines/Input/CInputReplay.chunkl) | Yes | Yes | N/A |
| KinematicConstraint.Gbx | [NPlugDyna_SKinematicConstraint](Src/GBX.NET/Engines/Meta/NPlugDyna_SKinematicConstraint.chunkl) | Yes | Yes | N/A |
| LightMap.Gbx, PackLightMap.Gbx | [CHmsLightMap](Src/GBX.NET/Engines/Hms/CHmsLightMap.chunkl) | Yes | Yes | N/A |
| LightMapMood.Gbx, PackLightMapMood.Gbx | [CHmsLightMapMood](Src/GBX.NET/Engines/Hms/CHmsLightMapMood.chunkl) | Yes | Yes | N/A |
| Link.Gbx | [CSceneObjectLink](Src/GBX.NET/Engines/Scene/CSceneObjectLink.chunkl) | Yes | Yes | N/A |
| Mobil.Gbx | [CSceneMobil](Src/GBX.NET/Engines/Scene/CSceneMobil.chunkl) | Yes | Yes | N/A |
| MotionBase.Gbx | [CMotionCmdBaseParams](Src/GBX.NET/Engines/Motion/CMotionCmdBaseParams.chunkl) | Yes | Yes | N/A |
| MotionTrackShader.Gbx | [CMotionShader](Src/GBX.NET/Engines/Motion/CMotionShader.chunkl) | Yes | Yes | N/A |
| RallyLeafManager.Gbx | [CMotionManagerLeaves](Src/GBX.NET/Engines/Motion/CMotionManagerLeaves.chunkl) | Yes | Yes | N/A |
| RefBuffer.Gbx, FidCache.Gbx | [CMwRefBuffer](Src/GBX.NET/Engines/MwFoundations/CMwRefBuffer.chunkl) | Partial<sup>2</sup> | Partial<sup>2</sup> | N/A |
| Scene2d.Gbx | [CScene2d](Src/GBX.NET/Engines/Scene/CScene2d.chunkl) | Yes | Yes | N/A |
| Scene3d.Gbx | [CSceneLayout](Src/GBX.NET/Engines/Scene/CSceneLayout.chunkl) | Yes | Yes | N/A |
| SceneFx.Gbx | [CSceneFxBloom](Src/GBX.NET/Engines/Scene/CSceneFxBloom.chunkl) | Yes | Yes | N/A |
| SceneFx.Gbx | [CSceneFxCameraBlend](Src/GBX.NET/Engines/Scene/CSceneFxCameraBlend.chunkl) | Yes | Yes | N/A |
| SceneFx.Gbx | [CSceneFxColors](Src/GBX.NET/Engines/Scene/CSceneFxColors.chunkl) | Yes | Yes | N/A |
| SceneFx.Gbx | [CSceneFxDepthOfField](Src/GBX.NET/Engines/Scene/CSceneFxDepthOfField.chunkl) | Yes | Yes | N/A |
| SceneFx.Gbx | [CSceneFxDistor2d](Src/GBX.NET/Engines/Scene/CSceneFxDistor2d.chunkl) | Yes | Yes | N/A |
| SceneFx.Gbx | [CSceneFxEdgeBlender](Src/GBX.NET/Engines/Scene/CSceneFxEdgeBlender.chunkl) | Yes | Yes | N/A |
| SceneFx.Gbx | [CSceneFxHeadTrack](Src/GBX.NET/Engines/Scene/CSceneFxHeadTrack.chunkl) | Yes | Yes | N/A |
| SceneFx.Gbx | [CSceneFxStereoscopy](Src/GBX.NET/Engines/Scene/CSceneFxStereoscopy.chunkl) | Yes | Yes | N/A |
| SceneFx.Gbx | [CSceneFxSuperSample](Src/GBX.NET/Engines/Scene/CSceneFxSuperSample.chunkl) | Yes | Yes | N/A |
| SceneFx.Gbx | [CVisPostFx_BloomHdr](Src/GBX.NET/Engines/Vision/CVisPostFx_BloomHdr.chunkl) | Yes | Yes | N/A |
| SceneFx.Gbx | [CVisPostFx_MotionBlur](Src/GBX.NET/Engines/Vision/CVisPostFx_MotionBlur.chunkl) | Yes | Yes | N/A |
| SceneFx.Gbx | [CVisPostFx_ToneMapping](Src/GBX.NET/Engines/Vision/CVisPostFx_ToneMapping.chunkl) | Yes | Yes | N/A |
| SceneFxNod.Gbx | [CSceneFxNod](Src/GBX.NET/Engines/Scene/CSceneFxNod.chunkl) | Yes | Yes | N/A |
| SceneLight.Gbx | [CSceneLight](Src/GBX.NET/Engines/Scene/CSceneLight.chunkl) | Yes | Yes | N/A |
| Script.Gbx | [CMwCmdBlockMain](Src/GBX.NET/Engines/MwFoundations/CMwCmdBlockMain.chunkl) | Yes | Yes | N/A |
| Toy.Gbx | [CSceneToy](Src/GBX.NET/Engines/Scene/CSceneToy.chunkl) | Yes | Yes | N/A |
| VisionResourceFile.Gbx | [CVisionResourceFile](Src/GBX.NET/Engines/Vision/CVisionResourceFile.chunkl) | Yes | Yes | N/A |

## Limitations

1. Replay writing is restricted. To modify a ghost, extract `CGameCtnGhost`, assign it to a `CGameCtnMediaBlockGhost`, add the block to a `CGameCtnMediaClip`, and save a `.Clip.Gbx` file.
2. `CMwRefBuffer` stores arbitrary node classes. Reading and writing its contents depends on support for those classes.
3. `CPlugAnimFile` rejects unsupported legacy animation node types. Its legacy animation dependencies also have version restrictions.
4. `CGameCoverFlowDesc` does not support the inline mood in version 4 of chunk `0x0316F000`.
5. `CGameItemPlacementParam` supports version 0 of chunk `0x2E020000`. Versions 1 and later are rejected.
