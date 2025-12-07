// HtmlSingler, (c)2022-25 by Joerg Plenert, D-Voerde
// Licensed under GPL v3
using System;
using System.Collections.Generic;

namespace HtmlSingler
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("HtmlSingler V0.6, (c)2022-2025 by Joerg Plenert, D-Voerde");

            if (args.Length == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Usage: HtmlSingler <input.html> [<output.html>] [<List of files to not uglify>]");
                Console.WriteLine();
                Console.WriteLine("Combines a HTML file with all direct includes (js/css) to a minified single HTML file.");
                Console.WriteLine("Javascript links must be in format '<script src=[NAME]></script>'.");
                Console.WriteLine("CSS links must be in format '<link href=[NAME] xxxxxx>'.");
                Console.WriteLine("Uses NUglify for minify and compression.");
                Console.WriteLine("");
                Console.WriteLine("if NoUglify-List is 'all' no file fill be uglified");
                return -1;
            }

            Singler singler = new Singler();

            try
            {
                List<string> noUglifxList = null;
                bool noUglify = false;

                if (args.Length > 2)
                {
                    if (args[2] == "all")
                    {
                        noUglify = true;
                    }
                    else
                    {
                        noUglifxList = new List<string>();
                        for (int i = 2; i < args.Length; i++)
                            noUglifxList.Add(args[i]);
                    }
                }


                singler.Execute(args[0], args.Length >= 2 ? args[1] : null, noUglify, noUglifxList);
            }
            catch (Exception ex)
            {
                if (ex is HtmlSinglerException)
                {
                    Console.Error.WriteLine("Error executing:");
                    Console.Error.WriteLine(ex.Message);
                    return -1;
                }
                else
                {
                    Console.Error.WriteLine("Exception executing:");
                    Console.Error.WriteLine(ex.Message);
                    return -1;
                }
            }

            return 0;
        }
    }
}