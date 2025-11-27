
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

public partial class Rich
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("RichStyleName")]
	public RichStyleName? RichStyleName { get; set; }

}
