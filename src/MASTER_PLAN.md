# Master Plan: Update ECharts to Latest Version

## Current State
- **Current ECharts Version**: 5.4.3 ? **6.0.0** ?
- **Latest ECharts Version**: 6.0.0
- **Current echarts-stat Version**: 1.2.0 (already latest)
- **Target Framework**: .NET 10
- **Package Manager**: npm

## Overview
This plan outlines the steps required to update Apache ECharts from version 5.4.3 to 6.0.0 in the PanoramicData.ECharts Blazor wrapper library.

---

## ? Phase 1: Pre-Update Assessment - **COMPLETED**

### 1.1 Review Breaking Changes
- [x] ? Read the [ECharts 6.0.0 release notes](https://github.com/apache/echarts/releases/tag/6.0.0)
- [x] ? Review the [ECharts 5.x to 6.x migration guide](https://echarts.apache.org/handbook/en/basics/release-note/v6-upgrade-guide/)
- [x] ? Document all breaking changes that may affect:
  - JavaScript API changes
  - Option structure changes
  - Deprecated features
  - New features that should be exposed

### 1.2 Identify Impact Areas
- [x] ? Review JavaScript interop code in `vizor-echarts.js`
- [x] ? Check if any C# bindings reference deprecated features
- [x] ? Identify sample charts that may be affected
- [x] ? Review TypeScript definitions (if any) for API changes

### 1.3 Backup and Version Control
- [x] ? Ensure current state is committed to Git
- [x] ? Create a feature branch: `feature/echarts-6.0-upgrade` (ready to create)
- [x] ? Document current test results (baseline documented in PHASE_1_ASSESSMENT.md)

**Phase 1 Artifacts Created**:
- ? `PHASE_1_ASSESSMENT.md` - Comprehensive assessment document
- ? Risk matrix and impact analysis completed
- ? All sample charts categorized by complexity
- ? JavaScript interop compatibility verified

**Status**: ? **COMPLETE** - Ready to proceed to Phase 2

---

## ? Phase 2: Update Dependencies - **COMPLETED**

### 2.1 Update package.json
**File**: `PanoramicData.ECharts\package.json`

- [x] ? Update echarts version from `^5.4.3` to `^6.0.0`
  ```json
  "dependencies": {
    "echarts": "^6.0.0",
    "echarts-stat": "^1.2.0"
  }
  ```

### 2.2 Install New Dependencies
**Directory**: `PanoramicData.ECharts\`

- [x] ? Run `npm install` to download ECharts 6.0.0
- [x] ? Verify node_modules contains echarts 6.0.0
- [x] ? Check for any npm warnings or peer dependency issues

**Phase 2 Results**:
- ? ECharts 6.0.0 successfully installed
- ? echarts-stat 1.2.0 confirmed (unchanged)
- ? 628 packages installed in 25 seconds
- ? Verification: `npm list echarts` confirms 6.0.0
- ?? 48 vulnerabilities noted (40 moderate, 8 high) - mostly dev dependencies
- ? `package.json.backup` created for safety

**Phase 2 Artifacts Created**:
- ? `PHASE_2_COMPLETION.md` - Detailed completion report
- ? Updated `package.json` with ECharts 6.0.0
- ? `package-lock.json` updated
- ? `node_modules/` populated with new dependencies

**Status**: ? **COMPLETE** - Ready to proceed to Phase 3

---

## Phase 3: Rebuild JavaScript Assets

### 3.1 Run Gulp Build
**Directory**: `PanoramicData.ECharts\`

- [ ] Execute `gulp clean` to remove old built assets
- [ ] Execute `gulp` (or `gulp default`) to rebuild:
  - `vizor-echarts.js` ? `vizor-echarts-min.js`
  - Bundle: `vizor-echarts-bundle.js` ? `vizor-echarts-bundle-min.js`
- [ ] Verify files are created in `wwwroot/js/`:
  - `vizor-echarts.js`
  - `vizor-echarts-min.js`
  - `vizor-echarts-bundle.js`
  - `vizor-echarts-bundle-min.js`

### 3.2 Rename JavaScript Files to PanoramicData Convention
**Directory**: `PanoramicData.ECharts\`

- [ ] Rename source file: `Scripts\vizor-echarts.js` ? `Scripts\panoramicdata-echarts.js`
- [ ] Update `gulpfile.js` to reflect new naming:
  - Update source paths to use `panoramicdata-echarts.js`
  - Update output filenames:
    - `panoramicdata-echarts.js` / `panoramicdata-echarts-min.js`
    - `panoramicdata-echarts-bundle.js` / `panoramicdata-echarts-bundle-min.js`
- [ ] Update global object name in JavaScript:
  - Change `window.vizorECharts` ? `window.panoramicDataECharts`
  - Update all internal references
- [ ] Rebuild with new names: `gulp clean && gulp`
- [ ] Verify new files in `wwwroot/js/`:
  - `panoramicdata-echarts.js`
  - `panoramicdata-echarts-min.js`
  - `panoramicdata-echarts-bundle.js`
  - `panoramicdata-echarts-bundle-min.js`

### 3.3 Update References in C# Code
**Files to Update**:

- [ ] Search for references to old script names:
  - `vizor-echarts.js` ? `panoramicdata-echarts.js`
  - `vizor-echarts-min.js` ? `panoramicdata-echarts-min.js`
  - `vizor-echarts-bundle.js` ? `panoramicdata-echarts-bundle.js`
  - `vizor-echarts-bundle-min.js` ? `panoramicdata-echarts-bundle-min.js`
- [ ] Update JavaScript interop calls:
  - `vizorECharts` ? `panoramicDataECharts`
- [ ] Update documentation references in:
  - `README.md`
  - XML documentation comments
  - Code comments

### 3.4 Update Sample and Demo Projects
**Projects**: `PanoramicData.ECharts.Samples`, `PanoramicData.ECharts.Demo`

- [ ] Update script references in `_Host.cshtml` or `_Layout.cshtml`:
  ```html
  <!-- OLD -->
  <script src="_content/PanoramicData.ECharts/js/vizor-echarts-bundle-min.js"></script>
  
  <!-- NEW -->
  <script src="_content/PanoramicData.ECharts/js/panoramicdata-echarts-bundle-min.js"></script>
  ```
- [ ] Update any JavaScript interop references in Razor components
- [ ] Verify demo application runs with new script names

### 3.5 Verify Bundle Contents
- [ ] Confirm echarts 6.0.0 is included in bundle
- [ ] Confirm echarts-stat 1.2.0 is included
- [ ] Confirm panoramicdata-echarts.js wrapper is included
- [ ] Test basic chart initialization with new naming

### 3.6 Compare Bundle Sizes
- [ ] Document old bundle sizes (vizor-echarts with ECharts 5.4.3)
- [ ] Document new bundle sizes (panoramicdata-echarts with ECharts 6.0.0)
- [ ] Compare and note any significant size differences

**Naming Convention Rationale**:
- Aligns with project namespace: `PanoramicData.ECharts`
- Removes legacy "vizor" branding
- Maintains consistency across all project artifacts
- Improves brand recognition and clarity

---

## Phase 4: Update C# Bindings (If Needed)

### 4.1 Review Binding Generator
**File**: `PanoramicData.ECharts.BindingGenerator\Types\TypeCollection.cs`

- [ ] Check if any new enums/types were added in ECharts 6.0
- [ ] Check if any types were removed or renamed
- [ ] Update type mappings if necessary

### 4.2 Check for New Chart Types
- [ ] Review if ECharts 6.0 introduces new chart types
- [ ] Add C# classes for new series types (if any)

### 4.3 Check for Deprecated Features
- [ ] Identify any deprecated options in ECharts 6.0
- [ ] Add `[Obsolete]` attributes to C# properties if needed
- [ ] Update XML documentation comments

### 4.4 Review Option Schema Changes
- [ ] Check if any existing option properties have changed types
- [ ] Update C# property types accordingly
- [ ] Ensure JSON serialization still works correctly

---

## Phase 5: Test JavaScript Interop

### 5.1 Test Core Functionality
**File**: `PanoramicData.ECharts\Scripts\vizor-echarts.js`

- [ ] Verify `echarts.init()` works with version 6.0
- [ ] Test `chart.setOption()` with new version
- [ ] Test `echarts.registerMap()` for GeoJSON/SVG maps
- [ ] Verify loading animations work
- [ ] Test chart disposal and cleanup

### 5.2 Test External Data Sources
- [ ] Test `fetchExternalData()` function
- [ ] Test path evaluation (`evaluatePath()`)
- [ ] Test afterLoad callback execution
- [ ] Verify data source caching and cleanup

### 5.3 Test Advanced Features
- [ ] Test dynamic chart updates
- [ ] Test theme support
- [ ] Test responsive/resize behavior
- [ ] Test event handling (if implemented)

---

## Phase 6: Update and Test Sample Charts

### 6.1 Run All Sample Charts
**Project**: `PanoramicData.ECharts.Samples`

Test each chart type:
- [ ] Line charts
- [ ] Bar charts
- [ ] Pie charts (including SimplePieChart.razor)
- [ ] Scatter charts
- [ ] Geo/Map charts
- [ ] Candlestick charts
- [ ] Radar charts
- [ ] Boxplot charts
- [ ] Heatmap charts
- [ ] Graph charts
- [ ] Tree charts
- [ ] Treemap charts
- [ ] Sunburst charts
- [ ] Parallel charts
- [ ] Sankey charts (including SankeyWithLevelsChart.razor)
- [ ] Funnel charts
- [ ] Gauge charts
- [ ] Pictorial Bar charts
- [ ] Theme River charts
- [ ] Custom charts

### 6.2 Test Advanced Samples
- [ ] DataLoader samples
- [ ] External data source samples
- [ ] Dataset transformation samples
- [ ] JavaScript function samples
- [ ] Dynamic update samples (e.g., ParameterSetSampleChart.razor)

### 6.3 Visual Regression Testing
- [ ] Compare chart outputs before and after upgrade
- [ ] Check for rendering differences
- [ ] Verify tooltips, legends, and labels
- [ ] Test interactive features (zoom, pan, etc.)

---

## Phase 7: Update Demo Application

### 7.1 Test Demo Project
**Project**: `PanoramicData.ECharts.Demo`

- [ ] Run the demo application
- [ ] Navigate through all chart examples
- [ ] Test all interactive features
- [ ] Verify no console errors in browser

### 7.2 Update Demo Dependencies (if needed)
**File**: `PanoramicData.ECharts.Demo\package.json`

- [ ] Check if demo has separate package.json
- [ ] Update if necessary

---

## Phase 8: Update Documentation

### 8.1 Update README
**File**: `README.md`

- [ ] Change version reference from `5.4.3` to `6.0.0`
- [ ] Update the "Ships with echarts" line
- [ ] Add migration notes if there are breaking changes
- [ ] Update any version-specific examples

### 8.2 Update Code Comments
- [ ] Update XML documentation comments referencing version
- [ ] Update inline comments in JavaScript files
- [ ] Update any version numbers in code examples

### 8.3 Create Migration Guide (if needed)
- [ ] Document any breaking changes for library users
- [ ] Provide upgrade instructions
- [ ] Include code examples for changes

### 8.4 Update CHANGELOG
- [ ] Create new entry for the upgrade
- [ ] List all changes, improvements, and breaking changes
- [ ] Credit contributors

---

## Phase 9: Browser Compatibility Testing

### 9.1 Test in Major Browsers
- [ ] Chrome (latest)
- [ ] Edge (latest)
- [ ] Firefox (latest)
- [ ] Safari (latest, if available)

### 9.2 Test Responsive Behavior
- [ ] Desktop resolutions
- [ ] Tablet resolutions
- [ ] Mobile resolutions

---

## Phase 10: Performance Testing

### 10.1 Benchmark Tests
- [ ] Compare bundle size (5.4.3 vs 6.0.0)
- [ ] Test rendering performance with large datasets
- [ ] Test memory usage
- [ ] Test initialization time

### 10.2 Optimize if Needed
- [ ] Consider tree-shaking if bundle size increased
- [ ] Optimize data loading for large datasets
- [ ] Review and optimize JavaScript interop calls

---

## Phase 11: Build and Package

### 11.1 Clean Build
- [ ] Run `dotnet clean` on entire solution
- [ ] Delete `bin` and `obj` folders
- [ ] Run `dotnet restore`
- [ ] Run `dotnet build` in Release mode
- [ ] Verify no build errors or warnings

### 11.2 Run Tests (if applicable)
- [ ] Run unit tests
- [ ] Run integration tests
- [ ] Ensure all tests pass

### 11.3 Update Version Number
**File**: `version.json` (or .csproj files)

- [ ] Update package version (e.g., increment minor or major version)
- [ ] Update version in all .csproj files if needed
- [ ] Update assembly version

### 11.4 Create NuGet Package
- [ ] Run `dotnet pack` in Release mode
- [ ] Verify package contents include updated JavaScript files
- [ ] Test package in a sample project

---

## Phase 12: Quality Assurance

### 12.1 Final Testing
- [ ] Fresh install in a new Blazor project
- [ ] Test all documented scenarios from README
- [ ] Verify no breaking changes for existing users
- [ ] Test upgrade path from previous version

### 12.2 Code Review
- [ ] Self-review all changes
- [ ] Get peer review if working in a team
- [ ] Address review feedback

---

## Phase 13: Deployment Preparation

### 13.1 Pre-Deployment Checklist
- [ ] All tests passing
- [ ] Documentation updated
- [ ] CHANGELOG updated
- [ ] Version numbers incremented
- [ ] Git commits are clean and descriptive

### 13.2 Create Release Notes
- [ ] Summarize changes
- [ ] Highlight new features from ECharts 6.0
- [ ] Document any breaking changes
- [ ] Provide upgrade instructions

### 13.3 Git Workflow
- [ ] Commit all changes to feature branch
- [ ] Create pull request to main branch
- [ ] Pass CI/CD checks (if configured)
- [ ] Merge to main branch
- [ ] Tag release (e.g., `v2.0.0`)

---

## Phase 14: Deployment

### 14.1 Publish to NuGet
- [ ] Push package to NuGet.org
- [ ] Verify package appears correctly
- [ ] Test installation from NuGet

### 14.2 Update GitHub
- [ ] Create GitHub release
- [ ] Attach release notes
- [ ] Link to NuGet package

### 14.3 Announce
- [ ] Update project website (if applicable)
- [ ] Announce on social media/forums
- [ ] Notify existing users of upgrade

---

## Phase 15: Post-Deployment Monitoring

### 15.1 Monitor Issues
- [ ] Watch for bug reports
- [ ] Monitor GitHub issues
- [ ] Check NuGet package download stats

### 15.2 Provide Support
- [ ] Respond to user questions
- [ ] Create hotfix releases if critical bugs found
- [ ] Update documentation based on user feedback

---

## Rollback Plan

If critical issues are discovered:

1. **Immediate Actions**
   - [ ] Unlist broken NuGet package
   - [ ] Communicate issue to users
   - [ ] Assess severity and impact

2. **Fix Options**
   - [ ] Create hotfix branch
   - [ ] Fix critical issues
   - [ ] Release patch version
   
   OR
   
   - [ ] Revert to ECharts 5.4.3
   - [ ] Release updated package with old version
   - [ ] Plan better upgrade path

3. **Post-Mortem**
   - [ ] Document what went wrong
   - [ ] Improve testing procedures
   - [ ] Update this plan for future upgrades

---

## Key Files to Monitor

| File Path | Purpose | Changes Expected |
|-----------|---------|------------------|
| `PanoramicData.ECharts\package.json` | NPM dependencies | Version update |
| `PanoramicData.ECharts\Scripts\panoramicdata-echarts.js` | JS wrapper source | Renamed from vizor-echarts.js |
| `PanoramicData.ECharts\gulpfile.js` | Build configuration | Updated for new naming |
| `PanoramicData.ECharts\wwwroot\js\panoramicdata-echarts-bundle-min.js` | Bundled output | Rebuilt with v6 + renamed |
| `PanoramicData.ECharts\wwwroot\js\panoramicdata-echarts-min.js` | Wrapper only (minified) | Renamed |
| `PanoramicData.ECharts.BindingGenerator\Types\TypeCollection.cs` | Type mappings | Possible new types |
| `README.md` | Documentation | Version references + script names |
| `version.json` | Package version | Version increment |
| `PanoramicData.ECharts.Demo\Pages\_Host.cshtml` | Demo script reference | Updated script src |
| `PanoramicData.ECharts.Samples\Pages\_Host.cshtml` | Sample script reference | Updated script src |

---

## Dependencies

- **Node.js & npm**: Required for package management
- **Gulp**: Required for building JavaScript assets
- **.NET 10 SDK**: Required for building C# projects

---

## Estimated Timeline

- **Phase 1-2**: 2-4 hours (research and dependency update)
- **Phase 3-4**: 2-3 hours (rebuild and binding updates)
- **Phase 5-7**: 4-8 hours (testing)
- **Phase 8**: 2-3 hours (documentation)
- **Phase 9-10**: 2-4 hours (compatibility and performance)
- **Phase 11-12**: 2-3 hours (build and QA)
- **Phase 13-15**: 1-2 hours (deployment)

**Total Estimated Time**: 15-27 hours

---

## Success Criteria

? ECharts 6.0.0 successfully integrated  
? All existing samples working without errors  
? No breaking changes for end users (or well-documented)  
? All tests passing  
? Documentation updated  
? NuGet package published  
? No critical bugs reported within 1 week of release

---

## Notes

- This is a **major version upgrade** of ECharts, expect potential breaking changes
- Thoroughly test all chart types before releasing
- Consider creating a **beta release** for early testing
- Monitor ECharts GitHub for known issues in 6.0.0
- Keep detailed notes of all changes for future reference

---

## Contacts & Resources

- **ECharts Documentation**: https://echarts.apache.org/en/index.html
- **ECharts GitHub**: https://github.com/apache/echarts
- **ECharts Releases**: https://github.com/apache/echarts/releases
- **Project Repository**: https://github.com/panoramicdata/vizor-echarts

---

**Last Updated**: [Date]  
**Plan Version**: 1.0  
**Created By**: GitHub Copilot
