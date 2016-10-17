using System;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace Sheepishly.Tracker.Interfaces
{
    public static class TrackerConnectionFactory
    {
        private static readonly Uri LocationReporterServiceUrl = new Uri("fabric:/Sheepishly/Tracker");

        public static ILocationReporter CreateLocationReporter()
        {
            return ServiceProxy.Create<ILocationReporter>(LocationReporterServiceUrl, new ServicePartitionKey(0));
        }
        public static ILocationViewer CreateLocationViewer()
        {
            return ServiceProxy.Create<ILocationViewer>(LocationReporterServiceUrl, new ServicePartitionKey(0));
        }
    }
}
