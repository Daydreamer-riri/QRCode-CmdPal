using Microsoft.CommandPalette.Extensions.Toolkit;

namespace QRCodeExtension.Helpers;

/// <summary>
/// Provides commonly used icons for the QRCodeExtension application
/// </summary>
public static class Icons
{
    /// <summary>
    /// Search icon
    /// </summary>
    public static IconInfo Search { get; } = new("\uE721");

    /// <summary>
    /// qrcode outlined
    /// </summary>
    public static IconInfo QRCode { get; } = new("\uED14");

    /// <summary>
    /// Delete icon (trash can)
    /// </summary>
    public static IconInfo Delete { get; } = new("\uE74D");

    public static IconInfo Info { get; } = new("\uE946");

}
