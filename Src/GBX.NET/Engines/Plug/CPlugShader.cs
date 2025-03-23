namespace GBX.NET.Engines.Plug;

public partial class CPlugShader
{
    public bool IsDoubleSided => (Flags >> 10 & 1) != 0;
    public int BiasZ => (int)(Flags >> 2 & 0x1F);
    public EFillMode FillMode => (EFillMode)(Flags & 3);
    public bool ShadowRecvGrp0 => (Flags >> 36 & 1) != 0; // idk
    public bool ShadowRecvGrp1 => (Flags >> 36 & 2) != 0; // idk
    public bool ShadowRecvGrp2 => (Flags >> 36 & 4) != 0; // idk
    public bool ShadowRecvGrp3 => (Flags >> 36 & 8) != 0; // idk
    public bool ShadowMPassEnable => (Flags >> 11 & 1) != 0;
    public bool ShadowDepthBiasExtra => (Flags >> 48 & 1) != 0; // idk
    public bool ShadowCasterDisable => (Flags >> 50 & 1) != 0; // idk
    public bool ShadowImageSpaceDisable => (Flags >> 53 & 1) != 0; // idk
    public bool ProjectorReceiver => (Flags >> 12 & 1) != 0;
    public bool StaticLighting => (Flags >> 15 & 1) != 0;
    public bool IsFogEyeZEnable => (Flags >> 13 & 1) != 0;
    public bool TweakFogColorBlack => (Flags >> 51 & 1) != 0;
    public bool IsFogPlaneEnable => (Flags >> 14 & 1) != 0;
    public bool TransTreeMip => (Flags >> 9 & 1) != 0;
    public int TexCoordCount => (int)(Flags >> 32 & 0xF); // idk
    //public bool GReqColor0
    //public bool GReqNormal
    //public bool GReqTangentU
    //public bool GReqTangentV
    public bool IsTranslucent => (Flags >> 7 & 1) != 0;
    public bool IsAlphaBlended => (Flags >> 8 & 1) != 0;
    public bool Alpha01SoftEdges => (Flags >> 31 & 1) != 0;
    public EVertexSpace VertexSpace => (EVertexSpace)(Flags >> 54 & 3);
    public ESpriteColor0 SpriteColor0 => (ESpriteColor0)(Flags >> 56 & 7);
    public bool VIdReflected => (VisibleId & 1) != 0;
    public bool VIdReflectMirror => (VisibleId >> 1 & 1) != 0;
    public bool VIdRefracted => (VisibleId >> 2 & 1) != 0;
    public bool VIdViewDepBump => (VisibleId >> 3 & 1) != 0;
    public bool VIdViewDepOcclusion => (VisibleId >> 4 & 1) != 0;
    public bool VIdOnlyRefracted => (VisibleId >> 5 & 1) != 0;
    public bool VIdHideWhenUnderground => (VisibleId >> 6 & 1) != 0;
    public bool VIdHideWhenOverground => (VisibleId >> 7 & 1) != 0;
    public bool VIdHideAlways => (VisibleId >> 8 & 1) != 0;
    public bool VIdViewDepWindIntens => (VisibleId >> 9 & 1) != 0;
    public bool VIdBackground => (VisibleId >> 10 & 1) != 0;
    public bool VIdGrassRGB => (VisibleId >> 11 & 1) != 0;
    public bool VIdLightGenP => (VisibleId >> 12 & 1) != 0;
    public bool VIdVehicle => (VisibleId >> 13 & 1) != 0;
    public bool VIdHideOnlyDirect => (VisibleId >> 14 & 1) != 0;
    public bool SortCustom => (Flags >> 18 & 1) != 0;
    public ESortPosition SortPosition => (ESortPosition)(Flags >> 26 & 3);
    public int SortIndex => (int)(Flags >> 19 & 0x7F);
    public bool SortZTest => (Flags >> 28 & 1) != 0;
    public bool SortZWrite => (Flags >> 29 & 1) != 0;
    public bool ComputeFillValue => (Flags >> 49 & 1) != 0;
}
