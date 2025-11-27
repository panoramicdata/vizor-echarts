using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

/// <summary>
/// Specifies how tooltips are triggered when interacting with the chart.
/// </summary>
/// <remarks>
/// <para>
/// The trigger mode determines what causes the tooltip to display:
/// </para>
/// <list type="bullet">
/// <item>
/// <term>Item</term>
/// <description>Best for scatter plots, pie charts, and other non-axis charts</description>
/// </item>
/// <item>
/// <term>Axis</term>
/// <description>Best for line charts, bar charts, and other category-based charts</description>
/// </item>
/// <item>
/// <term>None</term>
/// <description>Disables automatic tooltip triggering</description>
/// </item>
/// </list>
/// <para>
/// See: https://echarts.apache.org/en/option.html#tooltip.trigger
/// </para>
/// </remarks>
[JsonConverter(typeof(CamelCaseEnumConverter<TooltipTrigger>))]
public enum TooltipTrigger
{
	/// <summary>
	/// Triggered by individual data items.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Use for charts where each data point is independent, such as:
	/// - Scatter charts
	/// - Pie charts
	/// - Map charts
	/// - Graph/network charts
	/// </para>
	/// <para>
	/// Tooltip appears when hovering over a specific data point or chart element.
	/// </para>
	/// </remarks>
	Item,

	/// <summary>
	/// Triggered by chart axes.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Use for charts with category axes, such as:
	/// - Line charts
	/// - Bar charts
	/// - Area charts
	/// - Time series charts
	/// </para>
	/// <para>
	/// Tooltip appears when hovering anywhere on the chart and shows all series values
	/// for that axis position. An axis pointer line/shadow is typically displayed.
	/// </para>
	/// </remarks>
	Axis,

	/// <summary>
	/// Disables automatic tooltip triggering.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Use when you want to manually control tooltip display via API calls or
	/// when tooltips are not needed for the chart.
	/// </para>
	/// <para>
	/// Tooltips can still be triggered programmatically using the dispatchAction API.
	/// </para>
	/// </remarks>
	None
}