using System.CommandLine;
using RepoLinter;
using Tomlyn;
using Tomlyn.Model;

var rootCommand = new RootCommand("A simple linter that takes a GitHub URL or path to a repository and validates it.");

        var urlCommand = new Command("url", "Run linter on URL");
        var urlArg = new Argument<string>("url", "URL");

        urlArg.AddValidator((url) => {

            if (!URLValidator.IsValidURL(url.Tokens[0].Value)){
                url.ErrorMessage = $"Invalid URL: {url.Tokens[0].Value}";
            }
        });

        urlCommand.AddArgument(urlArg);

        var pathCommand = new Command("path", "Run linter on path");
        var pathArg = new Argument<string>("path", "Path");

        pathArg.AddValidator((path) =>
        {
            if (!pathValidator.isValidPath(path.Tokens[0].Value)){
                Console.WriteLine(Directory.GetCurrentDirectory());
                path.ErrorMessage = $"Invalid Path: {path.Tokens[0].Value}";
            }
            
        });

        pathCommand.AddArgument(pathArg);

        rootCommand.AddCommand(urlCommand);
        rootCommand.AddCommand(pathCommand);

        urlCommand.SetHandler((url) => {
            Console.WriteLine($"You entered URL: {url}");
            
            string tomlContent = File.ReadAllText("ConfigFile.toml");
            
            TomlTable tomlTable = Toml.ToModel(tomlContent);
            
            string ?thePath = tomlTable["pathtofolder"] as string;
            
            var theChecks = new List<string>();
            
            TomlTable checkTable = (TomlTable)tomlTable["tests"];
            var gitignore = checkTable["gitignore"] as string; theChecks.Add(gitignore!);
            var license = checkTable["license"] as string; theChecks.Add(license!);
            var secret = checkTable["secret"] as string; theChecks.Add(secret!);
            var readme = checkTable["readme"] as string; theChecks.Add(readme!);
            var test = checkTable["test"] as string; theChecks.Add(test!);
            var workflow = checkTable["workflow"] as string; theChecks.Add(workflow!);
            
            var git = new Git(url, thePath!);
            
            // Try to clone repository from given url
            try
            {
                git.Clone();
            }
            catch (Exception e) // Handle errors when cloning
            {
                Console.WriteLine("Error cloning: " + e.Message);
                Environment.Exit(1);
            }
            
            // Get all files in repository as a List
            var clonedFoldersPath = git.PathToGitRepository;
            var fileList = GetAllFiles.AsList(clonedFoldersPath);

            //foreach (var filepath in fileList)
            //{
                //Console.WriteLine(filepath);
            //}
            try
            {
                Console.WriteLine(git.GetCommitsAndContributors(thePath!));
                var output = Checks.RunAllChecks(fileList, clonedFoldersPath, theChecks);
                if (output[1] != "true")
                {
                    Environment.ExitCode = 100;
                }
                
                Console.WriteLine(output[0]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            
        }, urlArg);

        

        pathCommand.SetHandler((path)=> {
            Console.WriteLine($"You entered path: {path}");
            
            string tomlContent = File.ReadAllText("ConfigFile.toml");
            
            TomlTable tomlTable = Toml.ToModel(tomlContent);
            
            var theChecks = new List<string>();
            
            TomlTable checkTable = (TomlTable)tomlTable["tests"];
            var gitignore = checkTable["gitignore"] as string; theChecks.Add(gitignore!);
            var license = checkTable["license"] as string; theChecks.Add(license!);
            var secret = checkTable["secret"] as string; theChecks.Add(secret!);
            var readme = checkTable["readme"] as string; theChecks.Add(readme!);
            var test = checkTable["test"] as string; theChecks.Add(test!);
            var workflow = checkTable["workflow"] as string; theChecks.Add(workflow!);
            
            var git = new Git("", path);
            
            // Get all files in repository as a list
            var fileList = GetAllFiles.AsList(path);

            //foreach (var filepath in fileList)
            //{
            //    Console.WriteLine(filepath);
            //}

            try
            {
                Console.WriteLine(git.GetCommitsAndContributors(path));
                var output = Checks.RunAllChecks(fileList, path, theChecks);
                if (output[1] != "true")
                {
                    Environment.ExitCode = 100;
                }
                
                Console.WriteLine(output[0]);
                Console.WriteLine("Exited with ExitCode: " + Environment.ExitCode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            
        }, pathArg);

        //TODO: Return 0 if all checks are passed. A 1 if any of them fail
        //TODO: Finish Dockerfile

        return await rootCommand.InvokeAsync(args);