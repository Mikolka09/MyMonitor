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
    internal class Monitor
    {
        private string NameProcess;
        private int MaxLifeSpan;
        private int PeriodMonitoring;
        private readonly string Path = "logfile.txt";


        public string NameProcess1 { get => NameProcess; set => NameProcess = value; }
        public int MaxLifeSpan1 { get => MaxLifeSpan; set => MaxLifeSpan = value; }
        public int PeriodMonitoring1 { get => PeriodMonitoring; set => PeriodMonitoring = value; }

        private async void LogInfo(DateTime start, string status = "Closed")
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

        private int CkeckTimeLifeNow(DateTime timeSart)
        {
            return DateTime.Now.Minute - timeSart.Minute;
        }

        public async Task StartMonitor(CancellationToken token)
        {
            Console.WriteLine("\nMonitor Started!");
            Console.WriteLine("\nTo stop and close the application press - Q");
            try
            {

                while (true)
                {
                    CheckLifeProcess();
                    await Task.Delay(PeriodMonitoring * 60000, token);
                    token.ThrowIfCancellationRequested();
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

        private void CheckLifeProcess()
        {

            Process[] process = Process.GetProcessesByName(NameProcess);
            foreach (Process processItem in process)
            {
                if (CkeckTimeLifeNow(processItem.StartTime) >= MaxLifeSpan)
                {
                    try
                    {
                        LogInfo(processItem.StartTime);
                        processItem.CloseMainWindow();
                       
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
