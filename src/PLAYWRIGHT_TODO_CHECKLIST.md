# Playwright E2E Testing - User Action Checklist

## ? Completed (Already Done)

- [x] Test project created and configured
- [x] Playwright NuGet packages installed
- [x] TestBase infrastructure implemented
- [x] ChartTests.cs created (7 basic tests)
- [x] AllChartsTests.cs created (47+ comprehensive tests)
- [x] Compilation errors fixed
- [x] Build verified successful
- [x] README.md documentation created
- [x] setup-playwright.ps1 script created
- [x] run-tests.ps1 automation script created

---

## ?? Todo (Your Action Required)

### Step 1: Install Playwright Browsers (~5 minutes)

```powershell
# Navigate to test directory
cd PanoramicData.ECharts.Test

# Run setup script
.\setup-playwright.ps1
```

**Expected Output:**
```
Building test project...
? Build successful

Installing Playwright browsers...
This may take a few minutes on first run...
? Browsers installed successfully

Installation Complete!
```

**First Time Only**: This downloads Chromium, Firefox, and WebKit browsers (~500MB)

---

### Step 2: Run Tests Using Automation Script (~3 minutes)

**Option A: Automated (Recommended)**

```powershell
# From the src directory
.\run-tests.ps1
```

This script will:
1. ? Start the demo application automatically
2. ? Wait for it to be ready
3. ? Run all tests
4. ? Stop the demo application
5. ? Show you the results

**Expected Output:**
```
Step 1: Starting demo application...
? Demo application starting

Step 2: Waiting for demo application to be ready...
? Demo application is ready at http://localhost:5185

Step 3: Running Playwright tests...
Passed!  - Failed:     0, Passed:    54, Skipped:     0
Time: 2m 30s

Step 4: Stopping demo application...
? Demo application stopped

Tests Passed! ?
```

**Option B: Manual (Two Terminals)**

Terminal 1 - Start Demo:
```powershell
cd PanoramicData.ECharts.Demo
dotnet run
# Keep this running
```

Terminal 2 - Run Tests:
```powershell
cd PanoramicData.ECharts.Test
dotnet test
```

---

### Step 3: Review Results

**Console Output:**
Look for: `Passed!  - Failed: 0, Passed: 54`

**Screenshots:**
```powershell
# View screenshots folder
cd PanoramicData.ECharts.Test\bin\Debug\net10.0\screenshots
explorer .
```

You should see 47+ PNG files like:
- `pie-simple.png`
- `line-simple.png`
- `bar-dataset.png`
- `sunburst-simple.png`
- `sankey-levels.png`
- etc.

---

## ?? Quick Commands Reference

### First Time Setup
```powershell
cd PanoramicData.ECharts.Test
.\setup-playwright.ps1
```

### Run All Tests (Automated)
```powershell
cd src
.\run-tests.ps1
```

### Run All Tests (Manual)
```powershell
# Terminal 1
cd PanoramicData.ECharts.Demo
dotnet run

# Terminal 2
cd PanoramicData.ECharts.Test
dotnet test
```

### Run Specific Tests
```powershell
cd PanoramicData.ECharts.Test

# Basic tests only
dotnet test --filter "FullyQualifiedName~ChartTests"

# All charts comprehensive
dotnet test --filter "FullyQualifiedName~AllChartsTests"

# Single test
dotnet test --filter "SimplePieChart_Renders"
```

### View Detailed Output
```powershell
dotnet test --logger "console;verbosity=detailed"
```

---

## ?? What Gets Tested

### All Chart Types (47+ tests)
- ? Line (3): Simple, Smooth, Stacked
- ? Bar (4): Simple, Horizontal Stacked, Dataset, Time Series
- ? Pie (3): Simple, Half Doughnut, Rounded
- ? Scatter (3): Simple, Punchcard, Clustered
- ? Geo/Map (3): USA, Belgian, Flight Seats
- ? Heatmap (4): Simple, Year, Multi-Year, Visits
- ? Advanced (20+): Candlestick, Radar, Graph, Tree, Treemap, Sunburst, etc.
- ? Special Features (6): Data Loader, Parameter Set, Refresh, Multi-Axis, etc.

### Verifications Per Chart
- ? Chart canvas is visible
- ? ECharts version is 6.0.0
- ? Global object is `window.panoramicDataECharts`
- ? No JavaScript console errors
- ? Screenshot captured

---

## ?? Troubleshooting

### Issue: Browser installation fails
```powershell
# Try manual installation
cd PanoramicData.ECharts.Test
dotnet build
pwsh bin\Debug\net10.0\playwright.ps1 install
```

### Issue: Demo runs on different port
Edit `PanoramicData.ECharts.Test\TestBase.cs`:
```csharp
protected const string BaseUrl = "http://localhost:YOUR_PORT";
```

### Issue: Tests timeout
Edit `PanoramicData.ECharts.Test\TestBase.cs`:
```csharp
protected const int DefaultTimeout = 60000; // Increase to 60 seconds
```

### Issue: Can't find screenshots
Check: `PanoramicData.ECharts.Test\bin\Debug\net10.0\screenshots\`

---

## ?? Expected Timeline

| Task | Time | Status |
|------|------|--------|
| Install browsers (first time) | 5 min | ? Pending |
| Run automated tests | 3 min | ? Pending |
| Review screenshots | 2 min | ? Pending |
| **Total** | **~10 min** | |

---

## ? Success Criteria

You'll know it's working when:

1. ? Browser installation completes without errors
2. ? All 54 tests pass: `Passed: 54, Failed: 0`
3. ? Screenshots folder contains 47+ PNG files
4. ? No console errors reported
5. ? All charts render correctly in screenshots

---

## ?? Once Complete

When all tests pass, you have:

? **Automated regression testing** for all chart types  
? **ECharts 6.0.0 verification** on every test run  
? **Screenshot documentation** of all charts  
? **CI/CD ready** test suite  
? **3-minute validation** instead of 2 hours manual testing  

---

## ?? Documentation Files

- `README.md` - Complete testing guide
- `PLAYWRIGHT_COMPLETION.md` - Implementation summary
- `PHASE_6_PLAYWRIGHT_AUTOMATION.md` - Original automation guide
- This checklist

---

## ?? Next Steps After Success

1. **Commit the tests** to version control
2. **Add to CI/CD pipeline** (see README.md for GitHub Actions example)
3. **Run on every code change** to catch regressions early
4. **Expand test coverage** as needed (interactions, mobile, etc.)

---

## Need Help?

- Check `README.md` in the test directory
- Review `PLAYWRIGHT_COMPLETION.md` for detailed changes
- Consult Playwright docs: https://playwright.dev/dotnet/

---

**Ready to start?**

```powershell
cd PanoramicData.ECharts.Test
.\setup-playwright.ps1
```

Then:

```powershell
cd ..
.\run-tests.ps1
```

**Good luck! ??**
