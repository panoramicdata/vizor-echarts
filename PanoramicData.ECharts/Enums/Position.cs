using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

/// <summary>
/// Specifies the position of chart elements relative to the container.
/// </summary>
/// <remarks>
/// <para>
/// Used to position various chart components including:
/// - Legend placement
/// - Title positioning
/// - Label alignment
/// - Tooltip placement
/// - DataZoom controls
/// </para>
/// <para>
/// See: https://echarts.apache.org/en/option.html#legend.left
/// </para>
/// </remarks>
[JsonConverter(typeof(CamelCaseEnumConverter<Position>))]
public enum Position
{
	/// <summary>
	/// Positions the element at the left edge of the container.
	/// </summary>
	Left,

	/// <summary>
	/// Positions the element at the right edge of the container.
	/// </summary>
	Right,

	/// <summary>
	/// Positions the element at the top edge of the container.
	/// </summary>
	Top,

	/// <summary>
	/// Positions the element at the bottom edge of the container.
	/// </summary>
	Bottom
}
