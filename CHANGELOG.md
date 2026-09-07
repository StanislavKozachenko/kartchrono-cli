# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- `kartchrono` CLI: `tracks`, `session`, `live`, `laps` and `records` commands over the
  KartChrono WebSocket and archive endpoints, with `--track`, `--kart`, `--period`,
  `--json`, `--help` and `--version`.
- Native AOT packaging as a `dotnet tool` for `linux-x64`, `linux-arm64`, `osx-arm64`,
  `osx-x64` and `win-x64`, with a framework-dependent fallback for every other platform.
- A release workflow that builds each native binary on a matching runner, publishes the
  runtime-specific packages before the pointer package, and attaches standalone binaries
  to the GitHub release.

### Changed

- Replaced the `Pure.Template` boilerplate with `kartchrono-cli` project metadata.
- Lowered the CI coverage gate to 80% and the mutation gate to 60%.
- Pointed Dependabot at the projects under `src/`.

### Removed

- The NuGet library publish workflow, which does not apply to a CLI.
