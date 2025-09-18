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

    public int HistoryItemCount => 50;

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

    public void AddHistoryItem(HistoryItem historyItem)
    {
        try
        {
            _history.Add(historyItem);
        }
        catch (Exception ex)
        {
            ExtensionHost.LogMessage(new LogMessage() { Message = ex.ToString() });
        }
    }
}

