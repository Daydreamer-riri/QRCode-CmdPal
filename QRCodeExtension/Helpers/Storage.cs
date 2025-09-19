using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace QRCodeExtension.Helpers;

public class Storage
{
    public event EventHandler? HistoryChanged
    {
        add => _history.Changed += value;
        remove => _history.Changed -= value;
    }

    private readonly HistoryStore _history;

#pragma warning disable CA1822 // Mark members as static
    public int HistoryItemCount => 50;
#pragma warning restore CA1822 // Mark members as static

    public IReadOnlyList<HistoryItem> HistoryItems => _history.HistoryItems;


    public Storage()
    {
        _history = new HistoryStore(HistoryStateJsonPath(), HistoryItemCount);
    }

    private static string HistoryStateJsonPath()
    {
        var directory = Utilities.BaseSettingsPath("QRCodeExtension");
        Directory.CreateDirectory(directory);

        // now, the state is just next to the exe
        return Path.Combine(directory, "qrcode_history.json");
    }

    private string _lastAddedItemContent = null!;

    public void AddHistoryItem(string searchString)
    {
        if (searchString == _lastAddedItemContent)
        {
            return;
        }
        _lastAddedItemContent = searchString;
        try
        {
            _history.Add(new HistoryItem(searchString, DateTime.Now));
        }
        catch (Exception ex)
        {
            ExtensionHost.LogMessage(new LogMessage() { Message = ex.ToString() });
        }
    }

    public void RemoveHistoryItem(string searchString)
    {
        try
        {
            _history.Remove(searchString);
        }
        catch (Exception ex)
        {
            ExtensionHost.LogMessage(new LogMessage() { Message = ex.ToString() });
        }
    }
}

