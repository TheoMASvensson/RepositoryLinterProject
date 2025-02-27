namespace RepoLinter;

using System.Diagnostics;

public class Git
{
    private readonly string _url;
    public readonly string PathToGitRepository;

    public Git(string url = "", string pathToGitRepository = "/tmp/gitrepolinter")
    {
        _url = url;
        PathToGitRepository = pathToGitRepository;
        //+ "/" + Path.GetFileName(_url);
    }
    public void Clone()
    {
        if (_url == "")
        {
            throw new Exception("URL is empty");
        }
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
            throw new Exception("Failed to clone git repository");
        }
        p.WaitForExit();

        if (!Directory.Exists(PathToGitRepository))
        {
            throw new Exception("Failed to clone git repository");
        }
    }

    public string GetCommitCount(string folderPath)
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
        
        var output = "    NR  Names\n";
        output += p.StandardOutput.ReadToEnd();
        
        p.WaitForExit();

        if (output == "    NR  Names\n")
        {
            throw new Exception("Failed to get git commit count and contributors, not a git repository");
        }
        
        return output;
    }
}