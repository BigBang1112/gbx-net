namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090BF000</remarks>
[Class(0x090BF000)]
public partial class CPlugWeatherModel : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x090BF000;




    private CPlugWeather[]? weathers;
    [AppliedWithChunk<Chunk090BF001>]
    public CPlugWeather[]? Weathers { get => weathers; set => weathers = value; }

    private CPlugBitmap? bitmapSpecularDir;
    [AppliedWithChunk<Chunk090BF001>]
    public CPlugBitmap? BitmapSpecularDir { get => bitmapSpecularDir; set => bitmapSpecularDir = value; }

    private CFuncDayTime? funcDayTime;
    [AppliedWithChunk<Chunk090BF002>]
    public CFuncDayTime? FuncDayTime { get => funcDayTimeFile?.GetNode(ref funcDayTime) ?? funcDayTime; set => funcDayTime = value; }
    private Components.GbxRefTableFile? funcDayTimeFile;
    public Components.GbxRefTableFile? FuncDayTimeFile { get => funcDayTimeFile; set => funcDayTimeFile = value; }
    public CFuncDayTime? GetFuncDayTime(GbxReadSettings settings = default, bool exceptions = false) => funcDayTimeFile?.GetNode(ref funcDayTime, settings, exceptions) ?? funcDayTime;

    private CPlugBitmap? bitmapWaterFog;
    [AppliedWithChunk<Chunk090BF003>]
    public CPlugBitmap? BitmapWaterFog { get => bitmapWaterFogFile?.GetNode(ref bitmapWaterFog) ?? bitmapWaterFog; set => bitmapWaterFog = value; }
    private Components.GbxRefTableFile? bitmapWaterFogFile;
    public Components.GbxRefTableFile? BitmapWaterFogFile { get => bitmapWaterFogFile; set => bitmapWaterFogFile = value; }
    public CPlugBitmap? GetBitmapWaterFog(GbxReadSettings settings = default, bool exceptions = false) => bitmapWaterFogFile?.GetNode(ref bitmapWaterFog, settings, exceptions) ?? bitmapWaterFog;

    private CPlugMoodSetting? moodSetting;
    [AppliedWithChunk<Chunk090BF004>]
    public CPlugMoodSetting? MoodSetting { get => moodSettingFile?.GetNode(ref moodSetting) ?? moodSetting; set => moodSetting = value; }
    private Components.GbxRefTableFile? moodSettingFile;
    public Components.GbxRefTableFile? MoodSettingFile { get => moodSettingFile; set => moodSettingFile = value; }
    public CPlugMoodSetting? GetMoodSetting(GbxReadSettings settings = default, bool exceptions = false) => moodSettingFile?.GetNode(ref moodSetting, settings, exceptions) ?? moodSetting;

    /// <summary>
    /// Creates a new instance of <see cref="CPlugWeatherModel"/> with no chunks inside (no data will be serialized).
    /// </summary>
    public CPlugWeatherModel() { }


    /// <summary>
    /// CPlugWeatherModel 0x001 chunk
    /// </summary>
    [Chunk(0x090BF001)]
    public partial class Chunk090BF001 : Chunk<CPlugWeatherModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090BF001;


        public override void ReadWrite(CPlugWeatherModel n, GbxReaderWriter rw)
        {
            rw.ArrayNodeRef_deprec<CPlugWeather>(ref n.weathers!);
            rw.NodeRef<CPlugBitmap>(ref n.bitmapSpecularDir);
        }
    }

    /// <summary>
    /// CPlugWeatherModel 0x002 chunk
    /// </summary>
    [Chunk(0x090BF002)]
    public partial class Chunk090BF002 : Chunk<CPlugWeatherModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090BF002;


        public override void ReadWrite(CPlugWeatherModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CFuncDayTime>(ref n.funcDayTime, ref n.funcDayTimeFile);
        }
    }

    /// <summary>
    /// CPlugWeatherModel 0x003 chunk
    /// </summary>
    [Chunk(0x090BF003)]
    public partial class Chunk090BF003 : Chunk<CPlugWeatherModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090BF003;


        public override void ReadWrite(CPlugWeatherModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugBitmap>(ref n.bitmapWaterFog, ref n.bitmapWaterFogFile);
        }
    }

    /// <summary>
    /// CPlugWeatherModel 0x004 chunk
    /// </summary>
    [Chunk(0x090BF004)]
    public partial class Chunk090BF004 : Chunk<CPlugWeatherModel>
    {
        /// <inheritdoc />
        public override uint Id => 0x090BF004;


        public override void ReadWrite(CPlugWeatherModel n, GbxReaderWriter rw)
        {
            rw.NodeRef<CPlugMoodSetting>(ref n.moodSetting, ref n.moodSettingFile);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x090BF001 => new Chunk090BF001(),
        0x090BF002 => new Chunk090BF002(),
        0x090BF003 => new Chunk090BF003(),
        0x090BF004 => new Chunk090BF004(),
        _ => base.NewChunk(chunkId),
    };
}
