// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

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
        return [.. allItems];
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        allItems = [
            new ListItem(new DetailPage(newSearch))
            {
                Title = newSearch,
                Subtitle = $"Generate QR Code for \"{newSearch}\"",
                Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png"),
            }
        ];
        RaiseItemsChanged();
    }
}
