using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace QRCodeExtension;

public partial class DetailPage : ContentPage
{
    private readonly string _content;

    public DetailPage(string content)
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = $"QR Code";

        _content = content;
    }

    public override IContent[] GetContent()
    {
        {
            return
            [
                new MarkdownContent($"## {_content}"),
                BuildImageMarkdownContent(_content)
            ];
        }
    }

    static private MarkdownContent BuildImageMarkdownContent(string content)
    {
        var urlQuery = Uri.EscapeDataString(content);
        var imageUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=300x300&qzone=1&data={urlQuery}";
        var htmlContent = $"<p align=\"center\"><img src=\"{imageUrl}\" alt=\"{content}\"></p>";
        return new MarkdownContent(htmlContent);
    }
}