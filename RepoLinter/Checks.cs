using Newtonsoft.Json;

namespace RepoLinter;

public class Checks
{
    //List<string> passedChecks = new List<string>();
    //List<string> failedChecks = new List<string>();

    public static string RunAllChecks(List<string> filePaths, string currentDirectory, List<string> ignoredChecks)
    {
        var output = "";

        if (ignoredChecks[0] == "true")
        {
            output += GitignoreCheck(filePaths);
        }
        if (ignoredChecks[1] == "true")
        {
            output += LicenseCheck(filePaths);
        }
        if (ignoredChecks[2] == "true")
        {
            output += SecretCheck(currentDirectory);
        }
        if (ignoredChecks[3] == "true")
        {
            output += ReadmeCheck(filePaths);
        }
        if (ignoredChecks[4] == "true")
        {
            output += TestCheck(filePaths);
        }
        if (ignoredChecks[5] == "true")
        {
            output += WorkflowCheck(filePaths);
        }
        
        return output;
    }

    static string GitignoreCheck(List<string> filePaths)
    {
        var result = "";
        var numberOfGitignoreFiles = 0;
        var emptyGitignoreFiles = "";
        var numberOfEmptyGitignoreFiles = 0;

        foreach (var filePath in filePaths)
        {
            var filename = Path.GetFileName(filePath);
            if (filename == ".gitignore")
            {
                numberOfGitignoreFiles += 1;
                
                string content = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(content))
                {
                    numberOfEmptyGitignoreFiles += 1;
                    emptyGitignoreFiles += filePath + "\n";
                }
            }
        }

        
        if (numberOfGitignoreFiles >= 1)
        {
            if (numberOfEmptyGitignoreFiles >= 1)
            {
                result += $"\ud83d\udfe1 Repository contains {numberOfGitignoreFiles} gitignore files, " +
                          $"but {numberOfEmptyGitignoreFiles} gitignore file(s) is/are empty. These are: \n"
                          + emptyGitignoreFiles;
            }
            else
            {
                result += $"\u2705 Repository contains {numberOfGitignoreFiles} gitignore file(s)" + "\n";
            }
            
        }
        else
        {
            result += "\ud83d\udd34 Repository does not contain a gitignore file. Please fix" + "\n";
        }
        
        return result;
    }

    static string LicenseCheck(List<string> filePaths)
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
    
    static string SecretCheck(string currentDirectory)
    {
        var result = "";

        var trufflehog = new TruffleHogStuff();
        var trufflehogOutput = trufflehog.RunProcess(currentDirectory);
        
        if (!string.IsNullOrEmpty(trufflehogOutput))
        {
            var findings = JsonConvert.DeserializeObject<List<Finding>>(trufflehogOutput);
            
            result += "\ud83d\udd34 Repository contains " + findings?.Count + " secrets. Please fix" + "\n"; 
            result += "TruffleHog Findings: \n";
            result += "-------------------- \n";

            foreach (var finding in findings!)
            {
                result += $"Type: {finding.DetectorName} \n";
                result += ($"File: {finding.File} \n");
                result += ($"Line: {finding.Line} \n");
                result += ($"Secret: {finding.Raw} \n");
                result += ($"Description: {finding.Description} \n");
                result += ("--------------------- \n");
            }
        }
        else
        {
            result += "\u2705 Repository does not contain any secrets \n";
        }

        return result;
    }

    static string ReadmeCheck(List<string> filePaths)
    {
        var result = "";
        var numberOfReadMeFiles = 0;

        foreach (var filePath in filePaths)
        {
            var filename = Path.GetFileName(filePath);

            if (filename.ToLower().Contains("readme"))
            {
                numberOfReadMeFiles += 1;
            }
        }

        if (numberOfReadMeFiles >= 1)
        {
            result += $"\u2705 Repository contains {numberOfReadMeFiles} README file(s)" + "\n";
        }

        if (numberOfReadMeFiles == 0)
        {
            result += "\ud83d\udd34 Repository does not contain a README file. Please fix" + "\n";
        }
        return result;
    }

    static string TestCheck(List<string> filePaths)
    {
        var result = "";
        var numberOfTestFiles = 0;
        var testfiles = "";

        foreach (var filePath in filePaths)
        {
            if (filePath.ToLower().Contains("test"))
            {
                numberOfTestFiles += 1;
                testfiles += "  " + filePath + "\n";

            }
        }

        if (numberOfTestFiles >= 1)
        {
            result += $"\u2705 Repository contains {numberOfTestFiles} test files, their full paths are: \n" + testfiles;
        }

        if (numberOfTestFiles == 0)
        {
            result += "\ud83d\udd34 Repository does not contain a test file. Please fix" + "\n";
        }
        
        return result;
    }
    
    static string WorkflowCheck(List<string> filePaths)
    {
        var result = "";
        var numberOfWorkflowFiles = 0;
        var workflowFiles = "";

        foreach (var filePath in filePaths)
        {
            if (filePath.ToLower().Contains(Path.Join(".github", "workflows")))
            {
                numberOfWorkflowFiles += 1;
                workflowFiles += filePath + "\n";
            }
        }

        if (numberOfWorkflowFiles >= 1)
        {
            result += $"\u2705 Repository contains {numberOfWorkflowFiles} workflow file(s), they are: " + "\n";
            result += workflowFiles;
        }
        else
        {
            result += "\ud83d\udd34 Repository does not contain workflow file(s). Please fix \n";
        }
        
        return result;
    }
}