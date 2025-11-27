using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<ImageType>))]
public enum ImageType
{
	Png,
	Jpg
}
