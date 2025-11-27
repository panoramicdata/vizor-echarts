using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<TreeLayout>))]
public enum TreeLayout
{
	Curve,
	Polyline
}
