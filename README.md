# HtmlSingler
Combines a HTML file with all direct includes (js/css) to a minified single HTML file.
**That supports building real single page and file web sites, e.g. for very small web sites or embedded web servers.**
HtmlSingler is written in C# (dotnet 6) and can be used with Windows, Linux and Mac.
Releases are available for win-x64, linux-x64, linux-arm64 and universal (dotnet).

## Details

HtmlSingler takes a single html file as input. First the html file is minifier.
Then all linked files (`<script src=[NAME]></script>` or `<link href=[NAME] xxxxxx>`) will be read from disk or gathered from the web.
All files are minified and included in the html file.

The new **single** html file is written to a new file out STDOUT.

## Usage

`$> HtmlSingler ./index.html ./index.single.html`
or 
`$> HtmlSingler ./index.html`

or if using universal version

`$> dotnet HtmlSingler.dll ./index.html ./index.single.html`

## Copyright & License

(c) 2022 by Joerg Plenert, D-Voerde.
Under GPLv3 license.
