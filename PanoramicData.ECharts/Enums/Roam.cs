using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverterWithBoolean<Roam>))]
public enum Roam
{
	True,
	False,
	Scale,
	Move
}
