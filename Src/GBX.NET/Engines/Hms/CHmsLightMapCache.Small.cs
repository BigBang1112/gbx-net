namespace GBX.NET.Engines.Hms;

public partial class CHmsLightMapCache
{
    public enum EPlugImageFileFmt
    {
        Jpg = 2,
        Webp = 5,
    }

    public class Small
    {
        private int diffuseQuality = 91;
        private int diffuseWidth = 1024;
        private int diffuseHeight = 1024;
        private int bumpIntensWidth = 1024;
        private int bumpIntensHeight = 1024;
        private int bumpIntensQuality = 80;
        private EPlugImageFileFmt diffuseFormat = EPlugImageFileFmt.Webp;
        private int spriteWidth;
        private int spriteHeight;
        private EPlugImageFileFmt bumpIntensFormat = EPlugImageFileFmt.Webp;

        /// <summary>
        /// Diffuse quality (0-100)
        /// </summary>
        public int DiffuseQuality { get => diffuseQuality; set => diffuseQuality = value; }

        public int DiffuseWidth { get => diffuseWidth; set => diffuseWidth = value; }
        public int DiffuseHeight { get => diffuseHeight; set => diffuseHeight = value; }
        public int BumpIntensWidth { get => bumpIntensWidth; set => bumpIntensWidth = value; }
        public int BumpIntensHeight { get => bumpIntensHeight; set => bumpIntensHeight = value; }

        /// <summary>
        /// Bump intensity quality (0-100)
        /// </summary>
        public int BumpIntensQuality { get => bumpIntensQuality; set => bumpIntensQuality = value; }

        public EPlugImageFileFmt DiffuseFormat { get => diffuseFormat; set => diffuseFormat = value; }
        public int SpriteWidth { get => spriteWidth; set => spriteWidth = value; }
        public int SpriteHeight { get => spriteHeight; set => spriteHeight = value; }
        public EPlugImageFileFmt BumpIntensFormat { get => bumpIntensFormat; set => bumpIntensFormat = value; }

        public void ReadWrite(int version, ref CHmsLightMapCache? lightmapCache, Frame[] lightmapFrames, GbxReaderWriter rw)
        {
            if (lightmapFrames is null)
            {
                throw new ArgumentNullException(nameof(lightmapFrames));
            }

            rw.Node<CHmsLightMapCache>(ref lightmapCache);

            rw.Int32(ref diffuseQuality);

            if (version >= 3)
            {
                rw.Int32(ref diffuseWidth);
                rw.Int32(ref diffuseHeight);
                rw.Int32(ref bumpIntensWidth);
                rw.Int32(ref bumpIntensHeight);
                rw.Int32(ref bumpIntensQuality);

                if (version >= 4)
                {
                    rw.EnumInt32<EPlugImageFileFmt>(ref diffuseFormat);
                }
            }

            if (version < 5)
            {
                lightmapFrames[0].U01 = rw.Data(lightmapFrames[0].U01);
            }
            else
            {
                foreach (var frame in lightmapFrames)
                {
                    frame.U01 = rw.Data(frame.U01);
                    frame.U02 = rw.Single(frame.U02);

                    if (version >= 6)
                    {
                        // NHmsLightMapCache::ArchiveToZip
                        frame.Version = rw.Int32(frame.Version);

                        if (frame.Version < 2)
                        {
                            frame.U03 = rw.Single(frame.U03);
                            frame.U04 = rw.Int32(frame.U04);

                            if (frame.Version != 0)
                            {
                                frame.U05 = rw.Int32(frame.U05);
                            }

                            frame.U06 = rw.Int32(frame.U06);
                            frame.U07 = rw.Int32(frame.U07);
                            frame.U08 = rw.Int32(frame.U08);
                            frame.U09 = rw.Iso4(frame.U09);
                            frame.U10 = rw.Array<short>(frame.U10);
                        }
                        else
                        {
                            frame.U11 = rw.Single(frame.U11);
                            frame.U12 = rw.Int32(frame.U12);

                            if (frame.Version >= 5)
                            {
                                frame.U39 = rw.Single(frame.U39);
                                frame.U40 = rw.Int32(frame.U40);

                                if (frame.Version >= 6)
                                {
                                    frame.U41 = rw.Single(frame.U41);
                                    frame.U42 = rw.Int32(frame.U42);
                                }
                            }

                            frame.U13 = rw.Int32(frame.U13);
                            frame.U14 = rw.Int32(frame.U14);
                            frame.U15 = rw.Int32(frame.U15);

                            if (frame.Version < 4)
                            {
                                frame.U16 = rw.ArrayReadableWritable<ProbeGridBoxOld>(frame.U16);
                            }
                            else
                            {
                                frame.U17 = rw.ArrayReadableWritable<ProbeGridBox>(frame.U17);
                            }

                            frame.U18 = rw.Array<Int2>(frame.U18);
                            frame.U19 = rw.Array<short>(frame.U19);

                            if (frame.Version >= 3)
                            {
                                frame.U20 = rw.Int32(frame.U20);
                                frame.U21 = rw.Int32(frame.U21);
                                frame.U22 = rw.Int32(frame.U22);
                                frame.U23 = rw.Int32(frame.U23);
                                frame.U24 = rw.Int32(frame.U24);
                                frame.U25 = rw.Int32(frame.U25);

                                if (frame.Version >= 4)
                                {
                                    frame.U33 = rw.Int32(frame.U33);
                                    frame.U34 = rw.Int32(frame.U34);
                                    frame.U35 = rw.Int32(frame.U35);
                                }

                                frame.U26 = rw.Single(frame.U26);
                                frame.U27 = rw.Single(frame.U27);
                                frame.U28 = rw.Single(frame.U28);
                                frame.U29 = rw.Single(frame.U29);
                                frame.U30 = rw.Single(frame.U30);
                                frame.U31 = rw.Single(frame.U31);
                                frame.U32 = rw.Array<int>(frame.U32);
                            }
                        }
                        //

                        if (version >= 8)
                        {
                            frame.U36 = rw.Int32(frame.U36);
                            frame.U37 = rw.Int32(frame.U37);

                            // Unchecked
                            if (version >= 10)
                            {
                                frame.U38 = rw.Int32(frame.U38);
                            }
                        }
                    }
                }
            }

            rw.Int32(ref spriteWidth);
            rw.Int32(ref spriteHeight);

            if (version < 5)
            {
                lightmapFrames[0].U02 = rw.Single(lightmapFrames[0].U02);
            }

            if (version >= 7)
            {
                rw.EnumInt32<EPlugImageFileFmt>(ref bumpIntensFormat);
            }
        }
    }

    public partial class Frame : IVersionable
    {
        public byte[]? U01 { get; set; }
        public float U02 { get; set; }
        public int Version { get; set; }
        public float? U03 { get; set; }
        public int? U04 { get; set; }
        public int? U05 { get; set; }
        public int? U06 { get; set; }
        public int? U07 { get; set; }
        public int? U08 { get; set; }
        public Iso4? U09 { get; set; }
        public short[]? U10 { get; set; }
        public float? U11 { get; set; }
        public int? U12 { get; set; }
        public int? U13 { get; set; }
        public int? U14 { get; set; }
        public int? U15 { get; set; }
        public ProbeGridBoxOld[]? U16 { get; set; }
        public ProbeGridBox[]? U17 { get; set; }
        public Int2[]? U18 { get; set; }
        public short[]? U19 { get; set; }
        public int? U20 { get; set; }
        public int? U21 { get; set; }
        public int? U22 { get; set; }
        public int? U23 { get; set; }
        public int? U24 { get; set; }
        public int? U25 { get; set; }
        public float? U26 { get; set; }
        public float? U27 { get; set; }
        public float? U28 { get; set; }
        public float? U29 { get; set; }
        public float? U30 { get; set; }
        public float? U31 { get; set; }
        public int[]? U32 { get; set; }
        public int? U33 { get; set; }
        public int? U34 { get; set; }
        public int? U35 { get; set; }
        public int? U36 { get; set; }
        public int? U37 { get; set; }
        public int? U38 { get; set; }
        public float? U39 { get; set; }
        public int? U40 { get; set; }
        public float? U41 { get; set; }
        public int? U42 { get; set; }
    }
}
