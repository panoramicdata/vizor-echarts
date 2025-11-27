using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<LeftOrRight>))]
public enum LeftOrRight
{
	Left,
	Right
}
