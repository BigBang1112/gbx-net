# Trimming Sample

This is an example of how trimming + explicit parse can significantly improve the self-contained app size.

There are 4 apps that do the exact same thing - display the map name of the imported map. **Only the header part is read**, just to see how far it can go, but expect slightly smaller differences on full read scenarios. The categories are:

- **Untrimmed** - uses `Gbx.ParseHeaderNode` (implicit parse)
  - `PublishSingleFile` + `SelfContained`
- **UntrimmedGeneric** - uses `Gbx.ParseHeaderNode<CGameCtnChallenge>` (explicit parse)
  - `PublishSingleFile` + `SelfContained`
- **Trimmed** - uses `Gbx.ParseHeaderNode` (implicit parse)
  - `PublishSingleFile` + `SelfContained` + `PublishTrimmed`
- **TrimmedGeneric** - uses `Gbx.ParseHeaderNode<CGameCtnChallenge>` (explicit parse)
  - `PublishSingleFile` + `SelfContained` + `PublishTrimmed`

## Test yourself

Run `dotnet publish` command on all 4 projects.

## Results

Here are the results of the final `GBX.NET.dll` file size (GBX.NET 2.0.1):

- **Untrimmed** - 1 949 kB
- **UntrimmedGeneric** - 1 949 kB
- **Trimmed** - 751 kB
- **TrimmedGeneric** - **83 kB**

### Use `<PublishTrimmed>` and explicit parse in your code whenever it's possible!