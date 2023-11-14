using System;
using System.Collections.Generic;
using System.IO;

namespace FolderExplorer
{
    class FolderExplorer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Folder Explorer");
            Console.WriteLine("===============");

            //Console.Write("Enter a drive letter (e.g. C:): ");
            //string drive = Console.ReadLine();
            FoldersMenu("C:\\",0);
            
        }

        static void FoldersMenu(string path,int level)
        {
            List<string> folders = GetFolders(path);

            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose a folder:");

                for (int i = 0; i < folders.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }

                    Console.WriteLine(folders[i]);
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex = selectedIndex == 0 ? folders.Count - 1 : selectedIndex - 1;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex = selectedIndex == folders.Count - 1 ? 0 : selectedIndex + 1;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {

                    string selectedFolder = Path.Combine(path, folders[selectedIndex]);
                    FoldersMenu(selectedFolder,level+1);
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    level--;
                    break;
                }
            }
        }

        static List<string> GetFolders(string path)
        {
            List<string> folders = new List<string>();

            try
            {
                string[] folderPaths = Directory.GetDirectories(path);
                foreach (string folderPath in folderPaths)
                {
                    string folderName = Path.GetFileName(folderPath);
                    folders.Add(folderName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return folders;
        }
    }
}