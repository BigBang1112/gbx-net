namespace GbxExplorerOld.Client.Sections.SectionEditor;

public static class TemplateProvider
{
    public const int Version = 1;

    public const string Template = @"using System;
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.Engines.Scene;
using GBX.NET.Inputs;
using TmEssentials;
using System.Linq;
using GbxExplorerOld.Client.Sections.SectionEditor;

public static class Program
{
    //entrypoint
    public static void Run(Gbx gbx, EditorServices services)
    {
        
    }
}
";
}