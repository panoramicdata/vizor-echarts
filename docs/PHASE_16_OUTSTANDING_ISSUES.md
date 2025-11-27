# Phase 16: Outstanding Issues and Enhancements

**Status**: IN PROGRESS  
**Total Estimated Time**: 150-277 hours (phased over multiple releases)

---

## Overview

This phase documents outstanding issues and enhancements to be addressed after the core v6.0.0 release. These are organized by priority and can be tackled incrementally.

---

## Issues Summary

| Issue | Priority | Estimated Time | Status |
|-------|----------|----------------|--------|
| [16.1: xUnit Duplicate Logging](#161-xunit-duplicate-logging) | ?? Medium | 1-2 hours | ? Not Started |
| [16.2: NBGV Versioning](#162-nbgv-version-number-not-applied) | ?? High | 2-3 hours | ? **COMPLETE** |
| [16.3: Symbol Package](#163-generate-and-publish-symbol-package-snupkg) | ?? Medium | 1-2 hours | ? Not Started |
| [16.4: Dark Mode](#164-demo-project-dark-mode-support) | ?? Low | 3-4 hours | ? Not Started |
| [16.5: GitHub Pages](#165-publish-demo-to-github-pages) | ?? Medium | 4-6 hours | ? Not Started |
| [16.6: Complete Examples](#166-complete-echarts-examples-coverage) | ?? Low | 100-200 hours | ? Not Started |
| [16.7: Zero Warnings](#167-eliminate-all-build-warnings) | ?? Medium | 42-63 hours | ? Not Started |

---

## 16.1 xUnit Duplicate Logging

**Issue**: Every line is doubled in the logging output  
**Priority**: ?? Medium (cosmetic issue, doesn't affect test functionality)  
**File**: `PanoramicData.ECharts.Test\xunit.runner.json`

### Investigation Required
- [ ] Research why xUnit VSTest Adapter outputs duplicate lines
- [ ] Check if issue is related to:
  - `parallelizeTestCollections: true`
  - `parallelizeAssembly: false`
  - VSTest adapter configuration
  - Multiple test output writers
- [ ] Test with different xunit.runner.json configurations
- [ ] Review xUnit VSTest Adapter version (v3.1.5+1b188a7b0a)
- [ ] Check for known issues in xUnit GitHub repository

### Potential Solutions
- [ ] Update xUnit packages to latest versions
- [ ] Modify xunit.runner.json settings
- [ ] Add custom test logger configuration
- [ ] Report bug to xUnit if confirmed as adapter issue

---

## 16.2 NBGV Version Number Not Applied ?

**Status**: ? **RESOLVED** (2024-12-27)  
**Issue**: Nerdbank.GitVersioning (NBGV) version not being applied to generated NuGet package  
**Priority**: ?? High (versioning is critical for package management) - ~~BLOCKING FOR RELEASE~~  
**Resolution**: NBGV was already properly configured and working

### Investigation Results
- [x] ? Verified `version.json` configuration at repository root - **FOUND and correct**
- [x] ? Checked `Nerdbank.GitVersioning` package referenced - **v3.9.50 installed**
- [x] ? Reviewed .csproj PropertyGroups for version override - **No overrides found**
- [x] ? Tested NBGV CLI: `nbgv get-version` - **Working correctly**
  - Version: 6.0.0.28
  - AssemblyVersion: 6.0.0.0
  - NuGetPackageVersion: 6.0.0 ?
- [x] ? Verified package generation - **PanoramicData.ECharts.6.0.0.nupkg created successfully**

### Root Cause
**False alarm** - NBGV was already properly configured and functioning correctly. The issue was misidentified.

### Configuration Details
**version.json** (repository root):
```json
{
  "$schema": "https://raw.githubusercontent.com/dotnet/Nerdbank.GitVersioning/master/src/NerdBank.GitVersioning/version.schema.json",
  "version": "6.0.0",
  "publicReleaseRefSpec": [
    "^refs/heads/main"
  ]
}
```

**PanoramicData.ECharts.csproj**:
- ? Nerdbank.GitVersioning package reference (v3.9.50)
- ? No hard-coded version properties (correct for NBGV)
- ? GeneratePackageOnBuild = True (Release mode only)

### Verification
- ? `nbgv get-version` returns: NuGetPackageVersion: 6.0.0
- ? Package file created: `PanoramicData.ECharts.6.0.0.nupkg`
- ? No changes required to .csproj or version.json

**Status**: ? **COMPLETE** - No blocking issue, ready for release

---

## 16.3 Generate and Publish Symbol Package (.snupkg)

**Issue**: Symbol package not being generated for NuGet debugging support  
**Priority**: ?? Medium (improves developer experience)  
**File**: `PanoramicData.ECharts\PanoramicData.ECharts.csproj`

### Implementation Tasks
- [ ] Add `<IncludeSymbols>true</IncludeSymbols>` to .csproj
- [ ] Add `<SymbolPackageFormat>snupkg</SymbolPackageFormat>` to .csproj
- [ ] Verify PDB files are included in package
- [ ] Test symbol package generation with `dotnet pack`
- [ ] Update `Publish.ps1` script to publish .snupkg alongside .nupkg
- [ ] Test debugging with published symbols

### Expected PropertyGroup Addition
```xml
<PropertyGroup>
  <IncludeSymbols>true</IncludeSymbols>
  <SymbolPackageFormat>snupkg</SymbolPackageFormat>
  <PublishRepositoryUrl>true</PublishRepositoryUrl>
  <EmbedUntrackedSources>true</EmbedUntrackedSources>
</PropertyGroup>
```

---

## 16.4 Demo Project Dark Mode Support

**Issue**: Demo application should support automatic dark mode detection and toggle  
**Priority**: ?? Low (nice-to-have enhancement)  
**Project**: `PanoramicData.ECharts.Demo`

### Implementation Tasks
- [ ] Add dark mode detection via JavaScript
- [ ] Create dark mode toggle component
- [ ] Add dark ECharts theme (built-in 'dark' theme)
- [ ] Update CSS for dark mode support
- [ ] Add theme preference persistence (localStorage)
- [ ] Test all chart samples in dark mode
- [ ] Update demo UI with theme switcher

### Files to Modify
- `PanoramicData.ECharts.Demo\Pages\_Host.cshtml` - Add dark mode script
- `PanoramicData.ECharts.Demo\wwwroot\css\site.css` - Add dark mode styles
- `PanoramicData.ECharts.Demo\Shared\MainLayout.razor` - Add theme toggle

---

## 16.5 Publish Demo to GitHub Pages

**Issue**: Demo application should be published to https://panoramicdata.github.io/PanoramicData.ECharts  
**Priority**: ?? Medium (helps showcase the library)  
**Project**: `PanoramicData.ECharts.Demo`

**Recommended Approach**: Convert to Blazor WebAssembly

### Implementation Tasks (WebAssembly)
- [ ] Create new Blazor WebAssembly project
- [ ] Migrate all demo pages and components
- [ ] Update project references
- [ ] Configure base path: `<base href="/PanoramicData.ECharts/" />`
- [ ] Add GitHub Actions workflow for deployment
- [ ] Test deployed site at GitHub Pages URL
- [ ] Update README with demo link

---

## 16.6 Complete ECharts Examples Coverage

**Goal**: Implement all examples from https://echarts.apache.org/examples/  
**Priority**: ?? Low (enhancement, can be incremental across releases)  
**Estimated**: 100-200 hours (phased over multiple releases)

See [PHASE_16_6_EXAMPLES_COVERAGE.md](PHASE_16_6_EXAMPLES_COVERAGE.md) for detailed breakdown.

### Implementation Strategy
- [ ] Phase 1: Complete all basic examples (1 sample per chart type)
- [ ] Phase 2: Add intermediate examples (2-3 per category)
- [ ] Phase 3: Add advanced examples (all remaining)
- [ ] Phase 4: Add 3D charts (requires echarts-gl integration)

---

## 16.7 Eliminate All Build Warnings

**Goal**: Achieve zero warnings in all projects  
**Priority**: ?? Medium (improves code quality and maintainability)  
**Estimated**: 42-63 hours

See [PHASE_16_7_ZERO_WARNINGS.md](PHASE_16_7_ZERO_WARNINGS.md) for detailed implementation plan.

### Key Tasks
- [ ] XML documentation for all public APIs (20-30 hours)
- [ ] Enable nullable reference types (10-15 hours)
- [ ] Code cleanup (5-10 hours)
- [ ] Configure `TreatWarningsAsErrors`
- [ ] Validation and testing (5 hours)

---

## Next Priority

**Phase 16.3 (Symbol Package)** - Medium priority for v6.0.0

Symbol packages improve developer experience and should be included in the initial release.

---

## Release Strategy

1. **v6.0.0** - Core upgrade with bug fixes (Phases 1-15 + 16.2 ? + 16.3)
2. **v6.1.0** - GitHub Pages demo + dark mode (Phases 16.4-16.5)
3. **v6.2.0** - Zero warnings + initial example expansion (Phase 16.7 + partial 16.6)
4. **v6.3.0+** - Continued example coverage + advanced features (Phase 16.6 continuation)
5. **v7.0.0** - ECharts v7 upgrade (when released) with complete example coverage

---

[? Back to Master Plan](../MASTER_PLAN.md) | [? Previous Phase](PHASE_15_POST_DEPLOYMENT_MONITORING.md)
