using System;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using QRCodeExtension.Helpers;

namespace QRCodeExtension;

public partial class DetailPage : ContentPage
{
    private readonly string _content;
    private readonly Storage _storage;

    public DetailPage(string content, Storage storage)
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = $"QR Code";
        Name = "Generate QR Code";

        _content = content;
        _storage = storage;
    }

    public override IContent[] GetContent()
    {
        _storage.AddHistoryItem(_content);
        return
        [
            new MarkdownContent($$"""
                > {{_content}}

                {{BuildImageMarkdownContent(_content)}}
                """),
        ];
    }

    static private string BuildImageMarkdownContent(string content)
    {
        var urlQuery = Uri.EscapeDataString(content);
        var imageUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=300x300&qzone=1&data={urlQuery}";
        var htmlContent = $"<p align=\"center\"><img src=\"{imageUrl}\" alt=\"{content}\"></p>";
        return htmlContent;
    }

}