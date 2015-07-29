using System;
using System.Diagnostics;

namespace DotNEET.Debug
{
    internal class DisposableStopwatch : IDisposable
    {
        private readonly string description;
        private readonly Stopwatch stopwatch;

        public DisposableStopwatch(string description)
        {
            this.description = description;
            this.stopwatch = new Stopwatch();
            Trace.TraceInformation("[START " + description + "]");
            this.stopwatch.Start();
        }

        public void Dispose()
        {
            stopwatch.Stop();
            Trace.TraceInformation("[STOP " + description + " TIME : " + this.stopwatch.Elapsed + "]");
        }
    }
}