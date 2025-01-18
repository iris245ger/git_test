using System.Net;
using System.Net.Sockets;
using System;
using System.Diagnostics;

//Console.WriteLine("Logs from your program will appear here!");

bool online = true;
// Wait for user input
while (online)
{
    try
    {
        Console.Write("$ ");

        var input = Console.ReadLine();
        
        if (input != null) { 
            string[] command = input.Split(' ');
            switch (command[0])
            {
                case "type":
                    Type(input);
                    break;
                case "echo":
                    Console.WriteLine(input.Remove(0, 5));
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                case "pwd":
                    Console.WriteLine(Directory.GetCurrentDirectory());
                    break;
                case "cd":
                    CD(input);
                    break;
                default:
                    if (IsValidPath(command[0]) == $"{command[0]}: not found")
                    {
                        Console.WriteLine($"{input}: not found");
                    }
                    else
                    {
                        RunningProgram(command[0], command[1]);
                    }
                    break;
            }
        }
    }
    catch (Exception ex) { 
        Console.WriteLine($"Error: {ex.Message}");
    }
}
static void Type (string typeInput)
{
    List<string> builInCommands = [ "echo", "exit", "type", "pwd", "cd" ];
    string commandWord = typeInput.Remove(0, 5);
    if (builInCommands.Contains(commandWord))
    {
        Console.WriteLine($"{commandWord} is a shell builtin");
    } else
    {
        string path = IsValidPath(commandWord);
        if (path == $"{commandWord}: not found")
        {
            Console.WriteLine($"{commandWord}: not found");
        }
        else
        {
            Console.WriteLine($"{commandWord} is {path}");
        }
    }
}
static string IsValidPath (string programInput)
{
    bool isFound = false;

    var paths = Environment.GetEnvironmentVariable("PATH");
    var pathsArr = paths.Split(':');
    var filePath = "";
    foreach (var path in pathsArr)
    {
        filePath = Path.Combine(path, programInput);
        if (Path.Exists(filePath))
        {
            isFound = true;
            break;
        }
    }
    if (!isFound)
    {
        return $"{programInput}: not found";
    }
    else
    {
        return filePath;
    }
}
static void CD (string input)
{
    string path = input.Remove(0, 3);
    string fullPath = null;
    
    if (path == @"~") {
        fullPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    } else
    {
        fullPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), path);
        fullPath = System.IO.Path.GetFullPath(fullPath);
    }

    
    if (IsValidPath(fullPath) == $"{fullPath}: not found")
    {
        Console.WriteLine($"cd: {fullPath}: No such file or directory");
    } else
    {
        Directory.SetCurrentDirectory(fullPath);
    }
}
static void RunningProgram (string arg1, string arg2)
{
    using var program = new Process();
    program.StartInfo.FileName = arg1;
    program.StartInfo.Arguments = arg2;
    program.Start();
}

