namespace RepoLinter;

using System.Diagnostics;

public class Git
{
    private readonly string _url;
    public readonly string PathToGitRepository;

    public Git(string url = "", string pathToGitRepository = "")
    {
        _url = url;
        if (pathToGitRepository == "")
        {
            var path = Path.GetTempPath();
            Directory.CreateDirectory(Path.Join(path, "gitrepolinter"));
            PathToGitRepository = Path.Join(path, "gitrepolinter");
        }
        else
        {
            Directory.CreateDirectory(pathToGitRepository);
            PathToGitRepository = pathToGitRepository;
        }
    }
    public void Clone()
    {
        if (_url == "")
        {
            throw new Exception("URL is empty");
        }
        
        Directory.Delete(PathToGitRepository, true);
        
        var p = new Process
        {
            StartInfo =
            {
                FileName = "git",
                Arguments = $"clone {_url} {PathToGitRepository}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };
        var started = p.Start();
        if (!started)
        {
            throw new Exception("Failed to start cloning of git repository");
        }
        
        p.WaitForExit();

        if (p.ExitCode != 0)
        {
            throw new Exception("Failed to clone of git repository. Exit code not 0.");
        }
        
        
        Console.WriteLine($"Cloned git repository to: {PathToGitRepository}");
    }

    public string GetCommitsAndContributors(string folderPath)
    {
        var p = new Process
        {
            StartInfo =
            {
                FileName = "git",
                Arguments = $"shortlog -sn --all",
                WorkingDirectory = folderPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };
        var started = p.Start();
        if (!started)
        {
            throw new Exception("Failed to start fetching of the git commit count and contributors");
        }
        
        var output = "     #  Names\n";
        output += p.StandardOutput.ReadToEnd();
        
        p.WaitForExit();

        if (output == "     #  Names\n")
        {
            throw new Exception("Failed to get git commit count and contributors, not a git repository");
        }
        
        return output;
    }
}