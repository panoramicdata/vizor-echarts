using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<BrushType>))]
public enum BrushType
{
	Stroke,
	Fill
}
