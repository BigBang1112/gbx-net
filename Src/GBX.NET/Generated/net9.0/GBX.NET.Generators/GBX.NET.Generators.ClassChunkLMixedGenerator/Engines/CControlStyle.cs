namespace GBX.NET.Engines.Control;

/// <remarks>ID: 0x07017000</remarks>
[Class(0x07017000)]
public partial class CControlStyle : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x07017000;




    private CMwCmdBlockMain? focusGainedScript;
    [AppliedWithChunk<Chunk07017000>]
    public CMwCmdBlockMain? FocusGainedScript { get => focusGainedScript; set => focusGainedScript = value; }

    private CMwCmdBlockMain? focusLostScript;
    [AppliedWithChunk<Chunk07017000>]
    public CMwCmdBlockMain? FocusLostScript { get => focusLostScript; set => focusLostScript = value; }

    private CPlugSound? focusSound;
    [AppliedWithChunk<Chunk07017000>]
    public CPlugSound? FocusSound { get => focusSoundFile?.GetNode(ref focusSound) ?? focusSound; set => focusSound = value; }
    private Components.GbxRefTableFile? focusSoundFile;
    public Components.GbxRefTableFile? FocusSoundFile { get => focusSoundFile; set => focusSoundFile = value; }
    public CPlugSound? GetFocusSound(GbxReadSettings settings = default, bool exceptions = false) => focusSoundFile?.GetNode(ref focusSound, settings, exceptions) ?? focusSound;

    private CPlugSound? entrySound;
    [AppliedWithChunk<Chunk07017004>]
    public CPlugSound? EntrySound { get => entrySound; set => entrySound = value; }

    private CPlugShader? defaultShader;
    [AppliedWithChunk<Chunk07017009>]
    public CPlugShader? DefaultShader { get => defaultShader; set => defaultShader = value; }

    private bool fitTextSize;
    [AppliedWithChunk<Chunk0701700B>]
    public bool FitTextSize { get => fitTextSize; set => fitTextSize = value; }

    private CPlugSound? enumSound;
    [AppliedWithChunk<Chunk0701700D>]
    [AppliedWithChunk<Chunk07017010>]
    public CPlugSound? EnumSound { get => enumSound; set => enumSound = value; }

    private CPlugShader? enumListShader;
    [AppliedWithChunk<Chunk0701700D>]
    [AppliedWithChunk<Chunk07017010>]
    public CPlugShader? EnumListShader { get => enumListShader; set => enumListShader = value; }

    private int enumMaxElemCount;
    [AppliedWithChunk<Chunk0701700D>]
    [AppliedWithChunk<Chunk07017010>]
    public int EnumMaxElemCount { get => enumMaxElemCount; set => enumMaxElemCount = value; }

    private float enumIconWidth;
    [AppliedWithChunk<Chunk0701700D>]
    [AppliedWithChunk<Chunk07017010>]
    public float EnumIconWidth { get => enumIconWidth; set => enumIconWidth = value; }

    private float enumIconHeight;
    [AppliedWithChunk<Chunk0701700D>]
    [AppliedWithChunk<Chunk07017010>]
    public float EnumIconHeight { get => enumIconHeight; set => enumIconHeight = value; }

    private CPlugSound? actionSound;
    [AppliedWithChunk<Chunk0701700E>]
    public CPlugSound? ActionSound { get => actionSoundFile?.GetNode(ref actionSound) ?? actionSound; set => actionSound = value; }
    private Components.GbxRefTableFile? actionSoundFile;
    public Components.GbxRefTableFile? ActionSoundFile { get => actionSoundFile; set => actionSoundFile = value; }
    public CPlugSound? GetActionSound(GbxReadSettings settings = default, bool exceptions = false) => actionSoundFile?.GetNode(ref actionSound, settings, exceptions) ?? actionSound;

    private float buttonIconWidth;
    [AppliedWithChunk<Chunk0701700E>]
    public float ButtonIconWidth { get => buttonIconWidth; set => buttonIconWidth = value; }

    private float buttonIconHeight;
    [AppliedWithChunk<Chunk0701700E>]
    public float ButtonIconHeight { get => buttonIconHeight; set => buttonIconHeight = value; }

    private CFuncEnum? buttonDefaultIcons;
    [AppliedWithChunk<Chunk0701700E>]
    public CFuncEnum? ButtonDefaultIcons { get => buttonDefaultIconsFile?.GetNode(ref buttonDefaultIcons) ?? buttonDefaultIcons; set => buttonDefaultIcons = value; }
    private Components.GbxRefTableFile? buttonDefaultIconsFile;
    public Components.GbxRefTableFile? ButtonDefaultIconsFile { get => buttonDefaultIconsFile; set => buttonDefaultIconsFile = value; }
    public CFuncEnum? GetButtonDefaultIcons(GbxReadSettings settings = default, bool exceptions = false) => buttonDefaultIconsFile?.GetNode(ref buttonDefaultIcons, settings, exceptions) ?? buttonDefaultIcons;

    private int enumForceDisplayType;
    [AppliedWithChunk<Chunk07017010>]
    public int EnumForceDisplayType { get => enumForceDisplayType; set => enumForceDisplayType = value; }

    private CFuncEnum? enumForceIcons;
    [AppliedWithChunk<Chunk07017010>]
    public CFuncEnum? EnumForceIcons { get => enumForceIconsFile?.GetNode(ref enumForceIcons) ?? enumForceIcons; set => enumForceIcons = value; }
    private Components.GbxRefTableFile? enumForceIconsFile;
    public Components.GbxRefTableFile? EnumForceIconsFile { get => enumForceIconsFile; set => enumForceIconsFile = value; }
    public CFuncEnum? GetEnumForceIcons(GbxReadSettings settings = default, bool exceptions = false) => enumForceIconsFile?.GetNode(ref enumForceIcons, settings, exceptions) ?? enumForceIcons;

    private bool quadIsLines;
    [AppliedWithChunk<Chunk07017014>]
    [AppliedWithChunk<Chunk07017015>]
    [AppliedWithChunk<Chunk07017017>]
    public bool QuadIsLines { get => quadIsLines; set => quadIsLines = value; }

    private bool quadIsFill;
    [AppliedWithChunk<Chunk07017014>]
    [AppliedWithChunk<Chunk07017015>]
    [AppliedWithChunk<Chunk07017017>]
    public bool QuadIsFill { get => quadIsFill; set => quadIsFill = value; }

    private float quadZ;
    [AppliedWithChunk<Chunk07017014>]
    [AppliedWithChunk<Chunk07017015>]
    [AppliedWithChunk<Chunk07017017>]
    public float QuadZ { get => quadZ; set => quadZ = value; }

    private float quadZLines;
    [AppliedWithChunk<Chunk07017014>]
    [AppliedWithChunk<Chunk07017015>]
    [AppliedWithChunk<Chunk07017017>]
    public float QuadZLines { get => quadZLines; set => quadZLines = value; }

    private Vec4 quadGradientColor0;
    [AppliedWithChunk<Chunk07017014>]
    [AppliedWithChunk<Chunk07017015>]
    [AppliedWithChunk<Chunk07017017>]
    public Vec4 QuadGradientColor0 { get => quadGradientColor0; set => quadGradientColor0 = value; }

    private Vec4 quadGradientColor1;
    [AppliedWithChunk<Chunk07017014>]
    [AppliedWithChunk<Chunk07017015>]
    [AppliedWithChunk<Chunk07017017>]
    public Vec4 QuadGradientColor1 { get => quadGradientColor1; set => quadGradientColor1 = value; }

    private Vec4 quadLinesColor;
    [AppliedWithChunk<Chunk07017014>]
    public Vec4 QuadLinesColor { get => quadLinesColor; set => quadLinesColor = value; }

    private Vec2 quad_UvTopLeft;
    [AppliedWithChunk<Chunk07017015>]
    [AppliedWithChunk<Chunk07017017>]
    public Vec2 Quad_UvTopLeft { get => quad_UvTopLeft; set => quad_UvTopLeft = value; }

    private Vec2 quad_UvBottomRight;
    [AppliedWithChunk<Chunk07017015>]
    [AppliedWithChunk<Chunk07017017>]
    public Vec2 Quad_UvBottomRight { get => quad_UvBottomRight; set => quad_UvBottomRight = value; }

    private CControlEffectMaster? effectMaster;
    [AppliedWithChunk<Chunk07017016>]
    public CControlEffectMaster? EffectMaster { get => effectMasterFile?.GetNode(ref effectMaster) ?? effectMaster; set => effectMaster = value; }
    private Components.GbxRefTableFile? effectMasterFile;
    public Components.GbxRefTableFile? EffectMasterFile { get => effectMasterFile; set => effectMasterFile = value; }
    public CControlEffectMaster? GetEffectMaster(GbxReadSettings settings = default, bool exceptions = false) => effectMasterFile?.GetNode(ref effectMaster, settings, exceptions) ?? effectMaster;

    private float skew;
    [AppliedWithChunk<Chunk07017017>]
    public float Skew { get => skew; set => skew = value; }

    private Vec4 lineGradientColor0;
    [AppliedWithChunk<Chunk07017018>]
    public Vec4 LineGradientColor0 { get => lineGradientColor0; set => lineGradientColor0 = value; }

    private Vec4 lineGradientColor1;
    [AppliedWithChunk<Chunk07017018>]
    public Vec4 LineGradientColor1 { get => lineGradientColor1; set => lineGradientColor1 = value; }

    private CPlugFont? font;
    [AppliedWithChunk<Chunk0701701B>]
    public CPlugFont? Font { get => fontFile?.GetNode(ref font) ?? font; set => font = value; }
    private Components.GbxRefTableFile? fontFile;
    public Components.GbxRefTableFile? FontFile { get => fontFile; set => fontFile = value; }
    public CPlugFont? GetFont(GbxReadSettings settings = default, bool exceptions = false) => fontFile?.GetNode(ref font, settings, exceptions) ?? font;

    private float sliderBarWidth;
    [AppliedWithChunk<Chunk0701701C>]
    public float SliderBarWidth { get => sliderBarWidth; set => sliderBarWidth = value; }

    private float sliderBarHeight;
    [AppliedWithChunk<Chunk0701701C>]
    public float SliderBarHeight { get => sliderBarHeight; set => sliderBarHeight = value; }

    private float sliderCursorWidth;
    [AppliedWithChunk<Chunk0701701C>]
    public float SliderCursorWidth { get => sliderCursorWidth; set => sliderCursorWidth = value; }

    private float sliderCursorHeight;
    [AppliedWithChunk<Chunk0701701C>]
    public float SliderCursorHeight { get => sliderCursorHeight; set => sliderCursorHeight = value; }

    private CPlugSound? sliderSound;
    [AppliedWithChunk<Chunk0701701C>]
    public CPlugSound? SliderSound { get => sliderSound; set => sliderSound = value; }

    private CFuncEnum? sliderBarIcons;
    [AppliedWithChunk<Chunk0701701C>]
    public CFuncEnum? SliderBarIcons { get => sliderBarIconsFile?.GetNode(ref sliderBarIcons) ?? sliderBarIcons; set => sliderBarIcons = value; }
    private Components.GbxRefTableFile? sliderBarIconsFile;
    public Components.GbxRefTableFile? SliderBarIconsFile { get => sliderBarIconsFile; set => sliderBarIconsFile = value; }
    public CFuncEnum? GetSliderBarIcons(GbxReadSettings settings = default, bool exceptions = false) => sliderBarIconsFile?.GetNode(ref sliderBarIcons, settings, exceptions) ?? sliderBarIcons;

    private CFuncEnum? sliderCursorIcons;
    [AppliedWithChunk<Chunk0701701C>]
    public CFuncEnum? SliderCursorIcons { get => sliderCursorIconsFile?.GetNode(ref sliderCursorIcons) ?? sliderCursorIcons; set => sliderCursorIcons = value; }
    private Components.GbxRefTableFile? sliderCursorIconsFile;
    public Components.GbxRefTableFile? SliderCursorIconsFile { get => sliderCursorIconsFile; set => sliderCursorIconsFile = value; }
    public CFuncEnum? GetSliderCursorIcons(GbxReadSettings settings = default, bool exceptions = false) => sliderCursorIconsFile?.GetNode(ref sliderCursorIcons, settings, exceptions) ?? sliderCursorIcons;

    private bool focusAreaEnable;
    [AppliedWithChunk<Chunk0701701D>]
    public bool FocusAreaEnable { get => focusAreaEnable; set => focusAreaEnable = value; }

    private CPlugMaterial? focusAreaMaterial;
    [AppliedWithChunk<Chunk0701701D>]
    public CPlugMaterial? FocusAreaMaterial { get => focusAreaMaterialFile?.GetNode(ref focusAreaMaterial) ?? focusAreaMaterial; set => focusAreaMaterial = value; }
    private Components.GbxRefTableFile? focusAreaMaterialFile;
    public Components.GbxRefTableFile? FocusAreaMaterialFile { get => focusAreaMaterialFile; set => focusAreaMaterialFile = value; }
    public CPlugMaterial? GetFocusAreaMaterial(GbxReadSettings settings = default, bool exceptions = false) => focusAreaMaterialFile?.GetNode(ref focusAreaMaterial, settings, exceptions) ?? focusAreaMaterial;

    private CPlugMaterial? focusAreaMaterialReadOnly;
    [AppliedWithChunk<Chunk0701701D>]
    public CPlugMaterial? FocusAreaMaterialReadOnly { get => focusAreaMaterialReadOnlyFile?.GetNode(ref focusAreaMaterialReadOnly) ?? focusAreaMaterialReadOnly; set => focusAreaMaterialReadOnly = value; }
    private Components.GbxRefTableFile? focusAreaMaterialReadOnlyFile;
    public Components.GbxRefTableFile? FocusAreaMaterialReadOnlyFile { get => focusAreaMaterialReadOnlyFile; set => focusAreaMaterialReadOnlyFile = value; }
    public CPlugMaterial? GetFocusAreaMaterialReadOnly(GbxReadSettings settings = default, bool exceptions = false) => focusAreaMaterialReadOnlyFile?.GetNode(ref focusAreaMaterialReadOnly, settings, exceptions) ?? focusAreaMaterialReadOnly;

    private CPlugMaterial? focusAreaMaterialSelected;
    [AppliedWithChunk<Chunk0701701D>]
    public CPlugMaterial? FocusAreaMaterialSelected { get => focusAreaMaterialSelectedFile?.GetNode(ref focusAreaMaterialSelected) ?? focusAreaMaterialSelected; set => focusAreaMaterialSelected = value; }
    private Components.GbxRefTableFile? focusAreaMaterialSelectedFile;
    public Components.GbxRefTableFile? FocusAreaMaterialSelectedFile { get => focusAreaMaterialSelectedFile; set => focusAreaMaterialSelectedFile = value; }
    public CPlugMaterial? GetFocusAreaMaterialSelected(GbxReadSettings settings = default, bool exceptions = false) => focusAreaMaterialSelectedFile?.GetNode(ref focusAreaMaterialSelected, settings, exceptions) ?? focusAreaMaterialSelected;

    private CPlugMaterial? focusAreaMaterialFocused;
    [AppliedWithChunk<Chunk0701701D>]
    public CPlugMaterial? FocusAreaMaterialFocused { get => focusAreaMaterialFocusedFile?.GetNode(ref focusAreaMaterialFocused) ?? focusAreaMaterialFocused; set => focusAreaMaterialFocused = value; }
    private Components.GbxRefTableFile? focusAreaMaterialFocusedFile;
    public Components.GbxRefTableFile? FocusAreaMaterialFocusedFile { get => focusAreaMaterialFocusedFile; set => focusAreaMaterialFocusedFile = value; }
    public CPlugMaterial? GetFocusAreaMaterialFocused(GbxReadSettings settings = default, bool exceptions = false) => focusAreaMaterialFocusedFile?.GetNode(ref focusAreaMaterialFocused, settings, exceptions) ?? focusAreaMaterialFocused;

    private float focusAreaMinWidth;
    [AppliedWithChunk<Chunk0701701D>]
    public float FocusAreaMinWidth { get => focusAreaMinWidth; set => focusAreaMinWidth = value; }

    private float focusAreaMinHeight;
    [AppliedWithChunk<Chunk0701701D>]
    public float FocusAreaMinHeight { get => focusAreaMinHeight; set => focusAreaMinHeight = value; }

    private float focusAreaXMargin;
    [AppliedWithChunk<Chunk0701701D>]
    public float FocusAreaXMargin { get => focusAreaXMargin; set => focusAreaXMargin = value; }

    private float focusAreaYMargin;
    [AppliedWithChunk<Chunk0701701D>]
    public float FocusAreaYMargin { get => focusAreaYMargin; set => focusAreaYMargin = value; }

    private CPlugSolid? focusAreaSolid;
    [AppliedWithChunk<Chunk0701701D>]
    public CPlugSolid? FocusAreaSolid { get => focusAreaSolid; set => focusAreaSolid = value; }

    private float focusAreaZOffset;
    [AppliedWithChunk<Chunk0701701D>]
    public float FocusAreaZOffset { get => focusAreaZOffset; set => focusAreaZOffset = value; }

    private string? buttonDefaultIconId;
    [AppliedWithChunk<Chunk0701701E>]
    public string? ButtonDefaultIconId { get => buttonDefaultIconId; set => buttonDefaultIconId = value; }

    /// <summary>
    /// Creates a new instance of <see cref="CControlStyle"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CControlStyle() { }


    /// <summary>
    /// CControlStyle 0x000 chunk
    /// </summary>
    [Chunk(0x07017000)]
    public partial class Chunk07017000 : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x07017000;

        public CMwNod? U01;
        public CMwNod? U02;

        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.NodeRef<CMwCmdBlockMain>(ref n.focusGainedScript);
            rw.NodeRef<CMwCmdBlockMain>(ref n.focusLostScript);
            rw.NodeRef<CPlugSound>(ref n.focusSound, ref n.focusSoundFile);
            rw.NodeRef<CMwNod>(ref U01);
            rw.NodeRef<CMwNod>(ref U02);
        }
    }

    /// <summary>
    /// CControlStyle 0x004 chunk
    /// </summary>
    [Chunk(0x07017004)]
    public partial class Chunk07017004 : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x07017004;


        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugSound>(ref n.entrySound);
        }
    }

    /// <summary>
    /// CControlStyle 0x009 chunk
    /// </summary>
    [Chunk(0x07017009)]
    public partial class Chunk07017009 : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x07017009;


        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugShader>(ref n.defaultShader);
        }
    }

    /// <summary>
    /// CControlStyle 0x00B chunk
    /// </summary>
    [Chunk(0x0701700B)]
    public partial class Chunk0701700B : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x0701700B;

        public int U01;
        public int U02;

        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.fitTextSize);
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    /// <summary>
    /// CControlStyle 0x00C chunk
    /// </summary>
    [Chunk(0x0701700C)]
    public partial class Chunk0701700C : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x0701700C;

        public string? U01;

        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    /// <summary>
    /// CControlStyle 0x00D chunk
    /// </summary>
    [Chunk(0x0701700D)]
    public partial class Chunk0701700D : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x0701700D;


        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugSound>(ref n.enumSound);
            rw.NodeRef<CPlugShader>(ref n.enumListShader);
            rw.Int32(ref n.enumMaxElemCount);
            rw.Single(ref n.enumIconWidth);
            rw.Single(ref n.enumIconHeight);
        }
    }

    /// <summary>
    /// CControlStyle 0x00E chunk
    /// </summary>
    [Chunk(0x0701700E)]
    public partial class Chunk0701700E : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x0701700E;


        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugSound>(ref n.actionSound, ref n.actionSoundFile);
            rw.Single(ref n.buttonIconWidth);
            rw.Single(ref n.buttonIconHeight);
            rw.NodeRef<CFuncEnum>(ref n.buttonDefaultIcons, ref n.buttonDefaultIconsFile);
        }
    }

    /// <summary>
    /// CControlStyle 0x010 chunk
    /// </summary>
    [Chunk(0x07017010)]
    public partial class Chunk07017010 : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x07017010;


        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugSound>(ref n.enumSound);
            rw.NodeRef<CPlugShader>(ref n.enumListShader);
            rw.Int32(ref n.enumMaxElemCount);
            rw.Single(ref n.enumIconWidth);
            rw.Single(ref n.enumIconHeight);
            rw.Int32(ref n.enumForceDisplayType);
            rw.NodeRef<CFuncEnum>(ref n.enumForceIcons, ref n.enumForceIconsFile);
        }
    }

    /// <summary>
    /// CControlStyle 0x014 chunk
    /// </summary>
    [Chunk(0x07017014)]
    public partial class Chunk07017014 : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x07017014;


        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.quadIsLines);
            rw.Boolean(ref n.quadIsFill);
            rw.Single(ref n.quadZ);
            rw.Single(ref n.quadZLines);
            rw.Vec4(ref n.quadGradientColor0);
            rw.Vec4(ref n.quadGradientColor1);
            rw.Vec4(ref n.quadLinesColor);
        }
    }

    /// <summary>
    /// CControlStyle 0x015 chunk
    /// </summary>
    [Chunk(0x07017015)]
    public partial class Chunk07017015 : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x07017015;


        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.quadIsLines);
            rw.Boolean(ref n.quadIsFill);
            rw.Single(ref n.quadZ);
            rw.Single(ref n.quadZLines);
            rw.Vec4(ref n.quadGradientColor0);
            rw.Vec4(ref n.quadGradientColor1);
            rw.Vec2(ref n.quad_UvTopLeft);
            rw.Vec2(ref n.quad_UvBottomRight);
        }
    }

    /// <summary>
    /// CControlStyle 0x016 chunk
    /// </summary>
    [Chunk(0x07017016)]
    public partial class Chunk07017016 : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x07017016;


        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.NodeRef<CControlEffectMaster>(ref n.effectMaster, ref n.effectMasterFile);
        }
    }

    /// <summary>
    /// CControlStyle 0x017 chunk
    /// </summary>
    [Chunk(0x07017017)]
    public partial class Chunk07017017 : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x07017017;


        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.quadIsLines);
            rw.Boolean(ref n.quadIsFill);
            rw.Single(ref n.quadZ);
            rw.Single(ref n.quadZLines);
            rw.Vec4(ref n.quadGradientColor0);
            rw.Vec4(ref n.quadGradientColor1);
            rw.Vec2(ref n.quad_UvTopLeft);
            rw.Vec2(ref n.quad_UvBottomRight);
            rw.Single(ref n.skew);
        }
    }

    /// <summary>
    /// CControlStyle 0x018 chunk
    /// </summary>
    [Chunk(0x07017018)]
    public partial class Chunk07017018 : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x07017018;


        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.Vec4(ref n.lineGradientColor0);
            rw.Vec4(ref n.lineGradientColor1);
        }
    }

    /// <summary>
    /// CControlStyle 0x01B chunk
    /// </summary>
    [Chunk(0x0701701B)]
    public partial class Chunk0701701B : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x0701701B;

        public float U01;
        public float U02;
        public STextSettings_ColorAndChars? U03;
        public STextSettings_ColorAndChars? U04;
        public STextSettings_ColorAndChars? U05;

        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugFont>(ref n.font, ref n.fontFile);
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.ReadableWritable<STextSettings_ColorAndChars>(ref U03);
            rw.ReadableWritable<STextSettings_ColorAndChars>(ref U04);
            rw.ReadableWritable<STextSettings_ColorAndChars>(ref U05);
        }
    }

    /// <summary>
    /// CControlStyle 0x01C chunk
    /// </summary>
    [Chunk(0x0701701C)]
    public partial class Chunk0701701C : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x0701701C;


        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.Single(ref n.sliderBarWidth);
            rw.Single(ref n.sliderBarHeight);
            rw.Single(ref n.sliderCursorWidth);
            rw.Single(ref n.sliderCursorHeight);
            rw.NodeRef<CPlugSound>(ref n.sliderSound);
            rw.NodeRef<CFuncEnum>(ref n.sliderBarIcons, ref n.sliderBarIconsFile);
            rw.NodeRef<CFuncEnum>(ref n.sliderCursorIcons, ref n.sliderCursorIconsFile);
        }
    }

    /// <summary>
    /// CControlStyle 0x01D chunk
    /// </summary>
    [Chunk(0x0701701D)]
    public partial class Chunk0701701D : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x0701701D;


        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.focusAreaEnable);
            rw.NodeRef<CPlugMaterial>(ref n.focusAreaMaterial, ref n.focusAreaMaterialFile);
            rw.NodeRef<CPlugMaterial>(ref n.focusAreaMaterialReadOnly, ref n.focusAreaMaterialReadOnlyFile);
            rw.NodeRef<CPlugMaterial>(ref n.focusAreaMaterialSelected, ref n.focusAreaMaterialSelectedFile);
            rw.NodeRef<CPlugMaterial>(ref n.focusAreaMaterialFocused, ref n.focusAreaMaterialFocusedFile);
            rw.Single(ref n.focusAreaMinWidth);
            rw.Single(ref n.focusAreaMinHeight);
            rw.Single(ref n.focusAreaXMargin);
            rw.Single(ref n.focusAreaYMargin);
            rw.NodeRef<CPlugSolid>(ref n.focusAreaSolid);
            rw.Single(ref n.focusAreaZOffset);
        }
    }

    /// <summary>
    /// CControlStyle 0x01E chunk
    /// </summary>
    [Chunk(0x0701701E)]
    public partial class Chunk0701701E : Chunk<CControlStyle>
    {
        /// <inheritdoc />
        public override uint Id => 0x0701701E;


        public override void ReadWrite(CControlStyle n, GbxReaderWriter rw)
        {
            rw.Id(ref n.buttonDefaultIconId);
        }
    }


    public sealed partial class STextSettings_ColorAndChars : IReadableWritable
    {

        private int u01;
        public int U01 { get => u01; set => u01 = value; }

        private int u02;
        public int U02 { get => u02; set => u02 = value; }

        private float u03;
        public float U03 { get => u03; set => u03 = value; }

        private float u04;
        public float U04 { get => u04; set => u04 = value; }

        private float u05;
        public float U05 { get => u05; set => u05 = value; }

        private float u06;
        public float U06 { get => u06; set => u06 = value; }

        private string? u07;
        public string? U07 { get => u07; set => u07 = value; }

        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Single(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.String(ref u07);
        }
    }



    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x07017000 => new Chunk07017000(),
        0x07017004 => new Chunk07017004(),
        0x07017009 => new Chunk07017009(),
        0x0701700B => new Chunk0701700B(),
        0x0701700C => new Chunk0701700C(),
        0x0701700D => new Chunk0701700D(),
        0x0701700E => new Chunk0701700E(),
        0x07017010 => new Chunk07017010(),
        0x07017014 => new Chunk07017014(),
        0x07017015 => new Chunk07017015(),
        0x07017016 => new Chunk07017016(),
        0x07017017 => new Chunk07017017(),
        0x07017018 => new Chunk07017018(),
        0x0701701B => new Chunk0701701B(),
        0x0701701C => new Chunk0701701C(),
        0x0701701D => new Chunk0701701D(),
        0x0701701E => new Chunk0701701E(),
        _ => base.NewChunk(chunkId),
    };
}
