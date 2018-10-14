using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Tools.HostFile
{
    public class Options
    {
        [Option('w', "www", HelpText = "Include www. subdomain automatically", Default = true)]
        public bool IncludeWwwSubdomain { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
