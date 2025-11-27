using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverterWithBoolean<SelectionMode>))]
public enum SelectionMode
{
	True,
	False,
	Single,
	Multiple
}