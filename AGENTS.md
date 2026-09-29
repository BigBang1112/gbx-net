# Guidelines

GBX.NET is a C# library for reading and writing Gbx files.

Do not edit generated files directly.

## Structure

- `Src/` contains the library and related packages
- Gbx layouts live in `Src/GBX.NET/Engines/<Engine>/<Class>.chunkl`
- `Generators/GBX.NET.Generators/` reads the layouts
- A build generates C# into `Src/GBX.NET/Generated/<TargetFramework>/`
- `Templates/` contains code templates
- `Tests/` contains test projects and Gbx fixtures
- `Samples/` and `Tools/` contain examples and applications
- `Resources/` holds useful information often used by generators
- `Media/` holds artwork

## Style

- Follow [.agents/coding-style.md](.agents/coding-style.md) for coding conventions
- Follow [.agents/writing-style.md](.agents/writing-style.md) for Markdown documentation
