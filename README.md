# Vizor.ECharts

Blazor wrapper for [Apache ECharts](https://echarts.apache.org/en/index.html).

 - Supports .NET >= 8.0
 - Ships with echarts 5.4.3
 - `Apache-2.0` Licensed (same as echarts)
 - Lots of examples in the `Vizor.ECharts.Demo` project
 - Refer to the official echarts [cheat sheet](https://echarts.apache.org/en/cheat-sheet.html) for a quick introduction
 
Supported Chart Types:
 - Line
 - Bar
 - Pie
 - Scatter
 - Geo/Map
 - Candlestick
 - Radar
 - Boxplot
 - Heatmap
 - Graph
 - Tree
 - Treemap
 - Sunburst
 - Parallel
 - Sankey
 - Funnel
 - Gauge
 - Pictorial Bar
 - Theme River
 - Custom

## How to include

1. Add a package reference to `Vizor.ECharts`
2. Add `vizor-echarts-bundle-min.js` OR `vizor-echarts-min.js` to your `_Host.cshtml` or `_Layout.cshtml` file
    - `vizor-echarts-bundle-min.js` includes apache echarts and echarts-stat.
	- `vizor-echarts-min.js` ONLY contains the binding code and requires you to manually include apache-echarts and plugins.
	
```
<script src="_content/Vizor.ECharts/js/vizor-echarts-bundle-min.js"></script>
```

See the [example](https://github.com/datahint-eu/vizor-echarts/blob/main/src/Vizor.ECharts.Demo/Pages/_Host.cshtml) from the demo application.

## How to use

The bindings are nearly identical to the javascript/typescript version.
This makes it very easy to translate the examples from the official documentation to C#.

For example: [a simple pie chart](https://echarts.apache.org/examples/en/editor.html?c=pie-simple).

Add a using statement:
```
@using Vizor.ECharts;
```

Chart component in your .razor file:
```
<Vizor.ECharts.EChart Options="@options" Width="auto" Height="400px" />
```

Chart options in the code section of your razor file:
```
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
		Trigger = ECharts.TooltipTrigger.Item
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
				new() { Value = 300, Name = "Video Ads" },
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
```

See the [full C# code](https://github.com/datahint-eu/vizor-echarts/blob/main/src/Vizor.ECharts.Samples/Areas/Pie/SimplePieChart.razor).

## Data loading

Most examples that you will find online have very basic datasets.
However, in real life, data sets are often huge and come from various different sources.

Vizor.ECharts allows you to define data in 3 different ways:
1. Inside the ChartOptions, as demonstrated in most examples.
2. Using async data loaders in C#, allowing you to fetch data directly from the database.
3. Using external data sources (e.g.: REST API) fetched by the browser.

### Async data loading

Specify the DataLoader parameter, this can be a sync or async function.
```
<Vizor.ECharts.EChart Options="@options" DataLoader="@LoadChartData" Width="800px" Height="400px" />
```

Typically in the data loader function you update the Series property. However, you can update any chart option.
```
private async Task LoadChartData()
{
	options.Series = ... ;
}
```

See [full example](https://github.com/datahint-eu/vizor-echarts/blob/main/src/Vizor.ECharts.Samples/Areas/Misc/DataLoaderSampleChart.razor).

### External Data Sources (fetch)

The short version: `ExternalDataSource` is provided as an `EChart` parameter, `ExternalDataSourceRef` is used in the `ChartOptions` to refer to a specific `ExternalDataSource`.

Any `Data` property inside the `ChartOptions` of type `object?` accepts a `ExternalDataSourceRef` allowing you to specify a reference to an external data source.
```
... = new ExternalDataSourceRef(dataSource);
```

An array of `ExternalDataSource` instances must be supplied to the the `EChart` `ExternalDataSources` parameter.
```
<Vizor.ECharts.EChart ExternalDataSources="@(new[] { extData })" ... />
```

An example on how to construct an `ExternalDataSource` instance:
```
... = new ExternalDataSource("https://example.com/api/data/sunburst_simple.json")
```
See [full example](https://github.com/datahint-eu/vizor-echarts/blob/main/src/Vizor.ECharts.Samples/Areas/Sunburst/SimpleSunburstChart.razor).


It is also possible to provide a *simple* path expression to retrieve only a part of the external data:
```
... = new ExternalDataSource("https://example.com/api/data/sankey_simple.json", path: "nodes")
```
See [full example](https://github.com/datahint-eu/vizor-echarts/blob/main/src/Vizor.ECharts.Samples/Areas/Sankey/SankeyWithLevelsChart.razor).


Or you can execute a function after load to manipulate the loaded data:
```
... = new ExternalDataSource("/data/les-miserables.json", ExternalDataFetchAs.Json)
{
	AfterLoad = new JavascriptFunction(@"function (graph) {
		graph.nodes.forEach(function (node) { node.symbolSize = 5; });
		return graph;
	}")
};
```
See [full example](https://github.com/datahint-eu/vizor-echarts/blob/main/src/Vizor.ECharts.Samples/Areas/Graph/ForceLayoutGraphChart.razor).

See *Javascript functions* chapter in the readme for more details about JS functions.


An `ExternalDataSourceRef` also supports a path expression to select a child object.
```
... = new ExternalDataSourceRef(graph, "nodes")
... = new ExternalDataSourceRef(graph, "links")
... = new ExternalDataSourceRef(graph, "categories")
}
```

See [example](https://github.com/datahint-eu/vizor-echarts/blob/main/src/Vizor.ECharts.Samples/Areas/Graph/ForceLayoutGraphChart.razor).


Additional credentials, headers, policies, ... can also be supplied.
See [ExternalDataSource](https://github.com/datahint-eu/vizor-echarts/blob/main/src/Vizor.ECharts/Types/ExternalDataSource.cs) and [FetchOptions](https://github.com/datahint-eu/vizor-echarts/blob/main/src/Vizor.ECharts/Types/FetchOptions.cs) for more details.


**Remark 1:** Never make an `ExternalDataSource` static, you need 1 instance per chart.

**Remark 2:** You will get a `InvalidOperationException` if you try to use `ExternalDataSource` in the chart options.


### Datasets

ECharts supports dataset transformations.
This allows for simplified data retrieval, without the need to have a separate dataset for different charts or chart types.

See [example #](https://github.com/datahint-eu/vizor-echarts/blob/main/src/Vizor.ECharts.Samples/Areas/Bar/SimpleDatasetBarChart.razor) and [example 2](https://github.com/datahint-eu/vizor-echarts/blob/main/src/Vizor.ECharts.Samples/Areas/Bar/StackedBarTimeSeriesChart.razor) .

See also the [echarts dataset documentation](https://echarts.apache.org/en/option.html#dataset) and [tutorial](https://echarts.apache.org/en/tutorial.html#Dataset) .

## Javascript functions

ECharts sometimes allows you to assign custom functions instead of values.
This can be achieved with the `JavascriptFunction` class.
The class accepts a string literal containing the Javascript function. The function is evaluated inside the brower.
Be carefull: syntax errors in the JS function will break the chart serialization.

For example:
```
Formatter = new JavascriptFunction("function (param) { return param.name + ' (' + param.percent * 2 + '%)'; }")
```

See [full example](https://github.com/datahint-eu/vizor-echarts/blob/main/src/Vizor.ECharts.Samples/Areas/Pie/HalfDoughnutChart.razor).

## Updating charts

Chart options and/or data can be updated. For example: to show a never ending line chart, a temperature gauge, ... .

First store a reference to your chart.
```
<Vizor.ECharts.EChart @ref="chart" Options="@options" Width="800px" Height="400px" />
...
private Vizor.ECharts.EChart? chart;
```

Next modify the chart options.
Modified options have full support for Javascript functions and external data sources.
```
private async Task UpdateChartAsync()
{
	if (chart == null)
		return;

	// modify chart options
	
	await chart.UpdateAsync();
}
```

See [full example](https://github.com/datahint-eu/vizor-echarts/blob/main/src/Vizor.ECharts.Samples/Areas/Gauge/TempGaugeChart.razor).


# Filing Bugs / Future Development

See [Issues](https://github.com/datahint-eu/vizor-echarts/issues) for a list of open tasks/bugs.

Please provide a runnable sample using the [ECharts Online Editor](https://echarts.apache.org/examples/en/editor.html) and a description of what is wrong with the C# mapping.