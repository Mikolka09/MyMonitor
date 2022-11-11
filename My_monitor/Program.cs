using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyMonitor
{
    internal class Program
    {
        static CancellationTokenSource cancellationToken;
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Monitor!");

            Monitor monitor = new Monitor();
            Console.Write("\nInsert name process: ");
            monitor.NameProcess1 = Console.ReadLine();

            Console.Write("\nInsert max life span minutes: ");
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int numb))
                {
                    monitor.MaxLifeSpan1 = numb;
                    break;
                }
                else
                {
                    Console.WriteLine("\n[ERROR]: Invalid input");
                    Console.Write("Retry Insert max life span minutes: ");
                }
            }

            Console.Write("\nInsert period monitoring minutes: ");
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int numb))
                {
                    monitor.PeriodMonitoring1 = numb;
                    break;
                }
                else
                {
                    Console.WriteLine("\n[ERROR]: Invalid input");
                    Console.Write("Retry Insert period monitoring minutes: ");
                }
            }

            cancellationToken = new CancellationTokenSource();
            Task.Run(() => monitor.StartMonitor(cancellationToken.Token));

            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    cancellationToken?.Cancel();
                    Console.WriteLine("\nApplication closed!");
                    break;
                }

            }

            Console.WriteLine("\nPlease press any key...");
            Console.ReadKey();
        }
    }
}
