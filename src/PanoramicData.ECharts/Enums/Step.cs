using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverterWithBoolean<Step>))]
public enum Step
{
    True,
    False,
    Start,
    Middle,
    End
}