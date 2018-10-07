<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <NuGetReference>Commander.NET</NuGetReference>
  <NuGetReference>Serilog</NuGetReference>
  <NuGetReference>Serilog.Sinks.Console</NuGetReference>
  <Namespace>Commander.NET</Namespace>
  <Namespace>Commander.NET.Attributes</Namespace>
  <Namespace>Commander.NET.Exceptions</Namespace>
  <Namespace>Commander.NET.Interfaces</Namespace>
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

public class Options
{
	[Command("block")]
	public BlockOptions BlockOptions { get; set; }
	
	[Command("unblock")]
	public UnblockOptions UnblockOptions { get; set; }
	
	[Parameter("-w", "--www", Description="Automatically include the www. subdomain")
	public bool IncludeWwwDomain { get; set; }
}

public class UnblockOptions
{
	[PositionalParameterList()]
	public string[] Domains { get; set; }
}

public class BlockOptions
{
	[PositionalParameterList]
	public string[] Domains { get; set; }
}

void Main(string[] args)
{
	Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Console()
				.CreateLogger();
	Options options;		
	try
	{
		options = new CommanderParser<Options>().Parse(args);
	}
	catch (ParameterMissingException e)
	{
		Log.Fatal(e, "Missing parameter: {Parameter}", e.ParameterName);
		Console.WriteLine(new CommanderParser<Options>().Usage());
		return;
	}
	catch (ParameterFormatException e)
	{
		Log.Fatal(e, "There was a problem parsing {ParameterName}. It is a {RequiredType}, but found {Value}", e.ParameterName, e.RequiredType, e.Value);
		Console.WriteLine(new CommanderParser<Options>().Usage());
		return;
	}
	catch (Exception e)
	{
		Log.Fatal(e, "There was an unknown problem parsing the options");
		Console.WriteLine(new CommanderParser<Options>().Usage());
		return;
	}
	List<string> lines = null;
	try
	{
		lines = File.ReadAllLines(HOST_FILE).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
	}
	catch(IOException e)
	{
		Log.Fatal(e, "Unable to open the hosts file. Is this process elevated?");
		return;
	}
	catch(Exception  e)
	{
		Log.Fatal(e, "There was an unknown problem opening the hosts file");
		return;
	}
	if(options.BlockOptions != null)
	{
		foreach(var domain in options.BlockOptions.Domains)
		{
			var newLine = $"127.0.0.1 {domain}";
			var www = $"127.0.0.1 www.{domain}";
			if(!lines.Contains(newLine))
				lines.Add(newLine);
			if(!lines.Contains(www) && options.IncludeWwwDomain)
				lines.Add(www);
		}
	}
	else if(options.UnblockOptions != null)
	{
		var altered = lines.Except(options.UnblockOptions.Domains.Select(x => $"127.0.0.1 {x}")).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
		if(options.IncludeWwwDomain)
		{
			altered = lines.Except(options.UnblockOptions.Domains.Select(x => $"127.0.0.1 www.{x}")).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
		}
		lines = altered;
	}
	File.WriteAllLines(HOST_FILE, lines);
	DnsFlusher.Flush();
}
