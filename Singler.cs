// HtmlSingler, (c)2022 by Joerg Plenert, D-Voerde
// Licensed under GPL v3
using NUglify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HtmlSingler
{
    internal class Singler
    {
        const string cssLinkStart = "<link href=";
        const string jsLinkStart = "<script src=";
        const string jsLinkEnd = "</script>";

        private string _htmlContent;
        private List<string> _replaceList = new List<string>();

        void SetReplaceEntry(int startTagIdx, int endTagIdx, string fileName)
        {
            _replaceList.Add(fileName);

            // Remove old include
            _htmlContent = _htmlContent.Remove(startTagIdx, endTagIdx - startTagIdx);
            // Insert new ReplaceTag
            _htmlContent = _htmlContent.Insert(startTagIdx, $"####HS{_replaceList.Count}####");
        }

        /// <summary>
        /// Gets the value from an attribute from an HTML element
        /// </summary>
        /// <returns>value</returns>
        string GetAttributeValueFromElement(string element, string attributeName)
        {
            int indexStart = element.IndexOf(attributeName + "=");
            if (indexStart == -1)
                throw new HtmlSinglerException($"Attribute '{attributeName}' not found on element '{element}'");
            indexStart += attributeName.Length + 1;

            int endIndex = element.IndexOf(' ', indexStart);
            if (endIndex == -1)
                endIndex = element.IndexOf('>', indexStart);
            if (indexStart == -1)
                throw new HtmlSinglerException($"End of attribute '{attributeName}' not found on element '{element}'");

            string value = element.Substring(indexStart, endIndex - indexStart);

            // Clear leading and ending quotes
            if (value.StartsWith('\'') || value.StartsWith("\""))
                value = value.Substring(1);

            if (value.EndsWith('\'') || value.EndsWith("\""))
                value = value.Substring(0, value.Length - 1);

            return value;
        }

        private string GetFileContent(string fileName, string inputFilePath)
        {
            if (fileName.StartsWith("http"))
            {
                HttpClient client = new HttpClient();
                try
                {
                    Task<string> task = client.GetStringAsync(fileName);
                    task.Wait();
                    return task.Result;
                }
                catch (Exception ex)
                {
                    throw new HtmlSinglerException($"Unable to getting '{fileName}': {ex.Message}");
                }
            }
            else
            {
                // Load file
                string linkedFileName = Path.GetFullPath(Path.Join(inputFilePath, fileName));

                try
                {
                     return File.ReadAllText(linkedFileName);
                }
                catch (Exception ex)
                {
                    throw new HtmlSinglerException($"Unable to read file '{linkedFileName}': {ex.Message}");
                }
            }
        }

        public void Execute(string inputFileName, string outputFileName)
        {
            string inputFilePath = Path.GetDirectoryName(inputFileName);
            string htmlContent = File.ReadAllText(inputFileName);
            // Get the main html
            var htmlResult = Uglify.Html(htmlContent);
            _htmlContent = htmlResult.Code;

            // Find css (<link href=[NAME] xxxxxx>)
            while (true)
            {
                int startIndex = _htmlContent.IndexOf(cssLinkStart);
                if (startIndex == -1)
                    break;

                // Get end of css-Link
                int endIndex = _htmlContent.IndexOf(">", startIndex);

                string linkElement = _htmlContent.Substring(startIndex, endIndex - startIndex + 1);
                string fileName = GetAttributeValueFromElement(linkElement, "href");

                SetReplaceEntry(startIndex, endIndex + 1, fileName);
            }

            // Find js (<script src=[NAME]></script>)
            while (true)
            {
                int startIndex = _htmlContent.IndexOf(jsLinkStart);
                if (startIndex == -1)
                    break;

                // Get end of js-link
                int endIndex = _htmlContent.IndexOf(jsLinkEnd, startIndex);
                if (endIndex == -1)
                    throw new Exception($"Found '<stript' element without '</script> at '{_htmlContent.Substring(startIndex, 30)}...'");
                endIndex += jsLinkEnd.Length;

                string linkElement = _htmlContent.Substring(startIndex, endIndex - startIndex);
                string fileName = GetAttributeValueFromElement(linkElement, "src");

                SetReplaceEntry(startIndex, endIndex + jsLinkEnd.Length, fileName);
            }

            // Replace the marks with the read and 
            for (int repIdx = 0; repIdx < _replaceList.Count; repIdx++)
            {
                // Load file
                string linkedFileName = _replaceList[repIdx];
                string linkedFileContent = GetFileContent(linkedFileName, inputFilePath);

                if (Path.GetExtension(linkedFileName) == ".css")
                    linkedFileContent = "<style>" + Uglify.Css(linkedFileContent).Code + "</style>";
                else if (Path.GetExtension(linkedFileName) == ".js")
                    linkedFileContent = "<script>" + Uglify.Js(linkedFileContent).Code + "</script>";
                else
                    throw new NotImplementedException($"File extension {Path.GetExtension(linkedFileName)} is not supported");

                _htmlContent = _htmlContent.Replace($"####HS{repIdx + 1}####", linkedFileContent);
            }

            if (outputFileName != null)
                File.WriteAllText(outputFileName, _htmlContent);
            else
                Console.WriteLine(outputFileName);
        }
    }
}
