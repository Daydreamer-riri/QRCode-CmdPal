using System.Collections.Generic;
using System.Text.Json.Serialization;
using QRCodeExtension.Helpers;

namespace QRCodeExtension;

[JsonSerializable(typeof(float))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(HistoryItem))]
[JsonSerializable(typeof(List<HistoryItem>))]
[JsonSourceGenerationOptions(UseStringEnumConverter = true, WriteIndented = true, IncludeFields = true, PropertyNameCaseInsensitive = true, AllowTrailingCommas = true)]
internal sealed partial class QRCodeSerializationContext : JsonSerializerContext
{
}