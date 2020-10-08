# CGameCtnMediaBlock3dStereo (0x03024000)

### Inherits [CGameCtnMediaBlock](CGameCtnMediaBlock.md)

## Chunks

- [0x000](#0x000)

### 0x000

```cs
void Read(GameBoxReader r)
{
	int numKeys = r.ReadInt32();

	for (var i = 0; i < numKeys; i++)
	{
		float time = rw.Reader.ReadSingle();
        float upToMax = rw.Reader.ReadSingle();
        float screenDist = rw.Reader.ReadSingle();
	}
}
```