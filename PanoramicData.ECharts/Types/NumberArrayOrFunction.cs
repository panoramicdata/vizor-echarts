using System.Text.Json;
using System.Text.Json.Serialization;

namespace PanoramicData.ECharts;

/// <summary>
/// Represents a value that can be either a single number, an array of numbers, or a JavaScript function.
/// </summary>
/// <remarks>
/// <para>
/// This type is commonly used in ECharts options where a property can accept multiple value types,
/// such as padding, margins, or position values that can be specified as:
/// - A single number applied uniformly
/// - An array of numbers for specific positioning
/// - A JavaScript function for dynamic calculation
/// </para>
/// <para>
/// The type includes implicit conversion operators for convenient usage.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Single number
/// NumberArrayOrFunction padding = 10;
/// 
/// // Array of numbers
/// NumberArrayOrFunction padding = new double[] { 10, 20, 30, 40 };
/// 
/// // JavaScript function
/// NumberArrayOrFunction padding = new JavascriptFunction("function(params) { return params.value * 2; }");
/// </code>
/// </example>
[JsonConverter(typeof(NumberArrayOrFunctionConverter))]
public class NumberArrayOrFunction
{
	/// <summary>
	/// Initializes a new instance of the <see cref="NumberArrayOrFunction"/> class with a single number.
	/// </summary>
	/// <param name="number">The numeric value to be used uniformly.</param>
	/// <remarks>
	/// The number is internally stored as a single-element array.
	/// </remarks>
	public NumberArrayOrFunction(double number)
	{
		Numbers = [number];
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NumberArrayOrFunction"/> class with an array of numbers.
	/// </summary>
	/// <param name="numbers">The array of numeric values.</param>
	/// <remarks>
	/// Use this constructor when different values are needed for different positions
	/// (e.g., top, right, bottom, left padding).
	/// </remarks>
	public NumberArrayOrFunction(double[] numbers)
	{
		Numbers = numbers;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="NumberArrayOrFunction"/> class with a JavaScript function.
	/// </summary>
	/// <param name="function">The JavaScript function for dynamic value calculation.</param>
	/// <remarks>
	/// The function will be executed in the browser context and should return appropriate numeric values.
	/// </remarks>
	public NumberArrayOrFunction(JavascriptFunction function)
	{
		Function = function;
	}

	/// <summary>
	/// Gets the array of numeric values, or <c>null</c> if a function is used instead.
	/// </summary>
	/// <value>
	/// An array of <see cref="double"/> values, or <c>null</c> if <see cref="Function"/> is set.
	/// </value>
	public double[]? Numbers { get; }

	/// <summary>
	/// Gets the JavaScript function, or <c>null</c> if numeric values are used instead.
	/// </summary>
	/// <value>
	/// A <see cref="JavascriptFunction"/> instance, or <c>null</c> if <see cref="Numbers"/> is set.
	/// </value>
	public JavascriptFunction? Function { get; }

	/// <summary>
	/// Implicitly converts a <see cref="double"/> to a <see cref="NumberArrayOrFunction"/>.
	/// </summary>
	/// <param name="number">The number to convert.</param>
	/// <returns>A new <see cref="NumberArrayOrFunction"/> instance containing the number.</returns>
	/// <example>
	/// <code>
	/// NumberArrayOrFunction value = 42.0;
	/// </code>
	/// </example>
	public static implicit operator NumberArrayOrFunction(double number)
	{
		return new NumberArrayOrFunction(number);
	}

	/// <summary>
	/// Implicitly converts an array of <see cref="double"/> to a <see cref="NumberArrayOrFunction"/>.
	/// </summary>
	/// <param name="numbers">The array of numbers to convert.</param>
	/// <returns>A new <see cref="NumberArrayOrFunction"/> instance containing the array.</returns>
	/// <example>
	/// <code>
	/// NumberArrayOrFunction value = new double[] { 10, 20, 30, 40 };
	/// </code>
	/// </example>
	public static implicit operator NumberArrayOrFunction(double[] numbers)
	{
		return new NumberArrayOrFunction(numbers);
	}

	/// <summary>
	/// Implicitly converts a <see cref="JavascriptFunction"/> to a <see cref="NumberArrayOrFunction"/>.
	/// </summary>
	/// <param name="function">The JavaScript function to convert.</param>
	/// <returns>A new <see cref="NumberArrayOrFunction"/> instance containing the function.</returns>
	/// <example>
	/// <code>
	/// NumberArrayOrFunction value = new JavascriptFunction("function() { return 100; }");
	/// </code>
	/// </example>
	public static implicit operator NumberArrayOrFunction(JavascriptFunction function)
	{
		return new NumberArrayOrFunction(function);
	}
}

/// <summary>
/// JSON converter for serializing <see cref="NumberArrayOrFunction"/> instances to ECharts-compatible JSON.
/// </summary>
/// <remarks>
/// <para>
/// This converter handles three serialization cases:
/// - Single number: serialized as a JSON number
/// - Array of numbers: serialized as a JSON array
/// - JavaScript function: serialized using the JavascriptFunction converter
/// </para>
/// <para>
/// Deserialization is not supported as data flows one-way from C# to JavaScript.
/// </para>
/// </remarks>
public class NumberArrayOrFunctionConverter : JsonConverter<NumberArrayOrFunction>
{
	/// <summary>
	/// Reads and converts JSON to a <see cref="NumberArrayOrFunction"/> instance.
	/// </summary>
	/// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
	/// <param name="typeToConvert">The type to convert.</param>
	/// <param name="options">The serializer options.</param>
	/// <returns>The deserialized <see cref="NumberArrayOrFunction"/> instance.</returns>
	/// <exception cref="NotImplementedException">
	/// Always thrown as deserialization is not implemented.
	/// </exception>
	/// <remarks>
	/// Deserialization is not supported because chart options are only sent from C# to JavaScript,
	/// never read back.
	/// </remarks>
	public override NumberArrayOrFunction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException("Deserialization is not implemented for NumberArrayOrFunction.");

	/// <summary>
	/// Writes a <see cref="NumberArrayOrFunction"/> instance as JSON.
	/// </summary>
	/// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
	/// <param name="value">The <see cref="NumberArrayOrFunction"/> value to serialize.</param>
	/// <param name="options">The serializer options.</param>
	/// <remarks>
	/// <para>
	/// Serialization behavior:
	/// </para>
	/// <list type="bullet">
	/// <item>
	/// <term>Single number</term>
	/// <description>Written as a JSON number (e.g., <c>10</c>)</description>
	/// </item>
	/// <item>
	/// <term>Array of numbers</term>
	/// <description>Written as a JSON array (e.g., <c>[10, 20, 30, 40]</c>)</description>
	/// </item>
	/// <item>
	/// <term>JavaScript function</term>
	/// <description>Written as a raw JavaScript function using <see cref="JavascriptFunctionConverter"/></description>
	/// </item>
	/// </list>
	/// </remarks>
	public override void Write(Utf8JsonWriter writer, NumberArrayOrFunction value, JsonSerializerOptions options)
	{
		if (value.Numbers != null)
		{
			if (value.Numbers.Length == 1)
			{
				writer.WriteNumberValue(value.Numbers[0]);
			}
			else
			{
				writer.WriteStartArray();
				foreach (var val in value.Numbers)
				{
					writer.WriteNumberValue(val);
				}

				writer.WriteEndArray();
			}
		}
		else if (value.Function != null)
		{
			JavascriptFunctionConverter.Instance.Write(writer, value.Function, options);
		}
	}
}