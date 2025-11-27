using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<ExternalDataFetchAs>))]
public enum ExternalDataFetchAs
{
	Json,
	String
}
