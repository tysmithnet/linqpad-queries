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

        [Option('l', "log", HelpText = "Specify if logging is enabled")]
        public bool IsLoggingEnabled { get; set; }
    }

    public class FacadeOptions : Options
    {
        [Option('d', "domains", HelpText = "Domains to modify")]
        public IEnumerable<string> Domains { get; set; }
    }

    [Verb("block", HelpText = "Block domains by setting their IP to loopback")]
    public class BlockOptions : FacadeOptions
    {

    }

    [Verb("unblock", HelpText = "Unblock any domains that have the IP set to loopback")]
    public class UnblockOptions : FacadeOptions
    {

    }

    [Verb("set", HelpText = "Set the ip address resolution for a domain")]
    public class SetOptions : Options
    {
        [Option('a', "address", HelpText = "IP address to use in the mapping", Required = true)]
        public string IpAddress { get; set; }

        [Option('d', "domain", HelpText = "Domain name to use in the mapping", Required = true)]
        public string Domain { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
