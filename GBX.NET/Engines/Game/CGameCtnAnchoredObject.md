# CGameCtnAnchoredObject (0x03101000)

## Chunks

- [0x002](#0x002)
- 0x004 - skippable

### 0x002

```cs
void Read(GameBoxReader r)
{
    int version = r.ReadInt32();
    Meta itemModel = r.ReadMeta();
    Vec3 pitchYawRoll = r.ReadVec3();
    Byte3 blockUnitCoord = r.ReadByte3();
    int u01 = r.ReadInt32();
    Vec3 absolutePositionInMap = r.ReadVec3();
    int specialWaypoint = r.ReadInt32();
    if(specialWaypoint != -1)
        CGameWaypointSpecialProperty waypointSpecialProperty = Parse<CGameWaypointSpecialProperty>();

    if (version >= 4)
    {
        short flags = r.ReadInt16();

        if (version >= 5)
        {
            Vec3 pivotPosition = r.ReadVec3();

            if (version >= 6)
            {
                float scale = r.ReadSingle();

                if (version >= 8) // TM 2020
                {
                    Vec3 u02 = r.ReadVec3();
                    Vec3 u03 = r.ReadVec3();
                }
            }
        }
    }
}
```