namespace RepoLinter;

public class GetAllFiles
{
    static public List<string> AsList(string thePath)
    {
        var allFiles = new List<string>();
        
        // Get all files in the current directory
        var files = Directory.EnumerateFiles(thePath).ToList();
        allFiles.AddRange(files);

        // Recursively get all files in subdirectories
        var subdirectories = Directory.GetDirectories(thePath);
        foreach (string subdirectory in subdirectories)
        {
            allFiles.AddRange(AsList(subdirectory));
        }
        
        return allFiles;
    }
}