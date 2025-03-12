# RepositoryLinter

This is a simple Git repository linter for a Sofware Development Course at BTH.

## Features

- The linter checks whether or not the repository contains a README, LICENSE file, .gitignore file and GitHub WorkFlow files (.github/workflows/).
- The linter checks what files filepaths include the word "test" and prints them out.
- The linter checks the repository for secrets using Trufflehog (https://github.com/trufflesecurity/trufflehog).

## Run the program

- Clone the repository
- Ensure that Git is installed
- Ensure that .NET Core 8 is installed
- Ensure that Trufflehog (https://github.com/trufflesecurity/trufflehog) is installed

## How to use

### Run the linter on a single repository using url
To run the linter on a GitHub repository using a url, traverse to the "RepoLinter" folder in the terminal and run the following command:
```bash
dotnet run url https://YourChosenGitHubURL
```
### Run the linter on a single repository using local path
To run the linter on a local Github repository, traverse to the "RepoLinter" folder in the terminal and run the following command:
```bash
dotnet run path /path/to/chosen/repository
```

## Configuration
The program can be configured using a toml file that is contained in the repository. If no changes are made it will run all checks and place the cloned repository in /tmp/gitrepo

```toml
#Full path to an empty folder where repository should be cloned, if left empty defaults to /tmp/gitrepo.
pathtofolder = ""

#Write "true" if related check should be run, anything else is considered to be an unwanted check.
[tests]
gitignore = "true"
license = "true"
secret = "true"
readme = "true"
test = "true"
workflow = "true"

```

## Run tests

The unit tests are using the xUnit framework but they are far from comprehensive, I apologize.

- Clone the repository
- Install all dependencies listed in the [Run without Docker](#run-without-docker) section
- Run ```dotnet test``` in the root of the repository to run tests
