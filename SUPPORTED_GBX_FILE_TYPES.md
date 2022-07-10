# Currently supported Gbx file types

Disclaimer: The list may be incomplete. Also, watch the last modified date.

Older extensions | Latest extension | Class | Can read | Can write
| --- | --- | --- | --- | ---
| Challenge.Gbx | Map.Gbx | [CGameCtnChallenge](Src/GBX.NET/Engines/Game/CGameCtnChallenge.cs) | Yes | Yes
| | Replay.Gbx | [CGameCtnReplayRecord](Src/GBX.NET/Engines/Game/CGameCtnReplayRecord.cs) | Yes | No
| | Ghost.Gbx | [CGameCtnGhost](Src/GBX.NET/Engines/Game/CGameCtnGhost.cs) | Yes | Yes
| | Clip.Gbx | [CGameCtnMediaClip](Src/GBX.NET/Engines/Game/CGameCtnMediaClip.cs) | Yes | Yes
| | Item.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No
| | Block.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No
| | Macroblock.Gbx | [CGameCtnMacroBlockInfo](Src/GBX.NET/Engines/Game/CGameCtnMacroBlockInfo.cs) | *Fix soon* | No
| | LightMapCache.Gbx | [CHmsLightMapCache](Src/GBX.NET/Engines/Hms/CHmsLightMapCache.cs) | Yes | Yes
| | SystemConfig.Gbx | [CSystemConfig](Src/GBX.NET/Engines/System/CSystemConfig.cs) | Yes | Yes
| ConstructionCampaign.Gbx | Campaign.Gbx | [CGameCtnCampaign](Src/GBX.NET/Engines/Game/CGameCtnCampaign.cs) | Yes | Yes
| TMCollection.Gbx | Collection.Gbx | [CGameCtnCollection](Src/GBX.NET/Engines/Game/CGameCtnCollection.cs) | Yes | Yes
| TMDecoration.Gbx | Decoration.Gbx | [CGameCtnDecoration](Src/GBX.NET/Engines/Game/CGameCtnDecoration.cs) | Yes | Yes
| TMEDClassic.Gbx | EDClassic.Gbx | [CGameCtnBlockInfoClassic](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoClassic.cs) | Up to ManiaPlanet 4 | Up to ManiaPlanet 4
| TMEDClip.Gbx | EDClip.Gbx | [CGameCtnBlockInfoClip](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoClip.cs) | Up to ManiaPlanet 4 | Up to ManiaPlanet 4
| TMEDFlat.Gbx | EDFlat.Gbx | [CGameCtnBlockInfoFlat](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoFlat.cs) | Up to ManiaPlanet 4 | Up to ManiaPlanet 4
| TMEDFrontier.Gbx | EDFrontier.Gbx | [CGameCtnBlockInfoFrontier](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoFrontier.cs) | Ues | Yes
| TMEDPylon.Gbx | EDPylon.Gbx | [CGameCtnBlockInfoPylon](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoPylon.cs) | Yes | Yes
| TMEDRectAsym.Gbx | EDRectAsym.Gbx | [CGameCtnBlockInfoRectAsym](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoRectAsym.cs) | Yes | Yes
| TMEDRoad.Gbx | EDRoad.Gbx | [CGameCtnBlockInfoRoad](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoRoad.cs) | Yes | Yes
| | EDTransition.Gbx | [CGameCtnBlockInfoTransition](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoTransition.cs) | Yes | Yes
| | Solid.Gbx | [CPlugSolid](Src/GBX.NET/Engines/Plug/CPlugSolid.cs) | Up to TMUF | No
| | Material.Gbx | [CPlugMaterial](Src/GBX.NET/Engines/Plug/CPlugMaterial.cs) | Up to TMUF | No
| | Texture.Gbx | [CPlugBitmap](Src/GBX.NET/Engines/Plug/CPlugBitmap.cs) | Up to TMUF | No
