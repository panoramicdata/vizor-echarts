using AwesomeAssertions;
using Microsoft.Playwright;

namespace PanoramicData.ECharts.Test;

public class TestBase : IAsyncLifetime
{
	protected const string BaseUrl = "http://localhost:5185/example"; // Fixed: added /example prefix
	protected const int DefaultTimeout = 10000; // 10 seconds

	protected IPlaywright? Playwright { get; private set; }
	protected IBrowser? Browser { get; private set; }
	protected IBrowserContext? Context { get; private set; }
	protected IPage Page { get; private set; } = null!;

	public async ValueTask InitializeAsync()
	{
		// Install playwright
		var exitCode = Microsoft.Playwright.Program.Main(["install"]);
		if (exitCode != 0)
		{
			throw new Exception($"Playwright install failed with exit code {exitCode}");
		}

		// Create playwright instance
		Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

		// Launch browser
		Browser = await Playwright.Chromium.LaunchAsync(new()
		{
			Headless = true
		});

		// Create context
		Context = await Browser.NewContextAsync(new()
		{
			ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
			IgnoreHTTPSErrors = true,
		});

		// Create page
		Page = await Context.NewPageAsync();
		Page.SetDefaultTimeout(DefaultTimeout);
	}

	public async ValueTask DisposeAsync()
	{
		if (Page != null)
		{
			await Page.CloseAsync();
		}

		if (Context != null)
		{
			await Context.CloseAsync();
		}

		if (Browser != null)
		{
			await Browser.CloseAsync();
		}

		Playwright?.Dispose();
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
		await Page.WaitForTimeoutAsync(500);
	}

	/// <summary>
	/// Verify no JavaScript console errors
	/// </summary>
	protected async Task<List<string>> GetConsoleErrors()
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
		return errors;
	}

	/// <summary>
	/// Verify ECharts global object exists with correct name
	/// </summary>
	protected async Task VerifyEChartsGlobalAsync()
	{
		var hasGlobal = await Page.EvaluateAsync<bool>(
			"() => typeof window.panoramicDataECharts !== 'undefined'"
		);
		hasGlobal.Should().BeTrue("window.panoramicDataECharts not found");

		var oldGlobal = await Page.EvaluateAsync<bool>(
			"() => typeof window.vizorECharts !== 'undefined'"
		);
		oldGlobal.Should().BeFalse("Old window.vizorECharts still exists");

		var version = await Page.EvaluateAsync<string>("() => echarts.version");
		version.Should().Be("6.0.0");
	}

	/// <summary>
	/// Verify chart canvas is visible
	/// </summary>
	protected async Task<bool> IsChartVisibleAsync() => await Page.Locator("[id^='chart'] canvas").IsVisibleAsync();

	/// <summary>
	/// Take a screenshot for the current test
	/// </summary>
	protected async Task TakeScreenshotAsync(string filename)
	{
		var screenshotsDir = Path.Combine(Directory.GetCurrentDirectory(), "screenshots");
		Directory.CreateDirectory(screenshotsDir);

		var path = Path.Combine(screenshotsDir, filename);
		await Page.ScreenshotAsync(new() { Path = path, FullPage = false });
	}
}
