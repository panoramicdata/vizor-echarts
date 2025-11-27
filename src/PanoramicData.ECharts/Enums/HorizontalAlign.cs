using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlign>))]
public enum HorizontalAlign
{
    Auto,
    Left,
    Right,
    Center
}