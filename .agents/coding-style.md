# C# coding style

When you edit an existing file, follow the surrounding code and [.editorconfig](../.editorconfig). Use latest language features supported by the project's C# version and target frameworks.

For Markdown documentation, see [Writing style](writing-style.md).

## Formatting

Put `using` directives above the namespace.

- Separate members and logical groups of statements with a blank line. Related fields and simple properties can stay together.
- Keep short signatures on one line. Break long parameter lists into one parameter per line.
- Keep imports together. The reference projects do not consistently put `System` first.
- Put each class in its own file, named after the class. An interface may share its matching class's file. For a larger partial type, use a name such as `BinaryReader.Header.cs` to identify the part.

There is no fixed line limit in the reviewed code. Break a line when it makes the declaration or expression easier to read.

## Naming

- Use `this.field = field` when a parameter shares the field name. You can omit `this.` when the reference is clear.

Keep related overloads together and delegate to a shared implementation when useful. Put attributes on their own lines above the declaration.

## Async and resources

Place `CancellationToken cancellationToken` last in the parameter list. Use `= default` when cancellation is optional, and pass the token to operations that accept it.

- Use structured logging templates with named placeholders, for example `logger.LogDebug("Reading file {FileName}...", fileName)`.

## Comments and documentation

Use XML documentation for public APIs that need an explanation. The reference libraries use `<summary>`, `<param>`, `<returns>`, and `<remarks>`, with `<see cref="..."/>` and `<paramref name="..."/>` for references.

Inline comments should explain format details, compatibility constraints, or intent that is not clear from the code.

Keep compatibility directives such as `#if` when APIs differ across supported frameworks. Generated code can have its own conventions, so use handwritten code as the style reference for new implementation work.

## References

These local files were used to check the conventions:

- `Src/GBX.NET.Tool.CLI/SettingsManager.cs` - fields, constructors, async methods, cancellation, disposal, and logging
- `Src/GBX.NET/Serialization/Encapsulation.cs` - null guards, lazy collections, and resource state
- `Src/GBX.NET/Vec3.cs` - value records, expression bodies, operators, XML docs, and framework compatibility
- `Tests/GBX.NET.Tests/Unit/Serialization/EncapsulationTests.cs` - test naming and assertions
