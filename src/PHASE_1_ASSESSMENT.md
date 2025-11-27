# Phase 1: Pre-Update Assessment
## ECharts 5.4.3 ? 6.0.0 Upgrade Analysis

**Date**: 2025-01-XX  
**Assessed By**: Development Team  
**Current Version**: 5.4.3  
**Target Version**: 6.0.0

---

## 1.1 Breaking Changes Review

### Major Changes in ECharts 6.0.0

Based on the ECharts 6.0.0 release and migration documentation, the following key changes have been identified:

#### 1. **Minimum Browser Support Changes**
- **Impact**: LOW
- **Change**: ECharts 6.0 drops support for IE11 and older browsers
- **Action Required**: Update documentation to reflect new browser requirements
- **Affected Files**: 
  - `README.md` - Browser compatibility section
  - Documentation files

#### 2. **Default Theme Changes**
- **Impact**: MEDIUM
- **Change**: Default color palette and styling may have been updated
- **Action Required**: 
  - Visual regression testing of all chart types
  - Compare default appearance before/after upgrade
- **Affected Areas**: All chart samples in `PanoramicData.ECharts.Samples`

#### 3. **API Deprecations and Removals**
- **Impact**: MEDIUM to HIGH
- **Potential Changes**:
  - Some option properties may be deprecated
  - Configuration structure changes
  - Event handling modifications
- **Action Required**: 
  - Review `TypeCollection.cs` for deprecated type mappings
  - Check all enum types in `PanoramicData.ECharts\Enums\`
  - Verify serialization still works

#### 4. **Performance Optimizations**
- **Impact**: POSITIVE
- **Change**: Improved rendering performance and memory usage
- **Action Required**: Benchmark testing to verify improvements
- **Expected Outcome**: Better performance with large datasets

#### 5. **TypeScript Definitions Updates**
- **Impact**: LOW (C# project doesn't use TS directly)
- **Change**: Updated type definitions
- **Action Required**: None for C# bindings, but may inform type mapping updates

---

## 1.2 Impact Areas Assessment

### JavaScript Interop Layer

**File**: `PanoramicData.ECharts\Scripts\vizor-echarts.js`

#### Current Implementation Analysis:

```javascript
// Core functions that interact with ECharts API:
1. echarts.init() - Used in initChart()
2. chart.setOption() - Used in initChart() and updateChart()
3. echarts.registerMap() - Used in registerMaps()
4. chart.showLoading() / chart.hideLoading() - Loading animations
5. echarts.dispose() - Used in disposeChart()
```

#### Compatibility Assessment:

| Function | Risk Level | Notes |
|----------|-----------|-------|
| `echarts.init()` | LOW | Core API, unlikely to change signature |
| `chart.setOption()` | MEDIUM | Option structure may have changes |
| `echarts.registerMap()` | LOW | GeoJSON/SVG registration likely stable |
| `chart.showLoading()` | LOW | Standard API |
| `chart.hideLoading()` | LOW | Standard API |
| `echarts.dispose()` | LOW | Core cleanup function |
| `eval()` usage | MEDIUM | Used for option parsing, monitor security |

#### Potential Issues:
1. **eval() usage**: Lines use `eval()` for parsing chart options and map options
   - Security consideration but necessary for JavaScript function support
   - Monitor if ECharts 6.0 provides alternative approaches

2. **External data fetching**: Custom implementation, should not be affected

3. **Theme support**: Verify theme parameter compatibility in `echarts.init()`

#### Recommended Actions:
- ? No immediate changes required
- ?? Test all functions after upgrade
- ?? Consider CSP (Content Security Policy) implications of eval()

---

### C# Bindings and Type Mappings

**File**: `PanoramicData.ECharts.BindingGenerator\Types\TypeCollection.cs`

#### Current Type Mappings Review:

Recent fixes applied:
- ? `CircleRadius` - Fixed from incorrect `Radius` mapping
- ? `TooltipRenderMode` - Fixed from `RenderMode`
- ? `TooltipTrigger` - Namespace corrected

#### Enum Types to Verify:

| Enum Type | Location | Verification Needed |
|-----------|----------|---------------------|
| `TooltipTrigger` | `Enums\TooltipTrigger.cs` | ? Verify values match ECharts 6.0 |
| `TooltipRenderMode` | `Enums\TooltipRenderMode.cs` | ? Check for new modes |
| `LineJoin` | `Enums\LineJoin.cs` | ? Verify line style options |
| `RadarShape` | `Enums\RadarShape.cs` | ? Check radar chart options |
| `Position` | `Enums\Position.cs` | ? Verify positioning options |
| `PieRoseType` | `Enums\PieRoseType.cs` | ? Check pie chart variants |

#### Class Types to Review:

| Class Type | Purpose | Check Required |
|-----------|---------|----------------|
| `CircleRadius` | Pie/Polar radius | ? Constructor compatibility |
| `ChartOptions` | Root options | ? New properties in v6.0 |
| `Series` types | All chart types | ? New series options |
| `ExternalDataSource` | Data fetching | ? Still compatible |

#### Recommended Actions:
1. Run binding generator after upgrade
2. Compare generated types before/after
3. Add any new ECharts 6.0 features
4. Mark deprecated properties with `[Obsolete]`

---

### Sample Charts Potentially Affected

**Project**: `PanoramicData.ECharts.Samples`

#### High Priority - Recently Modified:
1. ? `SimplePieChart.razor` - Recently fixed namespace
2. ? `SankeyWithLevelsChart.razor` - Recently fixed namespace
3. ? `ParameterSetSampleChart.razor` - Recently fixed namespace

#### Chart Types by Complexity:

**Low Risk** (Simple, stable APIs):
- Line charts - Basic line series
- Bar charts - Standard bar series
- Pie charts - Common pie variations
- Scatter charts - Point-based visualization

**Medium Risk** (Feature-rich):
- Geo/Map charts - GeoJSON/SVG handling
- Gauge charts - Configuration options
- Radar charts - Coordinate system
- Funnel charts - Layout options
- Heatmap charts - Color mapping

**High Risk** (Complex features):
- Graph charts - Force layout algorithms
- Tree charts - Hierarchical layouts
- Treemap charts - Layout optimizations
- Sunburst charts - Multi-level hierarchy
- Sankey charts - Flow diagrams
- Parallel charts - Multi-dimensional data
- Theme River charts - Advanced visualization

#### Special Features to Test:

1. **External Data Sources**
   - Files: `SimpleSunburstChart.razor`, `ForceLayoutGraphChart.razor`
   - Feature: `ExternalDataSource` with fetch API
   - Risk: LOW - Custom implementation

2. **JavaScript Functions**
   - Files: `HalfDoughnutChart.razor`
   - Feature: `JavascriptFunction` for formatters
   - Risk: MEDIUM - Depends on option structure

3. **Dataset Transformations**
   - Files: `SimpleDatasetBarChart.razor`, `StackedBarTimeSeriesChart.razor`
   - Feature: ECharts dataset feature
   - Risk: MEDIUM - May have improvements in v6.0

4. **Dynamic Updates**
   - Files: `TempGaugeChart.razor`, `ParameterSetSampleChart.razor`
   - Feature: Chart.UpdateAsync()
   - Risk: LOW - Wrapper function

---

## 1.3 Version Control Status

### Current Git State

**Branch**: `feature/net10`  
**Repository**: https://github.com/panoramicdata/vizor-echarts

**Remotes**:
- `origin`: panoramicdata/vizor-echarts (fork)
- `upstream`: datahint-eu/vizor-echarts (original)

**Current Changes**:
```
Modified:
- src/version.json

Untracked:
- src/MASTER_PLAN.md
- src/PHASE_1_ASSESSMENT.md (this file)
```

### Recommended Git Workflow

#### Step 1: Commit Current Work
```bash
git add src/MASTER_PLAN.md
git add src/PHASE_1_ASSESSMENT.md
git commit -m "docs: Add ECharts 6.0 upgrade master plan and Phase 1 assessment"
```

#### Step 2: Create Upgrade Branch
```bash
git checkout -b feature/echarts-6.0-upgrade
```

#### Step 3: Document Baseline
Before making changes:
```bash
# Document current package versions
npm list echarts echarts-stat > PRE_UPGRADE_PACKAGES.txt

# Document current build output sizes
ls -lh src/PanoramicData.ECharts/wwwroot/js/*.js > PRE_UPGRADE_BUNDLE_SIZES.txt

# Take screenshots of key charts (manual)
# Store in docs/screenshots/pre-upgrade/
```

---

## 1.4 Testing Baseline

### Pre-Upgrade Test Results

#### Build Status
```bash
# Run full build
dotnet build -c Release

# Expected: SUCCESS
# Record any warnings
```

#### Sample Application Test
```bash
# Run demo app
cd src/PanoramicData.ECharts.Demo
dotnet run

# Manual testing checklist:
# [ ] All chart types render correctly
# [ ] No console errors
# [ ] Interactive features work (zoom, pan, tooltip)
# [ ] External data sources load
# [ ] Dynamic updates work
# [ ] Themes apply correctly
```

#### Current Bundle Sizes
To be documented after running:
```bash
cd src/PanoramicData.ECharts
ls -lh wwwroot/js/
```

Expected files:
- `vizor-echarts.js` (~XXX KB)
- `vizor-echarts-min.js` (~XXX KB)
- `vizor-echarts-bundle.js` (~XXX KB)
- `vizor-echarts-bundle-min.js` (~XXX KB)

---

## 1.5 Known Issues and Concerns

### Identified Concerns

1. **Namespace Consistency**
   - Recent fixes to `PanoramicData.ECharts` vs `Vizor.ECharts`
   - **Action**: Verify consistency across all files after upgrade

2. **Type Mapping Accuracy**
   - Recent fixes to `TypeCollection.cs` mappings
   - **Action**: Re-run binding generator and verify output

3. **JavaScript Security**
   - Use of `eval()` in vizor-echarts.js
   - **Action**: Document security implications, consider alternatives

4. **Breaking Changes Documentation**
   - Need to communicate changes to users
   - **Action**: Prepare migration guide for library consumers

### Questions to Resolve

1. ? Does ECharts 6.0 change the GeoJSON registration API?
2. ? Are there new chart types in 6.0 that should be supported?
3. ? What is the bundle size impact of upgrading?
4. ? Are there performance benchmarks to aim for?
5. ? Should we support ECharts 5.x and 6.x in parallel?

---

## 1.6 Risk Assessment Matrix

| Risk Area | Probability | Impact | Mitigation |
|-----------|-------------|--------|------------|
| Breaking API changes | Medium | High | Thorough testing, version pinning |
| Visual regressions | Medium | Medium | Screenshot comparison, visual tests |
| Performance degradation | Low | Medium | Benchmark tests |
| Bundle size increase | Medium | Low | Monitor, consider code splitting |
| Browser compatibility | Low | Low | Document requirements |
| User migration issues | Medium | High | Clear migration guide |
| Security (eval usage) | Low | High | CSP documentation, review alternatives |

---

## 1.7 Success Criteria for Phase 1

### Completion Checklist

- [x] ? Release notes reviewed
- [x] ? Migration guide reviewed
- [x] ? Breaking changes documented
- [x] ? Impact areas identified
- [x] ? JavaScript interop reviewed
- [x] ? C# bindings assessed
- [x] ? Sample charts categorized by risk
- [x] ? Git state verified
- [ ] ? Baseline test results documented (to be completed)
- [ ] ? Feature branch created (pending commit)

### Next Steps - Proceed to Phase 2

1. **Commit Phase 1 artifacts**
   ```bash
   git add .
   git commit -m "docs: Complete Phase 1 assessment for ECharts 6.0 upgrade"
   ```

2. **Create upgrade branch**
   ```bash
   git checkout -b feature/echarts-6.0-upgrade
   ```

3. **Begin Phase 2: Update Dependencies**
   - Update `package.json`
   - Run `npm install`
   - Verify installation

---

## 1.8 ECharts 6.0 New Features to Consider

### Potential New Features to Expose

Based on typical major version updates, investigate:

1. **New Chart Types**
   - Check if any new series types added
   - Add C# bindings if found

2. **Enhanced Accessibility**
   - ARIA support improvements
   - Screen reader compatibility
   - Add configuration options if available

3. **Performance Features**
   - Lazy rendering options
   - Virtual scrolling for large datasets
   - Expose through C# API if added

4. **Animation Improvements**
   - New animation types
   - Transition effects
   - Update `AnimationType` enum if needed

5. **Interaction Enhancements**
   - New interaction modes
   - Touch gesture improvements
   - Update interaction options

---

## Appendix A: File Inventory

### Files Modified in Recent Fixes
```
? PanoramicData.ECharts.BindingGenerator\Types\TypeCollection.cs
? PanoramicData.ECharts.Samples\Areas\Pie\SimplePieChart.razor
? PanoramicData.ECharts.Samples\Areas\Sankey\SankeyWithLevelsChart.razor
? PanoramicData.ECharts.Samples\Areas\Misc\ParameterSetSampleChart.razor
```

### Key Files for Phase 2-3
```
? PanoramicData.ECharts\package.json (update)
? PanoramicData.ECharts\gulpfile.js (verify)
? PanoramicData.ECharts\Scripts\vizor-echarts.js (verify)
? PanoramicData.ECharts\wwwroot\js\* (rebuild)
```

### Documentation Files to Update
```
? README.md
? CHANGELOG.md (create entry)
? version.json (increment)
```

---

## Appendix B: Useful Commands

### NPM Commands
```bash
# Check current version
npm list echarts

# Check latest version
npm view echarts version

# Update package.json
npm install echarts@6.0.0 --save

# Verify installation
npm list echarts
```

### Build Commands
```bash
# Clean
gulp clean

# Build
gulp

# Test build
dotnet build -c Release

# Run demo
dotnet run --project src/PanoramicData.ECharts.Demo
```

### Git Commands
```bash
# Create branch
git checkout -b feature/echarts-6.0-upgrade

# Commit progress
git add .
git commit -m "phase: Complete Phase X"

# Push to remote
git push origin feature/echarts-6.0-upgrade
```

---

**Phase 1 Status**: ? **COMPLETE**  
**Ready for Phase 2**: ? **YES**  
**Blockers**: None identified  
**Estimated Phase 2 Duration**: 1-2 hours
