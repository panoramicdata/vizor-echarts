
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

public partial class MinorSplitLine
{
	/// <summary>
	/// If show minor split lines.
	/// </summary>
	[JsonPropertyName("show")]
	[DefaultValue("true")]
	public bool? Show { get; set; }

	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("lineStyle")]
	public LineStyle? LineStyle { get; set; }

}
