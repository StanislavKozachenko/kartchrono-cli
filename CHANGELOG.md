# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed

- Replaced the `Pure.Template` boilerplate with `kartchrono-cli` project metadata.
- Lowered the CI coverage gate to 80% and the mutation gate to 70%.
- Pointed Dependabot at the projects under `src/`.

### Removed

- The NuGet library publish workflow, which does not apply to a CLI.
