using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Sheepishly.Ingest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                ServiceRuntime.RegisterServiceAsync("IngestType", context => new Ingest(context)).GetAwaiter().GetResult();
                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Ingest).Name);

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e);
                throw;
            }
        }
    }
}
