# Guidelines

GBX.NET is a C# library for reading and writing Gbx files.

Do not edit generated files directly.

## Structure

- `Src/` contains the library and related packages
- Gbx layouts live in `Src/GBX.NET/Engines/<Engine>/<Class>.chunkl`
- `Generators/GBX.NET.Generators/` reads the layouts
- A build generates C# into `Src/GBX.NET/Generated/<TargetFramework>/`
- `Src/GBX.NET.Builders/` contains the node builder runtime and builder partials. Its build generates the builders into `Src/GBX.NET.Builders/Generated/<TargetFramework>/`. Game version interfaces stay in `Src/GBX.NET/`
- `Templates/` contains code templates
- `Tests/` contains test projects and Gbx fixtures
- `Samples/` and `Tools/` contain examples and applications
- `Resources/` holds useful information often used by generators
- `Media/` holds artwork

## Style

- Follow [.agents/coding-style.md](.agents/coding-style.md) for coding conventions
- Follow [.agents/writing-style.md](.agents/writing-style.md) for Markdown documentation

## Development notes

- [.agents/game-installation-paths.md](.agents/game-installation-paths.md) lists common game installation paths and where to find loose Gbx files and pack archives.
- In chunk serialization, use `value >= 1` instead of `value != 0` for signed integer versions, including when the native unsigned version check uses `!= 0`.
- Omit `.v0` from chunk game qualifiers unless another game has a nonzero version.
- Prefer combined `ReadWrite` serialization. Remove separate `Read`/`Write` implementations and `SeparateReadAndWrite` options whenever a combined method can preserve the behavior, including after generator improvements. But don't always switch to `ReadWrite` if it complicates the code unnecessarily.
- [.agents/generator-verification.md](.agents/generator-verification.md) lists build and test commands for generator changes
