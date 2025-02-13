public class Checks
{

    public static string RunAllChecks(List<string> filePaths, string currentDirectory)
    {
        var output = "";
        output += GitignoreCheck(filePaths, currentDirectory);
        output += LicenseCheck(filePaths, currentDirectory);
        output += "";
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

        if (numberOfGitignoreFiles > 1)
        {
            result += "Repository contains to many gitignore files, numbering " + numberOfGitignoreFiles + ". Please fix: " + "\n";
        }
        else if (numberOfGitignoreFiles == 1)
        {
            result += "Repository contains a gitignore file: \u2705" + "\n";
        }
        if (numberOfGitignoreFiles == 0)
        {
            result += "Repository does not contain a gitignore file. Please fix: \ud83d\udd34" + "\n";
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
            if (filename == "LICENSE" || filename == "LICENCE" || filename == "LICENSE.txt" || filename == "LICENSE.md")
            {
                numberOfLicenseFiles += 1;
            }
        }
        
        if (numberOfLicenseFiles > 1)
        {
            result += "Repository contains to many License files, numbering " + numberOfLicenseFiles + ". Please fix: " + "\n";
        }
        else if (numberOfLicenseFiles == 1)
        {
            result += "Repository contains a License file: \u2705" + "\n";
        }
        if (numberOfLicenseFiles == 0)
        {
            result += "Repository does not contain a License file. Please fix: \ud83d\udd34" + "\n";
        }
        
        return result;
    }

}