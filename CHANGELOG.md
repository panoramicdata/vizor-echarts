# Changelog

All notable changes to PanoramicData.ECharts will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [6.0.0] - 2024-12-27

### Added
- ? **Symbol Packages (.snupkg)**: Now publishing debugging symbols for better development experience
- ? **Source Link Support**: Step through library source code during debugging via Microsoft.SourceLink.GitHub
- ? **NBGV Versioning**: Using Nerdbank.GitVersioning for consistent version management
- ? **.NET 10 Support**: Updated to target .NET 10.0

### Changed
- ?? **ECharts Upgraded**: Updated from Apache ECharts 5.4.3 to 6.0.0
  - Improved rendering performance
  - Better memory management
  - Enhanced stability
- ??? **JavaScript File Naming**: Renamed all JavaScript files to `panoramicdata-echarts-*` convention
  - `vizor-echarts.js` ? `panoramicdata-echarts.js`
  - `vizor-echarts-min.js` ? `panoramicdata-echarts-min.js`
  - `vizor-echarts-bundle.js` ? `panoramicdata-echarts-bundle.js`
  - `vizor-echarts-bundle-min.js` ? `panoramicdata-echarts-bundle-min.js`
- ??? **JavaScript Global Object**: Changed from `window.vizorECharts` to `window.panoramicDataECharts`

### Fixed
- ?? **External Data Sources**: Fixed reference to global object in `ExternalDataSourceRef.cs`
- ?? **Debug Builds**: Fixed `GeneratePackageOnBuild` to only run in Release configuration

### Breaking Changes

?? **JavaScript File Names Changed**

**Action Required**: Update script tags in your `_Host.cshtml`, `_Layout.cshtml`, or `App.razor`:

```html
<!-- OLD (v5.x) -->
<script src="_content/PanoramicData.ECharts/js/vizor-echarts-bundle-min.js"></script>

<!-- NEW (v6.0+) -->
<script src="_content/PanoramicData.ECharts/js/panoramicdata-echarts-bundle-min.js"></script>
```

?? **Custom JavaScript Interop**

If you have custom JavaScript code using the global object:

```javascript
// OLD (v5.x)
window.vizorECharts.getDataSource(...)

// NEW (v6.0+)
window.panoramicDataECharts.getDataSource(...)
```

### Migration Notes

- ? **C# API**: No changes required - all existing C# code remains 100% compatible
- ? **Chart Options**: No changes - all option structures unchanged
- ? **Component Properties**: No changes - all EChart component properties work as before
- ? **Data Loading**: All DataLoader, ExternalDataSource, and Dataset features unchanged
- ? **JavaScript Functions**: All JavascriptFunction usage unchanged

### Technical Details

- **Package Size**: Bundle increased by ~82 KB (+8%) due to ECharts 6.0 enhancements
- **Tests**: All 51 tests passing (47 chart samples + 4 functional tests)
- **Browser Compatibility**: Chrome, Edge, Firefox, Safari (latest versions)

### Credits

- Apache ECharts team for the 6.0.0 release
- Original Vizor.ECharts by DataHint BV
- Maintained by Panoramic Data Limited

---

## [5.x.x] - Previous Releases

See [GitHub Releases](https://github.com/panoramicdata/PanoramicData.ECharts/releases) for earlier versions.

---

[6.0.0]: https://github.com/panoramicdata/PanoramicData.ECharts/compare/v5.x.x...v6.0.0
