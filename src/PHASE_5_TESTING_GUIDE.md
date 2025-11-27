# Phase 5: Test JavaScript Interop - Testing Guide & Report

**Date**: 2025-01-27  
**Phase**: 5 of 15  
**Status**: ? **MANUAL TESTING REQUIRED**  
**Build Status**: ? Release build successful

---

## Executive Summary

Phase 5 requires **manual browser testing** to verify JavaScript interop with ECharts 6.0.0. This document provides comprehensive testing procedures and checklists.

---

## Build Verification ?

### Release Build Status

**Command**: `dotnet build --configuration Release`

**Result**: ? **SUCCESS**
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

**Projects Built**:
- ? PanoramicData.ECharts
- ? PanoramicData.ECharts.BindingGenerator
- ? PanoramicData.ECharts.Demo
- ? PanoramicData.ECharts.Samples
- ? PanoramicData.ECharts.Sandbox

---

## How to Run Demo Application

### Step 1: Start the Server

```powershell
cd C:\Users\david\Projects\PanoramicData.ECharts\src\PanoramicData.ECharts.Demo
dotnet run --configuration Release
```

**Expected Output**:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
      ...
```

### Step 2: Open Browser

Navigate to: **http://localhost:5000**

---

## Phase 5.1: Test Core Functionality

### Test 5.1.1: Verify `echarts.init()` Works with Version 6.0 ?

**Purpose**: Confirm ECharts 6.0 initializes correctly

**Steps**:
1. Open browser developer tools (F12)
2. Navigate to any chart page (e.g., Simple Pie Chart)
3. In Console tab, verify chart object exists:
   ```javascript
   // Should show ECharts instance
   window.panoramicDataECharts.charts
   ```

**Success Criteria**:
- [ ] No JavaScript errors in console
- [ ] Chart renders on screen
- [ ] panoramicDataECharts object exists
- [ ] charts Map contains at least one entry

**Expected Console Output**:
```
(none) - No errors should appear
```

---

### Test 5.1.2: Test `chart.setOption()` with New Version ?

**Purpose**: Verify chart options are properly set

**Steps**:
1. Navigate to SimplePieChart example
2. Chart should render with:
   - Title: "Referer of a Website"
   - Legend on the left
   - 5 pie segments
3. Open developer tools ? Console
4. Execute:
   ```javascript
   let chartId = document.querySelector('[id^="chart"]').id;
   let chart = window.panoramicDataECharts.getChart(chartId);
   console.log(chart.getOption());
   ```

**Success Criteria**:
- [ ] Chart renders correctly
- [ ] `getOption()` returns valid options object
- [ ] Options match C# serialization:
  ```json
  {
    "title": {"text": "Referer of a Website", ...},
    "tooltip": {"trigger": "item"},
    "legend": {"orient": "vertical", "left": "left"},
    "series": [...]
  }
  ```

---

### Test 5.1.3: Test `echarts.registerMap()` for GeoJSON/SVG Maps ?

**Purpose**: Verify map registration works

**Steps**:
1. Navigate to a Geo map example (e.g., USA Geo Map)
2. Chart should display a geographic map
3. Check console for map registration:
   ```javascript
   // Map should be registered before chart init
   ```

**Success Criteria**:
- [ ] Geographic map renders correctly
- [ ] Map boundaries visible
- [ ] No errors about missing map data
- [ ] Tooltip shows location names on hover

**Test Files**:
- `Areas/Geo/UsaGeoMap.razor`
- `Areas/Geo/BelgianMunicipalityMap.razor`
- `Areas/Geo/FlightSeatsGeoMap.razor`

---

### Test 5.1.4: Verify Loading Animations Work ?

**Purpose**: Confirm loading spinner displays

**Steps**:
1. Navigate to a chart with DataLoader
2. Observe chart area during initialization
3. Should see brief loading animation
4. Use browser devtools ? Network tab ? Slow 3G to simulate delay

**Success Criteria**:
- [ ] Loading animation appears
- [ ] Animation disappears when data loaded
- [ ] Chart renders after loading complete

**Test Files**:
- `Areas/Misc/DataLoaderSampleChart.razor`

---

### Test 5.1.5: Test Chart Disposal and Cleanup ?

**Purpose**: Verify charts are properly disposed

**Steps**:
1. Navigate to any chart page
2. Note the chart ID in browser console:
   ```javascript
   let charts = window.panoramicDataECharts.charts;
   console.log([...charts.keys()]); // List all chart IDs
   ```
3. Navigate away from the page
4. Check if chart was disposed:
   ```javascript
   console.log([...window.panoramicDataECharts.charts.keys()]); // Should be empty or different IDs
   ```

**Success Criteria**:
- [ ] Chart instances removed from Map on dispose
- [ ] No memory leaks (check devtools Memory profiler)
- [ ] echarts.dispose() called correctly

**Console Commands**:
```javascript
// Before navigation
let beforeCount = window.panoramicDataECharts.charts.size;
console.log('Charts before:', beforeCount);

// After navigation
let afterCount = window.panoramicDataECharts.charts.size;
console.log('Charts after:', afterCount);
// afterCount should be 0 or different from before
```

---

## Phase 5.2: Test External Data Sources

### Test 5.2.1: Test `fetchExternalData()` Function ?

**Purpose**: Verify external data fetching works

**Steps**:
1. Navigate to SimpleSunburstChart (uses external data)
2. Open Network tab in devtools
3. Verify HTTP request to `/data/sunburst_simple.json`
4. Chart should render with external data

**Success Criteria**:
- [ ] HTTP 200 response for data file
- [ ] Data fetched before chart initialization
- [ ] Chart renders with fetched data
- [ ] No CORS errors

**Test Files**:
- `Areas/Sunburst/SimpleSunburstChart.razor`
- `Areas/Graph/ForceLayoutGraphChart.razor`

**Console Verification**:
```javascript
// Check data sources cache
console.log(window.panoramicDataECharts.dataSources);
// Should contain fetched data
```

---

### Test 5.2.2: Test Path Evaluation (`evaluatePath()`) ?

**Purpose**: Verify JSON path expression evaluation

**Steps**:
1. Navigate to SankeyWithLevelsChart
2. Uses path expression: `path: "nodes"` and `path: "links"`
3. Chart should render Sankey diagram

**Success Criteria**:
- [ ] Chart renders correctly
- [ ] Nodes extracted from JSON data
- [ ] Links extracted from JSON data
- [ ] Path evaluation works for nested properties

**Test Files**:
- `Areas/Sankey/SankeyWithLevelsChart.razor`

**Console Test**:
```javascript
// Test evaluatePath function directly
let testData = {users: {active: [{name: "John"}]}};
let result = window.panoramicDataECharts.evaluatePath(testData, "users.active");
console.log(result); // Should show [{name: "John"}]
```

---

### Test 5.2.3: Test AfterLoad Callback Execution ?

**Purpose**: Verify afterLoad function executes

**Steps**:
1. Navigate to ForceLayoutGraphChart
2. Uses afterLoad to modify node sizes
3. Chart should render with modified data

**Success Criteria**:
- [ ] Chart renders correctly
- [ ] AfterLoad function executed (check if nodes have symbolSize)
- [ ] Data transformed before chart initialization
- [ ] No JavaScript errors from eval()

**Test Files**:
- `Areas/Graph/ForceLayoutGraphChart.razor`

**Expected Console** (if logging enabled):
```
FETCH {id: "...", url: "/data/les-miserables.json", ...}
(data fetched)
(afterLoad function executed)
```

---

### Test 5.2.4: Verify Data Source Caching and Cleanup ?

**Purpose**: Confirm data sources are cached and cleaned up

**Steps**:
1. Navigate to a chart with external data
2. Check cache:
   ```javascript
   console.log(window.panoramicDataECharts.dataSources.size);
   ```
3. Navigate away
4. Check cache again (should be cleared)

**Success Criteria**:
- [ ] Data cached after fetch
- [ ] Cache cleared on dispose
- [ ] No memory leaks

---

## Phase 5.3: Test Advanced Features

### Test 5.3.1: Test Dynamic Chart Updates ?

**Purpose**: Verify `UpdateAsync()` works

**Steps**:
1. Navigate to ParameterSetSampleChart or TempGaugeChart
2. Chart should update dynamically (e.g., temperature changing)
3. Observe smooth transitions

**Success Criteria**:
- [ ] Chart updates without full page reload
- [ ] Animations smooth
- [ ] No flickering
- [ ] Data persists between updates

**Test Files**:
- `Areas/Misc/ParameterSetSampleChart.razor` (updates after 3 seconds)
- `Areas/Gauge/TempGaugeChart.razor` (temperature updates)

**Console Verification**:
```javascript
// updateChart should be called
window.panoramicDataECharts.updateChart = new Proxy(
  window.panoramicDataECharts.updateChart, {
    apply(target, thisArg, args) {
      console.log('updateChart called:', args[0]); // chart ID
      return target.apply(thisArg, args);
    }
  }
);
```

---

### Test 5.3.2: Test Theme Support ?

**Purpose**: Verify themes apply correctly

**Steps**:
1. Check if any samples use themes
2. If available, verify theme colors applied

**Success Criteria**:
- [ ] Theme colors match expected
- [ ] Theme passed to echarts.init()

**Note**: May not be extensively used in samples

---

### Test 5.3.3: Test Responsive/Resize Behavior ?

**Purpose**: Verify charts resize with window

**Steps**:
1. Navigate to any chart
2. Resize browser window
3. Chart should resize accordingly

**Success Criteria**:
- [ ] Chart resizes when window resizes
- [ ] Proportions maintained
- [ ] No visual glitches

**Browser Test**:
- Resize window from 1920px ? 1024px ? 768px
- Chart should adjust smoothly

---

### Test 5.3.4: Test Event Handling (if implemented) ?

**Purpose**: Check if event handlers work

**Steps**:
1. Navigate to charts with interactive features
2. Click on chart elements
3. Hover for tooltips
4. Test zoom/pan (if available)

**Success Criteria**:
- [ ] Click events fire (if implemented)
- [ ] Hover shows tooltips
- [ ] Zoom/pan works on appropriate charts

---

## Browser Console Verification Commands

### Check ECharts Version

```javascript
// Verify ECharts 6.0.0 is loaded
console.log(echarts.version); // Should show "6.0.0"
```

### Check Global Object

```javascript
// Verify panoramicDataECharts exists
console.log(window.panoramicDataECharts);
// Should show object with: charts, dataSources, logging, methods

// Old object should NOT exist
console.log(window.vizorECharts); // Should be undefined
```

### Check Active Charts

```javascript
// List all active charts
let charts = window.panoramicDataECharts.charts;
console.log('Active charts:', charts.size);
for(let [id, chart] of charts) {
  console.log('Chart ID:', id);
  console.log('Chart instance:', chart);
}
```

### Enable Debug Logging

```javascript
// Enable verbose logging
window.panoramicDataECharts.changeLogging(true);
// Now navigate to charts to see fetch/init logs
```

### Test Chart Retrieval

```javascript
// Get a chart by ID
let chartId = 'chart12345'; // Replace with actual ID from DOM
let chart = window.panoramicDataECharts.getChart(chartId);
if (chart) {
  console.log('Chart found:', chart);
  console.log('Chart options:', chart.getOption());
} else {
  console.error('Chart not found');
}
```

---

## Sample Charts to Test

### Priority 1: Core Chart Types

| Chart Type | Test File | Key Features | Status |
|------------|-----------|--------------|--------|
| Pie | SimplePieChart.razor | Basic rendering, tooltip | [ ] |
| Line | SimpleLineChart.razor | Basic line, smooth curves | [ ] |
| Bar | SimpleBarChart.razor | Bar chart, axis | [ ] |
| Scatter | SimpleScatterChart.razor | Point data | [ ] |

### Priority 2: Complex Visualizations

| Chart Type | Test File | Key Features | Status |
|------------|-----------|--------------|--------|
| Sankey | SankeyWithLevelsChart.razor | External data, path eval | [ ] |
| Graph | ForceLayoutGraphChart.razor | AfterLoad callback | [ ] |
| Sunburst | SimpleSunburstChart.razor | External data | [ ] |
| Heatmap | SimpleHeatmapChart.razor | Color mapping | [ ] |

### Priority 3: Advanced Features

| Feature | Test File | Key Features | Status |
|---------|-----------|--------------|--------|
| DataLoader | DataLoaderSampleChart.razor | Async data loading | [ ] |
| Dynamic Update | ParameterSetSampleChart.razor | UpdateAsync() | [ ] |
| Dynamic Update | TempGaugeChart.razor | Real-time updates | [ ] |
| Dataset | SimpleDatasetBarChart.razor | Dataset transform | [ ] |

### Priority 4: Geographic Charts

| Chart Type | Test File | Key Features | Status |
|------------|-----------|--------------|--------|
| Geo (USA) | UsaGeoMap.razor | GeoJSON registration | [ ] |
| Geo (Belgium) | BelgianMunicipalityMap.razor | Custom map | [ ] |

---

## Known Issues to Watch For

### Issue #1: Script Loading
**Symptom**: "panoramicDataECharts is not defined"  
**Cause**: Script tag not updated or bundle not loaded  
**Fix**: Verify `panoramicdata-echarts-bundle-min.js` in _Host.cshtml

### Issue #2: Chart Not Rendering
**Symptom**: Blank chart area  
**Cause**: ECharts 6.0 API incompatibility  
**Check**: Browser console for errors

### Issue #3: External Data Fails
**Symptom**: Charts with external data don't render  
**Cause**: Fetch API issues or path evaluation  
**Check**: Network tab for 404s, console for errors

### Issue #4: Memory Leaks
**Symptom**: Charts not disposed  
**Cause**: disposeChart not called  
**Check**: Memory profiler, dataSources Map size

---

## Performance Observations

### Bundle Size
- panoramicdata-echarts-bundle-min.js: **1103.49 KB**
- ECharts 6.0 portion: ~1100 KB
- Wrapper portion: ~3.15 KB

### Load Time (to observe)
- [ ] Initial page load time: _____ ms
- [ ] Chart initialization time: _____ ms
- [ ] Data fetch time (external): _____ ms
- [ ] Update time (dynamic): _____ ms

### Memory Usage (to observe)
- [ ] Initial heap size: _____ MB
- [ ] After 10 charts: _____ MB
- [ ] After navigation/disposal: _____ MB

---

## Automated Testing Checklist

### Console Errors ?
```javascript
// No errors should appear for:
- [ ] Chart initialization
- [ ] Data fetching
- [ ] Chart updates
- [ ] Chart disposal
- [ ] Theme application
- [ ] Event handling
```

### Network Requests ?
```
Status | Method | URL                           | Size
-------|--------|-------------------------------|------
200    | GET    | /data/sunburst_simple.json    | ~XX KB
200    | GET    | /data/les-miserables.json     | ~XX KB
200    | GET    | /data/sankey_simple.json      | ~XX KB
```

### DOM Elements ?
```javascript
// Verify chart containers exist
document.querySelectorAll('[id^="chart"]').length > 0 // true
```

---

## Completion Criteria

### Phase 5.1: Core Functionality
- [ ] echarts.init() works with 6.0
- [ ] chart.setOption() applies options correctly
- [ ] echarts.registerMap() works for geo charts
- [ ] Loading animations display
- [ ] Charts dispose cleanly

### Phase 5.2: External Data Sources
- [ ] fetchExternalData() fetches data
- [ ] evaluatePath() extracts nested data
- [ ] afterLoad callbacks execute
- [ ] Data sources cached and cleaned up

### Phase 5.3: Advanced Features
- [ ] Dynamic updates work (UpdateAsync)
- [ ] Themes apply (if used)
- [ ] Charts resize responsively
- [ ] Events fire correctly

### Overall
- [ ] No JavaScript console errors
- [ ] All sample charts render correctly
- [ ] Performance acceptable
- [ ] Memory management working

---

## Manual Testing Instructions

### Quick Test Procedure (15 minutes)

1. **Start Demo** (2 min)
   ```bash
   cd C:\Users\david\Projects\PanoramicData.ECharts\src\PanoramicData.ECharts.Demo
   dotnet run --configuration Release
   ```

2. **Basic Verification** (3 min)
   - Open http://localhost:5000
   - Check browser console (F12) - should be no errors
   - Verify `window.panoramicDataECharts` exists
   - Verify `echarts.version` is "6.0.0"

3. **Test Core Charts** (5 min)
   - Navigate to 3-4 different chart types
   - Verify each renders correctly
   - Check tooltips appear on hover
   - Check legends are interactive

4. **Test Advanced Features** (3 min)
   - Open ParameterSetSampleChart (tests UpdateAsync)
   - Wait 3 seconds, verify chart updates
   - Open SankeyWithLevelsChart (tests external data)
   - Verify Sankey diagram renders

5. **Check Console** (2 min)
   ```javascript
   console.log('ECharts version:', echarts.version); // 6.0.0
   console.log('Global object:', window.panoramicDataECharts);
   console.log('Active charts:', window.panoramicDataECharts.charts.size);
   console.log('Old object (should be undefined):', window.vizorECharts);
   ```

### Comprehensive Test Procedure (1-2 hours)

Follow all checklists above for each chart type and feature.

---

## Test Results Template

```markdown
### Test Results

**Date**: [Date]
**Tester**: [Name]
**Browser**: [Browser] [Version]
**ECharts Version**: 6.0.0

#### Core Functionality
- [ ] echarts.init() - PASS/FAIL
- [ ] chart.setOption() - PASS/FAIL
- [ ] echarts.registerMap() - PASS/FAIL
- [ ] Loading animations - PASS/FAIL
- [ ] Chart disposal - PASS/FAIL

#### External Data
- [ ] fetchExternalData() - PASS/FAIL
- [ ] evaluatePath() - PASS/FAIL
- [ ] afterLoad - PASS/FAIL
- [ ] Data caching - PASS/FAIL

#### Advanced Features
- [ ] Dynamic updates - PASS/FAIL
- [ ] Themes - PASS/FAIL
- [ ] Responsive - PASS/FAIL
- [ ] Events - PASS/FAIL

#### Issues Found
1. [Description]
2. [Description]

#### Notes
[Additional observations]
```

---

## Next Steps After Testing

### If All Tests Pass ?
- Mark Phase 5 as complete
- Proceed to Phase 6: Update and Test Sample Charts
- Document any minor issues for future improvement

### If Issues Found ??
1. Document each issue in detail
2. Categorize severity (Critical/High/Medium/Low)
3. Fix critical issues before proceeding
4. Medium/Low issues can be tracked for later

### Critical Issues (Must Fix)
- Charts not rendering at all
- JavaScript errors on page load
- Data fetching failures
- Memory leaks

### Non-Critical Issues (Can Defer)
- Minor visual differences
- Performance optimizations
- Edge case bugs

---

## Appendix A: Browser Compatibility Targets

| Browser | Version | Priority | Notes |
|---------|---------|----------|-------|
| Chrome | Latest (120+) | HIGH | Primary development browser |
| Edge | Latest (120+) | HIGH | Chromium-based |
| Firefox | Latest (120+) | MEDIUM | Different rendering engine |
| Safari | Latest (17+) | LOW | If available |

**Note**: ECharts 6.0 drops IE11 support (documented in Phase 1)

---

## Appendix B: Debugging Tips

### Enable Verbose Logging
```javascript
window.panoramicDataECharts.changeLogging(true);
// Now all fetch/init operations will log to console
```

### Inspect Chart State
```javascript
let chartId = document.querySelector('[id^="chart"]').id;
let chart = window.panoramicDataECharts.getChart(chartId);
console.log('Chart options:', chart.getOption());
console.log('Chart dimensions:', chart.getWidth(), chart.getHeight());
```

### Monitor Chart Lifecycle
```javascript
// Wrap methods to log calls
let original = window.panoramicDataECharts.initChart;
window.panoramicDataECharts.initChart = function(...args) {
  console.log('initChart called with:', args[0]); // chart ID
  return original.apply(this, args);
};
```

---

**Phase 5 Status**: ? **MANUAL TESTING REQUIRED**  
**Build Ready**: ? **YES**  
**Test Environment**: Ready  
**Estimated Testing Time**: 15 minutes (quick) to 2 hours (comprehensive)

---

**Document Created**: 2025-01-27  
**Phase**: 5 of 15 (JavaScript Interop Testing)  
**Next Phase**: Phase 6 - Update and Test Sample Charts  
**Instructions**: Follow manual testing procedures above
