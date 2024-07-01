namespace GBX.NET.Tool;

public interface IConfigurable<TConfig> where TConfig : Config
{
    TConfig Config { get; set; }
}