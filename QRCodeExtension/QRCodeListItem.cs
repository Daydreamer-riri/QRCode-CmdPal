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
    }
}
