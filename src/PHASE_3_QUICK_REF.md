# Phase 3 Quick Reference Card

## Overview
**Goal**: Rebuild JavaScript assets with ECharts 6.0.0 + Rename to PanoramicData branding  
**Time**: ~2-3 hours  
**Risk**: Medium (breaking change for consumers)

---

## Quick Commands

### 1. Verify Prerequisites
```powershell
cd C:\Users\david\Projects\PanoramicData.ECharts\src\PanoramicData.ECharts

# Check Gulp installed
gulp --version

# Check node_modules
npm list echarts echarts-stat
```

**Expected**:
- Gulp CLI: 2.x
- echarts: 6.0.0
- echarts-stat: 1.2.0

---

### 2. Backup Current Assets
```powershell
# Backup current wwwroot/js files
$backupDir = "wwwroot\js\backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
New-Item -ItemType Directory -Path $backupDir
Copy-Item wwwroot\js\*.js $backupDir
```

---

### 3. Rename Source File
```bash
# Use git mv to preserve history
git mv Scripts\vizor-echarts.js Scripts\panoramicdata-echarts.js
```

---

### 4. Update JavaScript Global Object

**File**: `Scripts\panoramicdata-echarts.js`

**Search & Replace**:
- Find: `vizorECharts`
- Replace: `panoramicDataECharts`
- Count: ~30 occurrences

**Critical Lines**:
```javascript
// Line 1: Global object declaration
window.panoramicDataECharts = {

// All internal references
panoramicDataECharts.charts.get(id)
panoramicDataECharts.dataSources.set(...)
panoramicDataECharts.logging
// etc.
```

---

### 5. Update gulpfile.js

**File**: `gulpfile.js`

**Changes**:
```javascript
// Line ~30: Source path
var srcPaths = {
	js: [
		path.resolve(vizorScripts, 'panoramicdata-echarts.js'),  // CHANGED
	],
	jsBundle: [
		path.resolve(libroot, 'echarts/dist/echarts.min.js'),
		path.resolve(libroot, 'echarts-stat/dist/ecStat.min.js'),
		path.resolve(vizorScripts, 'panoramicdata-echarts.js'),  // CHANGED
	]
};

// Line ~52: Output name
gulp.task('buildJs', () => {
	return gulp.src(srcPaths.js)
		.pipe(concat('panoramicdata-echarts.js'))  // CHANGED
		.pipe(minify())
		.pipe(gulp.dest(destPaths.js))
});

// Line ~58: Bundle output name
gulp.task('buildJsBundle', () => {
	return gulp.src(srcPaths.jsBundle)
		.pipe(concat('panoramicdata-echarts-bundle.js'))  // CHANGED
		.pipe(minify())
		.pipe(gulp.dest(destPaths.js))
});
```

---

### 6. Run Gulp Build
```powershell
# Clean old files
gulp clean

# Build with new names
gulp

# Verify output
ls wwwroot\js\*.js
```

**Expected Output**:
```
panoramicdata-echarts.js
panoramicdata-echarts-min.js
panoramicdata-echarts-bundle.js
panoramicdata-echarts-bundle-min.js
```

---

### 7. Update C# Interop References

**Search Pattern**: `vizorECharts`

**Files to Update**:
```powershell
# Find all .cs files with references
Get-ChildItem -Recurse -Include *.cs | Select-String "vizorECharts" -List

# Find all .razor files
Get-ChildItem -Recurse -Include *.razor | Select-String "vizorECharts" -List
```

**Replace With**: `panoramicDataECharts`

**Common Locations**:
- `PanoramicData.ECharts\Components\EChart.razor.cs`
- JavaScript interop calls

---

### 8. Update Demo & Samples

**Files**:
- `PanoramicData.ECharts.Demo\Pages\_Host.cshtml`
- `PanoramicData.ECharts.Samples\Pages\_Host.cshtml`

**Change**:
```html
<!-- OLD -->
<script src="_content/PanoramicData.ECharts/js/vizor-echarts-bundle-min.js"></script>

<!-- NEW -->
<script src="_content/PanoramicData.ECharts/js/panoramicdata-echarts-bundle-min.js"></script>
```

---

### 9. Test Demo Application
```powershell
cd ..\PanoramicData.ECharts.Demo
dotnet run
```

**Browser Tests**:
1. Open: http://localhost:5000
2. Open Console (F12)
3. Verify:
   ```javascript
   window.panoramicDataECharts  // Should exist
   window.vizorECharts          // Should be undefined
   ```
4. Navigate through chart examples
5. Check for console errors

---

### 10. Update Documentation

**README.md**:
- Update script tag examples
- Add migration guide section
- Update version from 5.4.3 to 6.0.0

**Key Section**:
```markdown
## How to include

<script src="_content/PanoramicData.ECharts/js/panoramicdata-echarts-bundle-min.js"></script>

## Migration from Previous Versions

**Breaking Change in v2.0**: Script files renamed from `vizor-echarts` to `panoramicdata-echarts`

Update your script reference as shown above.
```

---

### 11. Git Commit
```bash
git add .
git status  # Review changes

git commit -m "refactor: Rename vizor-echarts to panoramicdata-echarts + upgrade to ECharts 6.0.0

- Rename JavaScript source and output files
- Update global object: vizorECharts ? panoramicDataECharts
- Update gulpfile.js build configuration
- Rebuild bundles with ECharts 6.0.0
- Update all script references in demo and samples
- Update documentation and add migration guide

BREAKING CHANGE: JavaScript file names and global object renamed.
See README.md for migration instructions.

Phase: 3 of 15 (JavaScript Assets Rebuilt)
Ref: MASTER_PLAN.md, PHASE_3_RENAMING_STRATEGY.md"
```

---

## Verification Checklist

### Build Verification
- [ ] `gulp clean` runs without errors
- [ ] `gulp` runs without errors
- [ ] 4 output files created in `wwwroot/js/`
- [ ] File sizes reasonable (bundle ~1-2MB)

### Code Verification
- [ ] No "vizorECharts" in C# files
- [ ] No "vizor-echarts" in script tags
- [ ] No old files in `wwwroot/js/`

### Runtime Verification
- [ ] Demo app starts
- [ ] Charts render correctly
- [ ] Console shows no errors
- [ ] `window.panoramicDataECharts` exists
- [ ] `window.vizorECharts` is undefined

### Documentation Verification
- [ ] README.md updated
- [ ] Migration guide present
- [ ] Version 6.0.0 mentioned
- [ ] Script examples use new names

---

## Troubleshooting

### Gulp Fails
```powershell
# Check node_modules
Test-Path node_modules

# Reinstall if missing
npm install

# Check Gulp globally
npm list -g gulp-cli

# Install if missing
npm install -g gulp-cli
```

### Files Not Generated
```powershell
# Check gulpfile.js syntax
gulp --tasks

# Check for errors in source
Get-Content Scripts\panoramicdata-echarts.js | Select-String "syntax"
```

### Charts Don't Render
1. Check browser console for errors
2. Verify script tag path
3. Check network tab for 404s
4. Verify global object exists

---

## Bundle Size Reference

### Expected Sizes (ECharts 6.0.0)
- `panoramicdata-echarts-bundle-min.js`: ~1.2 MB
- `panoramicdata-echarts-bundle.js`: ~4 MB
- `panoramicdata-echarts-min.js`: ~5 KB
- `panoramicdata-echarts.js`: ~8 KB

**Note**: Sizes may vary, these are estimates.

---

## Rollback Commands

If issues arise:
```bash
# Revert git changes
git reset --hard HEAD

# Restore backup files
$latest = Get-ChildItem wwwroot\js\backup_* | Sort-Object LastWriteTime -Descending | Select-Object -First 1
Copy-Item "$latest\*.js" wwwroot\js\

# Rebuild with old version
npm install echarts@5.4.3
gulp clean && gulp
```

---

## Next Phase

After Phase 3 complete:
- **Phase 4**: Update C# Bindings (if needed)
- **Phase 5**: Test JavaScript Interop
- **Phase 6**: Test Sample Charts

---

**Document**: PHASE_3_QUICK_REF.md  
**Phase**: 3 of 15  
**Status**: Ready to execute  
**Full Details**: See PHASE_3_RENAMING_STRATEGY.md
