using MyMonitor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace MyMonitor
{
    public class Monitoring : IMonitoring
    {
        private string NameProcess;
        private int MaxLifeSpan;
        private int PeriodMonitoring;
        private readonly string Path = "logfile.txt";


        public string NameProcessGetSet { get => NameProcess; set => NameProcess = value; }
        public int MaxLifeSpanGetSet { get => MaxLifeSpan; set => MaxLifeSpan = value; }
        public int PeriodMonitoringGetSet { get => PeriodMonitoring; set => PeriodMonitoring = value; }


        //Saving information about closed processes to a file
        public async void LogInfo(DateTime start, string status = "Closed")
        {
            string header = "Date Time\t\t\tName Process\t\tStart Time\t\tExit Time\tMax Life Span, min\tStatus";
            using (StreamWriter writer = new StreamWriter(Path, true))
            {
                if (File.Exists(Path))
                {
                    if (new FileInfo(Path).Length == 0) await writer.WriteLineAsync(header);
                }
                string infoFile = string.Format("{0,-23} {1,-25} {2,-15} {3,-15} {4,-19} {5,-10}", start.ToString("dd/MM/yyyy"),
                     NameProcess, start.ToString("HH:mm:ss"), DateTime.Now.ToString("HH:mm:ss"), MaxLifeSpan, status);
                await writer.WriteLineAsync(infoFile);
            }
        }

        //I calculate the running time of the process from the moment it was opened
        public int CkeckTimeLifeNow(DateTime timeSart)
        {
            return DateTime.Now.Minute - timeSart.Minute;
        }

        //Application start method
        public async Task StartMonitor(CancellationToken token)
        {
            Console.WriteLine("\nMonitor Started!");
            Console.WriteLine("\nTo stop and close the application press - Q");
            try
            {
                //Run an infinite loop to check processes within a certain period
                while (true)
                {
                    CheckLifeProcess();
                    await Task.Delay(PeriodMonitoring * 60000, token);
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //The method in which the process is searched for and, if necessary, its closing
        public void CheckLifeProcess()
        {
            //I get an array of all processes by the given name
            Process[] process = Process.GetProcessesByName(NameProcess);
            foreach (Process processItem in process)
            {
                //Checking the amount of time a process has been running
                if (CkeckTimeLifeNow(processItem.StartTime) >= MaxLifeSpan)
                {
                    try
                    {
                        //I write information about the process to the file and close it
                        LogInfo(processItem.StartTime);
                        processItem.CloseMainWindow();

                        //If the process is not closed, then completely kill it
                        if (!processItem.HasExited)
                        {
                            processItem.Kill();
                            processItem.WaitForExit();
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }

    }
}
