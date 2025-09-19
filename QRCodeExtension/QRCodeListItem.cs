using System;
using Microsoft.CommandPalette.Extensions.Toolkit;
using QRCodeExtension.Helpers;

namespace QRCodeExtension;

public partial class QRCodeListItem : ListItem
{
    public QRCodeListItem(string content, Storage storage)
    : base(new DetailPage(content, storage))
    {
        Title = content;
        Subtitle = $"Generate QR Code for \"{content}\"";
        Icon = Icons.QRCode;

        var _details = new Details()
        {
            Body = $$"""
            {{content}}

            <br>

            {{BuildImageMarkdownContent(content)}}
            """,
        };

        CommandContextItem deleteCommandItem = new(
            title: "Delete",
            name: "Delete",
            result: CommandResult.KeepOpen(),
            action: () =>
            {
                storage.RemoveHistoryItem(content);
            }
        )
        {
            IsCritical = true,
            Icon = Icons.Delete,
        };

        CommandContextItem showDetailsCommandItem = null!;
        CommandContextItem hideDetailsCommandItem = null!;

        hideDetailsCommandItem = new CommandContextItem(
            title: "Hide Details",
            name: "Hide Details",
            result: CommandResult.KeepOpen(),
            action: () =>
            {
                Details = null;
                MoreCommands = [
                    showDetailsCommandItem,
                    deleteCommandItem
                ];
            }
        )
        { Icon = Icons.Info };

        showDetailsCommandItem = new CommandContextItem(
            title: "Show Details",
            name: "Show Details",
            result: CommandResult.KeepOpen(),
            action: () =>
            {
                Details = _details;
                MoreCommands = [
                    hideDetailsCommandItem,
                    deleteCommandItem
                ];
            }
        )
        { Icon = Icons.Info };

        MoreCommands = [
            showDetailsCommandItem,
            deleteCommandItem
        ];
    }


    static private string BuildImageMarkdownContent(string content)
    {
        var urlQuery = Uri.EscapeDataString(content);
        var imageUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&qzone=1&data={urlQuery}";
        var htmlContent = $"<p align=\"center\"><img src=\"{imageUrl}\" alt=\"{content}\"></p>";
        return htmlContent;
    }
}
