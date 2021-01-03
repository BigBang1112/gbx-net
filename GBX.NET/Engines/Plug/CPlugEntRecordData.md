# CPlugEntRecordData (0x0911F000)

## Chunks

- [0x000](#0x000)

### 0x000

```cs
void Read (GameBoxReader r)
{
    int version = r.ReadInt32();
	int uncompressedSize = r.ReadInt32();
	int compressedSize = r.ReadInt32();
	byte[] data = r.ReadBytes(compressedSize); // Deflate ZLIB compressed recorded data
}
```
