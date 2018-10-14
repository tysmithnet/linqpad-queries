using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Tools.Clipboard
{
    [Verb("meme", HelpText = "Meme-ify the image on the clipboard")]
    public class MemeOptions : Options
    {
        [Option('t', "top", HelpText = "Top text")]
        public string TopText { get; set; }

        [Option('b', "bot", HelpText = "Bot text")]
        public string BottomText { get; set; }

        [Option('f', "file", HelpText = "Filename")]
        public string Name { get; set; }
    }
}
