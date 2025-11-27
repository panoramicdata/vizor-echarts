using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<ScatterEffectType>))]
public enum ScatterEffectType
{
	Ripple
}
