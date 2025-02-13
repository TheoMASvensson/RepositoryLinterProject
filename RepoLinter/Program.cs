using System.CommandLine;
using System.Threading.Channels;
using RepoLinter;

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
                path.ErrorMessage = $"Invalid Path: {path.Tokens[0].Value}";
            }
            
        });

        pathCommand.AddArgument(pathArg);

        rootCommand.AddCommand(urlCommand);
        rootCommand.AddCommand(pathCommand);

        urlCommand.SetHandler((url) => {
            Console.WriteLine($"You entered URL: {url}");
            var git = new Git(url);
            
            // Try to clone repository from given url
            try
            {
                git.Clone();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            
            // Get all files in repository as a List
            var clonedFoldersPath = git.PathToGitRepository;
            var fileList = getAllFiles.AsList(clonedFoldersPath);

            //foreach (var filepath in fileList)
            //{
                //Console.WriteLine(filepath);
            //}

            Console.WriteLine(Checks.RunAllChecks(fileList, clonedFoldersPath));
            
        }, urlArg);

        

        pathCommand.SetHandler((path)=> {
            Console.WriteLine($"You entered path: {path}");
            
            // Get all files in repository as a list
            var fileList = getAllFiles.AsList(path);

            //foreach (var filepath in fileList)
            //{
            //    Console.WriteLine(filepath);
            //}
            
            Console.WriteLine(Checks.RunAllChecks(fileList, path));
            
        }, pathArg);

        return await rootCommand.InvokeAsync(args);