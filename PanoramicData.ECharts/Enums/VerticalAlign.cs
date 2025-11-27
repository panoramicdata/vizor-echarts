using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

/// <summary>
/// Specifies the vertical alignment of chart elements.
/// </summary>
/// <remarks>
/// <para>
/// Used for aligning:
/// - Text labels and titles vertically
/// - Legend items within their container
/// - Axis label positioning
/// - Tooltip content alignment
/// </para>
/// <para>
/// See: https://echarts.apache.org/en/option.html#title.textVerticalAlign
/// </para>
/// </remarks>
[JsonConverter(typeof(CamelCaseEnumConverter<VerticalAlign>))]
public enum VerticalAlign
{
	/// <summary>
	/// Automatically determines vertical alignment based on position and layout.
	/// </summary>
	/// <remarks>
	/// ECharts will choose the most appropriate vertical alignment based on the element's position.
	/// </remarks>
	Auto,

	/// <summary>
	/// Aligns the element to the top.
	/// </summary>
	/// <remarks>
	/// Text and content will be aligned flush with the top edge.
	/// </remarks>
	Top,

	/// <summary>
	/// Aligns the element to the bottom.
	/// </summary>
	/// <remarks>
	/// Text and content will be aligned flush with the bottom edge.
	/// </remarks>
	Bottom,

	/// <summary>
	/// Centers the element vertically.
	/// </summary>
	/// <remarks>
	/// Text and content will be centered vertically within the available space.
	/// Useful for ensuring visual balance in labels and titles.
	/// </remarks>
	Middle
}