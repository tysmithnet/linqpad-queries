<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <NuGetReference>Commander.NET</NuGetReference>
  <Namespace>Commander.NET</Namespace>
  <Namespace>Commander.NET.Attributes</Namespace>
  <Namespace>Commander.NET.Exceptions</Namespace>
  <Namespace>Commander.NET.Interfaces</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

public class Options
{
	[Command("sort")]
	public SortOptions SortOptions { get; set; }
	
	[Parameter("-h", "--help", Description="Show help page")]
	public bool IsHelpRequested { get; set; }
}

public class SortOptions
{
	[Parameter("-o", "--overwrite", Description="Overwrite the clipboard with the contents")]
	public bool IsOverwritingClipboard { get; set; }
	
	[Parameter("-q", "--quiet", Description="Surpress output")]
	public bool IsQuiet { get; set; }
	
	[Parameter("-d", "--desc", Description="Sort in descending order")]
	public bool IsSortedDescending { get; set; }

	[Parameter("-p", "--preview", Description = "Write the text to console, and then prompt if an overwrite should take place")]
	public bool IsPreviewRequested { get; set; }

	[Parameter("-h", "--help", Description = "Show help page")]
	public bool IsHelpRequested { get; set; }
}

void Main(string[] args)
{
	try
	{
		if(args == null || args.All(x => string.IsNullOrWhiteSpace(x)))
		{
			string usage = new CommanderParser<Options>().Usage(executableName: "lprun clipboard.linq");
			Console.WriteLine(usage);
			return;
		}
		Options options = new CommanderParser<Options>()
			.Parse(args);
		if(options.IsHelpRequested)
		{
			string usage = new CommanderParser<Options>().Usage(executableName: "lprun clipboard.linq");
			Console.WriteLine(usage);
			return;
		}
		if (options.SortOptions != null)
		{
			if(options.SortOptions.IsHelpRequested)
			{
				string usage = new CommanderParser<SortOptions>().Usage(executableName: "lprun clipboard.linq sort");
				Console.WriteLine(usage);
				return;
			}
			var text = Clipboard.GetText();
			IEnumerable<string> lines = Regex.Split(text, Environment.NewLine);
			lines = options.SortOptions.IsSortedDescending ? lines.OrderByDescending(x => x) : lines.OrderBy(x => x);
			var sorted = string.Join(Environment.NewLine, lines);
			if (options.SortOptions.IsPreviewRequested)
			{
				Console.WriteLine(sorted);
				Console.Write("Overwrite clipboard? Y/n: ");
				string line = Console.ReadLine();
				if(Regex.IsMatch(line, @"n", RegexOptions.IgnoreCase))
				{
					return;
				}
				Clipboard.SetText(sorted);
			}
			else
			{
				if (options.SortOptions.IsOverwritingClipboard)
					Clipboard.SetText(sorted);
				if (!options.SortOptions.IsQuiet)
					Console.WriteLine(sorted);
			}			
		}
	}
	catch(Exception e)
	{
		Console.Error.WriteLine(e);
	}
}