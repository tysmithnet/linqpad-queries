<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <NuGetReference>CommandLineParser</NuGetReference>
  <NuGetReference>Serilog</NuGetReference>
  <NuGetReference>Serilog.Sinks.Console</NuGetReference>
  <NuGetReference>UACHelper</NuGetReference>
  <Namespace>CommandLine</Namespace>
  <Namespace>CommandLine.Text</Namespace>
  <Namespace>Serilog</Namespace>
  <Namespace>Serilog.Configuration</Namespace>
  <Namespace>Serilog.Context</Namespace>
  <Namespace>Serilog.Core</Namespace>
  <Namespace>Serilog.Core.Enrichers</Namespace>
  <Namespace>Serilog.Data</Namespace>
  <Namespace>Serilog.Debugging</Namespace>
  <Namespace>Serilog.Events</Namespace>
  <Namespace>Serilog.Filters</Namespace>
  <Namespace>Serilog.Formatting</Namespace>
  <Namespace>Serilog.Formatting.Display</Namespace>
  <Namespace>Serilog.Formatting.Json</Namespace>
  <Namespace>Serilog.Formatting.Raw</Namespace>
  <Namespace>Serilog.Parsing</Namespace>
  <Namespace>Serilog.Sinks.SystemConsole.Themes</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>UACHelper</Namespace>
</Query>

const string HOST_FILE = @"c:\windows\system32\drivers\etc\hosts";

public static class DnsFlusher
{
	[DllImport("dnsapi.dll", EntryPoint = "DnsFlushResolverCache")]
	private static extern UInt32 DnsFlushResolverCache();

	public static void Flush()
	{
		UInt32 result = DnsFlushResolverCache();
	}
}

[Verb("block", HelpText="Block domains")]
public class Options
{
	[Option('d', "domains", HelpText = "Domains to block")]
	public IEnumerable<string> DomainsToBlock { get; set; }

	[Option('w', "www", HelpText="Include www. domain")]
	public bool IncludeWwwDomain { get; set; }
}

[Verb("block", HelpText="Block domains")]
public class BlockOptions : Options
{
	
}

[Verb("unblock", HelpText="Unblock domains")]
public class UnBlockOptions : Options
{
	
}

static void Main(string[] args)
{
	Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.Console()
			.CreateLogger();

	CommandLine.Parser.Default.ParseArguments<BlockOptions, UnBlockOptions>(args)
		.MapResult((BlockOptions o) => Block(o), (UnBlockOptions o) => UnBlock(o), errs => HandleErrors(errs));
}

static int Block(BlockOptions options)
{
	var lines = File.ReadAllLines(HOST_FILE);
	options.DomainsToBlock = options.DomainsToBlock.OrderBy(x => x).Distinct();
	var newGuys = options.IncludeWwwDomain ? options.DomainsToBlock.SelectMany(x => new[] { x, $"www.{x}" }) : options.DomainsToBlock;
	File.WriteAllLines(HOST_FILE, lines.Concat(newGuys.Select(x => $"127.0.0.1 {x}")));
	return 0;
}

static int UnBlock(UnBlockOptions options)
{
	var lines = File.ReadAllLines(HOST_FILE);
	options.DomainsToBlock = options.DomainsToBlock.OrderBy(x => x).Distinct();
	var newGuys = options.IncludeWwwDomain ? options.DomainsToBlock.SelectMany(x => new[] { x, $"www.{x}" }) : options.DomainsToBlock;
	lines = lines.Except(newGuys.Select(x => $"127.0.0.1 {x}")).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
	File.WriteAllLines(HOST_FILE, lines);
	return 0;
}

static int HandleErrors(IEnumerable<Error> errors)
{
	foreach (var error in errors)
	{
		Log.Fatal("{Error}", error);
	}
	return 1;
}