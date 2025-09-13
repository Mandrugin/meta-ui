using System.Collections.Generic;

namespace Entities
{
    public class Hangar
    {
        public Vehicle CurrentVehicle;
        public List<Vehicle> AllVehicles;
    }

    public class Vehicle
    {
        public string Id;
        public Wheels Wheels;
    }

    public class Wheels
    {
        public string Id;
        public int Price;
    }
}
