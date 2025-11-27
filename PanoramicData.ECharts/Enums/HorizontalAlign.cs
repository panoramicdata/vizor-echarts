using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

/// <summary>
/// Specifies the horizontal alignment of chart elements.
/// </summary>
/// <remarks>
/// <para>
/// Used for aligning:
/// - Text labels and titles
/// - Legend items
/// - Axis labels
/// - Tooltip content
/// </para>
/// <para>
/// See: https://echarts.apache.org/en/option.html#legend.align
/// </para>
/// </remarks>
[JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlign>))]
public enum HorizontalAlign
{
	/// <summary>
	/// Automatically determines alignment based on position and layout.
	/// </summary>
	/// <remarks>
	/// ECharts will choose the most appropriate alignment based on the element's position.
	/// </remarks>
	Auto,

	/// <summary>
	/// Aligns the element to the left.
	/// </summary>
	/// <remarks>
	/// Text and content will be aligned flush with the left edge.
	/// </remarks>
	Left,

	/// <summary>
	/// Aligns the element to the right.
	/// </summary>
	/// <remarks>
	/// Text and content will be aligned flush with the right edge.
	/// </remarks>
	Right,

	/// <summary>
	/// Centers the element horizontally.
	/// </summary>
	/// <remarks>
	/// Text and content will be centered horizontally within the available space.
	/// </remarks>
	Center
}