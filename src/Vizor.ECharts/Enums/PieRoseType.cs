﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vizor.ECharts;

[JsonConverter(typeof(CamelCaseEnumConverter<PieRoseType>))]
public enum PieRoseType
{
	Radius,
	Area,
	True,
	False
}