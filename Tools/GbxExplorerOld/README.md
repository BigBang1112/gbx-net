# Editor

Editor itself is a `StandaloneCodeEditor` component.

Custom behaviors:
- makes section for `using` foldable and automatically folds it when Note opened
- provides custom shortcuts for running code and saving note

## Intellisense 
Custom providers available for 
- completion items
- method signature
- hover

Providers then query Omnisharp Roslyn. Its API was modified and copypasted into project. \
Inspired by: \
https://github.com/Apollo3zehn/MonacoBlazorSample \
https://github.com/knervous/intellisage 

## User code

To provide intellisense, diagnostics and eventually compile user code, we need to give Roslyn all referenced dependencies as raw DLLs.
To ensure DLLs are not trimmed, they added as TrimmerRootAssembly.

## Development and Release

### Client

When working on Client alone, 2 custom targets are present:
- GenerateDllList
- CopyDllsToWwwroot
They put dlls into project files as static assets. However, this requires double build.

#### Release
To make autocomplete work on release, a workaround was needed to avoid reflection
https://www.meziantou.net/accessing-private-members-without-reflection-in-csharp.htm

### Server
There are also 2 custom targets
- GenerateDllListProd
- CopyDllsToWwwrootProd

This is important to serve DLLs:
```csharp
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
});
```

#### Release
Custom targets will be executed specifically on Release. Difference from Client targets is that DLLs will be copied straight into publish assets, therefore double build is not required.

#### Development
During development, DLLs are served straight from build directory of Client, so double build is not required.
