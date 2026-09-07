# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All `dotnet` commands must be run from the `./src` directory.

```bash
dotnet restore
dotnet build --no-restore -warnaserror
dotnet format --verify-no-changes             # check code style (CI enforces this)
dotnet format                                 # auto-fix code style
csharpier check .                             # second style gate enforced by CI
dotnet test --no-build --verbosity normal     # run tests
```

## Architecture

A Native AOT CLI that reads KartChrono live timing over WebSocket.

- `src/KartChrono.Abstractions` — interfaces only, over `Pure.Primitives.Abstractions`.
- `src/KartChrono` — WebSocket transport, frame decoding, domain records, rendering.
- `src/KartChrono.Cli` — the executable; argument parsing and `Program.Main`.
- `src/Tests/` — xunit suites driven by recorded protocol fixtures, never the live network.

### Protocol

One shared endpoint for every track: `wss://kartchrono.com:9180`. On open, send
`{"trackId":"<32-hex>"}`. Track ids come from `POST https://kartchrono.com/racer/app/gettrackslist.php`.

Text frames are JSON keyed by *numeric* field ids (`1`=position, `3`=kart number, `6`=best lap,
`13`=laps, `100`=race time, `106`=flag status, …). Binary frames start with the ASCII header
`BINLAPS:` followed by 52-byte little-endian lap records.

Three things that are easy to get wrong:

- All times are integer milliseconds **except** fields `100` and `101`, which are seconds.
- Fields `14` (diff) and `15` (gap) are overloaded: negative values encode laps or sectors
  behind, not times.
- Only the first frame is a full snapshot; the rest are sparse deltas that must be folded into
  retained state. Competitor ids may be negative.

## Code Style

Enforced via `src/.editorconfig`, `dotnet format --verify-no-changes` and `csharpier check .`:

- One `public sealed record` per file, implementing exactly one interface.
- Behaviour lives in property getters. Do not add methods — the only ones permitted are those
  the language forces (`GetAsyncEnumerator`) plus `GetHashCode`/`ToString`, which both
  `throw new NotSupportedException()`.
- No static helpers or utility classes.
- Values are lazy: recompute on each property access, never cache.
- No `var` — always explicit types.
- Expression-bodied members for properties, indexers and accessors only; never for methods.
- File-scoped namespaces. All braces on new lines. Max line length 90.
- Private fields `_camelCase`.
- `IAsyncEnumerable<T>` for I/O-backed sequences; `IEnumerable<T>` only for in-memory ones.

## Delivery

Small PRs, each linked to an issue with `Closes #N`. Commit messages are `<type>: <subject>` —
no scope, no body. Keep `CHANGELOG.md` current.

Do not mention Claude or AI assistance in commits, PRs, or code.
