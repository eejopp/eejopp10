using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DiskExplorer
{
    class Program
    {
        static void Main(string[] args)
        {            

            DriveInfo[] disks = GetDisks();


            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                //Console.WriteLine("Выберите диск:");
                Menu.PrintXY(20, 0, " Этот компьютер");
                Menu.PrintXY(0, 1, "--------------------------------------------------------------------------------------");
                Console.WriteLine();

                for (int i = 0; i < disks.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }

                    try
                    {
                        Console.WriteLine($"{disks[i].Name,4}{disks[i].TotalSize / 1024 / 1024,10} MB{disks[i].TotalFreeSpace / 1024 / 1024,10} MB");
                    }
                    catch
                    {
                        Console.WriteLine($"{disks[i].Name,4}");
                    }
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex = selectedIndex == 0 ? disks.Length - 1 : selectedIndex - 1;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex = selectedIndex == disks.Length - 1 ? 0 : selectedIndex + 1;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    Console.WriteLine("Выбранный диск: " + disks[selectedIndex].Name);
                    FolderExplorer.FoldersMenu(disks[selectedIndex].Name,0);
                    //Console.ReadLine();
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        static DriveInfo[] GetDisks()
        {
            //List<Disk> disks = new List<Disk>();
            DriveInfo[] disks=DriveInfo.GetDrives();
       

            return disks;
        }
    }

    class Disk
    {
        public string Name { get; set; }//автоматическое свойство
    }


}

class FolderExplorer
{
/*    static void Main(string[] args)
    {
        Console.WriteLine("Folder Explorer");
        Console.WriteLine("===============");

        //Console.Write("Enter a drive letter (e.g. C:): ");
        //string drive = Console.ReadLine();
        FoldersMenu("C:\\", 0);

    }
*/
    public static void FoldersMenu(string path, int level)
    {
        begin:
        List<string> folders = GetFoldersAndFiles(path);

        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
            //Console.WriteLine("Выберите папку:");
            Menu.PrintXY(25, 0, "Папка " + path);
            Menu.PrintXY(0, 1, "-----------------------------------------------------------------------------");
            Menu.PrintXY(60, 2, "F1 - создать папку");
            Menu.PrintXY(60, 3, "F2 - Создать файл");
            Menu.PrintXY(60, 4, "F3 - Удалить");
            Menu.PrintXY(60, 5, "----------------------------------");
            Menu.PrintXY(0, 2, "");
            //Console.WriteLine();
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
                if (File.Exists(selectedFolder))
                {
                    ProcessStartInfo procInfo = new ProcessStartInfo();
                    procInfo.FileName = selectedFolder;
                    procInfo.UseShellExecute = true;
                    Process.Start(procInfo);
                    
                    //Process process =new Process();
                    
                    //process.Start();
                }

                else
                    FoldersMenu(selectedFolder, level + 1);
            }
            else if (keyInfo.Key==ConsoleKey.F1)
            {
                Menu.PrintXY(60, 6, "Введите имя папки:");
                string name=Console.ReadLine();
                Directory.CreateDirectory(path+"\\"+name);
                //FoldersMenu(path, level);
                goto begin;
            }
            else if (keyInfo.Key == ConsoleKey.F2)
            {
                Menu.PrintXY(60, 6, "Введите имя файла:");
                string name = Console.ReadLine();
                try
                {
                    File.Create(path + "\\" + name);
                    goto begin;
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else if (keyInfo.Key == ConsoleKey.F3)
            {
                Menu.PrintXY(60, 6, "Вы уверены?(Y,N):");
                string yn = Console.ReadLine();
                if (yn.ToLower() == "y")
                {
                    try
                    {
                        File.Delete(path + folders[selectedIndex]);
                        goto begin;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
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
            Console.WriteLine("Произошла ошибка: " + ex.Message);
        }

        return folders;
    }

    static List<string> GetFoldersAndFiles(string path)
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
            string[] files = Directory.GetFiles(path);
            foreach (string folderPath in files)
            {
                string folderName = Path.GetFileName(folderPath);
                folders.Add(folderName);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка: " + ex.Message);
        }

        return folders;
    }
}

class Menu
{
    static public void PrintXY(int x,int y,string text)
    {
        Console.SetCursorPosition(x,y);
        Console.Write(text);
    }
}
