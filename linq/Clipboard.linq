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
}

public class SortOptions
{
	[Parameter("-o", "--overwrite", Description="Overwrite the clipboard with the contents")]
	public bool IsOverwritingClipboard { get; set; }
	
	[Parameter("-q", "--quiet", Description="Surpress output")]
	public bool IsQuiet { get; set; }
	
	[Parameter("-d", "--desc", Description="Sort in descending order")]
	public bool IsSortedDescending { get; set; } = false;
}

void Main(string[] args)
{
	try
	{
		Options options = new CommanderParser<Options>()
			.Parse(args);
		if(options.SortOptions != null)
		{
			var text = Clipboard.GetText();
			IEnumerable<string> lines = Regex.Split(text, Environment.NewLine);
			lines = options.SortOptions.IsSortedDescending ? lines.OrderByDescending(x => x) : lines.OrderBy(x => x);
			var sorted = string.Join(Environment.NewLine, lines);
			if(options.SortOptions.IsOverwritingClipboard)
				Clipboard.SetText(sorted);
			if(!options.SortOptions.IsQuiet)
				Console.WriteLine(sorted);
		}
	}
	catch(Exception e)
	{
		Console.Error.WriteLine(e);
	}
}
