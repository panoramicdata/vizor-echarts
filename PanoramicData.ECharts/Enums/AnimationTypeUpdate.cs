using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<AnimationTypeUpdate>))]
public enum AnimationTypeUpdate
{
	Transition,
	Expansion
}