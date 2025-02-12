using System.CommandLine;
using RepoLinter;

var rootCommand = new RootCommand("A simple program that takes a URL or a path and validates it.");

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

            try
            {
                git.Clone();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            
            string clonedFoldersPath = Path.Join(Directory.GetCurrentDirectory(), "/git");
            var fileList = Directory.EnumerateFiles(clonedFoldersPath).ToList();
            
            if (fileList.Count == 0)
            {
                Console.WriteLine("No files found in given folder.");
                Environment.Exit(1);
            }

            foreach (var filepath in fileList)
            {
                Console.WriteLine(filepath);
            }
            
            Console.WriteLine(fileList.Contains(Path.Join(clonedFoldersPath, ".gitignore")));
            Console.WriteLine(fileList.Contains(Path.Join(clonedFoldersPath, "LICENSE"))); //LICENCE
            
        }, urlArg);

        

        pathCommand.SetHandler((path)=> {
            Console.WriteLine($"You entered path: {path}");
            
            List<string> fileList = new List<string>(Directory.EnumerateFiles(path));

            if (fileList.Count == 0)
            {
                Console.WriteLine("No files found in given folder.");
                Environment.Exit(1);
            }

            foreach (var filepath in fileList)
            {
                Console.WriteLine(filepath);
            }
            
            Console.WriteLine(fileList.Contains(Path.Join(path, ".gitignore")));
            Console.WriteLine(fileList.Contains(Path.Join(path, "LICENSE"))); //LICENCE
            
            
        }, pathArg);

        return await rootCommand.InvokeAsync(args);