using System.Xml;
using RepoLinter;

public class Checks
{

    public static string RunAllChecks(List<string> filePaths, string currentDirectory)
    {
        var output = "";
        output += GitignoreCheck(filePaths, currentDirectory);
        output += LicenseCheck(filePaths, currentDirectory);
        return output;
    }

    static string GitignoreCheck(List<string> filePaths, string currentDirectory)
    {
        var result = "";
        if (filePaths.Contains(Path.Join(currentDirectory, ".gitignore")))
        {
            result += "Repository contains gitignore file: \u2705" + "\n";
        }
        else
        {
            result += "Repository does not contain gitignore file: \ud83d\udd34" + "\n";
        }
        
        return result;
    }

    static string LicenseCheck(List<string> filePaths, string currentDirectory)
    {
        var result = "";
        
        if (filePaths.Contains(Path.Join(currentDirectory, "LICENSE")))
        {
            result += "Repository contains LICENSE file: \u2705" + "\n";
        }
        else if (filePaths.Contains(Path.Join(currentDirectory, "LICENCE")))
        {
            result += "Repository contains LICENCE file: \u2705" + "\n";
        }
        else
        {
            result += "Repository does not contain LICENSE or LICENCE file: \ud83d\udd34" + "\n";
        }
        return result;
    }

}