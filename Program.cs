using System.CommandLine;


var bundleCommand = new Command("bundle", "Bundle code files to a single file");

var outputBundleOption = new Option<FileInfo>(new[]{"--output", "-o"}, "file path and name");
bundleCommand.AddOption(outputBundleOption);

var noteBundleOption = new Option<bool>(new[] { "--note", "-n" }, "add file name");
bundleCommand.AddOption(noteBundleOption);

var sortBundleOption = new Option<string>(
    new[]{"--sort","-s"},
    () => "name", 
    "Sort files by 'name' (default) or 'extension'"
);
bundleCommand.AddOption(sortBundleOption);

var authorBundleOption = new Option<string>(new[] { "--author", "-a" }, "add author to the file");
bundleCommand.AddOption(authorBundleOption);

var remove_empty_linesBundleOption = new Option<bool>(new[] { "--remove-empty-lines", "-r" }, "remove empty lines of files");
bundleCommand.AddOption(remove_empty_linesBundleOption);

var languageBundleOption = new Option<string[]>(new[] { "--language","-l","--lang" }, "List of programming languages (e.g., cs, js,txt,docx) to include. Use 'all' to include all files.")
{
    IsRequired = true,
    AllowMultipleArgumentsPerToken = true
};
bundleCommand.AddOption(languageBundleOption);



bundleCommand.SetHandler((output,note,sort, remove_empty_lines, author, languages) =>
{
    try {

       string currentDirectory = Path.GetFullPath(".");
        string outputPath = Path.GetFullPath(output.FullName);
        if (outputPath.StartsWith(currentDirectory, StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Error: The output file must be located outside of the current directory.\r\n");
            return;
        }

        // יצירת/איפוס הקובץ המאוחד
        using (StreamWriter writer = new StreamWriter(output.FullName, false))
        {
            if (author!=null)
            {
                writer.WriteLine($"//{author}");
            }


            if (sort != "name" && sort != "extension")
            {
                Console.WriteLine($"שגיאה: הערך '{sort}' אינו תקין. יש לבחור בין 'name' ל-'extension'.");
                return;
            }


      
            string[] files = Directory.GetFiles(".", "*", SearchOption.AllDirectories)
                .Where(file => !file.Contains("\\bin\\") && !file.Contains("\\debug\\"))
                .ToArray();



            if (!languages.Contains("all", StringComparer.OrdinalIgnoreCase))
            {
                var normalizedLanguages = languages.Select(lang => lang.Trim().ToLower()).ToArray();
                var extensions = normalizedLanguages.Select(lang => $".{lang}");
                files = files.Where(file => extensions.Contains(Path.GetExtension(file).ToLower())).ToArray();
            }

            files = sort switch
            {
                "extension" => files.OrderBy(f => Path.GetExtension(f)).ToArray(),
                _ => files.OrderBy(f => Path.GetFileName(f)).ToArray() 

            };

            foreach (string file in files)
            {
                if (note)
                {
                    writer.WriteLine($"//{file}");
                }
                string[] lines = File.ReadAllLines(file);
                
                if (remove_empty_lines)
                {
                    lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
                }

                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        Console.WriteLine("The contents of all files in the folder have been successfully merged into a single file.");



    } catch (DirectoryNotFoundException ex) {
        
        Console.WriteLine("file path is invalid");
        
    }
},outputBundleOption, noteBundleOption, sortBundleOption,remove_empty_linesBundleOption, authorBundleOption, languageBundleOption);

var create_rsp_command = new Command("create-rsp", "create rsp file");

create_rsp_command.SetHandler(() =>
{

    Console.WriteLine("enter output file to includes all fiels");
    var output = Console.ReadLine();
    while (string.IsNullOrEmpty(output))
    {
        Console.WriteLine("output is required");
        Console.WriteLine("enter output file to includes all fiels");
        output = Console.ReadLine();
    }

    Console.WriteLine("enter note if you whant to write file name to all files(yes/no)");
    var note = Console.ReadLine();
    if (note != "yes"&& note != "no")
    {
        Console.WriteLine("now note = no");
        note="no";
        
    }

    Console.WriteLine("enter sort by (name/extension)");
    var sort = Console.ReadLine();
    if (sort != "name" && sort != "extension")
    {
        Console.WriteLine("now sort =name");
        sort = "name";
    }

    Console.WriteLine("enter author if you whant authorname in the file");
    var author = Console.ReadLine();

    Console.WriteLine("enter (yes/no) if you whant to remove_empty_lines");
    var remove_empty_lines = Console.ReadLine();
    if (remove_empty_lines != "yes" && remove_empty_lines != "no")
    {
        Console.WriteLine("now remove_empty_lines = no");
        remove_empty_lines ="no";
    }

    Console.WriteLine("write list of language to includ separate by spaces.\n Write 'all' or skip to include all files ");
    var language = Console.ReadLine();
    while (string.IsNullOrEmpty(language))
    {
        Console.WriteLine("write list of language to includ separate by spaces.\n Write 'all' or skip to include all files ");
        language =Console.ReadLine();
    }

    string command = $"bundle --output \"{output}\"" +
                   (note == "yes" ? " --note" : "") +
                   $" --sort {sort}" +
                   (!string.IsNullOrEmpty(author) ? $" --author \"{author}\"" : "") +
                   (remove_empty_lines == "yes" ? " --remove-empty-lines" : "") +
                   $" --language {language}";
    
    Console.WriteLine(command);
    // כתיבת הפקודה לקובץ תגובה
    var rspFileName = "bundle.rsp";
    File.WriteAllText(rspFileName, command);

    Console.WriteLine($"Response file created: {rspFileName}");

});

var rootComand = new RootCommand("Root command for bundler CLI");
rootComand.AddCommand(create_rsp_command);
rootComand.AddCommand(bundleCommand);


await rootComand.InvokeAsync(args);

