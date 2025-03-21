﻿using Newtonsoft.Json;

namespace RepoLinter;

public class Checks
{
    public static List<string> RunAllChecks(List<string> filePaths, string currentDirectory, List<string> ignoredChecks)
    {
        var output = "";
        var clear = "";
        var failedChecks = new List<string>();

        if (ignoredChecks[0] == "true")
        {
            var answer = GitignoreCheck(filePaths);
            output += answer[0];
            if (answer[1] == "false")
            {
                failedChecks.Add("gitignore");
            }
        }
        if (ignoredChecks[1] == "true")
        {
            var answer = LicenseCheck(filePaths);
            output += answer[0];
            if (answer[1] == "false")
            {
                failedChecks.Add("license");
            }
        }
        if (ignoredChecks[2] == "true")
        {
            var answer = SecretCheck(currentDirectory);
            output += answer[0];
            if (answer[1] == "false")
            {
                failedChecks.Add("secret");
            }
        }
        if (ignoredChecks[3] == "true")
        {
            var answer = ReadmeCheck(filePaths);
            output += answer[0];
            if (answer[1] == "false")
            {
                failedChecks.Add("readme");
            }
        }
        if (ignoredChecks[4] == "true")
        {
            var answer = TestCheck(filePaths);
            output += answer[0];
            if (answer[1] == "false")
            {
                failedChecks.Add("test");
            }
        }
        if (ignoredChecks[5] == "true")
        {
            var answer = WorkflowCheck(filePaths);
            output += answer[0];
            if (answer[1] == "false")
            {
                failedChecks.Add("workflow");
            }
        }

        if (failedChecks.Count > 0)
        {
            clear = "false";
        }
        else
        {
            clear = "true";
        }
        var list = new List<string>();
        list.Add(output); list.Add(clear);
        return list;
    }

    public static List<string> GitignoreCheck(List<string> filePaths)
    {
        var result = "";
        var clear = "";
        var numberOfGitignoreFiles = 0;
        var emptyGitignoreFiles = "";
        var gitignoreFilesWithContent = "";
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
                    emptyGitignoreFiles += "   " + filePath + "\n";
                }
                else
                {
                    gitignoreFilesWithContent += "   " + filePath + "\n";
                }
            }
        }

        
        if (numberOfGitignoreFiles >= 1)
        {
            if (numberOfEmptyGitignoreFiles >= 1)
            {
                result += $"\ud83d\udfe1 Repository contains {numberOfGitignoreFiles} gitignore files, These are \n" + $"{gitignoreFilesWithContent}" +
                          $"   but {numberOfEmptyGitignoreFiles} gitignore file(s) is/are empty. These are: \n"
                          + emptyGitignoreFiles;
                clear = "true";
            }
            else
            {
                result += $"\u2705 Repository contains {numberOfGitignoreFiles} gitignore file(s), these are: " + "\n" + gitignoreFilesWithContent ;
                clear = "true";
            }
            
        }
        else
        {
            result += "\ud83d\udd34 Repository does not contain a gitignore file. Please fix" + "\n";
            clear = "false";
        }
        var list = new List<string>();
        list.Add(result); list.Add(clear);
        return list;
    }

    public static List<string> LicenseCheck(List<string> filePaths)
    {
        var result = "";
        var clear = "";
        var numberOfLicenseFiles = 0;
        var numberOfEmptyLicenseFiles = 0;
        var licenseFilesWithContent = "";
        var licenseFilesWithoutContent = "";
        
        foreach (var filePath in filePaths)
        {
            var filename = Path.GetFileName(filePath);
            if (filename.ToLower().Contains("license"))
            {
                numberOfLicenseFiles += 1;
                
                string content = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(content))
                {
                    numberOfEmptyLicenseFiles += 1;
                    licenseFilesWithoutContent += "   " + filePath + "\n";
                }
                else
                {
                    licenseFilesWithContent += "   " + filePath + "\n";
                }
            }
        }
        
        if (numberOfLicenseFiles > 1)
        {
            result += "\ud83d\udfe1 Repository contains more than one License files, numbering " + numberOfLicenseFiles + ". \n" + 
                      $"   Of these {numberOfLicenseFiles - numberOfEmptyLicenseFiles} have contents, they are: \n" + licenseFilesWithContent + 
                      $"   {numberOfEmptyLicenseFiles} file(s) have no contents, they are: \n" + licenseFilesWithoutContent;
            clear = "true";
        }
        else if (numberOfLicenseFiles == 1)
        {
            if (numberOfEmptyLicenseFiles == 1)
            {
                result += "\ud83d\udfe1 Repository contains a License file but it is empty, it is located at: \n" +
                          licenseFilesWithoutContent;
                clear = "true";
            }
            else
            {
                result += "\u2705 Repository contains a License file with content, it is located at: " + "\n" + licenseFilesWithContent;
                clear = "true";
            }
            
        }
        if (numberOfLicenseFiles == 0)
        {
            result += "\ud83d\udd34 Repository does not contain a License file. Please fix" + "\n";
            clear = "false";
        }
        var list = new List<string>();
        list.Add(result); list.Add(clear);
        return list;
    }
    
    public static List<string> SecretCheck(string currentDirectory)
    {
        var result = "";
        var clear = "";

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
            clear = "false";
        }
        else
        {
            result += "\u2705 Repository does not contain any secrets \n";
            clear = "true";
        }
        var list = new List<string>();
        list.Add(result); list.Add(clear);
        return list;
    }

    public static List<string> ReadmeCheck(List<string> filePaths)
    {
        var result = "";
        var clear = "";
        var filledReadmeFiles = "";
        var emptyReadmeFiles = "";
        var numberOfReadMeFiles = 0;
        var numberOfEmptyReadMeFiles = 0;

        foreach (var filePath in filePaths)
        {
            var filename = Path.GetFileName(filePath);

            if (filename.ToLower().Contains("readme"))
            {
                numberOfReadMeFiles += 1;
                
                string content = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(content))
                {
                    numberOfEmptyReadMeFiles += 1;
                    emptyReadmeFiles += "   " + filePath + "\n";
                }
                else
                {
                    filledReadmeFiles += "   " + filePath + "\n";
                }
            }
        }

        if (numberOfReadMeFiles >= 1)
        {
            result += $"\u2705 Repository contains {numberOfReadMeFiles} README file(s)" + "\n" + 
                      $"   Of these, {numberOfReadMeFiles - numberOfEmptyReadMeFiles} have contents, they are: \n" + filledReadmeFiles + 
                      $"   And {numberOfEmptyReadMeFiles} are empty, they are located at: \n" + emptyReadmeFiles;
            clear = "true";
        }

        if (numberOfReadMeFiles == 0)
        {
            result += "\ud83d\udd34 Repository does not contain a README file. Please fix" + "\n";
            clear = "false";
        }
        var list = new List<string>();
        list.Add(result); list.Add(clear);
        return list;
    }

    public static List<string> TestCheck(List<string> filePaths)
    {
        var result = "";
        var clear = "";
        var numberOfTestFiles = 0;
        var testfiles = "";

        foreach (var filePath in filePaths)
        {
            if (filePath.ToLower().Contains("test"))
            {
                numberOfTestFiles += 1;
                testfiles += "   " + filePath + "\n";

            }
        }

        if (numberOfTestFiles >= 1)
        {
            result += $"\u2705 Repository contains {numberOfTestFiles} test files, their full paths are: \n" + testfiles;
            clear = "true";
        }

        if (numberOfTestFiles == 0)
        {
            result += "\ud83d\udd34 Repository does not contain any test files. Please fix" + "\n";
            clear = "false";
        }
        var list = new List<string>();
        list.Add(result); list.Add(clear);
        return list;
    }
    
    public static List<string> WorkflowCheck(List<string> filePaths)
    {
        var result = "";
        var clear = "";
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
            clear = "true";
        }
        else
        {
            result += "\ud83d\udfe1 Repository does not contain workflow file(s). \n";
            clear = "true";
        }
        var list = new List<string>();
        list.Add(result); list.Add(clear);
        return list;
    }
}