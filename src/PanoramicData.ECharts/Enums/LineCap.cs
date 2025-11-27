using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<LineCap>))]
public enum LineCap
{
    Butt,
    Round,
    Square
}
