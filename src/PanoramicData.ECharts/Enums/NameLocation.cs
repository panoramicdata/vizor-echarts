using System.Text.Json.Serialization;
using PanoramicData.ECharts;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<NameLocation>))]
public enum NameLocation
{
	Start,
	Middle,
	End
}