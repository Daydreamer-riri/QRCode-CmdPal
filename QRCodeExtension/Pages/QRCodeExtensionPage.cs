// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using QRCodeExtension.Helpers;

namespace QRCodeExtension;

internal sealed partial class QRCodeExtensionPage : DynamicListPage
{
    private List<ListItem> allItems;
    public QRCodeExtensionPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = "QR Code";
        Name = "Open";
        ShowDetails = false;

        allItems = [];
    }

    public override IListItem[] GetItems()
    {
        if (allItems.Count == 0)
        {
            return [
                new ListItem(new NoOpCommand())
                {
                    Title = "Type to generate QR Code",
                    Subtitle = "Enter any text to generate its QR code",
                    Icon = Icons.QRCode,
                }
            ];
        }
        return [.. allItems];
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        if (string.IsNullOrWhiteSpace(newSearch))
        {
            allItems = [];
            RaiseItemsChanged();
            return;
        }
        allItems = [
            new ListItem(new DetailPage(newSearch))
            {
                Title = newSearch,
                Subtitle = $"Generate QR Code for \"{newSearch}\"",
                Icon = Icons.QRCode,
            }
        ];
        RaiseItemsChanged();
    }
}
