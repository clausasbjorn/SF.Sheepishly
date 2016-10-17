using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace Sheepishly.Sheep.Interfaces
{
    public interface ISheep : IActor
    {
        Task<KeyValuePair<float, float>> GetLatestLocation();

        Task SetLocation(DateTime timestamp, float latitude, float longitude);
    }
}
