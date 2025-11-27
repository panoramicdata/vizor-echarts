using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<StackStrategy>))]
public enum StackStrategy
{
    SameSign,
    All,
    Positive,
    Negative
}
