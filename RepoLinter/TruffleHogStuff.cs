using System.Diagnostics;
using Newtonsoft.Json;

namespace RepoLinter;

class TruffleHogStuff
{
    public string RunProcess(string directory)
    {
        var p = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "trufflehog",
                Arguments = $"filesystem {directory} --json --no-update",
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