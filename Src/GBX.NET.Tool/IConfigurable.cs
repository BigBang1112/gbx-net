namespace GBX.NET.Tool;

public interface IConfigurable<out TConfig> where TConfig : Config
{
    TConfig Config { get; }
}