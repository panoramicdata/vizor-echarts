using AwesomeAssertions;

namespace PanoramicData.ECharts.Test;

public class ChartTests : TestBase
{
	[Fact]
	public async Task SimplePieChart_Renders_Successfully()
	{
		// Navigate to pie chart
		await Page.GotoAsync($"{BaseUrl}/pie/simple");

		// Wait for chart
		await WaitForChartAsync();

		// Verify ECharts initialized
		await VerifyEChartsGlobalAsync();

		// Verify chart exists
		var chartExists = await IsChartVisibleAsync();
		chartExists.Should().BeTrue("Chart canvas not visible");

		// Verify title
		var titleVisible = await Page.Locator("text=Referer of a Website").IsVisibleAsync();
		titleVisible.Should().BeTrue("Chart title not visible");

		// Take screenshot
		await TakeScreenshotAsync("pie-simple.png");
	}

	[Fact]
	public async Task SimpleSunburstChart_WithExternalData_Renders()
	{
		await Page.GotoAsync($"{BaseUrl}/sunburst/simple");
		await WaitForChartAsync();

		// This chart uses external data - verify it loaded
		var chartExists = await IsChartVisibleAsync();
		chartExists.Should().BeTrue("Chart canvas not visible");

		// Verify data was fetched
		var hasData = await Page.EvaluateAsync<bool>(
			"() => window.panoramicDataECharts.dataSources.size > 0"
		);
		hasData.Should().BeTrue("External data not loaded");

		await TakeScreenshotAsync("sunburst-simple.png");
	}

	[Fact]
	public async Task SankeyWithLevels_WithPathExpression_Renders()
	{
		await Page.GotoAsync($"{BaseUrl}/sankey/levels");
		await WaitForChartAsync();

		var chartExists = await IsChartVisibleAsync();
		chartExists.Should().BeTrue("Chart canvas not visible");

		// Verify path expression worked (data extracted from JSON)
		var hasData = await Page.EvaluateAsync<bool>(
			"() => window.panoramicDataECharts.dataSources.size > 0"
		);
		hasData.Should().BeTrue("External data with path expression not loaded");

		await TakeScreenshotAsync("sankey-levels.png");
	}

	[Fact]
	public async Task SimpleLineChart_Renders_Successfully()
	{
		await Page.GotoAsync($"{BaseUrl}/line/simple");
		await WaitForChartAsync();

		var chartExists = await IsChartVisibleAsync();
		chartExists.Should().BeTrue("Line chart not visible");

		await VerifyEChartsGlobalAsync();
		await TakeScreenshotAsync("line-simple.png");
	}

	[Fact]
	public async Task SimpleBarChart_Renders_Successfully()
	{
		await Page.GotoAsync($"{BaseUrl}/bar/simple");
		await WaitForChartAsync();

		var chartExists = await IsChartVisibleAsync();
		chartExists.Should().BeTrue("Bar chart not visible");

		await TakeScreenshotAsync("bar-simple.png");
	}

	[Fact]
	public async Task ParameterSetSampleChart_DynamicUpdate_Works()
	{
		await Page.GotoAsync($"{BaseUrl}/misc/parameterset");
		await WaitForChartAsync();

		// Take initial screenshot
		await TakeScreenshotAsync("parameter-before.png");

		// Verify chart renders initially
		var chartExists = await IsChartVisibleAsync();
		chartExists.Should().BeTrue("Chart not visible before update");

		// Wait for auto-update (chart updates after 3 seconds)
		await Page.WaitForTimeoutAsync(4000);

		// Verify chart still renders after update
		chartExists = await IsChartVisibleAsync();
		chartExists.Should().BeTrue("Chart not visible after update");

		await TakeScreenshotAsync("parameter-after.png");
	}

	[Fact]
	public async Task NoConsoleErrors_OnChartLoad()
	{
		var errors = new List<string>();

		Page.Console += (_, msg) =>
		{
			if (msg.Type == "error")
			{
				errors.Add($"{msg.Location}: {msg.Text}");
			}
		};

		// Test a few different charts
		var charts = new[] { "pie/simple", "line/simple", "bar/simple" };

		foreach (var chart in charts)
		{
			await Page.GotoAsync($"{BaseUrl}/{chart}");
			await WaitForChartAsync();
		}

		errors.Should().BeEmpty();
	}
}
