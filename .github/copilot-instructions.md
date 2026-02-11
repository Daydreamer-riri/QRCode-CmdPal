# Project Guidelines

## Code Style
- C# with nullable enabled; analyzers set to "Recommended" and NuGet audit enabled in [Directory.Build.props](Directory.Build.props#L1-L9).
- Central package versions and StyleCop analyzers are used; align with existing conventions in [Directory.Packages.props](Directory.Packages.props#L1-L17).

## Architecture
- COM server entry point registers `QRCodeExtension` when launched with `-RegisterProcessAsComServer` in [QRCodeExtension/Program.cs](QRCodeExtension/Program.cs#L16-L36).
- Extension wiring and provider registration live in [QRCodeExtension/QRCodeExtension.cs](QRCodeExtension/QRCodeExtension.cs#L12-L33).
- Command Palette surface is implemented by `QRCodeExtensionCommandsProvider` and the `DynamicListPage` in [QRCodeExtension/QRCodeExtensionCommandsProvider.cs](QRCodeExtension/QRCodeExtensionCommandsProvider.cs#L11-L28) and [QRCodeExtension/Pages/QRCodeExtensionPage.cs](QRCodeExtension/Pages/QRCodeExtensionPage.cs#L15-L115).
- Detail rendering and history recording happen in [QRCodeExtension/Pages/DetailPage.cs](QRCodeExtension/Pages/DetailPage.cs#L23-L43).

## Build and Test
- Build/test commands are not documented in-repo; no automated test command found.
- Packaging helper collects MSIX outputs into `build/` via [CopyMsixFile.ps1](CopyMsixFile.ps1#L1-L4).

## Project Conventions
- History is stored as JSON at `BaseSettingsPath("QRCodeExtension")`, capped at 50 items, file name `qrcode_history.json` in [QRCodeExtension/Helpers/Storage.cs](QRCodeExtension/Helpers/Storage.cs#L18-L37) and serialized in [QRCodeExtension/Helpers/HistoryStore.cs](QRCodeExtension/Helpers/HistoryStore.cs#L110-L133).
- List items construct Show/Hide Details and Delete actions in [QRCodeExtension/QRCodeListItem.cs](QRCodeExtension/QRCodeListItem.cs#L18-L75).

## Integration Points
- QR images are fetched from `api.qrserver.com` as used in [QRCodeExtension/Pages/DetailPage.cs](QRCodeExtension/Pages/DetailPage.cs#L38-L43) and [QRCodeExtension/QRCodeListItem.cs](QRCodeExtension/QRCodeListItem.cs#L81-L86).
- App extension registration and COM ClassId wiring are in [QRCodeExtension/Package.appxmanifest](QRCodeExtension/Package.appxmanifest#L46-L71).

## Security
- App capability includes `internetClient` and `runFullTrust` in [QRCodeExtension/Package.appxmanifest](QRCodeExtension/Package.appxmanifest#L76-L79).
- History persistence writes to local settings path; exceptions are logged to the host in [QRCodeExtension/Helpers/Storage.cs](QRCodeExtension/Helpers/Storage.cs#L41-L67).
