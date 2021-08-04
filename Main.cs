using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Am_I_Tainted
{
    public class Program
    {
        public static int files_scanned = 0;
        public static int files_tainted = 0;

        public static string logspath = "";
        public static int logs_length = 0;

        public static Regex tainted = new Regex("istainted[\\s=]+true");
        public static List<string> tainted_files = new List<string>();

        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Title = "Am I Tainted? v1.1.0";
            Console.ForegroundColor = ConsoleColor.Green;


            DisplayInfo("Loading...", InfoType.Info);

            
            var localpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string robloxpath = localpath + "\\Roblox";
            bool archive = false;

            
            if (Directory.Exists(robloxpath))
            {
                DisplayInfo("Found Roblox directory", InfoType.Success);
            }
            else
            {
                DisplayInfo("Could not find Roblox directory", InfoType.Error);

                while (true)
                {
                    Console.WriteLine("Input directory of Roblox folder: ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(">> ");
                    string input = Console.ReadLine();

                    if (!input.Contains("AppData\\Local"))
                    {
                        DisplayInfo("This filepath isn't an AppData\\Local folder.", InfoType.Error);
                        continue;
                    }
                    else if (!input.Contains("Roblox"))
                    {
                        DisplayInfo("This filepath isn't the Roblox folder.", InfoType.Error);
                        continue;
                    }
                    else if (!(input.Substring(input.Length - 6) == "Roblox"))
                    {
                        DisplayInfo("This filepath cannot be within the Roblox directory.", InfoType.Error);
                        continue;
                    }
                    else
                    {
                        if (input.Contains("AppData\\Local\\Roblox"))
                        {
                            DisplayInfo("Found roblox folder", InfoType.Success);
                            robloxpath = input;
                            break;
                        }
                        else
                        {
                            DisplayInfo("Something went wrong.", InfoType.Error);
                            continue;
                        }
                    }

                }

            }

            if (Directory.Exists(robloxpath + "\\logs"))
            {
                DisplayInfo("Found logs folder", InfoType.Success);
                logspath = robloxpath + "\\logs";
            }
            else
            {
                DisplayInfo("Couldn't find logs folder, verify that it exists.", InfoType.Error);
                Console.ReadKey();
                Environment.Exit(0);
            }
            if (Directory.Exists(robloxpath + "\\logs\\archive"))
            {
                DisplayInfo("Found archive folder", InfoType.Success);
                DisplayInfo("Taints are stored inside of .ini files inside of the archive folder.", InfoType.Info);
                DisplayInfo("Files outside of the archive folder are still scanned anyways.", InfoType.Info);
                archive = true;

            }
            else
            {
                DisplayInfo("Couldn't find archive folder", InfoType.Success);
                DisplayInfo("Taints are stored inside of .ini files inside of the archive folder.", InfoType.Info);
                DisplayInfo("Files outside of the archive folder are still scanned anyways.", InfoType.Info);
            }
            Console.WriteLine("Press any key to begin. . .");
            Console.ReadKey(true);

            


            Console.Clear();

            DisplayInfo($"Scanning files [logs]...", InfoType.Info);

            var logs = Directory.GetFiles(logspath);
            logs_length = logs.Length;
            DisplayInfo($"Files scanned: {files_scanned}/{logs_length}", InfoType.Info);
            DisplayInfo($"Taints detected: {files_tainted}", InfoType.Info);


            foreach (string file in Directory.GetFiles(logspath))
            {
                string text = File.ReadAllText(file);
                if (tainted.Match(text.ToLower()).Success)
                {
                    files_tainted++;
                    tainted_files.Add(file);
                    Console.CursorTop = 2;
                    DisplayInfo($"Taints detected: {files_tainted}", InfoType.Info);
                    System.Threading.Thread.Sleep(1);
                }
                files_scanned++;

                Console.CursorTop = 1;
                DisplayInfo($"Files scanned: {files_scanned}/{logs_length}", InfoType.Info);

                
            }
            Console.CursorTop = 0;
            DisplayInfo($"Finished scanning logs!             ", InfoType.Success);
            if (archive)
            {
                files_scanned = 0;
                files_tainted = 0;

                logspath += "\\archive";
                Console.CursorTop = 4;

                DisplayInfo($"Scanning files [archive]...", InfoType.Info);

                logs = Directory.GetFiles(logspath);
                logs_length = logs.Length;
                DisplayInfo($"Files scanned: {files_scanned}/{logs_length}", InfoType.Info);
                DisplayInfo($"Taints detected: {files_tainted}", InfoType.Info);


                foreach (string file in Directory.GetFiles(logspath))
                {
                    string text = File.ReadAllText(file);
                    if (tainted.Match(text.ToLower()).Success)
                    {
                        files_tainted++;
                        tainted_files.Add(file);
                        Console.CursorTop = 6;
                        DisplayInfo($"Taints detected: {files_tainted}", InfoType.Info);
                        System.Threading.Thread.Sleep(1);
                    }
                    files_scanned++;

                    Console.CursorTop = 5;
                    DisplayInfo($"Files scanned: {files_scanned}/{logs_length}", InfoType.Info);


                }
                Console.CursorTop = 4;
                DisplayInfo($"Finished scanning archive!                   ", InfoType.Success);
                Console.CursorTop = 8;
            }
            Console.WriteLine();

            foreach (string file in tainted_files) 
            {
                DisplayInfo($"Tainted file: {file.Split("logs\\")[1]}", InfoType.Taint);
            }

            Console.WriteLine("Press any key to close. . .");
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        public static void DisplayInfo(string info, InfoType infotype = InfoType.Info)
        {
            switch (infotype)
            {
                case InfoType.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("INFO");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("] ");
                    break;

                case InfoType.Success:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("SUCCESS");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("] ");
                    break;

                case InfoType.Error:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("ERROR");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("] ");
                    break;

                case InfoType.Taint:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("TAINT");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("] ");
                    break;
            }
            Console.WriteLine(info);
        }

        public enum InfoType
        {
            Info,
            Success,
            Error,
            Taint

        }
    }
}
