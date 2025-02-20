﻿namespace RepoLinter;

using System.Diagnostics;

public class Git
{
    private readonly string _url;
    public readonly string PathToGitRepository;

    public Git(string url, string pathToGitRepository = "/tmp/gitrepolinter")
    {
        _url = url;
        PathToGitRepository = pathToGitRepository + "/" + Path.GetFileName(_url);
    }
    public void Clone()
    {
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
}