using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<EmphasisFocus>))]
public enum EmphasisFocus
{
    None,
    Self,
    Series
}