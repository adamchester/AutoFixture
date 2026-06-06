# CLAUDE.md

Guidance for AI assistants (Claude Code) working in this repository.

## What this project is

AutoFixture is a .NET library that helps write maintainable unit tests by
automating "anonymous variable" creation during test fixture setup (an
implementation of the Test Data Builder pattern). The core library is
`Ploeh.AutoFixture` (note the legacy `Ploeh` namespace root, after the
original author Mark Seemann / ploeh), plus a family of extension packages
for various mocking libraries and test frameworks.

This is a mature open-source project (originating pre-.NET Core, targeting
.NET Framework 4.0/3.5) with a very high degree of unit test coverage and a
strict TDD history — treat any change as needing accompanying tests.

## Repository layout

- `Src/` — all source code, organized as one folder per project plus a
  matching `*UnitTest` (or `*.UnitTest`) folder for its tests. Solutions
  (`*.sln`) group related projects; `Src/All.sln` builds everything.
  - `AutoFixture/` — the core library (`Ploeh.AutoFixture` namespace).
    - `Kernel/` — low-level building blocks: `ISpecimenBuilder`,
      `ISpecimenContext`, specifications, relays, commands, constructor/method
      queries, etc. This is the engine that the public DSL sits on top of.
    - `Dsl/` — the fluent customization API (`ICustomizationComposer`,
      `NodeComposer`, `CompositePostprocessComposer`, ...).
    - `DataAnnotations/` — support for `[StringLength]`, `[RegularExpression]`,
      `[Range]` attributes (includes a small regex-to-automaton engine, `Xeger`).
    - Top-level files include `Fixture.cs` (the main `IFixture`
      implementation), `ICustomization.cs`, `DefaultEngineParts.cs`,
      `DefaultRelays.cs`, generators (e.g. `GuidGenerator`,
      `DateTimeGenerator`, `*SequenceGenerator`), and customizations
      (e.g. `FreezingCustomization`, `ConstructorCustomization`).
  - `AutoFixtureUnitTest/` — unit tests for the core library; mirrors the
    core project's structure (e.g. `Kernel/`, `DataAnnotations/`).
  - `Idioms/` and `Idioms.FsCheck/` — assertions/idioms for testing API
    design conventions (guard clauses, equality members, etc.), with an
    FsCheck-based property-testing extension.
  - `SemanticComparison/` — likeness-based comparison of objects (used in
    assertions and as a building block for some idioms).
  - `TestTypeFoundation/` — shared test-only domain types reused across
    multiple `*UnitTest` projects (don't duplicate types — add here if a
    new shared test type is needed).
  - `AutoFixtureDocumentationTest/` — executable examples that double as
    documentation/regression checks for documented behavior.
  - Mocking-library integrations: `AutoMoq/`, `AutoFakeItEasy/`,
    `AutoFakeItEasy2/`, `AutoNSubstitute/`, `AutoRhinoMock/`, `AutoFoq/`
    — each provides an `ICustomization` (e.g. `AutoMoqCustomization`) that
    wires the mocking library into AutoFixture's specimen creation.
  - Test framework integrations: `AutoFixture.xUnit.net/`,
    `AutoFixture.xUnit.net2/`, `AutoFixture.NUnit2/`
    (+ `.Addins`), `AutoFixture.NUnit3/` — provide `[AutoData]`/`[Theory]`
    style attributes that auto-generate test parameters via AutoFixture.
  - Each extension/integration project follows the same
    project-plus-`*UnitTest` pairing and has its own `.sln`.
- `Build.fsx` / `build.cmd` / `build.sh` — FAKE-based build script and
  cross-platform launchers (`build.cmd` for Windows/.NET, `build.sh` for
  mono).
- `Packages/` — NuGet packages checked into source control (so the repo is
  buildable immediately after cloning, without a separate restore step for
  these legacy dependencies).
- `appveyor.yml` — CI configuration (AppVeyor, Visual Studio 2015 image).
- `CONTRIBUTING.md` — contribution guidelines (style, PR etiquette, CI).
- `.editorconfig` — 4-space indentation for `.cs`/`.fs`/`.fsx`, 2-space for
  `.sln`/`.csproj`/`.fsproj`/`.config`/`.xml`.

## Building and testing

The project uses [FAKE](http://fsharp.github.io/FAKE/) (F# Make) as its
build engine, driven by `Build.fsx`.

- Windows: `build.cmd [target] [params]`
- Linux/macOS (mono): `./build.sh [target] [params]`

Key FAKE targets (see `Build.fsx` for the full dependency graph):
- `Verify` — rebuild all projects in the `Verify` configuration (this runs
  Code Analysis with warnings-as-errors).
- `Build` — depends on `Verify`, `PatchAssemblyVersions`, `BuildOnly`,
  `RestorePatchedAssemblyVersionFiles`; produces a `Release` build.
- `TestOnly` / `Test` — runs all test assemblies. xUnit2 assemblies are run
  via the `xUnit2` runner, NUnit2 assemblies via `NUnitSequential`, and the
  NUnit3 unit test assembly via `NUnit3`. Test result files
  (`NUnit2TestResult.xml`, `NUnit3TestResult.xml`) are produced for CI.
- `NuGetPack` / `CompleteBuild` / `PublishNuGet*` — packaging/publishing
  (CI/release only — do not run these locally without explicit need).

If you cannot run the FAKE build (e.g. no MSBuild/mono toolchain available),
prefer building/running individual `*.sln` or `*.csproj`/test assemblies
directly with `dotnet`/`msbuild`/the relevant test runner, scoped to the
project(s) you changed, rather than invoking the full FAKE pipeline.

The final verification step before submitting changes is that all unit
tests pass — this is the same gate CI enforces (see `appveyor.yml` and
`CONTRIBUTING.md`).

## Coding conventions

- **Namespace root is `Ploeh.AutoFixture...`** — this is a long-standing
  convention tied to the library's history; keep new files consistent with
  the existing namespace of the folder they live in (e.g. extensions live
  under `Ploeh.AutoFixture.<ExtensionName>`).
- **Indentation**: 4 spaces for C#/F#, 2 spaces for project/solution/XML
  files (`.editorconfig` is the source of truth).
- **Line length**: keep lines under 120 characters (per `CONTRIBUTING.md`)
  so diffs/PRs are reviewable without horizontal scrolling.
- **Match the surrounding style.** This codebase predates many modern C#
  idioms (it still targets .NET 4.0/3.5 in places) — don't introduce
  language features or patterns inconsistent with a file's existing style
  just because newer C# supports them.
- **XML doc comments** are present on most public types/members
  (`<summary>`, `<param>`, `<returns>`, `<remarks>`); follow this pattern
  for new public API surface.
- **Static analysis**: the `Verify` build configuration runs FxCop/Code
  Analysis with warnings treated as errors (`AutoFixture.ruleset`,
  `GlobalSuppressions.cs`, `CodeAnalysisDictionary.xml`). Do not add
  suppressions casually — per `CONTRIBUTING.md`, suppressions require either
  a documented justification that satisfies the rule's documented
  conditions, or agreement between at least two active project maintainers.

## Testing conventions

Tests use **xUnit.net** (core library and most extensions), with some
projects on **NUnit2**/**NUnit3**. Conventions to follow when writing or
modifying tests:

- **Arrange/Act/Assert is written as commented phases**, using the
  classic AutoFixture phrasing:
  ```csharp
  [Fact]
  public void SomeBehaviorIsCorrect()
  {
      // Fixture setup
      var sut = new SomethingUnderTest();
      // Exercise system
      var result = sut.DoSomething();
      // Verify outcome
      Assert.Equal(expected, result);
      // Teardown
  }
  ```
- **The system under test is named `sut`.** Tests that merely check the
  type's role/contract are conventionally named `SutIsXxx` (e.g.
  `SutIsCustomization`).
- **Test method names are descriptive sentences** in PascalCase describing
  the scenario and expected outcome (e.g.
  `InitializedWithDefaultConstructorSutHasCorrectEngineParts`,
  `InitializeWithNullArrayThrows`).
- Test projects mirror the folder structure of the project under test
  (e.g. `AutoFixtureUnitTest/Kernel/` mirrors `AutoFixture/Kernel/`).
- Shared test doubles/helpers live alongside the tests that need them
  (e.g. `DelegatingCustomization`, `DelegatingSpecimenBuilder`,
  `CommandMock` in `AutoFixtureUnitTest/`) or in `TestTypeFoundation` when
  shared across multiple test projects — reuse these rather than creating
  near-duplicates.
- AutoFixture is famously "dogfooded" — many tests in this codebase use
  AutoFixture itself (and `[Theory, AutoData]`-style attributes from its own
  xUnit/NUnit integrations) to generate test data. Look at neighboring tests
  before deciding whether to hand-write or auto-generate fixture data.

## Architecture notes (core library)

Understanding the core pipeline helps when navigating or extending behavior:

- `IFixture` / `Fixture` is the main entry point (`Create<T>()`,
  `Customize<T>()`, etc.).
- Specimen creation flows through a tree of `ISpecimenBuilder` /
  `ISpecimenBuilderNode` instances (the "engine"), composed via
  `CompositeSpecimenBuilder`. `DefaultEngineParts` and `DefaultRelays`
  define the default pipeline.
- `ISpecimenContext` lets builders recursively resolve sub-requests
  (e.g. constructor parameters).
- `IRequestSpecification` implementations (in `Kernel/`) describe "what is
  being asked for" (type, member, seed, etc.) and are composed with
  `AndRequestSpecification`/`OrRequestSpecification`/`InverseRequestSpecification`.
- `ICustomization` is the extension point for bundling related
  configuration of an `IFixture` (e.g. `AutoMoqCustomization`,
  `ConstructorCustomization`, `FreezingCustomization`); mocking-library and
  test-framework integration projects are largely thin layers that supply
  customizations and/or attributes built on top of this engine.
- The `Dsl/` namespace provides the fluent `fixture.Build<T>()...` /
  `Customize<T>(c => c....)` composition API on top of the kernel.

## Notes for making changes

- Prefer the smallest, single-purpose change — `CONTRIBUTING.md` explicitly
  calls out the Single Responsibility Principle for pull requests.
- Add or update tests alongside any behavioral change; this codebase has
  high coverage and a TDD heritage — untested changes stand out.
- Several `.sln` files exist for different subsets of the codebase (e.g.
  `AutoMoq.sln`, `Idioms.sln`, `AutoFixture.NUnit3.sln`); when working on a
  specific extension, you can usually build/test just its solution rather
  than `All.sln`.
- Be mindful that this is a widely-used public NuGet package — public API
  changes (additions are generally fine; removals/breaking changes are not)
  should be approached cautiously and flagged explicitly.
