using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverterWithBoolean<AreaStyleOrigin>))]
public enum AreaStyleOrigin
{
	Auto,
	Start,
	End
}
