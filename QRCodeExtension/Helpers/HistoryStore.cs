using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace QRCodeExtension.Helpers;

internal sealed class HistoryStore
{
    private readonly string _filePath;
    private readonly List<HistoryItem> _items = [];
    private readonly Lock _lock = new();

    private int _capacity;

    public event EventHandler? Changed;

    public HistoryStore(string filePath, int capacity)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);

        _filePath = filePath;
        _capacity = capacity;

        _items.AddRange(LoadFromDiskSafe());
        TrimNoLock();
    }

    public IReadOnlyList<HistoryItem> HistoryItems
    {
        get
        {
            lock (_lock)
            {
                return [.. _items];
            }
        }
    }

    public void Add(HistoryItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        lock (_lock)
        {
            _items.RemoveAll(i => i.SearchString == item.SearchString);
            _items.Add(item);
            _ = TrimNoLock();
            SaveNoLock();
        }

        Changed?.Invoke(this, EventArgs.Empty);
    }

    public void SetCapacity(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);

        bool trimmed;
        lock (_lock)
        {
            _capacity = capacity;
            trimmed = TrimNoLock();
            if (trimmed)
            {
                SaveNoLock();
            }
        }

        if (trimmed)
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool TrimNoLock()
    {
        var max = _capacity;
        if (_items.Count > max)
        {
            _items.RemoveRange(0, _items.Count - max);
            return true;
        }

        return false;
    }

    private List<HistoryItem> LoadFromDiskSafe()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return [];
            }

            var fileContent = File.ReadAllText(_filePath);
            var historyItems = JsonSerializer.Deserialize<List<HistoryItem>>(fileContent, QRCodeSerializationContext.Default.ListHistoryItem) ?? [];
            return historyItems;
        }
        catch
        {
            return [];
        }
    }

    private void SaveNoLock()
    {
        var json = JsonSerializer.Serialize(_items, QRCodeSerializationContext.Default.ListHistoryItem);
        File.WriteAllText(_filePath, json);
    }
}