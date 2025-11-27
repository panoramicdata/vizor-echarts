namespace PanoramicData.ECharts.BindingGenerator.Types;

internal interface IPropertyType
{
	string Name { get; }

	string DotNetType { get; }

	string? TypeWarning { get; set; }
}
