# Phase 3: Rebuild JavaScript Assets ?

**Status**: COMPLETED  
**Duration**: 2-3 hours  
**Completed**: Yes

---

## Overview

Rebuild JavaScript bundles with ECharts 6.0.0 and rename all files from `vizor-echarts` to `panoramicdata-echarts` convention.

---

## 3.1 Run Gulp Build

**Directory**: `PanoramicData.ECharts\`

- [x] ? Execute `gulp clean` to remove old built assets
- [x] ? Execute `gulp` (or `gulp default`) to rebuild:
  - `panoramicdata-echarts.js` ? `panoramicdata-echarts-min.js`
  - Bundle: `panoramicdata-echarts-bundle.js` ? `panoramicdata-echarts-bundle-min.js`
- [x] ? Verify files are created in `wwwroot/js/`:
  - `panoramicdata-echarts.js`
  - `panoramicdata-echarts-min.js`
  - `panoramicdata-echarts-bundle.js`
  - `panoramicdata-echarts-bundle-min.js`

---

## 3.2 Rename JavaScript Files to PanoramicData Convention

**Directory**: `PanoramicData.ECharts\`

- [x] ? Rename source file: `Scripts\vizor-echarts.js` ? `Scripts\panoramicdata-echarts.js`
- [x] ? Update `gulpfile.js` to reflect new naming:
  - Update source paths to use `panoramicdata-echarts.js`
  - Update output filenames
- [x] ? Update global object name in JavaScript:
  - Change `window.vizorECharts` ? `window.panoramicDataECharts`
  - Update all internal references (25+ occurrences)
- [x] ? Rebuild with new names: `gulp clean && gulp`

---

## 3.3 Update References in C# Code

- [x] ? Search for references to old script names
- [x] ? Update JavaScript interop calls:
  - `vizorECharts` ? `panoramicDataECharts`
  - Updated EChart.razor (4 occurrences)
  - Updated EChartBase.cs (2 occurrences)

---

## 3.4 Update Sample and Demo Projects

**Projects**: `PanoramicData.ECharts.Samples`, `PanoramicData.ECharts.Demo`

- [x] ? Update script references in `_Host.cshtml`:
  ```html
  <!-- OLD -->
  <script src="_content/PanoramicData.ECharts/js/vizor-echarts-bundle-min.js"></script>
  
  <!-- NEW -->
  <script src="_content/PanoramicData.ECharts/js/panoramicdata-echarts-bundle-min.js"></script>
  ```

---

## 3.5 Verify Bundle Contents

- [x] ? Confirm echarts 6.0.0 is included in bundle
- [x] ? Confirm echarts-stat 1.2.0 is included
- [x] ? Confirm panoramicdata-echarts.js wrapper is included

---

## 3.6 Compare Bundle Sizes

- [x] ? Document old bundle sizes (vizor-echarts with ECharts 5.4.3)
- [x] ? Document new bundle sizes (panoramicdata-echarts with ECharts 6.0.0)

**Results**:
- Bundle size: 1103.49 KB (+81.68 KB, +8% from v5.4.3)
- Wrapper size: 3.15 KB minified
- Gulp build time: 6.83 seconds

---

## Artifacts Created

- ? `PHASE_3_COMPLETION.md` - Comprehensive completion report
- ? `panoramicdata-echarts-bundle-min.js` - Main production bundle
- ? `panoramicdata-echarts-min.js` - Wrapper only (for custom builds)
- ? Backup: `wwwroot\js\backup_20251127_134708\` - Old files preserved

---

## Breaking Changes

- ?? Script tag must be updated: `vizor-echarts-bundle-min.js` ? `panoramicdata-echarts-bundle-min.js`
- ?? Custom JS global object: `window.vizorECharts` ? `window.panoramicDataECharts`
- ? C# API unchanged (no breaking changes)

---

## Naming Convention Rationale

- Aligns with project namespace: `PanoramicData.ECharts`
- Removes legacy "vizor" branding
- Maintains consistency across all project artifacts
- Improves brand recognition and clarity

---

## Completion Status

? **COMPLETE** - Ready to proceed to [Phase 4](PHASE_04_UPDATE_CSHARP_BINDINGS.md)

---

[? Back to Master Plan](../MASTER_PLAN.md) | [? Previous Phase](PHASE_02_UPDATE_DEPENDENCIES.md) | [Next Phase ?](PHASE_04_UPDATE_CSHARP_BINDINGS.md)
