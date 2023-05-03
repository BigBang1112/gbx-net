# Currently supported Gbx file types

Disclaimer: The list may be incomplete. Also, watch the last modified date.

Older extensions | Latest extension | Class | Read (whole) | Write | Read (header)
| --- | --- | --- | --- | --- | ---
| Challenge.Gbx | Map.Gbx | [CGameCtnChallenge](Src/GBX.NET/Engines/Game/CGameCtnChallenge.cs) | Yes | Yes | Yes
| | Replay.Gbx | [CGameCtnReplayRecord](Src/GBX.NET/Engines/Game/CGameCtnReplayRecord.cs) | Yes | No<sup>1</sup> | Yes
| | Ghost.Gbx | [CGameCtnGhost](Src/GBX.NET/Engines/Game/CGameCtnGhost.cs) | Yes | Yes
| | Clip.Gbx | [CGameCtnMediaClip](Src/GBX.NET/Engines/Game/CGameCtnMediaClip.cs) | Yes | Yes
| | Item.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No | Yes
| | Block.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No | Yes
| | Mat.Gbx | [CPlugMaterialUserInst](Src/GBX.NET/Engines/Plug/CPlugMaterialUserInst.cs) | Yes | Yes
| Solid2.Gbx | Mesh.Gbx | [CPlugSolid2Model](Src/GBX.NET/Engines/Plug/CPlugSolid2Model.cs) | Yes<sup>2</sup> | Yes<sup>2</sup>
| | Shape.Gbx | [CPlugSurface](Src/GBX.NET/Engines/Plug/CPlugSurface.cs) | Yes | Yes
| | Macroblock.Gbx | [CGameCtnMacroBlockInfo](Src/GBX.NET/Engines/Game/CGameCtnMacroBlockInfo.cs) | Yes | Yes | Yes
| | LightMapCache.Gbx | [CHmsLightMapCache](Src/GBX.NET/Engines/Hms/CHmsLightMapCache.cs) | Yes | Yes
| | SystemConfig.Gbx | [CSystemConfig](Src/GBX.NET/Engines/System/CSystemConfig.cs) | Yes | Yes
| | FidCache.Gbx | [CMwRefBuffer](Src/GBX.NET/Engines/MwFoundations/CMwRefBuffer.cs) | Yes | Yes
| | Scores.Gbx | [CGamePlayerScore](Src/GBX.NET/Engines/Game/CGamePlayerScore.cs) | Yes | No
| | Profile.Gbx | [CGamePlayerProfile](Src/GBX.NET/Engines/Game/CGamePlayerProfile.cs) | No | No | Yes
| | Spawn.Gbx | [CGameSpawnModel](Src/GBX.NET/Engines/GameData/CGameSpawnModel.cs) | Yes | Yes
| ConstructionCampaign.Gbx | Campaign.Gbx | [CGameCtnCampaign](Src/GBX.NET/Engines/Game/CGameCtnCampaign.cs) | Yes | Yes
| TMCollection.Gbx | Collection.Gbx | [CGameCtnCollection](Src/GBX.NET/Engines/Game/CGameCtnCollection.cs) | Yes | Yes | Yes
| TMDecoration.Gbx | Decoration.Gbx | [CGameCtnDecoration](Src/GBX.NET/Engines/Game/CGameCtnDecoration.cs) | Yes | Yes | Partially
| TMDecorationSize.Gbx | DecorationSize.Gbx | [CGameCtnDecorationSize](Src/GBX.NET/Engines/Game/CGameCtnDecorationSize.cs) | Yes | Yes
| TMEDClassic.Gbx | EDClassic.Gbx | [CGameCtnBlockInfoClassic](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoClassic.cs) | Yes | Yes | Yes
| TMEDClip.Gbx | EDClip.Gbx | [CGameCtnBlockInfoClip](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoClip.cs) | Yes | Yes | Yes
| TMEDFlat.Gbx | EDFlat.Gbx | [CGameCtnBlockInfoFlat](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoFlat.cs) | Yes | Yes | Yes
| TMEDFrontier.Gbx | EDFrontier.Gbx | [CGameCtnBlockInfoFrontier](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoFrontier.cs) | Yes | Yes | Yes
| TMEDPylon.Gbx | EDPylon.Gbx | [CGameCtnBlockInfoPylon](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoPylon.cs) | Yes | Yes | Yes
| TMEDRectAsym.Gbx | EDRectAsym.Gbx | [CGameCtnBlockInfoRectAsym](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoRectAsym.cs) | Yes | Yes | Yes
| TMEDRoad.Gbx | EDRoad.Gbx | [CGameCtnBlockInfoRoad](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoRoad.cs) | Yes | Yes | Yes
| | EDTransition.Gbx | [CGameCtnBlockInfoTransition](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoTransition.cs) | Yes | Yes | Yes
| | VehicleTunings.Gbx | [CPlugVehiclePhyTunings](Src/GBX.NET/Engines/Plug/CPlugVehiclePhyTunings.cs) | Up to TM2 | Up to TM2 | Yes
| | ObjectInfo.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No | Yes
| | Mobil.Gbx | [CSceneMobil](Src/GBX.NET/Engines/Scene/CSceneMobil.cs) | Up to TMUF | No
| | Solid.Gbx | [CPlugSolid](Src/GBX.NET/Engines/Plug/CPlugSolid.cs) | Up to TMUF | No
| | Material.Gbx | [CPlugMaterial](Src/GBX.NET/Engines/Plug/CPlugMaterial.cs) | Up to TM2 | No
| | Texture.Gbx | [CPlugBitmap](Src/GBX.NET/Engines/Plug/CPlugBitmap.cs) | Up to TMUF | No
| | Light.Gbx | [CPlugLight](Src/GBX.NET/Engines/Plug/CPlugLight.cs) | Up to TMUF | Yes

- <sup>1</sup>Safety reasons. Consider extracting `CGameCtnGhost` from `CGameCtnReplayRecord`, transfer it over to `CGameCtnMediaBlockGhost`, add it to `CGameCtnMediaClip`, and save it as `.Clip.Gbx`, which you can then import in MediaTracker.
- <sup>2</sup>May not have perfect support for the `CPlugVisual3D` objects inside and TM2020 could be generally unstable as well.
