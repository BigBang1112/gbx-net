# Verifying generator changes

Build the library after changing a layout, a handwritten partial property, or the generator. The build writes generated C# into `Src/GBX.NET/Generated/<TargetFramework>/`. Build each supported target framework before reviewing tracked generated output.

```powershell
dotnet build Src/GBX.NET/GBX.NET.csproj -f net10.0 --no-restore
dotnet build Src/GBX.NET/GBX.NET.csproj -f net9.0 --no-restore
dotnet build Src/GBX.NET/GBX.NET.csproj -f net8.0 --no-restore
dotnet build Src/GBX.NET/GBX.NET.csproj -f netstandard2.0 --no-restore
```

The generator tests compile small layout and C# fixtures. Add a focused case in `GenerationTests.cs` when changing member detection or generated syntax. Check both the emitted declaration and compilation diagnostics. For a custom partial property, also check that serialization uses the property when its setter must run.

Run the generator tests after building the test project:

```powershell
dotnet build Tests/GBX.NET.Generators.Tests/GBX.NET.Generators.Tests.csproj -f net10.0 --no-restore
dotnet Tests/GBX.NET.Generators.Tests/bin/Debug/net10.0/GBX.NET.Generators.Tests.dll --no-ansi --no-progress
```

The generator test project uses Microsoft Testing Platform. Its runner does not accept the VSTest `--filter` option. Running the test assembly directly is a simple way to run the full suite.

Finally, use `git diff --check` to catch whitespace errors. Inspect the generated files for every target framework to confirm that the output matches the layout and generator changes.
