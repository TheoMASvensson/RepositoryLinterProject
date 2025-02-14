using System.Diagnostics;


public class Checks
{

    public static string RunAllChecks(List<string> filePaths, string currentDirectory)
    {
        var output = "";
        output += GitignoreCheck(filePaths, currentDirectory);
        output += LicenseCheck(filePaths, currentDirectory);
        //output += SecretCheck(filePaths, currentDirectory);
        output += READMECheck(filePaths, currentDirectory);
        //output += TestCheck(filePaths, currentDirectory);
        //output += WorkflowCheck(filePaths, currentDirectory);
        return output;
    }

    public static string GitignoreCheck(List<string> filePaths, string currentDirectory)
    {
        var result = "";
        var numberOfGitignoreFiles = 0;

        foreach (var filePath in filePaths)
        {
            var filename = Path.GetFileName(filePath);
            if (filename == ".gitignore")
            {
                numberOfGitignoreFiles += 1;
            }
        }

        
        if (numberOfGitignoreFiles >= 1)
        {
            result += $"\u2705 Repository contains {numberOfGitignoreFiles} gitignore file(s)" + "\n";
        }
        else
        {
            result += "\ud83d\udd34 Repository does not contain a gitignore file. Please fix" + "\n";
        }
        
        return result;
    }

    public static string LicenseCheck(List<string> filePaths, string currentDirectory)
    {
        var result = "";
        var numberOfLicenseFiles = 0;
        
        foreach (var filePath in filePaths)
        {
            var filename = Path.GetFileName(filePath);
            if (filename.ToLower().Contains("license"))
            {
                numberOfLicenseFiles += 1;
            }
        }
        
        if (numberOfLicenseFiles > 1)
        {
            result += "\ud83d\udfe1 Repository contains to many License files, numbering " + numberOfLicenseFiles + ". Please fix" + "\n";
        }
        else if (numberOfLicenseFiles == 1)
        {
            result += "\u2705 Repository contains a License file" + "\n";
        }
        if (numberOfLicenseFiles == 0)
        {
            result += "\ud83d\udd34 Repository does not contain a License file. Please fix" + "\n";
        }
        
        return result;
    }
    
    public static string SecretCheck(List<string> filePaths, string currentDirectory)
    {
        var result = "";
        
        var p = new Process
        {
            StartInfo =
            {
                FileName = "trufflehog",
                Arguments = $"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };
        var started = p.Start();

        if (!started)
        {
            throw new Exception("Failed to start truffle hog");
        }


        return result;
    }

    public static string READMECheck(List<string> filePaths, string currentDirectory)
    {
        var result = "";

        foreach (var filePath in filePaths)
        {
            var filename = Path.GetFileName(filePath);

            if (filename.ToLower().Contains("readme"))
            {
                result += "\u2705 Repository contains a README file" + "\n";
            }
        }

        if (result == "")
        {
            result += "\ud83d\udd34 Repository does not contain a README file. Please fix" + "\n";
        }
        return result;
    }

    public static string TestCheck(List<string> filePaths, string currentDirectory)
    {
        var result = "";
        
        return result;
    }
    
    public static string WorkflowCheck(List<string> filePaths, string currentDirectory)
    {
        var result = "";
        
        return result;
    }
}