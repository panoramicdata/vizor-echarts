using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

public class ExternalDataSourceRef
{
	public ExternalDataSourceRef(ExternalDataSource dataSource, string? path = null)
	{
		FetchId = dataSource.FetchId;
		Path = path;
	}

	public ExternalDataSourceRef(string fetchId, string? path = null)
	{
		FetchId = fetchId;
		Path = path;
	}

	public string FetchId { get; }
	public string? Path { get; }
}


public class ExternalDataSourceRefConverter : JsonConverter<ExternalDataSourceRef>
{
	internal ExternalDataSourceRefConverter()
	{
	}

	public override ExternalDataSourceRef Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException("Deserialization is not implemented for ExternalDataSourceRef.");

	public override void Write(Utf8JsonWriter writer, ExternalDataSourceRef value, JsonSerializerOptions options)
	{
		string raw = $"window.panoramicDataECharts.getDataSource('{value.FetchId}')";
		if (value.Path != null)
		{
			if (!value.Path.StartsWith('.'))
				raw += '.';
			raw += value.Path;
		}

		writer.WriteRawValue(raw, skipInputValidation: true);
	}
}