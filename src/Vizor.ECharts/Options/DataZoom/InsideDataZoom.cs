// AUTO GENERATED - DO NOT EDIT - All changes will be lost
// http://www.datahint.eu/


using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Vizor.ECharts;

public partial class InsideDataZoom
{
	/// <summary>
	/// 
	/// </summary>
	[JsonPropertyName("type")]
	[DefaultValue("inside")]
	public string? Type { get; set; }  = "inside";

	/// <summary>
	/// Component ID, not specified by default.
	/// If specified, it can be used to refer the component in option or API.
	/// </summary>
	[JsonPropertyName("id")]
	public string? Id { get; set; } 

	/// <summary>
	/// Whether disable inside zoom.
	/// </summary>
	[JsonPropertyName("disabled")]
	[DefaultValue(false)]
	public bool? Disabled { get; set; } 

	/// <summary>
	/// Specify which xAxis is/are controlled by the dataZoom-inside when catesian coordinate system is used.
	///  
	/// By default the first xAxis that parallel to dataZoom are controlled when dataZoom-inside.orient is set as 'horizontal' .
	/// But it is recommended to specify it explicitly but not use default value.
	///  
	/// If it is set as a single number , one axis is controlled, while if it is set as an Array , multiple axes are controlled.
	///  
	/// For example:  option: {
	///     xAxis: [
	///         {...}, // The first xAxis
	///         {...}, // The second xAxis
	///         {...}, // The third xAxis
	///         {...}  // The fourth xAxis
	///     ],
	///     dataZoom: [
	///         { // The first dataZoom component
	///             xAxisIndex: [0, 2] // Indicates that this dataZoom component
	///                                      // controls the first and the third xAxis
	///         },
	///         { // The second dataZoom component
	///             xAxisIndex: 3      // indicates that this dataZoom component
	///                                      // controls the fourth xAxis
	///         }
	///     ]
	/// }
	/// </summary>
	[JsonPropertyName("xAxisIndex")]
	public NumberOrNumberArray? XAxisIndex { get; set; } 

	/// <summary>
	/// Specify which yAxis is/are controlled by the dataZoom-inside when catesian coordinate system is used.
	///  
	/// By default the first yAxis that parallel to dataZoom are controlled when dataZoom-inside.orient is set as 'vertical' .
	/// But it is recommended to specify it explicitly but not use default value.
	///  
	/// If it is set as a single number , one axis is controlled, while if it is set as an Array , multiple axes are controlled.
	///  
	/// For example:  option: {
	///     yAxis: [
	///         {...}, // The first yAxis
	///         {...}, // The second yAxis
	///         {...}, // The third yAxis
	///         {...}  // The fourth yAxis
	///     ],
	///     dataZoom: [
	///         { // The first dataZoom component
	///             yAxisIndex: [0, 2] // Indicates that this dataZoom component
	///                                      // controls the first and the third yAxis
	///         },
	///         { // The second dataZoom component
	///             yAxisIndex: 3      // indicates that this dataZoom component
	///                                      // controls the fourth yAxis
	///         }
	///     ]
	/// }
	/// </summary>
	[JsonPropertyName("yAxisIndex")]
	public NumberOrNumberArray? YAxisIndex { get; set; } 

	/// <summary>
	/// Specify which radiusAxis is/are controlled by the dataZoom-inside when polar coordinate system is used.
	///  
	/// If it is set as a single number , one axis is controlled, while if it is set as an Array , multiple axes are controlled.
	///  
	/// For example:  option: {
	///     radiusAxis: [
	///         {...}, // The first radiusAxis
	///         {...}, // The second radiusAxis
	///         {...}, // The third radiusAxis
	///         {...}  // The fourth radiusAxis
	///     ],
	///     dataZoom: [
	///         { // The first dataZoom component
	///             radiusAxisIndex: [0, 2] // Indicates that this dataZoom component
	///                                      // controls the first and the third radiusAxis
	///         },
	///         { // The second dataZoom component
	///             radiusAxisIndex: 3      // indicates that this dataZoom component
	///                                      // controls the fourth radiusAxis
	///         }
	///     ]
	/// }
	/// </summary>
	[JsonPropertyName("radiusAxisIndex")]
	public NumberOrNumberArray? RadiusAxisIndex { get; set; } 

	/// <summary>
	/// Specify which angleAxis is/are controlled by the dataZoom-inside when polar coordinate system is used.
	///  
	/// If it is set as a single number , one axis is controlled, while if it is set as an Array , multiple axes are controlled.
	///  
	/// For example:  option: {
	///     angleAxis: [
	///         {...}, // The first angleAxis
	///         {...}, // The second angleAxis
	///         {...}, // The third angleAxis
	///         {...}  // The fourth angleAxis
	///     ],
	///     dataZoom: [
	///         { // The first dataZoom component
	///             angleAxisIndex: [0, 2] // Indicates that this dataZoom component
	///                                      // controls the first and the third angleAxis
	///         },
	///         { // The second dataZoom component
	///             angleAxisIndex: 3      // indicates that this dataZoom component
	///                                      // controls the fourth angleAxis
	///         }
	///     ]
	/// }
	/// </summary>
	[JsonPropertyName("angleAxisIndex")]
	public NumberOrNumberArray? AngleAxisIndex { get; set; } 

	/// <summary>
	/// Generally dataZoom component zoom or roam coordinate system through data filtering and set the windows of axes internally.
	///  
	/// Its behaviours vary according to filtering mode settings ( dataZoom.filterMode ).
	///  
	/// Possible values:   
	/// 'filter': data that outside the window will be filtered , which may lead to some changes of windows of other axes.
	/// For each data item, it will be filtered if one of the relevant dimensions is out of the window.
	///   
	/// 'weakFilter': data that outside the window will be filtered , which may lead to some changes of windows of other axes.
	/// For each data item, it will be filtered only if all of the relevant dimensions are out of the same side of the window.
	///   
	/// 'empty': data that outside the window will be set to NaN , which will not lead to changes of windows of other axes.
	///   
	/// 'none': Do not filter data.
	///    
	/// How to set filterMode is up to users, depending on the requirments and scenarios.
	/// Expirically:   
	/// If only xAxis or only yAxis is controlled by dataZoom , filterMode: 'filter' is typically used, which enable the other axis auto adapte its window to the extent of the filtered data.
	///   
	/// If both xAxis and yAxis are operated by dataZoom :   
	/// If xAxis and yAxis should not effect mutually (e.g.
	/// a scatter chart with both axes on the type of 'value' ), they should be set to be filterMode: 'empty' .
	///   
	/// If xAxis is the main axis and yAxis is the auxiliary axis (or vise versa) (e.g., in a bar chart, when dragging dataZoomX to change the window of xAxis, we need the yAxis to adapt to the clipped data, but when dragging dataZoomY to change the window of yAxis, we need the xAxis not to be changed), in this case, xAxis should be set to be filterMode: 'filter' , while yAxis should be set to be filterMode: 'empty' .
	///      
	/// It can be demonstrated by the sample:  option = {
	///     dataZoom: [
	///         {
	///             id: 'dataZoomX',
	///             type: 'slider',
	///             xAxisIndex: [0],
	///             filterMode: 'filter'
	///         },
	///         {
	///             id: 'dataZoomY',
	///             type: 'slider',
	///             yAxisIndex: [0],
	///             filterMode: 'empty'
	///         }
	///     ],
	///     xAxis: {type: 'value'},
	///     yAxis: {type: 'value'},
	///     series{
	///         type: 'bar',
	///         data: [
	///             // The first column corresponds to xAxis,
	///             // and the second coloum corresponds to yAxis.
	///             [12, 24, 36],
	///             [90, 80, 70],
	///             [3, 9, 27],
	///             [1, 11, 111]
	///         ]
	///     }
	/// }  
	/// In the sample above, dataZoomX is set as filterMode: 'filter' .
	/// When use drags dataZoomX (do not touch dataZoomY ) and the valueWindow of xAxis is changed to [2, 50] consequently, dataZoomX travel the first column of series.data and filter items that out of the window.
	/// The series.data turns out to be:  [
	///     [12, 24, 36],
	///     // [90, 80, 70] This item is filtered, as 90 is out of the window.
	///     [3, 9, 27]
	///     // [1, 11, 111] This item is filtered, as 1 is out of the window.
	/// ]  
	/// Before filtering, the second column, which corresponds to yAxis, has values 24 , 80 , 9 , 11 .
	/// After filtering, only 24 and 9 are left.
	/// Then the extent of yAxis is adjusted to adapt the two values (if yAxis.min and yAxis.max are not set).
	///  
	/// So filterMode: 'filter' can be used to enable the other axis to auto adapt the filtered data.
	///  
	/// Then let's review the sample from the beginning, dataZoomY is set as filterMode: 'empty' .
	/// So if user drags dataZoomY (do not touch dataZoomX ) and its window is changed to [10, 60] consequently, dataZoomY travels the second column of series.data and set NaN to all of the values that outside the window (NaN cause the graphical elements, i.e., bar elements, do not show, but still hold the place).
	/// The series.data turns out to be:  [
	///     [12, 24, 36],
	///     [90, NaN, 70], // Set to NaN
	///     [3, NaN, 27],  // Set to NaN
	///     [1, 11, 111]
	/// ]  
	/// In this case, the first column (i.e., 12 , 90 , 3 , 1 , which corresponds to xAxis ), will not be changed at all.
	/// So dragging yAxis will not change extent of xAxis , which is good for requirements like outlier filtering.
	///  
	/// See this example:
	/// </summary>
	[JsonPropertyName("filterMode")]
	[DefaultValue("filter")]
	//TODO: Type Warning: enum type 'filterMode' in 'InsideDataZoom' with values 'filter,weakFilter,empty,none' is not mapped
	public string? FilterMode { get; set; } 

	/// <summary>
	/// The start percentage of the window out of the data extent, in the range of 0 ~ 100.
	///  
	/// dataZoom-inside.start and dataZoom-inside.end define the window of the data in percent form.
	///  
	/// More info about the relationship between dataZoom-inside.start and axis extent can be checked in dataZoom-inside.rangeMode .
	/// </summary>
	[JsonPropertyName("start")]
	[DefaultValue(0)]
	public double? Start { get; set; } 

	/// <summary>
	/// The end percentage of the window out of the data extent, in the range of 0 ~ 100.
	///  
	/// dataZoom-inside.start and dataZoom-inside.end define the window of the data in percent form.
	///  
	/// More info about the relationship between dataZoom-inside.end and axis extent can be checked in dataZoom-inside.rangeMode .
	/// </summary>
	[JsonPropertyName("end")]
	[DefaultValue("100")]
	public double? End { get; set; } 

	/// <summary>
	/// The start absolute value of the window, not works when dataZoom-inside.start is set.
	///  
	/// dataZoom-inside.startValue and dataZoom-inside.endValue define the window of the data window in absolute value form.
	///  
	/// Notice, if an axis is set to be category , startValue could be set as index of the array of axis.data or as the array value itself.
	/// In the latter case, it will internally and automatically translate to the index of array.
	///  
	/// More info about the relationship between dataZoom-inside.startValue and axis extent can be checked in dataZoom-inside.rangeMode .
	/// </summary>
	[JsonPropertyName("startValue")]
	//TODO: Type Warning: Failed to map property 'startValue' in type 'InsideDataZoom' with types 'date,number,string'
	public object? StartValue { get; set; } 

	/// <summary>
	/// The end absolute value of the window, doesn't work when dataZoom-inside.end is set.
	///  
	/// dataZoom-inside.startValue and dataZoom-inside.endValue define the window of the data window in absolute value form.
	///  
	/// Notice, if an axis is set to be category , startValue could be set as index of the array of axis.data or as the array value itself.
	/// In the latter case, it will internally and automatically translate to the index of array.
	///  
	/// More info about the relationship between dataZoom-inside.endValue and axis extent can be checked in dataZoom-inside.rangeMode .
	/// </summary>
	[JsonPropertyName("endValue")]
	//TODO: Type Warning: Failed to map property 'endValue' in type 'InsideDataZoom' with types 'date,number,string'
	public object? EndValue { get; set; } 

	/// <summary>
	/// Used to restrict minimal window size, in percent, which value is in the range of [0, 100].
	///  
	/// If dataZoom-inside.minValueSpan is set, minSpan does not work any more.
	/// </summary>
	[JsonPropertyName("minSpan")]
	public double? MinSpan { get; set; } 

	/// <summary>
	/// Used to restrict maximal window size, in percent, which value is in the range of [0, 100].
	///  
	/// If dataZoom-inside.maxValueSpan is set, maxSpan does not work any more.
	/// </summary>
	[JsonPropertyName("maxSpan")]
	public double? MaxSpan { get; set; } 

	/// <summary>
	/// Used to restrict minimal window size.
	///  
	/// For example:
	/// In time axis it can be set as 3600 * 24 * 1000 * 5 to represent "5 day".
	/// In category axis it can be set as 5 to represent 5 categories.
	/// </summary>
	[JsonPropertyName("minValueSpan")]
	//TODO: Type Warning: Failed to map property 'minValueSpan' in type 'InsideDataZoom' with types 'date,number,string'
	public object? MinValueSpan { get; set; } 

	/// <summary>
	/// Used to restrict maximal window size.
	///  
	/// For example:
	/// In time axis it can be set as 3600 * 24 * 1000 * 5 to represent "5 day".
	/// In category axis it can be set as 5 to represent 5 categories.
	/// </summary>
	[JsonPropertyName("maxValueSpan")]
	//TODO: Type Warning: Failed to map property 'maxValueSpan' in type 'InsideDataZoom' with types 'date,number,string'
	public object? MaxValueSpan { get; set; } 

	/// <summary>
	/// Specify whether the layout of dataZoom component is horizontal or vertical.
	/// What's more, it indicates whether the horizontal axis or vertical axis is controlled by default in catesian coordinate system.
	///  
	/// Valid values:   
	/// 'horizontal' : horizontal.
	///   
	/// 'vertical' : vertical.
	/// </summary>
	[JsonPropertyName("orient")]
	public Orient? Orient { get; set; } 

	/// <summary>
	/// Specify whether to lock the size of window (selected area).
	///  
	/// When set as true , the size of window is locked, that is, only the translation (by mouse drag or touch drag) is avialable but zoom is not.
	/// </summary>
	[JsonPropertyName("zoomLock")]
	[DefaultValue(false)]
	public bool? ZoomLock { get; set; } 

	/// <summary>
	/// Specify the frame rate of views refreshing, with unit millisecond (ms).
	///  
	/// If animation set as true and animationDurationUpdate set as bigger than 0 , you can keep throttle as the default value 100 (or set it as a value bigger than 0 ), otherwise it might be not smooth when dragging.
	///  
	/// If animation set as false or animationDurationUpdate set as 0 , and data size is not very large, and it seems to be not smooth when dragging, you can set throttle as 0 to improve that.
	/// </summary>
	[JsonPropertyName("throttle")]
	[DefaultValue("100")]
	public double? Throttle { get; set; } 

	/// <summary>
	/// The format is [rangeModeForStart, rangeModeForEnd] .
	///  
	/// For example rangeMode: ['value', 'percent'] means that use absolute value in start and percent value in end .
	///  
	/// Optional value for each item: 'value' , 'percent' .
	///   'value' mode: the axis extent will always only be determined by dataZoom.startValue and dataZoom.endValue , despite how data like and how axis.min and axis.max are.
	///  'percent' mode: 100 represents 100% of the [dMin, dMax] , where dMin is axis.min if axis.min specified, otherwise data.extent[0] , and dMax is axis.max if axis.max specified, otherwise data.extent[1] .
	/// Axis extent will only be determined by the result of the percent of [dMin, dMax] .
	///   
	/// rangeMode are auto determined by whether option.start / option.end are specified (represents 'percent' mode) or option.startValue / option.endValue specified (represents 'value' mode).
	/// And when user behavior trigger the changing of the view, the rangeMode would be modified automatically.
	/// For example, if triggered by toolbox.dataZoom , it will be modefied to 'value' , and if triggered by dataZoom-inside or dataZoom-slider , it will be modified to 'percent' .
	///  
	/// If we specify rangeMode manually in option , it only works when both start and startValue specified or both end and endValue specified.
	/// So usually we do not need to specify dataZoom.rangeMode manually.
	///  
	/// Take a scenario as an example.
	/// When we are using dynamic data (update data periodically via setOption ), if in 'value ' mode, the window will be kept in a fixed value range despite how data are appended, while if in 'percent' mode, whe window range will be changed alone with the appended data (suppose axis.min and axis.max are not specified).
	/// </summary>
	[JsonPropertyName("rangeMode")]
	//TODO: Type Warning: array type 'rangeMode' in 'InsideDataZoom' will be mapped to List<object>
	public List<object>? RangeMode { get; set; } 

	/// <summary>
	/// How to trigger zoom.
	/// Optional values:   true ：Mouse wheel triggers zoom.
	///  false ：Mouse wheel can not triggers zoom.
	///  'shift' ：Holding shift and mouse wheel triggers zoom.
	///  'ctrl' ：Holding ctrl and mouse wheel triggers zoom.
	///  'alt' ：Holding alt and mouse wheel triggers zoom.
	/// </summary>
	[JsonPropertyName("zoomOnMouseWheel")]
	[DefaultValue("true")]
	//TODO: Type Warning: Failed to map property 'zoomOnMouseWheel' in type 'InsideDataZoom' with types 'boolean,enum'
	public object? ZoomOnMouseWheel { get; set; } 

	/// <summary>
	/// How to trigger data window move.
	/// Optional values:   true ：Mouse move triggers data window move.
	///  false ：Mouse move can not triggers data window move.
	///  'shift' ：Holding shift and mouse move triggers data window move.
	///  'ctrl' ：Holding ctrl and mouse move triggers data window move.
	///  'alt' ：Holding alt and mouse move triggers data window move.
	/// </summary>
	[JsonPropertyName("moveOnMouseMove")]
	[DefaultValue("true")]
	//TODO: Type Warning: Failed to map property 'moveOnMouseMove' in type 'InsideDataZoom' with types 'boolean,enum'
	public object? MoveOnMouseMove { get; set; } 

	/// <summary>
	/// How to trigger data window move.
	/// Optional values:   true ：Mouse wheel triggers data window move.
	///  false ：Mouse wheel can not triggers data window move.
	///  'shift' ：Holding shift and mouse wheel triggers data window move.
	///  'ctrl' ：Holding ctrl and mouse wheel triggers data window move.
	///  'alt' ：Holding alt and mouse wheel triggers data window move.
	/// </summary>
	[JsonPropertyName("moveOnMouseWheel")]
	[DefaultValue("true")]
	//TODO: Type Warning: Failed to map property 'moveOnMouseWheel' in type 'InsideDataZoom' with types 'boolean,enum'
	public object? MoveOnMouseWheel { get; set; } 

	/// <summary>
	/// Whether to prevent default behavior of mouse move event.
	/// </summary>
	[JsonPropertyName("preventDefaultMouseMove")]
	[DefaultValue("true")]
	public bool? PreventDefaultMouseMove { get; set; } 

}
