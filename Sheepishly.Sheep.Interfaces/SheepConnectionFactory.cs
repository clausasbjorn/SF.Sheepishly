using System;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace Sheepishly.Sheep.Interfaces
{
    public static class SheepConnectionFactory
    {
        private static readonly Uri SheepServiceUrl = new Uri("fabric:/Sheepishly/SheepActorService");

        public static ISheep GetSheep(ActorId actorId)
        {
            return ActorProxy.Create<ISheep>(actorId, SheepServiceUrl);
        }
    }
}
