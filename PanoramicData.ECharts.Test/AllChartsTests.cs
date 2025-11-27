using AwesomeAssertions;

namespace PanoramicData.ECharts.Test;

public class AllChartsTests : TestBase
{
	public static TheoryData<string, string, string> AllCharts =>
		new()
		{
			// Line Charts
			{ "line", "simple", "SimpleLineChart" },
			{ "line", "smooth", "SmoothLineChart" },
			{ "line", "stacked", "StackedLineChart" },

			// Bar Charts
			{ "bar", "simple", "SimpleBarChart" },
			{ "bar", "stacked-h", "HorizontalStackedBarChart" },
			{ "bar", "simple-dataset", "SimpleDatasetBarChart" },
			{ "bar", "stacked-timeseries", "StackedBarTimeSeriesChart" },

			// Pie Charts
			{ "pie", "simple", "SimplePieChart" },
			{ "pie", "half-doughnut", "HalfDoughnutChart" },
			{ "pie", "doughnut-rounded", "RoundedDoughnutChart" },

			// Scatter Charts
			{ "scatter", "simple", "SimpleScatterChart" },
			{ "scatter", "punchcard", "PunchCardScatterChart" },
			{ "scatter", "cluster", "ClusteredScatterChart" },

			// Geo/Map Charts
			{ "geo", "usa", "UsaGeoMap" },
			{ "geo", "belgium", "BelgianMunicipalityMap" },
			{ "geo", "flight-seats", "FlightSeatsGeoMap" },

			// Heatmap Charts
			{ "heatmap", "simple", "SimpleHeatmapChart" },
			{ "heatmap", "year", "YearHeatmapChart" },
			{ "heatmap", "multi-year", "MultiYearHeatmapChart" },
			{ "heatmap", "daily-visits", "VisitsPerDayHeatmapChart" },

			// Other Chart Types
			{ "candlestick", "simple", "SimpleCandlestickChart" },
			{ "radar", "simple", "SimpleRadarChart" },
			{ "graph", "simple", "SimpleGraphChart" },
			{ "graph", "force-layout", "ForceLayoutGraphChart" },
			{ "tree", "ltr", "TreeLeftToRightChart" },
			{ "treemap", "simple", "SimpleTreeMapChart" },
			{ "sunburst", "simple", "SimpleSunburstChart" },
			{ "parallel", "simple", "SimpleParallelChart" },
			{ "sankey", "simple", "SimpleSankeyChart" },
			{ "sankey", "levels", "SankeyWithLevelsChart" },
			{ "funnel", "simple", "SimpleFunnelChart" },
			{ "gauge", "temp", "TempGaugeChart" },
			{ "pictorial-bar", "simple", "SimplePictorialBarChart" },
			{ "themeriver", "simple", "SimpleThemeRiverChart" },

			// Area Charts
			{ "area", "simple", "SimpleAreaChart" },
			{ "area", "stacked", "StackedAreaChart" },
			{ "area", "timeseries", "TimeSeriesAreaChart" },

			// Advanced Features
			{ "misc", "dataloader", "DataLoaderSampleChart" },
			{ "misc", "parameters", "ParameterSetSampleChart" },
			{ "misc", "refresh", "RefreshSampleChart" },
			{ "misc", "multi-axis", "MultiAxisSampleChart" },
			{ "misc", "color-gradient", "ColorGradientBarChart" },
			{ "misc", "toolbox", "ToolboxSampleChart" },
		};

	[Theory]
	[MemberData(nameof(AllCharts))]
	public async Task Chart_Renders_WithoutErrors(string category, string route, string name)
	{
		var url = $"{BaseUrl}/{category}/{route}";

		// Verify chart canvas exists
		await Page.GotoAsync(url);
		await WaitForChartAsync();

		var chartExists = await IsChartVisibleAsync();
		chartExists.Should().BeTrue($"{name}: Chart canvas not visible");

		// Verify ECharts version
		var version = await Page.EvaluateAsync<string>("() => echarts.version");
		version.Should().Be("6.0.0");

		// Take screenshot
		var screenshotFilename = $"{category}-{route}.png";
		await TakeScreenshotAsync(screenshotFilename);
	}

	[Fact]
	public async Task VerifyAllCharts_HaveCorrectGlobalObject()
	{
		// Test first few charts to ensure global object is consistent
		var chartsToTest = new[]
		{
			("line", "simple"),
			("line", "smooth"),
			("bar", "simple"),
			("pie", "simple"),
			("scatter", "simple")
		};

		foreach (var (category, route) in chartsToTest)
		{
			await Page.GotoAsync($"{BaseUrl}/{category}/{route}");
			await WaitForChartAsync();
			await VerifyEChartsGlobalAsync();
		}
	}
}
