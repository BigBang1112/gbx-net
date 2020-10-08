# CGameCtnMediaBlockCameraEffectShake (0x030A4000)

### Inherits [CGameCtnMediaBlockCameraEffect](CGameCtnMediaBlockCameraEffect.md)

## Chunks

- [0x000](#0x000)

### 0x000

```cs
void Read(GameBoxReader r)
{
	int numKeys = r.ReadInt32();

	for (var i = 0; i < numKeys; i++)
	{
		var time = r.ReadSingle();
        var intensity = r.ReadSingle();
        var speed = r.ReadSingle();
	}
}
```