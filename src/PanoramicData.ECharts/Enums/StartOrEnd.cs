using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<StartOrEnd>))]
public enum StartOrEnd
{
	Start,
	End
}
