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

            Monitoring monitor = new Monitoring();

            //We accept the process name from the user, followed by input validation
            Console.Write("\nInsert name process: ");
            while (true)
            {
                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    monitor.NameProcessGetSet = input;
                    break;
                }
                else
                {
                    Console.WriteLine("\n[ERROR]: Invalid input");
                    Console.Write("Retry Insert name process: ");
                }
            }

            //We accept the lifetime of the process from the user, followed by input validation
            Console.Write("\nInsert max life span minutes: ");
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int numb))
                {
                    monitor.MaxLifeSpanGetSet = numb;
                    break;
                }
                else
                {
                    Console.WriteLine("\n[ERROR]: Invalid input");
                    Console.Write("Retry Insert max life span minutes: ");
                }
            }

            //We accept from the user a period for checking processes, followed by checking input
            Console.Write("\nInsert period monitoring minutes: ");
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int numb))
                {
                    monitor.PeriodMonitoringGetSet = numb;
                    break;
                }
                else
                {
                    Console.WriteLine("\n[ERROR]: Invalid input");
                    Console.Write("Retry Insert period monitoring minutes: ");
                }
            }

            cancellationToken = new CancellationTokenSource();

            //Start monitoring processes
            Task.Run(() => monitor.StartMonitor(cancellationToken.Token));

            //Waiting for a keypress - "Q" to exit and close the application
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
