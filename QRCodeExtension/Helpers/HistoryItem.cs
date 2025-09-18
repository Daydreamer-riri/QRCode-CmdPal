using System;
using System.Text.Json;

namespace QRCodeExtension.Helpers;

public class HistoryItem(string searchString, DateTime timestamp)
{
    public string SearchString { get; private set; } = searchString;

    public DateTime Timestamp { get; private set; } = timestamp;

    public string ToJson() => JsonSerializer.Serialize(this, QRCodeSerializationContext.Default.HistoryItem);
}