// HtmlSingler, (c)2022-25 by Joerg Plenert, D-Voerde
// Licensed under GPL v3
using System;

namespace HtmlSingler
{
    internal class HtmlSinglerException : Exception
    {
        public HtmlSinglerException(string message) : base(message) { }
    }
}
