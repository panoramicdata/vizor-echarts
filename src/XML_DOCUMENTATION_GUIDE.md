# XML Documentation Guide for PanoramicData.ECharts

## Overview

This guide provides standards and templates for adding XML documentation comments throughout the PanoramicData.ECharts library.

---

## Why XML Comments?

1. **IntelliSense Support** - Rich tooltips in Visual Studio and other IDEs
2. **API Documentation** - Generates documentation websites via DocFX or Sandcastle
3. **Code Clarity** - Helps developers understand intent and usage
4. **Maintainability** - Makes future changes easier to understand

---

## Documentation Standards

### General Rules

? **DO** document all public types, members, properties, and methods  
? **DO** include `<summary>` for all documented elements  
? **DO** include `<param>` for all method parameters  
? **DO** include `<returns>` for all methods that return values  
? **DO** include `<example>` for complex or non-obvious usage  
? **DO** include `<remarks>` for additional context, warnings, or best practices  
? **DO** use `<see cref=""/>` to reference other types  
? **DO** use `<c>` for inline code  
? **DO** use `<code>` for multi-line code examples  

? **DON'T** just repeat the member name in the summary  
? **DON'T** use vague descriptions like "Gets or sets the value"  
? **DON'T** forget to update docs when changing code  

---

## Templates by File Type

### 1. Enum Types

```csharp
/// <summary>
/// Specifies [what the enum represents] for [context].
/// </summary>
/// <remarks>
/// [Additional context, ECharts documentation links, or usage notes]
/// See: https://echarts.apache.org/en/option.html#[relevant-path]
/// </remarks>
[JsonConverter(typeof(EnumConverter<EnumName>))]
public enum EnumName
{
    /// <summary>
    /// [Description of what this value means and when to use it]
    /// </summary>
    [JsonPropertyName("valueName")]
    ValueName,

    /// <summary>
    /// [Description]
    /// </summary>
    [JsonPropertyName("anotherValue")]
    AnotherValue
}
```

**Example:**
```csharp
/// <summary>
/// Specifies the alignment position for chart elements.
/// </summary>
/// <remarks>
/// Used for positioning labels, legends, and other UI elements within the chart.
/// See: https://echarts.apache.org/en/option.html#legend.align
/// </remarks>
[JsonConverter(typeof(EnumConverter<HorizontalAlign>))]
public enum HorizontalAlign
{
    /// <summary>
    /// Aligns the element to the left edge.
    /// </summary>
    [JsonPropertyName("left")]
    Left,

    /// <summary>
    /// Centers the element horizontally.
    /// </summary>
    [JsonPropertyName("center")]
    Center,

    /// <summary>
    /// Aligns the element to the right edge.
    /// </summary>
    [JsonPropertyName("right")]
    Right
}
```

---

### 2. Type Wrapper Classes

```csharp
/// <summary>
/// Represents a value that can be [list the types it can hold].
/// </summary>
/// <remarks>
/// <para>
/// This type is commonly used in ECharts options where a property can accept multiple value types.
/// </para>
/// <para>
/// [Explain when/why you'd use each type option]
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // [Usage example 1]
/// TypeName value1 = [example];
/// 
/// // [Usage example 2]
/// TypeName value2 = [example];
/// </code>
/// </example>
[JsonConverter(typeof(TypeNameConverter))]
public class TypeName
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeName"/> class with [what this constructor takes].
    /// </summary>
    /// <param name="paramName">The [description of parameter].</param>
    /// <remarks>
    /// [Any special notes about this constructor]
    /// </remarks>
    public TypeName(Type paramName)
    {
        PropertyName = paramName;
    }

    /// <summary>
    /// Gets the [description of what this property represents], or <c>null</c> if [other option is used].
    /// </summary>
    /// <value>
    /// [Detailed description of the property value]
    /// </value>
    public Type? PropertyName { get; }

    /// <summary>
    /// Implicitly converts a <see cref="Type"/> to a <see cref="TypeName"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A new <see cref="TypeName"/> instance.</returns>
    public static implicit operator TypeName(Type value)
    {
        return new TypeName(value);
    }
}
```

---

### 3. Option Classes

```csharp
/// <summary>
/// Configures [what aspect of the chart this configures].
/// </summary>
/// <remarks>
/// <para>
/// [Explain the purpose and usage of this option]
/// </para>
/// <para>
/// See: https://echarts.apache.org/en/option.html#[path]
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var options = new ChartOptions
/// {
///     OptionName = new OptionClass
///     {
///         Property1 = value1,
///         Property2 = value2
///     }
/// };
/// </code>
/// </example>
public class OptionClass
{
    /// <summary>
    /// Gets or sets [what this property controls].
    /// </summary>
    /// <value>
    /// [Description of valid values and their effects]
    /// Default is [default value if applicable].
    /// </value>
    /// <remarks>
    /// [Additional context, warnings, or usage notes]
    /// </remarks>
    public Type? PropertyName { get; set; }
}
```

---

### 4. Series Classes

```csharp
/// <summary>
/// Represents a [chart type] series for displaying [what kind of data].
/// </summary>
/// <remarks>
/// <para>
/// [Explain what this chart type is good for]
/// </para>
/// <para>
/// See: https://echarts.apache.org/en/option.html#series-[type]
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var series = new SeriesType
/// {
///     Name = "Series Name",
///     Data = new[] { 1, 2, 3, 4, 5 }
/// };
/// </code>
/// </example>
public class SeriesType : ISeries
{
    /// <summary>
    /// Gets the series type identifier.
    /// </summary>
    /// <value>
    /// Always returns "[type]" for this series type.
    /// </value>
    public string Type => "[type]";

    /// <summary>
    /// Gets or sets the series data.
    /// </summary>
    /// <value>
    /// An array of data values or data objects.
    /// </value>
    public object? Data { get; set; }
}
```

---

### 5. Converter Classes

```csharp
/// <summary>
/// JSON converter for serializing <see cref="TypeName"/> instances to ECharts-compatible JSON.
/// </summary>
/// <remarks>
/// <para>
/// This converter handles [describe what conversions it performs].
/// </para>
/// <para>
/// Deserialization is not supported as data flows one-way from C# to JavaScript.
/// </para>
/// </remarks>
public class TypeNameConverter : JsonConverter<TypeName>
{
    /// <summary>
    /// Reads and converts JSON to a <see cref="TypeName"/> instance.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The deserialized <see cref="TypeName"/> instance.</returns>
    /// <exception cref="NotImplementedException">
    /// Always thrown as deserialization is not implemented.
    /// </exception>
    public override TypeName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException("Deserialization is not implemented for TypeName.");
    }

    /// <summary>
    /// Writes a <see cref="TypeName"/> instance as JSON.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
    /// <param name="value">The <see cref="TypeName"/> value to serialize.</param>
    /// <param name="options">The serializer options.</param>
    /// <remarks>
    /// [Describe the serialization logic and output format]
    /// </remarks>
    public override void Write(Utf8JsonWriter writer, TypeName value, JsonSerializerOptions options)
    {
        // Implementation
    }
}
```

---

## Common Patterns

### Properties with Multiple Types

```csharp
/// <summary>
/// Gets or sets [the property purpose].
/// </summary>
/// <value>
/// Can be:
/// <list type="bullet">
/// <item><description>A <see cref="string"/> - [when to use]</description></item>
/// <item><description>A <see cref="double"/> - [when to use]</description></item>
/// <item><description>A <see cref="JavascriptFunction"/> - [when to use]</description></item>
/// </list>
/// </value>
public NumberOrStringOrFunction? PropertyName { get; set; }
```

### ECharts Documentation Links

Always include links to the official ECharts documentation:

```csharp
/// <remarks>
/// See: https://echarts.apache.org/en/option.html#series-line.smooth
/// </remarks>
```

### Warnings and Important Notes

```csharp
/// <remarks>
/// <para>
/// <strong>Warning:</strong> Changing this value after the chart is rendered may cause unexpected behavior.
/// </para>
/// </remarks>
```

### Browser/Version Compatibility

```csharp
/// <remarks>
/// <para>
/// This feature requires ECharts 5.0 or later.
/// </para>
/// <para>
/// Not supported in Internet Explorer 11.
/// </para>
/// </remarks>
```

---

## Examples by Category

### Simple Property

```csharp
/// <summary>
/// Gets or sets the chart width.
/// </summary>
/// <value>
/// A CSS unit string (e.g., "800px", "100%") or null to use the container's width.
/// </value>
public string? Width { get; set; }
```

### Complex Property with Options

```csharp
/// <summary>
/// Gets or sets the tooltip configuration.
/// </summary>
/// <value>
/// A <see cref="Tooltip"/> instance defining tooltip appearance and behavior,
/// or null to disable tooltips.
/// </value>
/// <remarks>
/// <para>
/// Tooltips are shown when the user hovers over chart elements.
/// Configure trigger mode, formatting, and positioning through the <see cref="Tooltip"/> properties.
/// </para>
/// <para>
/// See: https://echarts.apache.org/en/option.html#tooltip
/// </para>
/// </remarks>
/// <example>
/// <code>
/// Options = new ChartOptions
/// {
///     Tooltip = new Tooltip
///     {
///         Trigger = TooltipTrigger.Axis,
///         AxisPointer = new AxisPointer { Type = AxisPointerType.Cross }
///     }
/// };
/// </code>
/// </example>
public Tooltip? Tooltip { get; set; }
```

### Methods

```csharp
/// <summary>
/// Updates the chart with the current options.
/// </summary>
/// <param name="executeDataLoader">
/// If <c>true</c>, invokes the data loader callback to fetch external data before updating.
/// Default is <c>true</c>.
/// </param>
/// <returns>
/// A <see cref="Task"/> representing the asynchronous update operation.
/// </returns>
/// <remarks>
/// <para>
/// Call this method after changing chart options to apply the changes.
/// </para>
/// <para>
/// The chart is automatically updated on initial render, so you only need to call this
/// for subsequent updates.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Update without fetching external data
/// await chart.UpdateAsync(executeDataLoader: false);
/// 
/// // Update and fetch external data
/// await chart.UpdateAsync();
/// </code>
/// </example>
public async Task UpdateAsync(bool executeDataLoader = true)
{
    // Implementation
}
```

---

## File-Specific Guidelines

### Enums Folder

- Document each enum value's meaning and usage
- Include ECharts documentation links
- Mention JavaScript equivalent values (from JsonPropertyName)

### Types Folder

- Explain why the union type exists
- Document all possible type combinations
- Show conversion examples
- Explain serialization behavior

### Series Folder

- Describe what data visualization the series provides
- Link to ECharts series documentation
- Show basic usage example
- List key properties

### Options Folder

- Explain what chart aspect this configures
- Link to ECharts option documentation
- Show configuration example
- Document property interactions

### Internal Folder

- Mark with `<remarks>` as internal implementation details
- Document for maintainers, not end users
- Explain converter logic and special cases

---

## Tools and Automation

### Visual Studio XML Comments Extension

Install: **GhostDoc** or **Visual Studio IntelliCode**

### DocFX for Documentation Generation

```powershell
# Install DocFX
dotnet tool install -g docfx

# Generate documentation
docfx init
docfx build
docfx serve
```

### StyleCop Analyzers

Add to enforce documentation:

```xml
<PackageReference Include="StyleCop.Analyzers" Version="1.2.0-beta.507">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
```

Enable in `.editorconfig`:
```ini
# SA1600: Elements should be documented
dotnet_diagnostic.SA1600.severity = warning
```

---

## Verification Checklist

Before submitting documented code:

- [ ] All public types have `<summary>` tags
- [ ] All public members have `<summary>` tags
- [ ] All parameters have `<param>` tags
- [ ] All return values have `<returns>` tags
- [ ] Complex types have `<example>` tags
- [ ] Important notes use `<remarks>` tags
- [ ] ECharts links included where relevant
- [ ] No spelling errors in comments
- [ ] Build succeeds without warnings
- [ ] IntelliSense shows rich tooltips

---

## Priority Order

1. **High Priority** (Public API surface)
   - EChartBase.cs ? (Already documented)
   - ChartOptions.cs
   - Series classes (ISeries implementations)
   - Common option classes (Tooltip, Legend, Title, Axis)

2. **Medium Priority** (Commonly used types)
   - Type wrappers (Color, NumberOrString, etc.)
   - Common enums (Orient, Position, Align, etc.)
   - Data classes (SeriesData, etc.)

3. **Lower Priority** (Advanced features)
   - Less common option classes
   - Specialized series types
   - Internal utilities

---

## Example: Complete File Documentation

See `PanoramicData.ECharts\EChartBase.cs` for a fully documented example following these guidelines.

---

## Questions?

For questions about documentation standards, consult:
- Microsoft's XML Documentation Guide: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
- This guide
- Existing documented files in the project

---

**Last Updated**: 2025-01-27  
**Status**: Active Development  
**Maintained By**: Development Team
