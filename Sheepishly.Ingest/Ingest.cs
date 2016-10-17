using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Linq;
using Microsoft.ServiceFabric.Services;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Sheepishly.Ingest
{
    public class Ingest : StatelessService
    {
        public Ingest(StatelessServiceContext serviceContext) : base(serviceContext)
        {
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return Context.CodePackageActivationContext.GetEndpoints()
                .Where(endpoint => endpoint.Protocol.Equals(EndpointProtocol.Http) || endpoint.Protocol.Equals(EndpointProtocol.Https))
                .Select(endpoint => new ServiceInstanceListener(serviceContext => new OwinCommunicationListener("api", new Startup(), serviceContext)));
        }
    }
}