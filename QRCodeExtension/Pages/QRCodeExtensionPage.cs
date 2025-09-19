// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using QRCodeExtension.Helpers;

namespace QRCodeExtension;

internal sealed partial class QRCodeExtensionPage : DynamicListPage, IDisposable
{
    private readonly Lock _sync = new();
    private IListItem[] allItems = [];
    private readonly Storage _storage;
    public QRCodeExtensionPage(Storage storage)
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = "QR Code";
        Name = "Open";
        ShowDetails = true;
        EmptyContent = new CommandItem(new NoOpCommand())
        {
            Icon = Icon,
            Title = "Type to generate QR Code",
            Subtitle = "Enter any text to generate its QR code",
        };
        _storage = storage;

        _storage.HistoryChanged += StorageOnHistoryChanged;

        RequeryAndUpdateItems(SearchText);
    }

    private void StorageOnHistoryChanged(object? sender, EventArgs e)
    {
        RequeryAndUpdateItems(SearchText);
    }

    private void RequeryAndUpdateItems(string search)
    {
        List<QRCodeListItem> historySnapshot;
        lock (_sync)
        {
            historySnapshot = [
                .. _storage.HistoryItems
            .Select(item =>
                new QRCodeListItem(item.SearchString, _storage){ TextToSuggest = item.SearchString }
            )
            .Reverse()
            ];
        }

        var items = Query(search ?? string.Empty, historySnapshot, _storage);

        lock (_sync)
        {
            allItems = items;
        }

        RaiseItemsChanged(allItems.Length);
    }

    private static IListItem[] Query(string query, List<QRCodeListItem> historySnapshot, Storage storage)
    {
        ArgumentNullException.ThrowIfNull(query);

        var filteredHistoryItems = storage.HistoryItemCount > 0
            ? ListHelpers.FilterList(historySnapshot, query)
            : [];

        var results = new List<IListItem>();

        if (!string.IsNullOrEmpty(query))
        {
            var searchTerm = query;
            var result = new QRCodeListItem(searchTerm, storage);
            results.Add(result);
        }
        // else
        // {
        //     results.Add(new ListItem(new NoOpCommand())
        //     {
        //         Title = "Type to generate QR Code",
        //         Subtitle = "Enter any text to generate its QR code",
        //         Icon = Icons.QRCode,
        //         TextToSuggest = string.Empty,
        //     });
        // }

        results.AddRange(filteredHistoryItems);

        return [.. results];
    }


    public override IListItem[] GetItems()
    {
        return allItems;
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        RequeryAndUpdateItems(newSearch);
    }

    public void Dispose()
    {
        _storage.HistoryChanged -= StorageOnHistoryChanged;
        GC.SuppressFinalize(this);
    }
}
