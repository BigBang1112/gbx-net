# Writing style

The main pattern is practical explanation with a conversational tone. Say what the code does, show how you can use it, and explain any limitation that affects the result. Short pages can stay short!

For C# conventions, see [Coding style](coding-style.md).

## Voice

Use plain English and exact technical names. A reader should be able to connect the explanation to a type, setting, command, or visible result.

- Start with what the project or page is for. One or two sentences are usually enough.
- Keep the project or feature as the subject when describing behavior. Use "you" when explaining a choice or a step.
- Explain why a detail matters. For example, describe what a setting changes instead of only saying that the setting exists.
- Use "I" for actual personal context or uncertainty from the author. Do not invent an opinion or a promise on the author's behalf.
- A short aside, parentheses, or an occasional exclamation can fit. Keep most of the page focused on the subject.
- Avoid promotional wording and generic introductions. A concrete use case is more useful than calling a feature powerful or seamless.
- Use periods, commas, parentheses, or a spaced hyphen in prose. Avoid semicolons and em dashes. Keep punctuation inside code unchanged.

The original pages sometimes use stronger emphasis or informal wording. Use that when it helps explain the point, without repeating old typos or turning every paragraph into an aside.

## Page structure

Open with `# Page title` and a compact description. Add sections for the information the reader needs, such as `Usage`, `How it works`, `Framework support`, or `Build`.

A package README may only need a description, supported frameworks, and a license. A larger guide can have examples, compatibility notes, and navigation. Scale the page to its contents.

- Use `##` for main sections and `###` for individual examples or topics.
- Use sentence case for headings, keeping project names and API identifiers as written.
- Keep paragraphs short, with one explanation or decision per paragraph.
- Use bullets for features, options, or constraints. Use numbered steps when order matters.
- Use tables for support matrices, settings, metadata fields, or measured results.
- Add navigation links when the page is long enough to need them.
- Leave a blank line around headings, lists, tables, and code blocks.

Badges and logos belong on project READMEs when they provide useful links or status. Internal guides do not need them.

## Technical examples

Introduce an example with the task it performs. Put the code immediately after that explanation, then describe the result or any important failure case.

Use inline code for paths, commands, packages, types, methods, and settings. Use fenced blocks with a language tag. The reference READMEs commonly use `cs` for C# examples.

- Include setup that is needed to understand or run the example.
- Show named arguments when they make an option easier to understand.
- Show expected output in comments or a separate output block when it helps.
- State version or framework requirements close to the example.
- Label illustrative examples if they are not ready to run.

For example, this illustrates the naming convention from [Coding style](coding-style.md):

```cs
private readonly Stream stream;

public BinaryReader(Stream stream)
{
    this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
}
```

The field and parameter use the same name. `this.stream` identifies the field during assignment.

## Limitations and plans

State partial support, bugs, and workarounds directly. Explain what happens and what the reader can do about it. Put the note near the affected feature.

Use bold for a requirement or consequence that is easy to miss. A blockquote or notice block such as `> [!NOTE]` can hold a short compatibility note before an example. Use notice blocks sparingly for information that needs to stand apart. Ordinary facts do not all need emphasis.

Keep current behavior separate from plans. Only describe a future feature when there is an actual plan, and make its status clear. Do not invent support, versions, performance results, or release dates to make a page look complete.

## Links and credit

Link related projects, dependencies, detailed guides, and contributors where the reference is useful. Use relative links for pages in this repository.

Keep exact domain terms such as Gbx, Trackmania, and .NET when describing those projects. Explain abbreviations if the intended reader may not know them. New BinaryBanger documentation should use the terminology of the feature it describes.

## Changelogs

Use compact bullets that name the change and the affected feature. Common opening verbs are `Added`, `Fixed`, `Updated`, `Renamed`, and `Removed`.

Add a short explanation when the change affects compatibility or requires an action. Group entries by package when a release contains changes to several packages. Use version headings, dates, and release links when those details are known.

An illustrative entry for a documentation change:

- Added [Coding style](coding-style.md) and [Writing style](writing-style.md)
