using RepoLinter;
using Xunit.Abstractions;

namespace UnitTests;

public class CheckingTests
{

    [Fact]
    public void CreateAllTestRepos()
    {
        Directory.CreateDirectory("/tmp/samplerepositories/trashrepo");
        File.WriteAllText("/tmp/samplerepositories/trashrepo/secret.txt", "aws_access_key_id=AKIAIOSFODNN7EXAMPLE");
        
        
        Directory.CreateDirectory("/tmp/samplerepositories/completerepo");
        File.Create("/tmp/samplerepositories/completerepo/LICENSE");
        File.Create("/tmp/samplerepositories/completerepo/Secrets.txt");
        File.Create("/tmp/samplerepositories/completerepo/README.md");
        Directory.CreateDirectory("/tmp/samplerepositories/completerepo/Tests");
        File.WriteAllText("/tmp/samplerepositories/completerepo/.gitignore", "randomshit");
        File.Create("/tmp/samplerepositories/completerepo/Tests/test1");
        File.Create("/tmp/samplerepositories/completerepo/Tests/test2");
        Directory.CreateDirectory("/tmp/samplerepositories/completerepo/.github/Workflows");
        File.Create("/tmp/samplerepositories/completerepo/.github/Workflows/surelyaclassic");
        
    }
    [Fact]
    public void GitignoreCheckWorksForTrashRepo()
    {
        var test = Checks.GitignoreCheck(GetAllFiles.AsList("/tmp/samplerepositories/trashrepo"));
        
        Assert.Equal("false", test[1]);
    }
    [Fact]
    public void LicenseCheckWorksForTrashRepo()
    {
        var test = Checks.LicenseCheck(GetAllFiles.AsList("/tmp/samplerepositories/trashrepo"));
        
        Assert.Equal("false", test[1]);
    }
    /*
    [Fact]
    public void SecretCheckWorksForTrashRepo()
    {
        var test = Checks.SecretCheck("/tmp/samplerepositories/trashrepo");
        
        Assert.Equal("false", test[1]);
    }*/
    [Fact]
    public void ReadmeCheckWorksForTrashRepo()
    {
        var test = Checks.ReadmeCheck(GetAllFiles.AsList("/tmp/samplerepositories/trashrepo"));
        
        Assert.Equal("false", test[1]);
    }
    [Fact]
    public void TestCheckWorksForTrashRepo()
    {
        var test = Checks.TestCheck(GetAllFiles.AsList("/tmp/samplerepositories/trashrepo"));
        
        Assert.Equal("false", test[1]);
    }
    [Fact]
    public void WorkflowCheckWorksForTrashRepo()
    {
        var test = Checks.WorkflowCheck(GetAllFiles.AsList("/tmp/samplerepositories/trashrepo"));
        
        Assert.Equal("true", test[1]);
    }

    [Fact]
    public void RunAllChecksWorksForTrashRepo()
    {
        var list = GetAllFiles.AsList("/tmp/samplerepositories/trashrepo");
        var emptyList = new List<string>(); emptyList.Add("true"); emptyList.Add("true"); emptyList.Add("true"); emptyList.Add("true"); emptyList.Add("true"); emptyList.Add("true");
        var test = Checks.RunAllChecks(list, "/tmp/samplerepositories/trashrepo", emptyList);
        
        Assert.Equal("false", test[1]);
    }
    
    
    
    [Fact]
    public void GitignoreCheckWorksForCompleteRepo()
    {
        var test = Checks.GitignoreCheck(GetAllFiles.AsList("/tmp/samplerepositories/completerepo"));
        
        Assert.Equal("true", test[1]);
    }
    [Fact]
    public void LicenseCheckWorksForCompleteRepo()
    {
        var test = Checks.LicenseCheck(GetAllFiles.AsList("/tmp/samplerepositories/completerepo"));
        
        Assert.Equal("true", test[1]);
    }
    [Fact]
    public void SecretCheckWorksForCompleteRepo()
    {
        var test = Checks.SecretCheck("/tmp/samplerepositories/completerepo");
        
        Assert.Equal("true", test[1]);
    }
    [Fact]
    public void ReadmeCheckWorksForCompleteRepo()
    {
        var test = Checks.ReadmeCheck(GetAllFiles.AsList("/tmp/samplerepositories/completerepo"));
        
        Assert.Equal("true", test[1]);
    }
    [Fact]
    public void TestCheckWorksForCompleteRepo()
    {
        var test = Checks.TestCheck(GetAllFiles.AsList("/tmp/samplerepositories/completerepo"));
        
        Assert.Equal("true", test[1]);
    }
    [Fact]
    public void WorkflowCheckWorksForCompleteRepo()
    {
        var test = Checks.WorkflowCheck(GetAllFiles.AsList("/tmp/samplerepositories/completerepo"));
        
        Assert.Equal("true", test[1]);
    }

    [Fact]
    public void RunAllChecksWorksForCompleteRepo()
    {
        var list = GetAllFiles.AsList("/tmp/samplerepositories/completerepo");
        var emptyList = new List<string>(); emptyList.Add("true"); emptyList.Add("true"); emptyList.Add("true"); emptyList.Add("true"); emptyList.Add("true"); emptyList.Add("true");
        var test = Checks.RunAllChecks(list, "/tmp/samplerepositories/completerepo", emptyList);
        
        Assert.Equal("true", test[1]);
    }
}