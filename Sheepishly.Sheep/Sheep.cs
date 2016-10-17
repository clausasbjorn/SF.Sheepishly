using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors.Runtime;
using Sheepishly.Sheep.Interfaces;

namespace Sheepishly.Sheep
{
    internal class Sheep : Actor, ISheep
    {
        [DataContract]
        internal sealed class LocationAtTime
        {
            public DateTime Timestamp { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }
        
        [DataContract]
        internal sealed class SheepState
        {
            [DataMember]
            public List<LocationAtTime> LocationHistory { get; set; }
        }
        
        protected override async Task OnActivateAsync()
        {
            var state = await StateManager.TryGetStateAsync<SheepState>("State");
            if (!state.HasValue)
                await StateManager.AddStateAsync("State", new SheepState { LocationHistory = new List<LocationAtTime>() });
        }
        
        public async Task SetLocation(DateTime timestamp, float latitude, float longitude)
        {
            var state = await StateManager.GetStateAsync<SheepState>("State");
            state.LocationHistory.Add(new LocationAtTime() { Timestamp = timestamp, Latitude = latitude, Longitude = longitude });

            await StateManager.AddOrUpdateStateAsync("State", state, (s, actorState) => state);
        }
        
        public async Task<KeyValuePair<float, float>> GetLatestLocation()
        {
            var state = await StateManager.GetStateAsync<SheepState>("State");
            var location = state.LocationHistory.OrderByDescending(x => x.Timestamp).Select(x => 
                new KeyValuePair<float, float>(x.Latitude, x.Longitude)
            ).FirstOrDefault();

            return location;
        }
    }
}
