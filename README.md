![GBX.NET](logo_outline.png)

Welcome to GBX.NET 2!

Visual Studio is probably the best IDE to use to see what's going on. Check out the `Dependencies -> Analyzers` section to see the source generators in action.

# Usage

Using the NuGet packages is recommended (coming soon, for now, just clone the Git repository).

### Create a new GBX.NET project (lightweight) SOON

1. Install .NET SDK 8.
    - Windows: `winget install Microsoft.DotNet.SDK.8` (make sure you have WinGet installed)
    - [Linux](https://learn.microsoft.com/en-us/dotnet/core/install/linux) (just SDK)
2. Create directory for your project (anywhere), **go inside it**.
3. Create new console project: `dotnet new console`
4. Add the pre-release GBX.NET 2 NuGet package: `dotnet add package GBX.NET --prerelease`
5. *(optional)* Add the pre-release GBX.NET.LZO 2 NuGet package: `dotnet add package GBX.NET.LZO --prerelease`
6. Open `Program.cs` with your favorite text editor: `code . -g Program.cs` (for example)
7. Write code - see [Examples (simple)](#examples-simple).
8. Use `dotnet run` to run the app.

Steps 2-8:
```
mkdir MyGbxProject
cd MyGbxProject
dotnet new console
dotnet add package GBX.NET --prerelease
dotnet add package GBX.NET.LZO --prerelease
code . -g Program.cs
dotnet run
```

### Create a new GBX.NET project (Visual Studio Code) SOON

1. Install C# Dev Kit extension.
2. Click on `Create .NET Project` button, or press <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>P</kbd>, type `.NET: New Project`.
3. Select `Console App` and create your project.
4. Open a new terminal and type `dotnet add package GBX.NET --prerelease` to add GBX.NET 2.
5. *(optional)* Add the GBX.NET.LZO 2 NuGet package: `dotnet add package GBX.NET.LZO --prerelease`
5. Write code - see [Examples (simple)](#examples-simple).
6. Run and debug as usual, select C# if prompted.

### Create a new GBX.NET project (Visual Studio) SOON

1. Create a new Console project
2. Under your project in Solution Explorer, right-click on Dependencies and select `Manage NuGet packages...`
3. Search `GBX.NET` and click install
4. Write code - see [Examples (simple)](#examples-simple).

## IMPORTANT INFO about the LZO and Gbx compression

Reading or writing compressed Gbx files **require** to include the GBX.NET.LZO 2 library (or any other implementation that uses the `ILzo` interface). This is how you can include it:

Command line:

```
dotnet add package GBX.NET.LZO --prerelease
```

C# code:

```cs
using GBX.NET;
using GBX.NET.LZO;

Gbx.LZO = new MiniLZO();
```

The compression logic is split up from the read/write logic to **allow GBX.NET 2 library to be distributed under the MIT license**, as Oberhumer distributes the open source version of LZO under the GNU GPL v3. Therefore, using GBX.NET.LZO 2 requires you to license your project under the GNU GPL v3, see [License](#license).

**Gbx header is not compressed** and can contain useful information (icon data, replay time, ...), and also many of the **internal Gbx files from Pak files are not compressed**, so you can avoid LZO for these purposes.

## Examples (simple)

### Load a map and display block count per block name

Required packages: `GBX.NET`, `GBX.NET.LZO`

> This project example expects you to have `<ImplicitUsings>enable</ImplicitUsings>`. If this does not work for you, add `using System.Linq;`.

```cs
using GBX.NET;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;

Gbx.LZO = new MiniLZO();

var map = Gbx.ParseNode<CGameCtnChallenge>("Path/To/My.Map.Gbx");

foreach (var block in map.GetBlocks().GroupBy(x => x.Name))
{
    Console.WriteLine($"{block.Key}: {block.Count()}");
}
```

This will print out all blocks on the map and their count. This code can potentially crash for at least 3 reasons:

1. The Gbx file is **not a map**. See [Explicit vs. Implicit parse](#explicit-vs-implicit-parse) in the [Optimization](#optimization) part.
2. There's **a Gbx exception**. See *Exceptions in GBX.NET 2* (TBD).
3. There's a file system problem.

### Read a large amount of replay metadata quickly

Required packages: `GBX.NET`

In case you only need the most basic information about the most common Gbx files (maps, replays, items, ...), do not read the full Gbx file, but only the header part. It is a great performance benefit.

FUN FACT: Reading only the header also does not infect you with GNU GPL v3 and you can use licenses compatible with MIT. Header is not compressed with LZO.

> This project example expects you to have `<ImplicitUsings>enable</ImplicitUsings>`. If this does not work for you, add `using System.IO;`.

```cs
using GBX.NET;
using GBX.NET.Engines.Game;

foreach (var filePath in Directory.EnumerateFiles("Path/To/My/Directory", "*.Replay.Gbx", SearchOption.AllDirectories))
{
    DisplayBasicReplayInfo(filePath);
}

void DisplayBasicReplayInfo(string filePath)
{
    try
    {
        var nodeHeader = Gbx.ParseHeaderNode(filePath);

        if (nodeHeader is CGameCtnReplayRecord replay)
        {
            Console.WriteLine($"{replay.MapInfo}: {replay.Time}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Gbx exception occurred {Path.GetFileName(filePath)}: {ex}");
    }
}
```

This code should only crash in case of a file system problem. Other problems will be printed out in the console.

The `Gbx.Parse...` approach is quite different here. Instead of `Gbx.Parse...<T>()`, `Gbx.Parse...()` was used with a pattern match afterwards:

```cs
if (nodeHeader is CGameCtnReplayRecord replay)
```

See [Explicit vs. Implicit parse](#explicit-vs-implicit-parse) in the [Optimization](#optimization) part why that is.

# Clarity

This section describes best practices to keep your projects clean when using GBX.NET 2.

## Differences between `Gbx.Parse/Header/Node`

Gbx files contain many different parameters that are not exactly part of the game objects. We commonly use `ParseNode` to simplify the access level, as Gbx parameters are usually unnecessary to know about, but they have to be present to ensure consistent serialization.

You can still save nodes into Gbx files by using the `Save` method - be careful specifying the Gbx parameters correctly, like the class ID mappings (wrap/unwrap).

### Gbx.Parse

TODO

### Gbx.ParseNode

TODO

### Gbx.ParseHeader

TODO

### Gbx.ParseHeaderNode

TODO

# Optimization

GBX.NET 2 introduced rich optimization techniques to improve both performance and compiled size of your applications.

The goal of these optimizations is to prove that GBX.NET is not "too big" for anything small.

## Trimming (tree shaking)

On *publish* (the final build), you can trim out unused code by using this property in `.csproj`:

```xml
<PropertyGroup>
    <PublishTrimmed>true</PublishTrimmed>
</PropertyGroup>
```

The library does not load anything dynamically and does not use reflection, so this is **fully supported**.

GBX.NET is a huge library when everything is included (over 1.5MB), so please use this whenever it's possible. Code was written to be as trimmable as possible, so the different is huge (much bigger than in GBX.NET v1).

> Expect this to work only with `dotnet publish`.

## Explicit vs. Implicit parse

TODO

## NativeAOT

GBX.NET **fully supports** NativeAOT, and it is highly recommended to use its potential on smaller-sized applications:

```xml
<PropertyGroup>
    <PublishAot>true</PublishAot>
</PropertyGroup>
```

It also automatically trims the application (no need for `<PublishTrimmed>true</PublishTrimmed>`).

On basic GBX.NET applications, native compilation has a couple of improvements:
- Reduces trimmed standalone binary size from ~7MB to 2.8MB.
- Startup time is reduced from 50ms to 0.5ms (JIT is removed, so you should be only bottlenecked by disk speed).
- If only
- The app feels generally lighter, but can be slightly slower for long-running process than a runtime app with JIT (very small difference).

> Expect this to work only with `dotnet publish`.