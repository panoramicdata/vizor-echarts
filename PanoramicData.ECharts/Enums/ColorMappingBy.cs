using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<ColorMappingBy>))]
public enum ColorMappingBy
{
	Index,
	Value,
	Id
}
