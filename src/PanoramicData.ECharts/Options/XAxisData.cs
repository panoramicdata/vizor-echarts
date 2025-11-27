
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

public partial class XAxisData
{
	/// <summary>
	/// Name of a category.
	/// </summary>
	[JsonPropertyName("value")]
	public string? Value { get; set; } 

	/// <summary>
	/// Text style of the category.
	/// </summary>
	[JsonPropertyName("textStyle")]
	public TextStyle? TextStyle { get; set; } 

}
