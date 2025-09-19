mkdir build
Get-ChildItem -Path ./ -Recurse -Include *.msix | Where-Object { $_.FullName -notmatch "\\Dependencies\\" } | ForEach-Object {
  Copy-Item -Path $_.FullName -Destination build
}