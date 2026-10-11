using static GBX.NET.BitHelper;

namespace GBX.NET.Engines.Plug;

public partial class CPlugShaderGeneric
{
    public CPlugShaderGeneric()
    {
        Material = new SMaterial();
    }

    public CPlugShaderGeneric(GameVersion gameVersion) : base(gameVersion)
    {
        Material = new SMaterial(gameVersion);
    }

    public partial class SMaterial
    {
        public bool UseAmbient
        {
            get => GetBit(Flags, 0);
            set => Flags = SetBit(Flags, 0, value);
        }

        public bool UseAmbientColorVertex
        {
            get => GetBit(Flags, 1);
            set => Flags = SetBit(Flags, 1, value);
        }

        public bool UseDiffuse
        {
            get => GetBit(Flags, 2);
            set => Flags = SetBit(Flags, 2, value);
        }

        public bool UseDiffuseColorVertex
        {
            get => GetBit(Flags, 3);
            set => Flags = SetBit(Flags, 3, value);
        }

        public bool UseSpecular
        {
            get => GetBit(Flags, 4);
            set => Flags = SetBit(Flags, 4, value);
        }

        public bool UseEmissive
        {
            get => GetBit(Flags, 5);
            set => Flags = SetBit(Flags, 5, value);
        }

        public bool UseEmissiveColorVertex
        {
            get => GetBit(Flags, 6);
            set => Flags = SetBit(Flags, 6, value);
        }

        public bool NormalizeNs
        {
            get => GetBit(Flags, 7);
            set => Flags = SetBit(Flags, 7, value);
        }

        public bool OnlyInfiniteLights
        {
            get => GetBit(Flags, 8);
            set => Flags = SetBit(Flags, 8, value);
        }

        public bool DblSidedLighting
        {
            get => GetBit(Flags, 9);
            set => Flags = SetBit(Flags, 9, value);
        }
    }
}
