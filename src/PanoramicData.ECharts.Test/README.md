# PanoramicData.ECharts Playwright E2E Tests

This project contains automated end-to-end tests for the PanoramicData.ECharts library using Playwright with **xUnit**.

## Prerequisites

- .NET 10 SDK installed
- PanoramicData.ECharts.Demo application running

## Initial Setup

### 1. Install Playwright Browsers

Before running tests for the first time, you need to install the Playwright browsers:

```powershell
# Navigate to the test project directory
cd PanoramicData.ECharts.Test

# Build the project first
dotnet build

# Install Playwright browsers (run this from the test project directory)
pwsh bin\Debug\net10.0\playwright.ps1 install

# Or install specific browsers only
pwsh bin\Debug\net10.0\playwright.ps1 install chromium
pwsh bin\Debug\net10.0\playwright.ps1 install firefox
pwsh bin\Debug\net10.0\playwright.ps1 install webkit
```

### 2. Start the Demo Application

The tests require the demo application to be running on `http://localhost:5185`:

```powershell
# In a separate terminal, navigate to the demo project
cd ..\PanoramicData.ECharts.Demo

# Run the demo application
dotnet run
```

**Note:** If your demo runs on a different port, update the `BaseUrl` constant in `TestBase.cs`.

## Running Tests

### Run All Tests

```powershell
# From the test project directory
dotnet test

# Or from the solution root
dotnet test PanoramicData.ECharts.Test
```

### Run Specific Test Class

```powershell
# Run only basic chart tests
dotnet test --filter "FullyQualifiedName~ChartTests"

# Run all charts comprehensive tests
dotnet test --filter "FullyQualifiedName~AllChartsTests"
```

### Run Specific Test

```powershell
# Run a single test by name
dotnet test --filter "FullyQualifiedName~SimplePieChart_Renders_Successfully"
```

### Run with Detailed Output

```powershell
# Show detailed console output
dotnet test --logger "console;verbosity=detailed"
```

### Run with Visual Studio Test Explorer

1. Open the solution in Visual Studio
2. Go to **Test** ? **Test Explorer**
3. Click **Run All** or select specific tests
4. View results in the Test Explorer pane

## Test Structure

### Test Files

- **`TestBase.cs`** - Base class with common test utilities
  - Browser configuration (1920x1080 viewport)
  - Playwright lifecycle management (`IAsyncLifetime`)
  - Chart waiting logic
  - ECharts global object verification
  - Screenshot capture
  
- **`ChartTests.cs`** - Basic tests for core functionality
  - Simple Pie Chart
  - Sunburst with external data
  - Sankey with path expressions
  - Line and Bar charts
  - Dynamic parameter updates
  - Console error detection

- **`AllChartsTests.cs`** - Comprehensive test suite
  - Tests all 47+ chart samples using `[Theory]` and `[MemberData]`
  - Verifies ECharts 6.0.0 version
  - Captures screenshots for all charts

### Test Categories

| Category | Count | Examples |
|----------|-------|----------|
| **Line Charts** | 3 | Simple, Smooth, Stacked |
| **Bar Charts** | 4 | Simple, Horizontal Stacked, Dataset |
| **Pie Charts** | 3 | Simple, Half Doughnut, Rounded |
| **Scatter Charts** | 3 | Simple, Punchcard, Clustered |
| **Geo/Map Charts** | 3 | USA, Belgian, Flight Seats |
| **Heatmap Charts** | 4 | Simple, Year, Multi-Year, Visits |
| **Other Types** | 20+ | Candlestick, Radar, Graph, Tree, Treemap, Sunburst, etc. |
| **Advanced Features** | 6 | Data Loader, Parameter Set, Refresh, Multi-Axis, etc. |

## Understanding Test Results

### Successful Test Output

```
Starting test execution, please wait...
A total of 3 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    54, Skipped:     0, Total:    54
Time: 2m 30s
```

### Screenshots

Tests automatically capture screenshots in:
```
PanoramicData.ECharts.Test\bin\Debug\net10.0\screenshots\
```

Examples:
- `pie-simple.png`
- `sunburst-simple.png`
- `sankey-levels.png`
- `line-simple.png`
- `bar-dataset.png`
- etc.

## Test Utilities

### `TestBase` Helper Methods

```csharp
// Wait for chart to fully render
await WaitForChartAsync();

// Verify ECharts 6.0.0 is loaded with correct global object
await VerifyEChartsGlobalAsync();

// Check if chart canvas is visible
bool isVisible = await IsChartVisibleAsync();

// Capture screenshot with automatic path handling
await TakeScreenshotAsync("my-chart.png");

// Get console errors (if any)
var errors = await GetConsoleErrors();
```

## xUnit Specifics

### Test Attributes

- **`[Fact]`** - Marks a single test method (replaces NUnit's `[Test]`)
- **`[Theory]`** - Marks a parameterized test method (replaces NUnit's `[TestCase]`)
- **`[MemberData]`** - Provides test data for theories (replaces NUnit's `[TestCaseSource]`)

### Assertions

xUnit uses a simpler assertion syntax:

```csharp
// Boolean assertions
Assert.True(condition, "message");
Assert.False(condition, "message");

// Equality assertions
Assert.Equal(expected, actual);
Assert.NotEqual(expected, actual);

// Collection assertions
Assert.Empty(collection);
Assert.NotEmpty(collection);
```

### Lifecycle Management

Tests use `IAsyncLifetime` for setup/teardown:

```csharp
public class TestBase : IAsyncLifetime
{
    // Called before each test
    public async Task InitializeAsync() { }
    
    // Called after each test
    public async Task DisposeAsync() { }
}
```

## Troubleshooting

### Port Conflicts

If the demo application runs on a different port:

1. Open `TestBase.cs`
2. Update the `BaseUrl` constant:
   ```csharp
   protected const string BaseUrl = "http://localhost:YOUR_PORT";
   ```

### Timeout Issues

If tests are timing out on slower machines:

1. Open `TestBase.cs`
2. Increase the `DefaultTimeout`:
   ```csharp
   protected const int DefaultTimeout = 60000; // 60 seconds
   ```

### Browser Not Installed

If you see "Executable doesn't exist" errors:

```powershell
# Re-run the browser installation
pwsh bin\Debug\net10.0\playwright.ps1 install
```

### Demo Application Not Running

Ensure the demo app is running before tests:

```powershell
# Check if app is responding
curl http://localhost:5185

# If not, start it
cd ..\PanoramicData.ECharts.Demo
dotnet run
```

## Continuous Integration

### GitHub Actions Example

See `PHASE_6_PLAYWRIGHT_AUTOMATION.md` for a complete GitHub Actions workflow that:
- Builds the solution
- Installs Playwright browsers
- Starts the demo application
- Runs all tests
- Uploads screenshots as artifacts

## Test Coverage

### What We Test

? Chart rendering for all 23 chart types  
? ECharts 6.0.0 version verification  
? Global object naming (`window.panoramicDataECharts`)  
? External data source loading  
? Path expression evaluation  
? Dynamic chart updates  
? Console error detection  
? Canvas visibility  

### What We Don't Test (Yet)

? Visual regression (pixel-perfect comparison)  
? Chart interactions (tooltips, zoom, pan)  
? Performance/load testing  
? Mobile responsiveness  
? Accessibility (ARIA labels, keyboard navigation)  

## Performance

- **Basic Tests (7 tests)**: ~30 seconds
- **All Charts Tests (47+ tests)**: ~2-3 minutes
- **Parallel Execution**: xUnit runs tests in parallel by default

## Next Steps

1. ? **Setup Complete** - Tests are ready to run
2. ? **Run Tests** - Execute `dotnet test`
3. ?? **Review Results** - Check Test Explorer or console output
4. ??? **View Screenshots** - Examine captured images
5. ?? **CI/CD Integration** - Add to your build pipeline

## Key Verifications

Each test in `AllChartsTests` verifies:
1. ? Chart canvas is visible
2. ? ECharts version is 6.0.0
3. ? Screenshot captured successfully
4. ? No unhandled exceptions

## Support

For issues or questions:
- Check `PHASE_6_PLAYWRIGHT_AUTOMATION.md` for detailed setup guide
- Review `PHASE_5_COMPLETION.md` for manual testing results
- Consult Playwright documentation: https://playwright.dev/dotnet/
- Consult xUnit documentation: https://xunit.net/

---

**Last Updated**: 2025-01-27  
**Test Framework**: xUnit 2.9.2  
**Playwright Version**: 1.49.1  
**Target Framework**: .NET 10
