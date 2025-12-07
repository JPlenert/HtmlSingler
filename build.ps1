Remove-Item './HtmlSingler/bin' -Recurse
dotnet build ./HtmlSingler -c Release --no-self-contained
dotnet build ./HtmlSingler -c Release -p:MyRuntimeIdentifier=win-x64 -p:PublishSingleFile=true --self-contained true
dotnet build ./HtmlSingler -c Release -p:MyRuntimeIdentifier=linux-x64 -p:PublishSingleFile=true --self-contained true
dotnet build ./HtmlSingler -c Release -p:MyRuntimeIdentifier=linux-arm64 -p:PublishSingleFile=true --self-contained true

New-Item -Path './HtmlSingler/bin/Release/net8.0/universal' -ItemType Directory
Move-Item -Path './HtmlSingler/bin/Release/net8.0/*.*' -Destination './HtmlSingler/bin/Release/net8.0/universal'

Remove-Item './HtmlSingler/bin/Release/net8.0/linux-arm64/*.*'
Remove-Item './HtmlSingler/bin/Release/net8.0/linux-arm64/createdump'

Remove-Item './HtmlSingler/bin/Release/net8.0/linux-x64/*.*'
Remove-Item './HtmlSingler/bin/Release/net8.0/linux-x64/createdump'

Remove-Item './HtmlSingler/bin/Release/net8.0/win-x64/*.dll'
Remove-Item './HtmlSingler/bin/Release/net8.0/win-x64/*.pdb'
Remove-Item './HtmlSingler/bin/Release/net8.0/win-x64/*.json'
Remove-Item './HtmlSingler/bin/Release/net8.0/win-x64/createdump.exe'

Compress-Archive -Path './HtmlSingler/bin/Release/net8.0/*' -DestinationPath './Release.zip'
