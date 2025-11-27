using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<ColorBy>))]
public enum ColorBy
{
	Series,
	Data
}
