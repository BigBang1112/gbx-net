# TBD

| Latest extension | Class | Can read | Can write | Other extension/s
| --- | --- | --- | --- | ---
| Map.Gbx | [CGameCtnChallenge](Src/GBX.NET/Engines/Game/CGameCtnChallenge.cs) | Yes | Yes | Challenge.Gbx
| Replay.Gbx | [CGameCtnReplayRecord](Src/GBX.NET/Engines/Game/CGameCtnReplayRecord.cs) | Yes | **No<sup>1</sup>**
| Ghost.Gbx | [CGameCtnGhost](Src/GBX.NET/Engines/Game/CGameCtnGhost.cs) | Yes | **Yes**
| Clip.Gbx | [CGameCtnMediaClip](Src/GBX.NET/Engines/Game/CGameCtnMediaClip.cs) | Yes | Yes
| Item.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No
| Block.Gbx | [CGameItemModel](Src/GBX.NET/Engines/GameData/CGameItemModel.cs) | Yes | No
| Macroblock.Gbx | [CGameCtnMacroBlockInfo](Src/GBX.NET/Engines/Game/CGameCtnMacroBlockInfo.cs) | *Fix soon* | No
| LightMapCache.Gbx | [CHmsLightMapCache](Src/GBX.NET/Engines/Hms/CHmsLightMapCache.cs) | Yes | Yes
| SystemConfig.Gbx | [CSystemConfig](Src/GBX.NET/Engines/System/CSystemConfig.cs) | Yes | Yes
| Campaign.Gbx | [CGameCtnCampaign](Src/GBX.NET/Engines/Game/CGameCtnCampaign.cs) | Yes | Yes | ConstructionCampaign.Gbx
| Collection.Gbx | [CGameCtnCollection](Src/GBX.NET/Engines/Game/CGameCtnCollection.cs) | Yes | Yes | TMCollection.Gbx
| Decoration.Gbx | [CGameCtnDecoration](Src/GBX.NET/Engines/Game/CGameCtnDecoration.cs) | Yes | Yes | TMDecoration.Gbx
| EDClassic.Gbx | [CGameCtnBlockInfoClassic](Src/GBX.NET/Engines/Game/CGameCtnBlockInfoClassic.cs) | *Fix soon* | No | TMEDClassic.Gbx
| Solid.Gbx | [CPlugSolid](Src/GBX.NET/Engines/Plug/CPlugSolid.cs) | Partially<sup>2</sup> | No
| Material.Gbx | [CPlugMaterial](Src/GBX.NET/Engines/Plug/CPlugMaterial.cs) | Partially<sup>2</sup> | No
| Texture.Gbx | [CPlugBitmap](Src/GBX.NET/Engines/Plug/CPlugBitmap.cs) | Partially<sup>2</sup> | No