# GBX.NET tests

The main library suite uses TUnit on .NET 8, 9, and 10. Install the .NET 10 SDK and all three runtimes to run every target.

## Run

From the repository root:

```sh
dotnet test --project Tests/GBX.NET.Tests/GBX.NET.Tests.csproj
```

- Add `-f net10.0` to run one framework.
- Add `--no-build` when the test binaries are already built.
- `global.json` selects Microsoft.Testing.Platform for `dotnet test` across the repository.

Run the categories separately:

```sh
dotnet test --project Tests/GBX.NET.Tests/GBX.NET.Tests.csproj -f net10.0 -- --treenode-filter "/*/*/*/*[Category=Unit]"
dotnet test --project Tests/GBX.NET.Tests/GBX.NET.Tests.csproj -f net10.0 -- --treenode-filter "/*/*/*/*[Category=Integration]"
```

Collect coverage:

```sh
dotnet test --project Tests/GBX.NET.Tests/GBX.NET.Tests.csproj -- --coverage --coverage-output-format cobertura --results-directory ./coverage/GBX.NET.Tests
```

## Coverage areas

- `Unit/` covers isolated library behavior using constructed nodes, in-memory streams, and explicit binary payloads. This includes value types, headers, chunk layouts, collections, stream ownership, partial reads, and encapsulation lifetime. These tests use the `Unit` category and do not read fixture files.
- `Unit/Engines/`, `Unit/Components/`, and `Unit/Serialization/` mirror the library's source folders.
- `Integration/` mainly covers roundtrips of real Gbx files through the public parse and save APIs. It also checks native surface archives, replay parsing, and embedded lightmap caches. These tests use the `Integration` category.
- `Files/Gbx/` holds fixtures copied to the output directory. Use `TestFiles.Gbx(...)` to locate them independently of the working directory.
- `Mocks/FragmentedReadStream.cs` simulates a non-seekable stream that returns fewer bytes than requested.

## Add tests

- Follow the [unit testing best practices](https://milanjovanovic.tech/blog/unit-testing-best-practices-dotnet): arrange inputs, act on one behavior, then assert its observable result. Use `Method_Scenario_ExpectedResult` names for new and refactored tests.
- Use `[Test]` with `[Arguments]` or `[MethodDataSource]` for boundary cases. Keep success and failure scenarios in separate tests so assertions do not branch between unrelated outcomes.
- Keep constructed test data small and local. Extract a factory when several tests need the same setup. Mock external boundaries only when needed.
- Await TUnit assertions. Compare serialized bytes with `CollectionOrdering.Matching` so changes in ordering fail the test.
- Check expected bytes or lengths and the following field when testing serialization. A round trip alone can miss matching reader and writer bugs.
- Add fixture files under `Files/Gbx/`. `TestFiles.GbxFixtures()` discovers every `.Gbx` recursively in a stable order, so new files automatically receive roundtrip cases. For surfaces, also add expected geometry to `GmSurfTests.Fixtures()`.
- Keep each test's streams and nodes local. TUnit runs tests concurrently. Assembly setup configures `Gbx.LZO`, `Gbx.ZLib`, and `Gbx.StrictBooleans` once, so individual tests should leave those settings alone.
- Keep regression cases for library fixes, including malformed input and boundary values when relevant.

## Roundtrip coverage

`GbxRoundTripTests` discovers all 65 current fixtures and exercises synchronous and asynchronous parsing with compressed and uncompressed output. It verifies two behaviors separately:

- Parse, save, and parse again preserves the object graph. `GbxAssert` uses CompareNETObjects to compare headers, reference tables, and nodes deeply, with collection order preserved and difference paths in assertion failures. It excludes container-only read settings, file location, and compressed body size metadata from the comparison.
- Saving the reparsed result produces the same bytes as the first save. The first save can normalize the game's original encoding, so this does not require original file byte equality. `GmSurfTests` additionally checks exact equality with the original uncompressed native archive.

The 55 writable fixtures cover maps, ghosts, items, macroblocks, media clips, system configurations, and surfaces. All 10 replay fixtures are registered too, but their roundtrip cases report an explicit skip while `CGameCtnReplayRecord.IsWriteSupported` is false. All replays have full parsing checks with chunk failures surfaced. The TM2020 replay also checks that synchronous and asynchronous parsing return the same object graph. Roundtrip cases resume automatically when replay writing is enabled.
