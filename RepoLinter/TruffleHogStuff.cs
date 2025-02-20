using System.Diagnostics;
using Newtonsoft.Json;

namespace RepoLinter;

class TruffleHogStuff
{
    public string RunProcess(string directory)
    {
        Directory.CreateDirectory("/tmp/repolinter/git");
        
        var trufflehogExists = File.Exists("/usr/local/bin/trufflehog");
        
        if (!trufflehogExists)
        {
            throw new Exception("Trufflehog is not installed");
        }
        
        var p = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/usr/local/bin/trufflehog",
                Arguments = $"filesystem {directory} --json",
                WorkingDirectory = directory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        var started = p.Start();

        if (!started)
        {
            throw new Exception("Failed to start trufflehog");
        }
        
        p.WaitForExit();
        
        var output = p.StandardOutput.ReadToEnd();

        Console.WriteLine("Exit code:" + p.ExitCode);
        
        return output;
    }
}

public class Finding
{
    [JsonProperty("detectorName")]
    public required string DetectorName { get; set; }

    [JsonProperty("file")]
    public required string File { get; set; }

    [JsonProperty("line")]
    public int Line { get; set; }

    [JsonProperty("raw")]
    public required string Raw { get; set; }

    [JsonProperty("description")]
    public required string Description { get; set; }
}