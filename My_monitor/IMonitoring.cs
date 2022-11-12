using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyMonitor
{
    public interface IMonitoring
    {
        void LogInfo(DateTime start, string status);
        int CkeckTimeLifeNow(DateTime timeSart);
        Task StartMonitor(CancellationToken token);
        void CheckLifeProcess();
    }
}
