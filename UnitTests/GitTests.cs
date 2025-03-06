using RepoLinter;
using Xunit.Abstractions;

namespace UnitTests;

public class GitTests
{
    [Fact]
    public void DefaultPathGitCloningWorks()
    {
        var git = new Git("https://github.com/TheoMASvensson/RepositoryLinterProject.git");
        git.Clone();
        var exists = Directory.Exists(git.PathToGitRepository);
        Directory.Delete(git.PathToGitRepository, true);
        Assert.True(exists);
    }

    [Fact]
    public void ChosenPathGitCloningWorks()
    {
        var git = new Git("https://github.com/TheoMASvensson/RepositoryLinterProject.git", "/tmp/gitrepolinter");
        git.Clone();
        var exists = Directory.Exists(git.PathToGitRepository);
        Directory.Delete(git.PathToGitRepository, true);
        Assert.True(exists);
    }

    [Fact]
    public void PathGitCloningWorks()
    {
        
    }
}