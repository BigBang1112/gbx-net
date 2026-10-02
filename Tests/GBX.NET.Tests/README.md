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

Collect coverage:

```sh
dotnet test --project Tests/GBX.NET.Tests/GBX.NET.Tests.csproj -- --coverage --coverage-output-format cobertura --results-directory ./coverage/GBX.NET.Tests
```

## Coverage areas

- `Unit/` covers value types, headers, chunk collections, bit reading, bounded streams, string encoding, optimized arrays, and encapsulation lifetime.
- `Integration/` covers parsing and saving real Gbx files, surface archives, asynchronous partial reads, stream ownership, and raw body preservation. These tests use the `Integration` category.
- `Files/Gbx/` holds fixtures copied to the output directory. Use `TestFiles.Gbx(...)` to locate them independently of the working directory.
- `Mocks/FragmentedReadStream.cs` simulates a non-seekable stream that returns fewer bytes than requested.

## Add tests

- Use `[Test]` with `[Arguments]` or `[MethodDataSource]` for boundary cases. Name tests after the behavior they verify.
- Await TUnit assertions. Compare serialized bytes with `CollectionOrdering.Matching` so changes in ordering fail the test.
- Check expected bytes or lengths and the following field when testing serialization. A round trip alone can miss matching reader and writer bugs.
- Add shared round-trip fixtures to `GbxEqualTests.Fixtures()` and surface fixtures to `GmSurfTests.Fixtures()`.
- Keep each test's streams and nodes local. TUnit runs tests concurrently. Assembly setup configures `Gbx.LZO` and `Gbx.StrictBooleans` once, so individual tests should leave those settings alone.
- Keep regression cases for library fixes, including malformed input and boundary values when relevant.
