# PanoramicData.ECharts

[![Nuget](https://img.shields.io/nuget/v/PanoramicData.ECharts)](https://www.nuget.org/packages/PanoramicData.ECharts/)
[![Nuget](https://img.shields.io/nuget/dt/PanoramicData.ECharts)](https://www.nuget.org/packages/PanoramicData.ECharts/)
[![License: Apache-2.0](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/563d18517c424783814728f76d8aca6f)](https://app.codacy.com/gh/panoramicdata/PanoramicData.ECharts/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)

A comprehensive Blazor wrapper for [Apache ECharts](https://echarts.apache.org/en/index.html), providing type-safe C# bindings for creating interactive data visualizations in Blazor applications.

## Features

- **Full .NET Support**: Compatible with .NET 10.0
- **Latest ECharts**: Ships with Apache ECharts 6.0.0
- **Type-Safe Bindings**: Strong typing throughout the API
- **Open Source**: Apache-2.0 licensed (same as Apache ECharts)
- **Comprehensive Examples**: Extensive demo project with real-world examples
- **Multiple Data Loading Strategies**: Sync, async, and external data sources
- **JavaScript Functions**: Support for custom formatters and callbacks
- **Real-time Updates**: Dynamic chart updates and animations

## Supported Chart Types

| Category | Chart Types |
|----------|-------------|
| **Basic** | Line, Bar, Pie, Scatter |
| **Geographic** | Geo, Map |
| **Financial** | Candlestick |
| **Statistical** | Radar, Boxplot, Heatmap |
| **Graph** | Graph, Tree, Treemap, Sunburst |
| **Flow** | Parallel, Sankey, Funnel |
| **Other** | Gauge, Pictorial Bar, Theme River, Custom |

## Quick Start

### Installation

Add the NuGet package to your Blazor project:

```bash
dotnet add package PanoramicData.ECharts
```

### Include JavaScript

Add the JavaScript bundle to your `_Host.cshtml`, `_Layout.cshtml`, or `App.razor`:

```html
<!-- Option 1: Bundle with ECharts and echarts-stat (recommended) -->
<script src="_content/PanoramicData.ECharts/js/panoramicdata-echarts-bundle.js"></script>

<!-- Option 2: Minified bundle (recommended for production) -->
<script src="_content/PanoramicData.ECharts/js/panoramicdata-echarts-bundle-min.js"></script>

<!-- Option 3: Wrapper only (requires manual ECharts inclusion) -->
<script src="_content/PanoramicData.ECharts/js/panoramicdata-echarts.js"></script>

<!-- Option 4: Minified wrapper (recommended for custom builds) -->
<script src="_content/PanoramicData.ECharts/js/panoramicdata-echarts-min.js"></script>
```

**Note**: JavaScript file names changed in v6.0.0 from `vizor-echarts-*` to `panoramicdata-echarts-*`. See [Migration Guide](#migration-guide) for upgrade instructions.

See the [demo application example](https://github.com/panoramicdata/PanoramicData.ECharts/blob/main/PanoramicData.ECharts.Demo/Pages/_Host.cshtml).

### Basic Usage

The C# bindings mirror the JavaScript/TypeScript API, making it easy to translate official ECharts examples.

**Example: [Simple Pie Chart](https://echarts.apache.org/examples/en/editor.html?c=pie-simple)**

Add the using statement:
```csharp
@using PanoramicData.ECharts
```

Add the chart component:
```html
<EChart Options="@options" Width="auto" Height="400px" />
```

Define the chart options:
```csharp
@code {
    private ChartOptions options = new()
    {
        Title = new()
        {
            Text = "Referer of a Website",
            Subtext = "Fake Data",
            Left = "center"
        },
        Tooltip = new()
        {
            Trigger = TooltipTrigger.Item
        },
        Legend = new()
        {
            Orient = Orient.Vertical,
            Left = "left"
        },
        Series = new()
        {
            new PieSeries()
            {
                Name = "Access From",
                Radius = new CircleRadius("50%"),
                Data = new List<PieSeriesData>()
                {
                    new() { Value = 1048, Name = "Search Engine" },
                    new() { Value = 735, Name = "Direct" },
                    new() { Value = 580, Name = "Email" },
                    new() { Value = 484, Name = "Union Ads" },
                    new() { Value = 300, Name = "Video Ads" }
                },
                Emphasis = new()
                {
                    ItemStyle = new()
                    {
                        ShadowBlur = 10,
                        ShadowOffsetX = 0,
                        ShadowColor = Color.FromRGBA(0, 0, 0, 0.5)
                    }
                }
            }
        }
    };
}
```

[View full example](https://github.com/panoramicdata/PanoramicData.ECharts/blob/main/PanoramicData.ECharts.Samples/Areas/Pie/SimplePieChart.razor)

## Advanced Features

### 1. Async Data Loading

Load data asynchronously from databases or APIs:

```html
<EChart Options="@options" DataLoader="@LoadChartData" Width="800px" Height="400px" />
```

```csharp
private async Task LoadChartData()
{
    // Fetch data from your database or API
    var data = await _dataService.GetChartDataAsync();
    
    // Update chart options
    options.Series = new()
    {
        new LineSeries()
        {
            Data = data.Select(d => d.Value).ToList()
        }
    };
}
```

[View full example](https://github.com/panoramicdata/PanoramicData.ECharts/blob/main/PanoramicData.ECharts.Samples/Areas/Misc/DataLoaderSampleChart.razor)

### 2. External Data Sources (REST API)

Fetch data directly in the browser from external sources:

```csharp
private ExternalDataSource extData = new("https://example.com/api/data/sunburst.json");
```

```html
<EChart ExternalDataSources="@(new[] { extData })" Options="@options" />
```

```csharp
options.Series = new()
{
    new SunburstSeries()
    {
        Data = new ExternalDataSourceRef(extData)
    }
};
```

**Advanced features:**
- Path expressions: `new ExternalDataSource("url", path: "data.items")`
- Post-load transformations: `AfterLoad = new JavascriptFunction("...")`
- Custom fetch options: headers, credentials, policies

[View full example](https://github.com/panoramicdata/PanoramicData.ECharts/blob/main/PanoramicData.ECharts.Samples/Areas/Sunburst/SimpleSunburstChart.razor)

### 3. Datasets and Transformations

Simplify data retrieval with ECharts' dataset feature:

```csharp
options.Dataset = new Dataset
{
    Source = new object[]
    {
        new[] { "product", "2015", "2016", "2017" },
        new object[] { "Matcha Latte", 43.3, 85.8, 93.7 },
        new object[] { "Milk Tea", 83.1, 73.4, 55.1 }
    }
};
```

[Dataset examples](https://github.com/panoramicdata/PanoramicData.ECharts/blob/main/PanoramicData.ECharts.Samples/Areas/Bar/SimpleDatasetBarChart.razor) | [ECharts dataset documentation](https://echarts.apache.org/en/tutorial.html#Dataset)

### 4. JavaScript Functions

Use custom JavaScript for formatters and callbacks:

```csharp
Formatter = new JavascriptFunction(@"
    function (param) { 
        return param.name + ' (' + param.percent * 2 + '%)'; 
    }
")
```

[View full example](https://github.com/panoramicdata/PanoramicData.ECharts/blob/main/PanoramicData.ECharts.Samples/Areas/Pie/HalfDoughnutChart.razor)

### 5. Real-Time Updates

Update charts dynamically for live dashboards:

```html
<EChart @ref="chart" Options="@options" />
```

```csharp
private EChart? chart;

private async Task UpdateChartAsync()
{
    if (chart == null) return;
    
    // Modify chart options
    var series = (LineSeries)options.Series[0];
    series.Data.Add(newValue);
    
    // Trigger update
    await chart.UpdateAsync();
}
```

[View full example](https://github.com/panoramicdata/PanoramicData.ECharts/blob/main/PanoramicData.ECharts.Samples/Areas/Gauge/TempGaugeChart.razor)

## Documentation

- **[Apache ECharts Documentation](https://echarts.apache.org/en/index.html)** - Official ECharts documentation
- **[ECharts Cheat Sheet](https://echarts.apache.org/en/cheat-sheet.html)** - Quick reference guide
- **[ECharts Examples](https://echarts.apache.org/examples/en/index.html)** - Interactive examples gallery
- **[Demo Application](https://github.com/panoramicdata/PanoramicData.ECharts/tree/main/PanoramicData.ECharts.Demo)** - Comprehensive examples in C#

## Migration Guide

### Upgrading from v5.x to v6.0

#### Breaking Changes

**JavaScript File Names Changed**

The JavaScript file names have been renamed to match the PanoramicData.ECharts branding:

| Old Name (v5.x) | New Name (v6.0+) |
|-----------------|------------------|
| `vizor-echarts.js` | `panoramicdata-echarts.js` |
| `vizor-echarts-min.js` | `panoramicdata-echarts-min.js` |
| `vizor-echarts-bundle.js` | `panoramicdata-echarts-bundle.js` |
| `vizor-echarts-bundle-min.js` | `panoramicdata-echarts-bundle-min.js` |

**Update Required**:
```html
<!-- OLD (v5.x) -->
<script src="_content/PanoramicData.ECharts/js/vizor-echarts-bundle-min.js"></script>

<!-- NEW (v6.0+) -->
<script src="_content/PanoramicData.ECharts/js/panoramicdata-echarts-bundle-min.js"></script>
```

**JavaScript Global Object Changed**

If you have custom JavaScript interop code:

```javascript
// OLD (v5.x)
window.vizorECharts.getDataSource([...])

// NEW (v6.0+)
window.panoramicDataECharts.getDataSource([...])
```

#### Non-Breaking Changes

- **C# API**: No changes - all existing C# code remains compatible
- **Chart Options**: No changes - all option structures unchanged
- **Component Properties**: No changes - all EChart component properties work as before
- **ECharts Upgraded**: Now ships with Apache ECharts 6.0.0 (performance improvements)

#### New Features in v6.0

- **Symbol Packages**: Debugging symbols (.snupkg) now published for better debugging experience
- **Source Link**: Step through library source code during debugging
- **Performance**: Improved rendering performance from ECharts 6.0.0 engine
- **Versioning**: Now using Nerdbank.GitVersioning for consistent versioning

## Contributing & Support

### Filing Issues

See [Issues](https://github.com/panoramicdata/PanoramicData.ECharts/issues) for open tasks and bug reports.

When reporting issues:
1. Provide a runnable example using the [ECharts Online Editor](https://echarts.apache.org/examples/en/editor.html)
2. Describe the expected behavior vs. actual behavior
3. Include your C# code mapping

### Contributing

We welcome contributions! Please:
- Fork the repository
- Create a feature branch
- Make your changes
- Submit a pull request

Please ensure your code follows the existing style and includes appropriate tests.

## License

This project is licensed under the **Apache License 2.0** - the same license as Apache ECharts.

See [LICENSE](LICENSE) for details.

## Acknowledgments

- Built on [Apache ECharts](https://echarts.apache.org/) - A powerful, interactive charting and visualization library
- Originally created by [DataHint BV](https://github.com/datahint-eu) as Vizor.ECharts
- Maintained by [Panoramic Data Limited](https://github.com/panoramicdata)

---

**[Star this repo ?](https://github.com/panoramicdata/PanoramicData.ECharts)** if you find it useful!