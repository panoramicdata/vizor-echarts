using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<ShowEffectOn>))]
public enum ShowEffectOn
{
	Render,
	Emphasis
}
