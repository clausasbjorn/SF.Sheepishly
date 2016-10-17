using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Sheepishly.Sheep.Interfaces;
using Sheepishly.Tracker.Interfaces;

namespace Sheepishly.Tracker
{
    internal sealed class Tracker : StatefulService, ILocationReporter, ILocationViewer
    {
        public Tracker(StatefulServiceContext serviceContext) : base(serviceContext)
        {
        }

        public Tracker(StatefulServiceContext serviceContext, IReliableStateManagerReplica reliableStateManagerReplica) : base(serviceContext, reliableStateManagerReplica)
        {
        }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[] {
                new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context))
            };
        }

        protected override async Task RunAsync(CancellationToken cancelServicePartitionReplica)
        {
        }

        public async Task ReportLocation(Location location)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var timestamps = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, DateTime>>("timestamps");
                var sheepIds = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, ActorId>>("sheepIds");

                var timestamp = DateTime.UtcNow;

                // Update sheep
                var sheepActorId = await sheepIds.GetOrAddAsync(tx, location.SheepId, ActorId.CreateRandom());
                await SheepConnectionFactory.GetSheep(sheepActorId).SetLocation(timestamp, location.Latitude, location.Longitude);

                // Update service with new timestamp
                await timestamps.AddOrUpdateAsync(tx, location.SheepId, DateTime.UtcNow, (guid, time) => timestamp);
                await tx.CommitAsync();
            }
        }

        public async Task<DateTime?> GetLastReportTime(Guid sheepId)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var timestamps = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, DateTime>>("timestamps");

                var timestamp = await timestamps.TryGetValueAsync(tx, sheepId);
                await tx.CommitAsync();

                return timestamp.HasValue ? (DateTime?)timestamp.Value : null;
            }
        }

        public async Task<KeyValuePair<float, float>?> GetLastSheepLocation(Guid sheepId)
        {
            using (var tx = StateManager.CreateTransaction())
            {
                var sheepIds = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, ActorId>>("sheepIds");

                var sheepActorId = await sheepIds.TryGetValueAsync(tx, sheepId);
                if (!sheepActorId.HasValue)
                    return null;

                var sheep = SheepConnectionFactory.GetSheep(sheepActorId.Value);
                return await sheep.GetLatestLocation();
            }
        }
    }
}
