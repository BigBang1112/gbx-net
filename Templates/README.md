# GBX.NET.Templates

[![NuGet](https://img.shields.io/nuget/vpre/GBX.NET.Templates?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/GBX.NET.Templates/)
[![Discord](https://img.shields.io/discord/1012862402611642448?style=for-the-badge&logo=discord)](https://discord.gg/tECTQcAWC9)

Templates for GBX.NET projects and related.

## Build

Inside this repository, template projects use local project references for GBX.NET packages. Optional LZO and ZLib references still follow their project settings. This lets CI build templates before the referenced package versions are published.

`Directory.Build.targets` applies this substitution and is excluded from the template package. Projects created with `dotnet new` keep the package references and versions declared in the templates. Use `-p:UseLocalTemplateReferences=false` to build the repository templates against those NuGet packages instead.

## License

GBX.NET.Templates library is MIT Licensed.
