// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using QRCodeExtension.Helpers;

namespace QRCodeExtension;

public partial class QRCodeExtensionCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;
    private readonly Storage _storage = new();

    public QRCodeExtensionCommandsProvider()
    {
        DisplayName = "QR Code";
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        _commands = [
            new CommandItem(new QRCodeExtensionPage(_storage)) { Title = DisplayName },
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }

}
