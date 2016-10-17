using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Sheepishly.Tracker.Interfaces
{
    public interface ILocationViewer : IService
    {
        Task<KeyValuePair<float, float>?> GetLastSheepLocation(Guid sheepId);
        Task<DateTime?> GetLastReportTime(Guid sheepId);
    }
}
