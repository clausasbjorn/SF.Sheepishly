using System;
using System.Threading;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace Sheepishly.Sheep
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                ActorRuntime.RegisterActorAsync<Sheep>((context, information) => new ActorService(context, information, () => new Sheep()));
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
