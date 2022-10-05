// See https://aka.ms/new-console-template for more information

const int REQUIRED_ARGUMENTS = 1;

if (args.Length != REQUIRED_ARGUMENTS)
{
    Console.Error.WriteLine("Incorrect number of arguments supplied. Expected: " + REQUIRED_ARGUMENTS + ", Supplied: " + args.Length);
    return;
}

string file;

if (!File.Exists(args[0]))
{
    if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), args[0])))
    {
        Console.Error.WriteLine("No file found at directory.");
        return;
    }
    else
    {
        file = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
    }    
}
else
{
    file = args[0];
}


if (Path.GetExtension(file) != ".md")
{
    Console.Error.WriteLine("File is not a markdown file.");
    return;
}

var contents = File.ReadLines(file);
bool foundTable = false;
string tableHeader = "| :-: | :-: | :-- |";

var teams = new List<string>();


foreach (var line in contents)
{
    if (!foundTable)
    {
        if (line == tableHeader)
        {
            foundTable = true;
        }
    }
    else
    {
        if (line == "")
        {
            break;
        }

        string team = "";
        var segments = line.Split('|');

        // Parse Country name
        team += segments[2].Replace("*", "") + ',';

        // Parse Country code
        team += segments[1].Substring(10, 2) + ',';

        // Parse Players
        segments[3] = segments[3].Replace("\\", "");
        segments[3] = segments[3].Replace("*", "");
        var players = segments[3].Split(',');

        foreach (var player in players)
        { 
            team += player.Substring(2, player.LastIndexOf(']') - 2) + ',';
            team += player.Substring(player.LastIndexOf('/') + 1, player.LastIndexOf(')') - player.LastIndexOf('/') - 1) + ',';
        }

        teams.Add(team);
    }
}

File.WriteAllLines("teams.csv", teams);