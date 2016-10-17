using System;

namespace Sheepishly.Tracker.Interfaces
{
    public class Location
    {
        public Guid SheepId { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
