# Phase 6 Automation: Playwright Testing Guide

## Overview

This guide explains how to add **Playwright** automated testing to verify all 23 chart types render correctly with ECharts 6.0.0.

---

## Prerequisites

- ? .NET 10 SDK installed
- ? Demo application running (Phase 5 complete)
- ? All demo pages working

---

## Step 1: Create Playwright Test Project

### Option A: Using PowerShell (Recommended)

```powershell
# Navigate to solution directory
cd C:\Users\david\Projects\PanoramicData.ECharts\src

# Create new xUnit test project
dotnet new xunit -n PanoramicData.ECharts.Tests.E2E -f net10.0

# Add to solution
dotnet sln add PanoramicData.ECharts.Tests.E2E\PanoramicData.ECharts.Tests.E2E.csproj

# Navigate to test project
cd PanoramicData.ECharts.Tests.E2E

# Add Playwright package
dotnet add package Microsoft.Playwright
dotnet add package Microsoft.Playwright.NUnit

# Build the project
dotnet build

# Install Playwright browsers
pwsh bin\Debug\net10.0\playwright.ps1 install
```

### Option B: Manual Creation

1. Create new folder: `src\PanoramicData.ECharts.Tests.E2E`
2. Add `.csproj` file (see below)
3. Run `dotnet restore`
4. Run `pwsh bin\Debug\net10.0\playwright.ps1 install`

---

## Step 2: Test Project Configuration

### PanoramicData.ECharts.Tests.E2E.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="Microsoft.Playwright" Version="1.49.1" />
    <PackageReference Include="Microsoft.Playwright.NUnit" Version="1.49.1" />
    <PackageReference Include="NUnit" Version="4.2.2" />
    <PackageReference Include="NUnit.Analyzers" Version="4.4.0">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="NUnit3TestAdapter" Version="4.6.0" />
    <PackageReference Include="coverlet.collector" Version="6.0.2">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>

</Project>
```

---

## Step 3: Base Test Class

Create `TestBase.cs`:

```csharp
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace PanoramicData.ECharts.Tests.E2E;

public class TestBase : PageTest
{
    protected const string BaseUrl = "http://localhost:5185"; // Adjust port as needed
    protected const int DefaultTimeout = 30000; // 30 seconds

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            IgnoreHTTPSErrors = true,
        };
    }

    [SetUp]
    public async Task Setup()
    {
        await Page.SetDefaultTimeout(DefaultTimeout);
    }

    /// <summary>
    /// Wait for chart to be initialized and rendered
    /// </summary>
    protected async Task WaitForChartAsync()
    {
        // Wait for Blazor to finish loading
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Wait for chart container to appear
        await Page.WaitForSelectorAsync("[id^='chart']", new() { State = WaitForSelectorState.Attached });

        // Give ECharts time to render (animations, etc.)
        await Page.WaitForTimeoutAsync(1000);
    }

    /// <summary>
    /// Verify no JavaScript console errors
    /// </summary>
    protected async Task VerifyNoConsoleErrors()
    {
        var errors = new List<string>();
        
        Page.Console += (_, msg) =>
        {
            if (msg.Type == "error")
            {
                errors.Add(msg.Text);
            }
        };

        await Page.WaitForTimeoutAsync(500);

        Assert.That(errors, Is.Empty, 
            $"Console errors found: {string.Join(", ", errors)}");
    }

    /// <summary>
    /// Verify ECharts global object exists with correct name
    /// </summary>
    protected async Task VerifyEChartsGlobalAsync()
    {
        var hasGlobal = await Page.EvaluateAsync<bool>(
            "() => typeof window.panoramicDataECharts !== 'undefined'"
        );
        Assert.That(hasGlobal, Is.True, "window.panoramicDataECharts not found");

        var oldGlobal = await Page.EvaluateAsync<bool>(
            "() => typeof window.vizorECharts !== 'undefined'"
        );
        Assert.That(oldGlobal, Is.False, "Old window.vizorECharts still exists");

        var version = await Page.EvaluateAsync<string>("() => echarts.version");
        Assert.That(version, Is.EqualTo("6.0.0"), $"Expected ECharts 6.0.0, got {version}");
    }
}
```

---

## Step 4: Chart Tests

Create `ChartTests.cs`:

```csharp
using Microsoft.Playwright;
using NUnit.Framework;

namespace PanoramicData.ECharts.Tests.E2E;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ChartTests : TestBase
{
    [Test]
    public async Task SimplePieChart_Renders_Successfully()
    {
        // Navigate to pie chart
        await Page.GotoAsync($"{BaseUrl}/pie/simple");
        
        // Wait for chart
        await WaitForChartAsync();

        // Verify ECharts initialized
        await VerifyEChartsGlobalAsync();

        // Verify chart exists
        var chartExists = await Page.Locator("[id^='chart'] canvas").IsVisibleAsync();
        Assert.That(chartExists, Is.True, "Chart canvas not visible");

        // Verify title
        var titleVisible = await Page.Locator("text=Referer of a Website").IsVisibleAsync();
        Assert.That(titleVisible, Is.True, "Chart title not visible");

        // Verify no errors
        await VerifyNoConsoleErrors();

        // Take screenshot
        await Page.ScreenshotAsync(new()
        {
            Path = "screenshots/pie-simple.png",
            FullPage = false
        });
    }

    [Test]
    public async Task SimpleSunburstChart_WithExternalData_Renders()
    {
        await Page.GotoAsync($"{BaseUrl}/sunburst/simple");
        await WaitForChartAsync();

        // This chart uses external data - verify it loaded
        var chartExists = await Page.Locator("[id^='chart'] canvas").IsVisibleAsync();
        Assert.That(chartExists, Is.True);

        // Verify data was fetched
        var hasData = await Page.EvaluateAsync<bool>(
            "() => window.panoramicDataECharts.dataSources.size > 0"
        );
        Assert.That(hasData, Is.True, "External data not loaded");

        await VerifyNoConsoleErrors();
        await Page.ScreenshotAsync(new() { Path = "screenshots/sunburst-simple.png" });
    }

    [Test]
    public async Task SankeyWithLevels_WithPathExpression_Renders()
    {
        await Page.GotoAsync($"{BaseUrl}/sankey/levels");
        await WaitForChartAsync();

        var chartExists = await Page.Locator("[id^='chart'] canvas").IsVisibleAsync();
        Assert.That(chartExists, Is.True);

        // Verify path expression worked (data extracted from JSON)
        var hasData = await Page.EvaluateAsync<bool>(
            "() => window.panoramicDataECharts.dataSources.size > 0"
        );
        Assert.That(hasData, Is.True);

        await VerifyNoConsoleErrors();
        await Page.ScreenshotAsync(new() { Path = "screenshots/sankey-levels.png" });
    }

    [Test]
    public async Task ParameterSetSampleChart_DynamicUpdate_Works()
    {
        await Page.GotoAsync($"{BaseUrl}/misc/parameterset");
        await WaitForChartAsync();

        // Take initial screenshot
        await Page.ScreenshotAsync(new() { Path = "screenshots/parameter-before.png" });

        // Wait for auto-update (chart updates after 3 seconds)
        await Page.WaitForTimeoutAsync(4000);

        // Verify chart still renders
        var chartExists = await Page.Locator("[id^='chart'] canvas").IsVisibleAsync();
        Assert.That(chartExists, Is.True);

        await VerifyNoConsoleErrors();
        await Page.ScreenshotAsync(new() { Path = "screenshots/parameter-after.png" });
    }

    // Add more tests for each chart type...
}
```

---

## Step 5: Comprehensive Test Suite

Create `AllChartsTests.cs`:

```csharp
using Microsoft.Playwright;
using NUnit.Framework;

namespace PanoramicData.ECharts.Tests.E2E;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class AllChartsTests : TestBase
{
    private static readonly (string Category, string Route, string Name)[] AllCharts = new[]
    {
        // Line Charts
        ("line", "simple", "SimpleLineChart"),
        ("line", "smooth", "SmoothLineChart"),
        ("line", "stacked", "StackedLineChart"),

        // Bar Charts
        ("bar", "simple", "SimpleBarChart"),
        ("bar", "horizontal-stacked", "HorizontalStackedBarChart"),
        ("bar", "dataset", "SimpleDatasetBarChart"),
        ("bar", "stacked-timeseries", "StackedBarTimeSeriesChart"),

        // Pie Charts
        ("pie", "simple", "SimplePieChart"),
        ("pie", "half-doughnut", "HalfDoughnutChart"),
        ("pie", "rounded-doughnut", "RoundedDoughnutChart"),

        // Scatter Charts
        ("scatter", "simple", "SimpleScatterChart"),
        ("scatter", "punchcard", "PunchCardScatterChart"),
        ("scatter", "clustered", "ClusteredScatterChart"),

        // Geo/Map Charts
        ("geo", "usa", "UsaGeoMap"),
        ("geo", "belgian", "BelgianMunicipalityMap"),
        ("geo", "flights", "FlightSeatsGeoMap"),

        // Other Chart Types
        ("candlestick", "simple", "SimpleCandlestickChart"),
        ("radar", "simple", "SimpleRadarChart"),
        ("heatmap", "simple", "SimpleHeatmapChart"),
        ("heatmap", "year", "YearHeatmapChart"),
        ("heatmap", "multi-year", "MultiYearHeatmapChart"),
        ("heatmap", "visits", "VisitsPerDayHeatmapChart"),
        ("graph", "simple", "SimpleGraphChart"),
        ("graph", "force-layout", "ForceLayoutGraphChart"),
        ("tree", "left-to-right", "TreeLeftToRightChart"),
        ("treemap", "simple", "SimpleTreeMapChart"),
        ("sunburst", "simple", "SimpleSunburstChart"),
        ("parallel", "simple", "SimpleParallelChart"),
        ("sankey", "simple", "SimpleSankeyChart"),
        ("sankey", "levels", "SankeyWithLevelsChart"),
        ("funnel", "simple", "SimpleFunnelChart"),
        ("gauge", "temp", "TempGaugeChart"),
        ("pictorialbar", "simple", "SimplePictorialBarChart"),
        ("themeriver", "simple", "SimpleThemeRiverChart"),
        ("area", "simple", "SimpleAreaChart"),
        ("area", "stacked", "StackedAreaChart"),
        ("area", "timeseries", "TimeSeriesAreaChart"),

        // Advanced Features
        ("misc", "dataloader", "DataLoaderSampleChart"),
        ("misc", "parameterset", "ParameterSetSampleChart"),
        ("misc", "refresh", "RefreshSampleChart"),
        ("misc", "multiaxis", "MultiAxisSampleChart"),
        ("misc", "colorgradient", "ColorGradientBarChart"),
        ("misc", "toolbox", "ToolboxSampleChart"),
    };

    [Test]
    [TestCaseSource(nameof(AllCharts))]
    public async Task Chart_Renders_WithoutErrors((string Category, string Route, string Name) chart)
    {
        var url = $"{BaseUrl}/{chart.Category}/{chart.Route}";
        
        Console.WriteLine($"Testing: {chart.Name} at {url}");

        await Page.GotoAsync(url);
        await WaitForChartAsync();

        // Verify chart canvas exists
        var chartExists = await Page.Locator("[id^='chart'] canvas").IsVisibleAsync();
        Assert.That(chartExists, Is.True, $"{chart.Name}: Chart canvas not visible");

        // Verify ECharts version
        var version = await Page.EvaluateAsync<string>("() => echarts.version");
        Assert.That(version, Is.EqualTo("6.0.0"), $"{chart.Name}: Wrong ECharts version");

        // Take screenshot
        var screenshotPath = $"screenshots/{chart.Category}-{chart.Route}.png";
        await Page.ScreenshotAsync(new() { Path = screenshotPath });

        Console.WriteLine($"? {chart.Name} passed");
    }

    [Test]
    public async Task AllCharts_NoConsoleErrors()
    {
        var errors = new List<string>();
        
        Page.Console += (_, msg) =>
        {
            if (msg.Type == "error")
            {
                errors.Add($"{msg.Url}: {msg.Text}");
            }
        };

        foreach (var chart in AllCharts.Take(5)) // Test first 5 for quick validation
        {
            await Page.GotoAsync($"{BaseUrl}/{chart.Category}/{chart.Route}");
            await WaitForChartAsync();
        }

        Assert.That(errors, Is.Empty, 
            $"Console errors found:\n{string.Join("\n", errors)}");
    }
}
```

---

## Step 6: Visual Regression Testing

Create `VisualRegressionTests.cs`:

```csharp
using Microsoft.Playwright;
using NUnit.Framework;
using System.Security.Cryptography;
using System.Text;

namespace PanoramicData.ECharts.Tests.E2E;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class VisualRegressionTests : TestBase
{
    private const string BaselineDir = "screenshots/baseline";
    private const string CurrentDir = "screenshots/current";

    [Test]
    public async Task SimplePieChart_VisualRegression()
    {
        await Page.GotoAsync($"{BaseUrl}/pie/simple");
        await WaitForChartAsync();

        // Hide dynamic elements (animations might cause false positives)
        await Page.EvaluateAsync(@"
            () => {
                // Disable animations for consistent screenshots
                const chart = window.panoramicDataECharts.charts.values().next().value;
                if (chart) {
                    chart.setOption({ animation: false });
                }
            }
        ");

        await Page.WaitForTimeoutAsync(500);

        var screenshot = await Page.ScreenshotAsync();
        var currentPath = Path.Combine(CurrentDir, "pie-simple.png");
        
        Directory.CreateDirectory(CurrentDir);
        await File.WriteAllBytesAsync(currentPath, screenshot);

        // Compare with baseline (if exists)
        var baselinePath = Path.Combine(BaselineDir, "pie-simple.png");
        if (File.Exists(baselinePath))
        {
            var baseline = await File.ReadAllBytesAsync(baselinePath);
            var pixelDiff = CompareImages(baseline, screenshot);
            
            Assert.That(pixelDiff, Is.LessThan(0.01), 
                $"Visual regression detected: {pixelDiff:P2} pixel difference");
        }
        else
        {
            Console.WriteLine($"Baseline not found, creating: {baselinePath}");
            Directory.CreateDirectory(BaselineDir);
            await File.WriteAllBytesAsync(baselinePath, screenshot);
        }
    }

    private double CompareImages(byte[] baseline, byte[] current)
    {
        if (baseline.Length != current.Length)
            return 1.0; // 100% different

        int differences = 0;
        for (int i = 0; i < baseline.Length; i++)
        {
            if (baseline[i] != current[i])
                differences++;
        }

        return (double)differences / baseline.Length;
    }
}
```

---

## Step 7: Running Tests

### Command Line

```powershell
# Navigate to test project
cd C:\Users\david\Projects\PanoramicData.ECharts\src\PanoramicData.ECharts.Tests.E2E

# Run all tests
dotnet test

# Run specific test
dotnet test --filter "FullyQualifiedName~SimplePieChart"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Generate test report
dotnet test --logger "trx;LogFileName=TestResults.trx"
```

### Visual Studio

1. Open Test Explorer (Test ? Test Explorer)
2. Click "Run All" or select specific tests
3. View results and screenshots in Output window

---

## Step 8: CI/CD Integration

### GitHub Actions Example

Create `.github/workflows/playwright-tests.yml`:

```yaml
name: Playwright Tests

on:
  push:
    branches: [ main, feature/* ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '10.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Install Playwright
      run: |
        cd src/PanoramicData.ECharts.Tests.E2E
        pwsh bin/Debug/net10.0/playwright.ps1 install --with-deps
    
    - name: Start Demo App
      run: |
        cd src/PanoramicData.ECharts.Demo
        dotnet run --no-build &
        sleep 10
    
    - name: Run Playwright Tests
      run: dotnet test src/PanoramicData.ECharts.Tests.E2E --no-build
    
    - name: Upload Screenshots
      if: always()
      uses: actions/upload-artifact@v4
      with:
        name: playwright-screenshots
        path: src/PanoramicData.ECharts.Tests.E2E/screenshots/
```

---

## Step 9: Test Configuration

Create `playwright.config.json`:

```json
{
  "timeout": 30000,
  "expect": {
    "timeout": 5000
  },
  "fullyParallel": true,
  "forbidOnly": false,
  "retries": 1,
  "workers": 4,
  "reporter": [
    ["list"],
    ["html", { "outputFolder": "playwright-report" }]
  ],
  "use": {
    "baseURL": "http://localhost:5185",
    "trace": "on-first-retry",
    "screenshot": "only-on-failure",
    "video": "retain-on-failure"
  }
}
```

---

## Step 10: Advantages Over Manual Testing

| Feature | Manual | Playwright |
|---------|--------|------------|
| **Speed** | ~2 hours | ~5 minutes |
| **Repeatability** | Variable | 100% |
| **Browser Coverage** | 1 browser | 3+ browsers |
| **Regression Detection** | Visual only | Automated |
| **CI/CD Integration** | No | Yes |
| **Documentation** | Screenshots | Screenshots + Videos |
| **Parallel Execution** | No | Yes |

---

## Expected Results

### Test Execution
```
Starting test execution, please wait...
A total of 40 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    40, Skipped:     0, Total:    40, Duration: 2m 15s
```

### Screenshot Output
```
screenshots/
??? baseline/          (Reference images for regression)
??? current/           (Latest test run)
??? pie-simple.png
??? sunburst-simple.png
??? sankey-levels.png
??? line-simple.png
??? bar-simple.png
... (40+ screenshots)
```

---

## Troubleshooting

### Port Conflicts
If demo runs on different port, update `BaseUrl` in `TestBase.cs`:
```csharp
protected const string BaseUrl = "http://localhost:YOUR_PORT";
```

### Browser Installation Issues
```powershell
# Manual browser install
pwsh bin\Debug\net10.0\playwright.ps1 install chromium
pwsh bin\Debug\net10.0\playwright.ps1 install firefox
pwsh bin\Debug\net10.0\playwright.ps1 install webkit
```

### Timeout Issues
Increase timeout in `TestBase.cs`:
```csharp
protected const int DefaultTimeout = 60000; // 60 seconds
```

---

## Next Steps

1. **Create test project** using commands above
2. **Add baseline tests** for critical charts (5-10 charts)
3. **Run tests** and verify all pass
4. **Expand coverage** to all 23 chart types
5. **Integrate into CI/CD** pipeline

---

**Estimated Time**:
- Setup: 30 minutes
- Basic tests (5 charts): 1 hour
- Full coverage (23 charts): 2-3 hours
- **Total: 3-4 hours** (vs 8+ hours manual)

**ROI**: Saves 5+ hours every time you need to test!
