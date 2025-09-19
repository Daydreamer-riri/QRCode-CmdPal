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
            Title = "Text",
            Body = $"```\n{content}\n```",
        };

        MoreCommands = [
            new CommandContextItem(
                title: "Show Details",
                name: "ShowDetails",
                result: CommandResult.KeepOpen(),
                action: () =>
                {
                    if (Details == null)
                    {
                        Details = _details;
                    }
                    else
                    {
                        Details = null;
                    }
                }
            )
            {
                Icon = Icons.Info,
            },
            new CommandContextItem(
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
            },
        ];
    }
}
