using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PanoramicData.ECharts.Internal;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

/// <summary>
/// Base class for all ECharts components providing core functionality for chart rendering,
/// serialization, and lifecycle management.
/// </summary>
public abstract class EChartBase : ComponentBase, IAsyncDisposable
{
	/// <summary>
	/// Cached JSON serializer options to improve performance by reusing the same instance.
	/// See https://www.meziantou.net/avoid-performance-issue-with-jsonserializer-by-reusing-the-same-instance-of-json.htm
	/// </summary>
	private static JsonSerializerOptions? cachedJsonOpts;

	/// <summary>
	/// Instance-specific JSON serializer options for this chart.
	/// </summary>
	protected JsonSerializerOptions? jsonOpts;

	/// <summary>
	/// Gets or sets the JavaScript runtime used for interop with the browser.
	/// </summary>
	[Inject]
	public IJSRuntime JSRuntime { get; set; } = default!;

	/// <summary>
	/// Gets or sets a unique identifier for the chart component.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This ID is used to identify the chart DOM element and must be unique within the page.
	/// </para>
	/// <para>
	/// <strong>Warning:</strong> DO NOT change this value after the chart has been rendered,
	/// as it will break the JavaScript interop binding.
	/// </para>
	/// <para>
	/// If unsure, leave the default auto-generated value or use <see cref="GenerateRandomId"/> 
	/// to create a new unique ID.
	/// </para>
	/// </remarks>
	[Parameter]
	public string Id { get; set; } = "chart" + GenerateRandomId();

	/// <summary>
	/// Gets or sets additional CSS class names to apply to the chart container.
	/// </summary>
	/// <remarks>
	/// Multiple classes can be specified separated by spaces.
	/// </remarks>
	[Parameter]
	public string? CssClass { get; set; }

	/// <summary>
	/// Gets or sets inline CSS styles to apply to the chart container.
	/// </summary>
	/// <example>
	/// <code>
	/// Style="border: 1px solid #ccc; background-color: #f5f5f5;"
	/// </code>
	/// </example>
	[Parameter]
	public string? Style { get; set; }

	/// <summary>
	/// Gets or sets the width of the chart container.
	/// </summary>
	/// <remarks>
	/// Can be specified in any valid CSS unit (px, %, em, vh, etc.).
	/// If not specified, the chart will use the container's natural width.
	/// </remarks>
	/// <example>
	/// <code>
	/// Width="800px"
	/// Width="100%"
	/// </code>
	/// </example>
	[Parameter]
	public string? Width { get; set; }

	/// <summary>
	/// Gets or sets the height of the chart container.
	/// </summary>
	/// <remarks>
	/// Can be specified in any valid CSS unit (px, %, em, vh, etc.).
	/// If not specified, the chart will use the container's natural height.
	/// </remarks>
	/// <example>
	/// <code>
	/// Height="400px"
	/// Height="50vh"
	/// </code>
	/// </example>
	[Parameter]
	public string? Height { get; set; }

	/// <summary>
	/// Gets or sets the ECharts theme name to apply to the chart.
	/// </summary>
	/// <remarks>
	/// <para>
	/// ECharts supports built-in themes like 'dark' or custom registered themes.
	/// </para>
	/// <para>
	/// Custom themes must be registered using echarts.registerTheme() before use.
	/// See https://echarts.apache.org/en/theme-builder.html for theme creation.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	/// Theme="dark"
	/// </code>
	/// </example>
	[Parameter]
	public string? Theme { get; set; }

	/// <summary>
	/// Gets or sets the chart group for coordinating multiple charts.
	/// </summary>
	/// <remarks>
	/// Charts in the same group can be connected for features like:
	/// - Synchronized tooltips
	/// - Coordinated axis ranges
	/// - Shared data zoom controls
	/// </remarks>
	[Parameter]
	public ChartGroup? Group { get; set; }

	/// <summary>
	/// Gets or sets the rendering mode for the chart.
	/// </summary>
	/// <remarks>
	/// <list type="bullet">
	/// <item>
	/// <term>SVG (default)</term>
	/// <description>Better for charts with fewer data points, supports better quality when scaling</description>
	/// </item>
	/// <item>
	/// <term>Canvas</term>
	/// <description>Better performance for charts with large amounts of data</description>
	/// </item>
	/// </list>
	/// </remarks>
	[Parameter]
	public ChartRenderer Renderer { get; set; } = ChartRenderer.Svg;

	/// <summary>
	/// Gets or sets custom JSON converters to use during serialization.
	/// </summary>
	/// <remarks>
	/// <para>
	/// <strong>Important:</strong> When using <see cref="CacheJsonSerializerOptions"/> = true (default),
	/// you must use the same list of custom converters across all charts, or set caching to false.
	/// </para>
	/// <para>
	/// For optimal memory consumption, reuse the same cached JsonSerializerOptions instance.
	/// </para>
	/// </remarks>
	[Parameter]
	public JsonConverter[]? JsonConverters { get; set; }

	/// <summary>
	/// Gets or sets whether to cache and reuse the JsonSerializerOptions instance.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Default is <c>true</c> for better performance and memory efficiency.
	/// </para>
	/// <para>
	/// Setting this to <c>false</c> is generally a bad idea unless you have different
	/// custom converters per chart instance.
	/// </para>
	/// <para>
	/// See: https://www.meziantou.net/avoid-performance-issue-with-jsonserializer-by-reusing-the-same-instance-of-json.htm
	/// </para>
	/// </remarks>
	[Parameter]
	public bool CacheJsonSerializerOptions { get; set; } = true;

	/// <summary>
	/// Gets or sets geographic map definitions to register with ECharts.
	/// </summary>
	/// <remarks>
	/// Maps must be registered before they can be used in geo or map series.
	/// See https://echarts.apache.org/en/api.html#echarts.registerMap
	/// </remarks>
	/// <example>
	/// <code>
	/// Maps = new[] 
	/// { 
	///     new MapDefinition 
	///     { 
	///         Name = "USA", 
	///         GeoJson = usaGeoJson 
	///     } 
	/// }
	/// </code>
	/// </example>
	[Parameter]
	public IMapDefinition[]? Maps { get; set; }

	/// <summary>
	/// Gets or sets a callback function used to retrieve data from external sources.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This callback is invoked when the chart needs to load or refresh external data.
	/// </para>
	/// <para>
	/// Use in conjunction with <see cref="ExternalDataSources"/> to specify which
	/// data sources to fetch.
	/// </para>
	/// </remarks>
	[Parameter]
	public EventCallback DataLoader { get; set; }

	/// <summary>
	/// Gets or sets external data source definitions for fetching remote data.
	/// </summary>
	/// <remarks>
	/// <para>
	/// External data sources allow charts to load data from URLs or other external locations
	/// rather than embedding all data in the chart options.
	/// </para>
	/// <para>
	/// The <see cref="DataLoader"/> callback is invoked to perform the actual data retrieval.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	/// ExternalDataSources = new[] 
	/// { 
	///     new ExternalDataSource 
	///     { 
	///         Id = "salesData",
	///         Url = "/api/sales/data",
	///         Path = "$.items"
	///     } 
	/// }
	/// </code>
	/// </example>
	[Parameter]
	public ExternalDataSource[]? ExternalDataSources { get; set; }

	/// <summary>
	/// Gets or sets the chart configuration options.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is the main configuration object that defines all aspects of the chart,
	/// including series, axes, legend, tooltip, and visual styles.
	/// </para>
	/// <para>
	/// This parameter is required and must be set for the chart to render.
	/// </para>
	/// </remarks>
	[Parameter, EditorRequired]
	public ChartOptions Options { get; set; } = default!;

	/// <summary>
	/// Generates a random unique identifier suitable for chart IDs.
	/// </summary>
	/// <returns>A 32-character hexadecimal string without hyphens.</returns>
	/// <remarks>
	/// Uses <see cref="Guid.NewGuid()"/> with format "N" to generate IDs that are:
	/// - Unique across the application
	/// - Valid as HTML element IDs (no special characters)
	/// - Suitable for use in JavaScript interop
	/// </remarks>
	public static string GenerateRandomId() => Guid.NewGuid().ToString("N");

	/// <summary>
	/// Performs asynchronous cleanup of chart resources.
	/// </summary>
	/// <returns>A <see cref="ValueTask"/> representing the asynchronous dispose operation.</returns>
	/// <remarks>
	/// <para>
	/// This method:
	/// - Removes the chart from its group (if assigned)
	/// - Disposes the ECharts instance in the browser
	/// - Clears cached serialization options
	/// - Suppresses finalization to improve GC performance
	/// </para>
	/// <para>
	/// This method is called automatically by the Blazor framework when the component is disposed.
	/// </para>
	/// </remarks>
	public async ValueTask DisposeAsync()
	{
		GC.SuppressFinalize(this);

		try
		{
			jsonOpts = null;

			// remove the chart from a group (if needed)
			Group?.Remove(this);

			await JSRuntime.InvokeVoidAsync("panoramicDataECharts.disposeChart", Id);
		}
		catch { }
	}

	/// <summary>
	/// Clears all content from the chart without disposing it.
	/// </summary>
	/// <returns>A <see cref="ValueTask"/> representing the asynchronous clear operation.</returns>
	/// <remarks>
	/// <para>
	/// This method clears the chart's visual content while keeping the ECharts instance alive.
	/// The chart can be repopulated with new data using <see cref="UpdateAsync"/>.
	/// </para>
	/// <para>
	/// This is useful when you want to temporarily hide chart content or prepare for
	/// a complete data refresh.
	/// </para>
	/// </remarks>
	public async ValueTask ClearAsync()
	{
		try
		{
			await JSRuntime.InvokeVoidAsync("panoramicDataECharts.clearChart", Id);
		}
		catch { }
	}

	/// <summary>
	/// Updates the chart with current options and optionally executes the data loader.
	/// </summary>
	/// <param name="executeDataLoader">
	/// If <c>true</c>, invokes the <see cref="DataLoader"/> callback to fetch external data
	/// before updating the chart. Default is <c>true</c>.
	/// </param>
	/// <returns>A <see cref="Task"/> representing the asynchronous update operation.</returns>
	/// <remarks>
	/// <para>
	/// This method must be implemented by derived classes to provide chart-specific
	/// update logic.
	/// </para>
	/// <para>
	/// Typical update operations include:
	/// - Serializing the current <see cref="Options"/>
	/// - Fetching external data if needed
	/// - Invoking JavaScript interop to update the chart in the browser
	/// </para>
	/// </remarks>
	public abstract Task UpdateAsync(bool executeDataLoader = true);

	/// <summary>
	/// Serializes the chart options, map definitions, and external data sources to JSON.
	/// </summary>
	/// <param name="serializeMapOpts">
	/// If <c>true</c>, includes map definitions in the serialization output.
	/// Set to <c>false</c> if maps are already registered.
	/// </param>
	/// <returns>
	/// A tuple containing:
	/// <list type="bullet">
	/// <item><term>chartOpts</term><description>Serialized chart options JSON string</description></item>
	/// <item><term>mapOpts</term><description>Serialized map definitions JSON string, or null if no maps</description></item>
	/// <item><term>fetchOpts</term><description>Serialized external data fetch commands JSON string, or null if no external data</description></item>
	/// </list>
	/// </returns>
	/// <remarks>
	/// <para>
	/// This method uses the configured JSON serializer options to ensure proper
	/// conversion of C# objects to ECharts-compatible JSON format.
	/// </para>
	/// <para>
	/// External data sources are converted to fetch commands that the JavaScript
	/// layer can execute.
	/// </para>
	/// </remarks>
	protected (string chartOpts, string? mapOpts, string? fetchOpts) Serialize(bool serializeMapOpts)
	{
		// serialize the chart options
		var chartOpts = JsonSerializer.Serialize(Options, jsonOpts);

		// serialize the map options
		var mapOpts = (!serializeMapOpts || Maps == null || Maps.Length == 0) ? null : JsonSerializer.Serialize<object>(Maps, jsonOpts);

		string? fetchOpts = null;
		if (ExternalDataSources != null && ExternalDataSources.Length != 0)
		{
			// NOTE: use the default converter here, because our custom converter throws an exception
			fetchOpts = JsonSerializer.Serialize(ExternalDataSources.Select(s => new FetchCommand(s)));
		}

		return (chartOpts, mapOpts, fetchOpts);
	}

	/// <summary>
	/// Creates and configures a JSON serializer options instance for chart serialization.
	/// </summary>
	/// <returns>
	/// A configured <see cref="JsonSerializerOptions"/> instance with appropriate converters
	/// and settings for ECharts serialization.
	/// </returns>
	/// <remarks>
	/// <para>
	/// The method returns a cached instance if <see cref="CacheJsonSerializerOptions"/> is <c>true</c>
	/// and a cached instance exists, improving performance.
	/// </para>
	/// <para>
	/// The serializer is configured with:
	/// - CamelCase property naming (matches JavaScript conventions)
	/// - Null value omission (cleaner JSON output)
	/// - Indented formatting in DEBUG builds
	/// - Custom converters for dates, series data, and external data sources
	/// </para>
	/// <para>
	/// <strong>Note:</strong> Custom converters cannot be changed after the first use
	/// of a cached instance.
	/// </para>
	/// </remarks>
	protected JsonSerializerOptions CreateSerializerOptions()
	{
		// return cached instance if possible
		if (CacheJsonSerializerOptions && cachedJsonOpts != null)
		{
			//NOTE: custom converters cannot be changed after first use of cachedJsonOpts
			return cachedJsonOpts;
		}

		var jsonOpts = new JsonSerializerOptions()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
#if DEBUG
			WriteIndented = true
#endif
		};

		jsonOpts.Converters.Add(new DateOnlyJsonConverter());
		jsonOpts.Converters.Add(new DateTimeJsonConverter());
		jsonOpts.Converters.Add(new DateTimeOffsetJsonConverter());
		jsonOpts.Converters.Add(new SeriesDataConverterFactory());

		// register extra JSON converters if needed
		if (JsonConverters != null)
		{
			foreach (var converter in JsonConverters)
			{
				jsonOpts.Converters.Add(converter);
			}
		}

		// register the special converter for external data source fetches
		jsonOpts.Converters.Add(new ExternalDataSourceConverter());
		jsonOpts.Converters.Add(new ExternalDataSourceRefConverter());

		// store the json opts for re-use later on
		if (CacheJsonSerializerOptions && cachedJsonOpts == null)
			cachedJsonOpts = jsonOpts;

		return jsonOpts;
	}
}
