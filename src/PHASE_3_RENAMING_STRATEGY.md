# Phase 3 Addendum: Renaming Strategy
## vizor-echarts ? panoramicdata-echarts

**Purpose**: Align JavaScript naming with project branding (PanoramicData.ECharts)  
**Scope**: Phase 3.2 - Comprehensive renaming across all files and references  
**Impact**: Breaking change for consumers (requires documentation)

---

## Renaming Checklist

### 1. Source Files

#### JavaScript Source
- [ ] **Rename**: `PanoramicData.ECharts\Scripts\vizor-echarts.js`
  - **To**: `PanoramicData.ECharts\Scripts\panoramicdata-echarts.js`
  - **Action**: Use `git mv` to preserve history

#### Global Object in JavaScript
- [ ] **Update**: `window.vizorECharts` ? `window.panoramicDataECharts`
  - **File**: `Scripts\panoramicdata-echarts.js`
  - **Lines**: All references (approx. 30+ occurrences)
  - **Pattern**: 
    ```javascript
    // OLD
    window.vizorECharts = { ... }
    vizorECharts.charts.get(id)
    
    // NEW
    window.panoramicDataECharts = { ... }
    panoramicDataECharts.charts.get(id)
    ```

---

### 2. Build Configuration

#### Gulp Configuration
- [ ] **File**: `PanoramicData.ECharts\gulpfile.js`
  - [ ] Update `srcPaths.js` array:
    ```javascript
    // OLD
    path.resolve(vizorScripts, 'vizor-echarts.js')
    
    // NEW
    path.resolve(vizorScripts, 'panoramicdata-echarts.js')
    ```
  - [ ] Update `srcPaths.jsBundle` array:
    ```javascript
    // OLD
    path.resolve(vizorScripts, 'vizor-echarts.js')
    
    // NEW
    path.resolve(vizorScripts, 'panoramicdata-echarts.js')
    ```
  - [ ] Update gulp tasks output names:
    ```javascript
    // Task: buildJs
    .pipe(concat('panoramicdata-echarts.js'))  // was: vizor-echarts.js
    
    // Task: buildJsBundle
    .pipe(concat('panoramicdata-echarts-bundle.js'))  // was: vizor-echarts-bundle.js
    ```

#### NPM Package
- [ ] **File**: `PanoramicData.ECharts\package.json`
  - [ ] Update `name` field (optional, currently "vizor-echarts"):
    ```json
    {
      "name": "panoramicdata-echarts",  // was: "vizor-echarts"
      "private": true,
      // ...
    }
    ```

---

### 3. C# Interop Layer

#### Search and Replace Pattern
**Pattern**: `vizorECharts` ? `panoramicDataECharts`

#### Files to Check and Update:

- [ ] **EChart.razor.cs** (main component)
  - JavaScript interop calls
  - JSRuntime invocations
  - Example:
    ```csharp
    // OLD
    await JSRuntime.InvokeVoidAsync("vizorECharts.initChart", ...)
    
    // NEW
    await JSRuntime.InvokeVoidAsync("panoramicDataECharts.initChart", ...)
    ```

- [ ] **Search all .cs files** for "vizorECharts":
  ```powershell
  Get-ChildItem -Recurse -Include *.cs | Select-String "vizorECharts"
  ```

- [ ] **Search all .razor files** for "vizorECharts":
  ```powershell
  Get-ChildItem -Recurse -Include *.razor | Select-String "vizorECharts"
  ```

#### Potential Locations:
- `PanoramicData.ECharts\Components\EChart.razor`
- `PanoramicData.ECharts\Components\EChart.razor.cs`
- Any custom JSInterop wrappers

---

### 4. Output Files (wwwroot)

#### Before Renaming - Backup
- [ ] **Backup existing files** in `PanoramicData.ECharts\wwwroot\js\`:
  ```
  vizor-echarts.js
  vizor-echarts-min.js
  vizor-echarts-bundle.js
  vizor-echarts-bundle-min.js
  ```
  - Copy to: `wwwroot\js\backup\` (temporary)

#### After Gulp Build - Expected Output
- [ ] **Verify new files** created:
  ```
  panoramicdata-echarts.js
  panoramicdata-echarts-min.js
  panoramicdata-echarts-bundle.js
  panoramicdata-echarts-bundle-min.js
  ```

#### Clean Up
- [ ] **Delete old files** (after verification):
  ```
  rm vizor-echarts.js
  rm vizor-echarts-min.js
  rm vizor-echarts-bundle.js
  rm vizor-echarts-bundle-min.js
  ```

---

### 5. Demo Application

#### _Host.cshtml / App.razor
- [ ] **File**: `PanoramicData.ECharts.Demo\Pages\_Host.cshtml`
  - [ ] Update script tag:
    ```html
    <!-- OLD -->
    <script src="_content/PanoramicData.ECharts/js/vizor-echarts-bundle-min.js"></script>
    
    <!-- NEW -->
    <script src="_content/PanoramicData.ECharts/js/panoramicdata-echarts-bundle-min.js"></script>
    ```

- [ ] **File**: `PanoramicData.ECharts.Demo\App.razor` (if applicable)
  - Check for any script references

#### Manual Test
- [ ] Run demo application: `dotnet run`
- [ ] Open browser console (F12)
- [ ] Verify:
  ```javascript
  // Should exist:
  window.panoramicDataECharts
  
  // Should NOT exist:
  window.vizorECharts
  ```
- [ ] Test a sample chart renders correctly

---

### 6. Samples Application

#### _Host.cshtml
- [ ] **File**: `PanoramicData.ECharts.Samples\Pages\_Host.cshtml`
  - [ ] Update script tag (same as demo)

#### Test All Sample Charts
After renaming, verify these critical samples:
- [ ] SimplePieChart.razor
- [ ] SankeyWithLevelsChart.razor
- [ ] ParameterSetSampleChart.razor
- [ ] ForceLayoutGraphChart.razor (external data)
- [ ] TempGaugeChart.razor (dynamic updates)

---

### 7. Documentation Updates

#### README.md
- [ ] **File**: `README.md`
  - [ ] Update "How to include" section:
    ```markdown
    ## How to include
    
    1. Add a package reference to `PanoramicData.ECharts`
    2. Add script to your `_Host.cshtml` or `_Layout.cshtml` file
        - `panoramicdata-echarts-bundle-min.js` includes apache echarts and echarts-stat.
        - `panoramicdata-echarts-min.js` ONLY contains the binding code.
        
    ```html
    <script src="_content/PanoramicData.ECharts/js/panoramicdata-echarts-bundle-min.js"></script>
    ```
    ```

  - [ ] Update any code examples referencing old names
  - [ ] Add migration note for existing users

#### Migration Guide Section
- [ ] **Add to README.md**:
    ```markdown
    ## Migration from vizor-echarts naming
    
    If upgrading from a previous version that used `vizor-echarts` naming:
    
    **Breaking Change**: JavaScript files have been renamed to align with PanoramicData branding.
    
    **Action Required**:
    1. Update script references in your `_Host.cshtml` or `_Layout.cshtml`:
       ```html
       <!-- OLD -->
       <script src="_content/PanoramicData.ECharts/js/vizor-echarts-bundle-min.js"></script>
       
       <!-- NEW -->
       <script src="_content/PanoramicData.ECharts/js/panoramicdata-echarts-bundle-min.js"></script>
       ```
    
    2. If you have custom JavaScript that references the global object:
       ```javascript
       // OLD
       window.vizorECharts.getChart(id)
       
       // NEW
       window.panoramicDataECharts.getChart(id)
       ```
    
    **Note**: The C# API remains unchanged. Only JavaScript file names and global object are affected.
    ```

#### XML Documentation Comments
- [ ] Search for "vizor" in XML comments:
  ```powershell
  Get-ChildItem -Recurse -Include *.cs | Select-String "vizor" -CaseSensitive
  ```
- [ ] Update any references in `<remarks>` or `<example>` tags

#### Code Comments
- [ ] **File**: `panoramicdata-echarts.js`
  - [ ] Update header comment:
    ```javascript
    /**
     * PanoramicData.ECharts - Blazor wrapper for Apache ECharts
     * JavaScript interop layer
     */
    window.panoramicDataECharts = { ... }
    ```

---

### 8. Project Files and Configuration

#### .csproj Files
- [ ] **Check**: `PanoramicData.ECharts\PanoramicData.ECharts.csproj`
  - Look for any embedded resource paths referencing old names
  - Verify `<Content>` or `<EmbeddedResource>` tags

#### NuGet Package Spec
- [ ] **If exists**: `PanoramicData.ECharts.nuspec`
  - Update file references
  - Update description if it mentions "vizor"

---

### 9. Testing and Validation

#### Unit Tests (if applicable)
- [ ] **Search test files** for "vizor":
  ```powershell
  Get-ChildItem -Recurse -Include *Test*.cs | Select-String "vizor"
  ```
- [ ] Update any test references

#### Integration Tests
- [ ] Test chart initialization
- [ ] Test chart updates
- [ ] Test external data loading
- [ ] Test JavaScript functions
- [ ] Test map registration

#### Browser Console Validation
Run in browser console after loading demo:
```javascript
// Verify new global exists
console.log(window.panoramicDataECharts);  // Should be object

// Verify old global does NOT exist
console.log(window.vizorECharts);  // Should be undefined

// Test chart retrieval (if chart exists on page)
const chart = window.panoramicDataECharts.getChart('chart-id');
console.log(chart);  // Should return ECharts instance
```

---

### 10. Git Workflow

#### Renaming with Git
Use `git mv` to preserve file history:

```bash
cd C:\Users\david\Projects\PanoramicData.ECharts\src\PanoramicData.ECharts

# Rename source file
git mv Scripts\vizor-echarts.js Scripts\panoramicdata-echarts.js

# Stage other changes
git add gulpfile.js
git add wwwroot/js/*.js
git add README.md

# Commit with clear message
git commit -m "refactor: Rename vizor-echarts to panoramicdata-echarts

- Rename JavaScript source and output files
- Update global object: vizorECharts ? panoramicDataECharts
- Update gulpfile.js build configuration
- Update all script references in demo and samples
- Update documentation

BREAKING CHANGE: JavaScript file names and global object renamed.
See README.md migration guide for upgrade instructions.

Ref: MASTER_PLAN.md Phase 3.2"
```

#### Branch Strategy
- [ ] Ensure on correct branch: `feature/echarts-6.0-upgrade`
- [ ] Commit renaming separately from ECharts upgrade (for clarity)
- [ ] Consider creating sub-branch: `feature/echarts-6.0-upgrade-rename`

---

### 11. Rollback Plan

If renaming causes issues:

#### Quick Rollback
```bash
# Revert commit
git revert HEAD

# Or reset if not pushed
git reset --hard HEAD~1

# Rebuild with old names
gulp clean && gulp
```

#### Keep Both Versions (Temporary)
During migration period, could support both:
```javascript
// Backward compatibility alias (temporary)
window.vizorECharts = window.panoramicDataECharts;
```

**Note**: This is NOT recommended for long-term, only for gradual migration.

---

## Estimated Impact

### Breaking Changes for Consumers

| Impact Area | Severity | Mitigation |
|-------------|----------|------------|
| Script tag in HTML | **HIGH** | Clear migration guide |
| Custom JavaScript | **MEDIUM** | Global object documented |
| C# API | **NONE** | No changes to C# |
| NuGet package | **LOW** | File paths in _content |

### Affected User Count

**Estimate**: All users who reference script files directly
**Notification**: Release notes, README, migration guide

---

## Timeline

| Task | Estimated Time |
|------|---------------|
| Rename source file | 2 min |
| Update JavaScript global | 10 min |
| Update gulpfile.js | 5 min |
| Rebuild bundles | 2 min |
| Update C# interop | 15 min |
| Update demo/samples | 10 min |
| Update documentation | 30 min |
| Testing | 30 min |
| **Total** | **~2 hours** |

---

## Success Criteria

- [x] All source files renamed
- [x] Gulp builds successfully with new names
- [x] Output files created with new names
- [x] Demo application runs without errors
- [x] Sample charts render correctly
- [x] Browser console shows new global object
- [x] Old global object does not exist
- [x] Documentation updated
- [x] Migration guide provided
- [x] Git history preserved

---

**Status**: Ready for execution in Phase 3.2  
**Priority**: HIGH (breaking change, must be done before release)  
**Risk**: MEDIUM (affects all consumers, but straightforward to fix)
