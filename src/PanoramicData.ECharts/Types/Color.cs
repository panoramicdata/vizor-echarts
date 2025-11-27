using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

/// <summary>
/// Represents a color value for ECharts visualization elements.
/// </summary>
/// <remarks>
/// <para>
/// Colors in ECharts can be specified in multiple formats:
/// </para>
/// <list type="bullet">
/// <item><description>Hex color: <c>"#ff0000"</c> or <c>"#f00"</c></description></item>
/// <item><description>RGB: <c>"rgb(255, 0, 0)"</c></description></item>
/// <item><description>RGBA: <c>"rgba(255, 0, 0, 0.5)"</c></description></item>
/// <item><description>Named color: <c>"red"</c>, <c>"transparent"</c></description></item>
/// <item><description>Gradient: <see cref="LinearGradient"/> or <see cref="RadialGradient"/></description></item>
/// </list>
/// <para>
/// See: https://echarts.apache.org/en/option.html#color
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Hex color
/// var color1 = Color.FromHex("ff0000");
/// var color2 = new Color("#00ff00");
///
/// // RGB/RGBA
/// var color3 = Color.FromRGB(255, 0, 0);
/// var color4 = Color.FromRGBA(255, 0, 0, 0.5);
///
/// // Transparent
/// var color5 = Color.Transparent;
///
/// // Gradient
/// var gradient = new LinearGradient(0, 0, 1, 0)
/// {
///     ColorStops = new[]
///     {
///         new GradientColorStop { Offset = 0, Color = "red" },
///         new GradientColorStop { Offset = 1, Color = "blue" }
///     }
/// };
/// var color6 = new Color(gradient);
/// </code>
/// </example>
[JsonConverter(typeof(ColorConverter))]
public class Color
{
	private readonly string? color;
	private readonly object? graphicColor;

	/// <summary>
	/// Initializes a new instance of the <see cref="Color"/> class with a color string.
	/// </summary>
	/// <param name="color">
	/// The color value in one of the supported formats:
	/// hex (<c>"#ccc"</c>), RGB (<c>"rgb(128, 128, 128)"</c>),
	/// RGBA (<c>"rgba(128, 128, 128, 0.5)"</c>), or named color (<c>"transparent"</c>).
	/// </param>
	/// <remarks>
	/// Consider using the convenience methods <see cref="FromHex"/>, <see cref="FromRGB"/>,
	/// or <see cref="FromRGBA"/> for type-safe color creation.
	/// </remarks>
	public Color(string color)
	{
		this.color = color;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Color"/> class with a gradient.
	/// </summary>
	/// <param name="graphicColor">
	/// A gradient color object (<see cref="LinearGradient"/> or <see cref="RadialGradient"/>).
	/// </param>
	public Color(IGraphicColor graphicColor)
	{
		this.graphicColor = graphicColor;
	}

	/// <summary>
	/// Gets the color string value, or <c>null</c> if a gradient is used.
	/// </summary>
	/// <value>
	/// The color string in hex, RGB, RGBA, or named format.
	/// </value>
	public string? Value => color;

	/// <summary>
	/// Gets the gradient color object, or <c>null</c> if a string color is used.
	/// </summary>
	/// <value>
	/// A <see cref="LinearGradient"/> or <see cref="RadialGradient"/> instance.
	/// </value>
	/// <remarks>
	/// Returns <see cref="object"/> instead of <see cref="IGraphicColor"/> to maintain
	/// JSON serialization compatibility in .NET 6+.
	/// </remarks>
	public object? GraphicColor => graphicColor; //NOTE: in .NET 6 we cannot use IGraphicColor, it will break serialization

	/// <summary>
	/// Implicitly converts a color string to a <see cref="Color"/>.
	/// </summary>
	/// <param name="color">The color string.</param>
	public static implicit operator Color(string color)
	{
		return new Color(color);
	}

	/// <summary>
	/// Implicitly converts a <see cref="LinearGradient"/> to a <see cref="Color"/>.
	/// </summary>
	/// <param name="color">The linear gradient.</param>
	public static implicit operator Color(LinearGradient color)
	{
		return new Color(color);
	}

	/// <summary>
	/// Implicitly converts a <see cref="RadialGradient"/> to a <see cref="Color"/>.
	/// </summary>
	/// <param name="color">The radial gradient.</param>
	public static implicit operator Color(RadialGradient color)
	{
		return new Color(color);
	}

	/// <summary>
	/// Creates a <see cref="Color"/> from a hexadecimal color string.
	/// </summary>
	/// <param name="hex">
	/// The hex color string with or without the '#' prefix (e.g., <c>"ff0000"</c> or <c>"#ff0000"</c>).
	/// </param>
	/// <returns>A new <see cref="Color"/> instance.</returns>
	/// <example>
	/// <code>
	/// var red = Color.FromHex("ff0000");
	/// var blue = Color.FromHex("#0000ff");
	/// </code>
	/// </example>
	public static Color FromHex(string hex) => hex.StartsWith('#') ? new Color(hex) : new Color('#' + hex);

	/// <summary>
	/// Creates a <see cref="Color"/> from RGB values.
	/// </summary>
	/// <param name="r">The red component (0-255).</param>
	/// <param name="g">The green component (0-255).</param>
	/// <param name="b">The blue component (0-255).</param>
	/// <returns>A new <see cref="Color"/> instance.</returns>
	/// <example>
	/// <code>
	/// var red = Color.FromRGB(255, 0, 0);
	/// var purple = Color.FromRGB(128, 0, 128);
	/// </code>
	/// </example>
	public static Color FromRGB(byte r, byte g, byte b) => new($"rgb({(int)r}, {(int)g}, {(int)b})");

	/// <summary>
	/// Creates a <see cref="Color"/> from RGBA values with transparency.
	/// </summary>
	/// <param name="r">The red component (0-255).</param>
	/// <param name="g">The green component (0-255).</param>
	/// <param name="b">The blue component (0-255).</param>
	/// <param name="a">The alpha (opacity) value (0.0-1.0, where 0 is fully transparent and 1 is fully opaque).</param>
	/// <returns>A new <see cref="Color"/> instance.</returns>
	/// <example>
	/// <code>
	/// var semiTransparentRed = Color.FromRGBA(255, 0, 0, 0.5);
	/// var almostInvisibleBlue = Color.FromRGBA(0, 0, 255, 0.1);
	/// </code>
	/// </example>
	public static Color FromRGBA(byte r, byte g, byte b, double a) => new($"rgba({(int)r}, {(int)g}, {(int)b}, {a})");

	/// <summary>
	/// Gets a fully transparent color.
	/// </summary>
	/// <value>
	/// A <see cref="Color"/> with the value <c>"transparent"</c>.
	/// </value>
	public static Color Transparent => new("transparent");
}

/// <summary>
/// JSON converter for serializing <see cref="Color"/> instances to ECharts-compatible JSON.
/// </summary>
/// <remarks>
/// <para>
/// This converter handles both string colors and gradient objects:
/// </para>
/// <list type="bullet">
/// <item><description>String colors are written as JSON strings</description></item>
/// <item><description>Gradients are written as JavaScript constructor calls for ECharts</description></item>
/// </list>
/// </remarks>
public class ColorConverter : JsonConverter<Color>
{
	private static readonly ColorConverter instance = new();

	/// <summary>
	/// Reads and converts JSON to a <see cref="Color"/> instance.
	/// </summary>
	/// <param name="reader">The JSON reader.</param>
	/// <param name="typeToConvert">The type to convert.</param>
	/// <param name="options">The serializer options.</param>
	/// <returns>A <see cref="Color"/> instance.</returns>
	public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => new(reader.GetString() ?? "");

	/// <summary>
	/// Writes a <see cref="Color"/> instance as JSON.
	/// </summary>
	/// <param name="writer">The JSON writer.</param>
	/// <param name="value">The <see cref="Color"/> to serialize.</param>
	/// <param name="options">The serializer options.</param>
	/// <remarks>
	/// <para>
	/// Serialization behavior:
	/// </para>
	/// <list type="bullet">
	/// <item><description>String colors: written as JSON strings (e.g., <c>"#ff0000"</c>)</description></item>
	/// <item><description>Linear gradients: written as <c>new echarts.graphic.LinearGradient(...)</c></description></item>
	/// <item><description>Radial gradients: written as <c>new echarts.graphic.RadialGradient(...)</c></description></item>
	/// </list>
	/// </remarks>
	public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
	{
		if (value.GraphicColor != null)
		{
			string colorStops = "null";
			string global = "null";
			if (value.GraphicColor is Gradient g)
			{
				colorStops = JsonSerializer.Serialize(g.ColorStops, options);
				global = g.Global switch
				{
					true => "true",
					false => "false",
					_ => "null"
				};
			}

			switch (value.GraphicColor)
			{
				case LinearGradient lg:
					writer.WriteRawValue($"new echarts.graphic.LinearGradient({lg.X}, {lg.Y}, {lg.X2}, {lg.Y2}, {colorStops}, {global})", true);
					break;
				case RadialGradient rg:
					writer.WriteRawValue($"new echarts.graphic.RadialGradient({rg.X}, {rg.Y}, {rg.R}, {colorStops}, {global})", true);
					break;
				default:
					throw new NotSupportedException($"Serialization of type {value.GraphicColor.GetType()} not supported");
			}
		}
		else
		{
			writer.WriteStringValue(value.Value);
		}
	}

	/// <summary>
	/// Gets the singleton instance of the <see cref="ColorConverter"/>.
	/// </summary>
	public static ColorConverter Instance => instance;
}