using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<MarkPointType>))]
public enum MarkPointType
{
	Min,
	Max,
	Average
}
