
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

public partial class ScaleLimit
{
	/// <summary>
	/// Minimum scaling
	/// </summary>
	[JsonPropertyName("min")]
	public double? Min { get; set; }

	/// <summary>
	/// Maximum scaling
	/// </summary>
	[JsonPropertyName("max")]
	public double? Max { get; set; }

}
