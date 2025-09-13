using System.Collections.Generic;

namespace Entities
{
    public class Vehicle
    {
        public string Id;
        public Wheels CurrentWheels;
        public List<Wheels> BoughtWheels;
        public List<Wheels> AllWheels;
    }
}
