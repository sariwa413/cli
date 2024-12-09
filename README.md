# Files Bundler Cli

The Files Bundler CLI is a command line tool (CLI) that allows you to bundle multiple text files into one file easily and efficiently. The tool offers many options for customizing the way the bundle is packaged, including arranging the files, adding comments, filtering by programming languages, and more.

----
### Usage Example

**Submitting exercises to the teacher**:

When submitting exercises to the teacher is required, you can run this command to consolidate all the files into one file, to make it easier to transfer and check:

fib bundle --output "submission.cs" --sort name --remove-empty-lines -n --author "Student_Name" --lang cs
### Creating a Response File:

The tool also allows you to create a response file (`.rsp`), which includes all the options automatically. This file is useful when you want to store all the command settings in advance and run the tool easily.

To create a response file, run the following command: `fib create-rsp`

The tool will then ask you to enter the required parameters (such as the file name, whether to add comments, how to order the files, etc.). The response file `bundle.rsp` will be created, and can be run as follows: `fib @bundle.rsp`

### Output of the `fib bundle --help` command:
```bash
Options:
-o, --output <output> File path and name for the bundled output
-n, --note Include the source file path as a comment
-s, --sort <sort> Sort files by name (default) or extension [default: name]
-r, --remove-empty-lines, --rml Remove empty lines
-a, --author <author> Add the author's name as a header comment
-l, --lang, --language <language> Specify programming languages ​​to include.
Use 'all' to include all files (default) [default: all]
-?, -h, --help Show help and usage information
```
----
### Technology:

The tool is built on **.NET 8** and uses `System.CommandLine` to manage commands and options on the command line.
