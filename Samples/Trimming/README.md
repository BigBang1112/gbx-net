# Trimming Sample

This is an example of how trimming can significantly improve the self-contained app size.

There are 2 apps that do the exact same thing - display the map name of the imported map. **Only the header part is read**, just to see how far it can go, but expect slightly smaller differences on full read scenarios. The categories are:

- **Untrimmed** - uses `Gbx.ParseHeaderNode` (implicit parse)
  - `PublishSingleFile` + `SelfContained`
- **Trimmed** - uses `Gbx.ParseHeaderNode` (implicit parse)
  - `PublishSingleFile` + `SelfContained` + `PublishTrimmed`

## Test yourself

Run `dotnet publish` command on both of these projects.

## Results

Here are the results of the final `GBX.NET.dll` file size (GBX.NET 2.3.0):

- **Untrimmed** - 2 624 kB
- **Trimmed** - 1 169 kB

## Future

It is expected to use `FeatureSwitchDefinition` in the future to be able to disable various types of classes. For example when you only want to modify maps, you only want to include map-related things, like `CGameCtnBlock` or MediaTracker blocks like `CGameCtnMediaTrack`.