using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

/// <summary>
/// Specifies the orientation (layout direction) for chart components.
/// </summary>
/// <remarks>
/// <para>
/// Used to control the layout direction of various chart components including:
/// - Legend positioning
/// - Toolbox button arrangement
/// - DataZoom slider orientation
/// - Timeline layout
/// </para>
/// <para>
/// See: https://echarts.apache.org/en/option.html#legend.orient
/// </para>
/// </remarks>
[JsonConverter(typeof(CamelCaseEnumConverter<Orient>))]
public enum Orient
{
	/// <summary>
	/// Arranges elements horizontally from left to right.
	/// </summary>
	/// <remarks>
	/// Use for horizontal layouts where elements should be placed side by side.
	/// Common for legends at the top or bottom of charts.
	/// </remarks>
	Horizontal,

	/// <summary>
	/// Arranges elements vertically from top to bottom.
	/// </summary>
	/// <remarks>
	/// Use for vertical layouts where elements should be stacked.
	/// Common for legends on the left or right side of charts.
	/// </remarks>
	Vertical
}
