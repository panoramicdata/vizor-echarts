# Phase 11: Build and Package ??

**Status**: IN PROGRESS  
**Duration**: 2-3 hours  
**Blocking Issues**: NBGV versioning (Phase 16.2)

---

## Overview

Build the solution in Release mode, run tests, and create NuGet packages with proper versioning and symbols.

---

## 11.1 Clean Build

- [x] ? Run `dotnet clean` on entire solution
- [x] ? Delete `bin` and `obj` folders
- [x] ? Run `dotnet restore`
- [x] ? Run `dotnet build` in Release mode
- [x] ? Verify no build errors or warnings

**Status**: ? Complete - Solution builds successfully

---

## 11.2 Run Tests

- [x] ? Run unit tests (Playwright tests)
- [ ] ? Run integration tests (if applicable)
- [x] ? Ensure all tests pass

**Results**: 51/51 tests passing
- 47 chart sample tests (`AllChartsTests.cs`)
- 4 functional tests (`ChartTests.cs`)

**Status**: ? Complete

---

## 11.3 Update Version Number

**File**: `version.json` (NBGV)

- [ ] ?? **BLOCKING ISSUE**: NBGV version not applied to NuGet package
- [ ] Update package version (e.g., increment minor or major version)
- [ ] Verify NBGV configuration in .csproj files
- [ ] Test package version generation

**Current Issue**: 
The NuGet package is not getting its version from Nerdbank.GitVersioning. This must be resolved before publishing.

**Related**: See [Phase 16.2: NBGV Versioning](PHASE_16_OUTSTANDING_ISSUES.md#162-nbgv-version-number-not-applied)

**Status**: ? Pending - **BLOCKS RELEASE**

---

## 11.4 Create NuGet Package

- [ ] Run `dotnet pack` in Release mode
- [ ] Verify package contents include updated JavaScript files
- [ ] Test package in a sample project
- [ ] ?? **NEW REQUIREMENT**: Generate and publish .snupkg (symbol package)

**Required PropertyGroup Settings**:
```xml
<PropertyGroup>
  <!-- Versioning -->
  <Version><!-- From NBGV --></Version>
  
  <!-- Symbol Package -->
  <IncludeSymbols>true</IncludeSymbols>
  <SymbolPackageFormat>snupkg</SymbolPackageFormat>
  <PublishRepositoryUrl>true</PublishRepositoryUrl>
  <EmbedUntrackedSources>true</EmbedUntrackedSources>
</PropertyGroup>
```

**Related**: See [Phase 16.3: Symbol Packages](PHASE_16_OUTSTANDING_ISSUES.md#163-generate-and-publish-symbol-package-snupkg)

**Status**: ? Pending - Waiting for NBGV resolution

---

## Package Validation Checklist

Once packages are generated, verify:

- [ ] Package version matches NBGV output (`nbgv get-version`)
- [ ] .nupkg contains:
  - [ ] `wwwroot/js/panoramicdata-echarts-bundle-min.js`
  - [ ] `wwwroot/js/panoramicdata-echarts-bundle.js`
  - [ ] `wwwroot/js/panoramicdata-echarts-min.js`
  - [ ] `wwwroot/js/panoramicdata-echarts.js`
  - [ ] `wwwroot/js/panoramicdata-echarts-interop.js`
  - [ ] All C# assemblies
  - [ ] XML documentation files
- [ ] .snupkg contains:
  - [ ] PDB files for debugging
  - [ ] Source link information
- [ ] Package metadata is correct:
  - [ ] Title: PanoramicData.ECharts
  - [ ] Authors
  - [ ] Description
  - [ ] License
  - [ ] Project URL
  - [ ] Icon
  - [ ] Tags

---

## Files to Modify

**`PanoramicData.ECharts\PanoramicData.ECharts.csproj`**:
- Add/configure NBGV package reference
- Add symbol package settings
- Remove any hard-coded version numbers

**`version.json`** (repository root):
- Configure version format
- Set initial version (e.g., 6.0.0)

**`Publish.ps1`**:
- Update to publish both .nupkg and .snupkg
- Add validation for NBGV-generated version

---

## Completion Criteria

- [ ] Solution builds without errors or warnings
- [ ] All 51 tests pass
- [ ] NBGV versioning configured and working
- [ ] .nupkg generated with correct version
- [ ] .snupkg generated with symbols
- [ ] Package validation complete
- [ ] Test installation in sample project

---

## Blocking Issues

?? **CRITICAL**: Must resolve [Phase 16.2 (NBGV Versioning)](PHASE_16_OUTSTANDING_ISSUES.md#162-nbgv-version-number-not-applied) before this phase can be completed.

---

## Completion Status

?? **IN PROGRESS** - Waiting for NBGV configuration

Next: Once NBGV is configured, proceed to [Phase 12: Quality Assurance](PHASE_12_QUALITY_ASSURANCE.md)

---

[? Back to Master Plan](../MASTER_PLAN.md) | [? Previous Phase](PHASE_10_PERFORMANCE_TESTING.md) | [Next Phase ?](PHASE_12_QUALITY_ASSURANCE.md)
