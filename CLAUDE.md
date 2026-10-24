# Code style

When writing or modifying C# code, follow the rules in:

- `Source/.editorconfig` – formatting, naming and code-style preferences
- `Source/stylecop.json` and the StyleCop section of `Source/.editorconfig` – enabled StyleCop.Analyzers rules (added to all projects via `Source/Directory.Build.props`)

Read both files before generating code. Key rules:

- Member order: constants, fields, constructors, events, properties, methods (SA1201, SA1203).
- Private fields use `_camelCase`; private static fields use `PascalCase`.
- `using` directives outside the namespace, sorted alphabetically (`System.*` is not forced first).
- Prefer `var`, braces for every block, expression-bodied members only when they fit on one line.
- Files use CRLF line endings, UTF-8 with BOM, and no final newline (`insert_final_newline = false`).

## Project-specific constraints

- `VertaIT.WPF.Theme` / `VertaIT.WPF.Demo` target .NET Framework 4.7.2 (C# 7.3); `VertaIT.WPF.NetTheme` / `VertaIT.WPF.NetDemo` are .NET 10 copies of the same code. Keep the copies identical (only namespaces and pack URIs differ) and do not use language features newer than C# 7.3 (file-scoped namespaces, `new()`, collection expressions, primary constructors, etc.), even if an analyzer suggests them.
- The legacy `.csproj` files list sources explicitly – add new `.cs`/`.xaml` files to them.

## Verification

After changes, build the solution and make sure no new StyleCop warnings appear:

```bash
"/c/Program Files/Microsoft Visual Studio/18/Community/MSBuild/Current/Bin/MSBuild.exe" Source/VertaIT.Themes.sln -restore -t:Rebuild -v:m
```

For the .NET 10 projects, `dotnet format <project> --verify-no-changes --severity info --include <files>` lists remaining `.editorconfig` violations.
