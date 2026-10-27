# GBX.NET tests

Unit and integration tests for the main library, written with TUnit on **.NET 8, 9, and 10**. You need the .NET 10 SDK and all three runtimes to run every target.

## Run

From the repository root:

```sh
dotnet test --project Tests/GBX.NET.Tests/GBX.NET.Tests.csproj
```

> [!NOTE]
> `global.json` selects Microsoft.Testing.Platform, so VSTest options like `--filter` don't work.

## Structure

| Folder | Contents |
| --- | --- |
| `Unit/` | Isolated behavior with constructed nodes and in-memory payloads, no fixture files. Mirrors the library's `Engines/`, `Components/`, and `Serialization/` |
| `Integration/` | Real Gbx files through the public parse and save APIs |
| `Files/Gbx/` | Fixtures, located with `TestFiles.Gbx(...)` |
| `Mocks/` | `FragmentedReadStream`, a non-seekable stream with short reads |

## Fixtures

**Every `.Gbx` under `Files/Gbx/` gets roundtrip cases automatically**, with sync and async parsing and compressed and uncompressed output. Each case checks that:

- parsing, saving, and parsing again keeps the object graph (deep comparison with `GbxAssert`)
- saving again produces the same bytes as the first save

The first save can normalize the game's encoding, so the original file bytes don't have to match.

- Name fixtures `<Class> <Game> <NNN>.<Extension>.Gbx`, for example `CGameCtnChallenge TMF 004.Challenge.Gbx`.
- Replays are skipped until `CGameCtnReplayRecord.IsWriteSupported` is true.
- Surfaces also need expected geometry in `GmSurfTests.Fixtures()`.

## Writing tests

- Arrange, act on one behavior, and assert the observable result ([best practices](https://milanjovanovic.tech/blog/unit-testing-best-practices-dotnet)).
- Name unit tests `Method_Scenario_Result`, integration tests in descriptive PascalCase.
- Use `[Arguments]` or `[MethodDataSource]` for boundary cases. Keep success and failure cases in separate tests.
- Await TUnit assertions. Compare bytes with `CollectionOrdering.Matching`.
- For serialization, also check the expected bytes or lengths. A roundtrip alone can miss matching reader and writer bugs.
- Tests run concurrently. Keep streams and nodes local, and leave the global `Gbx.LZO`, `Gbx.ZLib`, and `Gbx.StrictBooleans` setup alone.
- Add a regression test with each library fix.
