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
- Run ```dotnet restore``` in the root of the repository
- Run the program ```dotnet run -- -h``` for help (the "--" is only required when no commands are given.)

## How to use



### Run the linter on a single repository


## Configuration
The program can be configured using a toml file that is contained in the repository. If no changes are made.......

```toml


```



## Run tests

The unit tests are using the [xUnit](https://xunit.net/) framework.

- Clone the repository
- Install all dependencies listed in the [Run without Docker](#run-without-docker) section
- Run ```dotnet restore``` in the root of the repository to restore dependencies
- Run ```dotnet test``` in the root of the repository to run tests
