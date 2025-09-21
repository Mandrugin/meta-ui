using System.Collections.Generic;

namespace Meta.Entities
{
    public class Vehicle
    {
        public string Id;
        public Wheels StockWheels;
        public Wheels CurrentWheels;
        public long BuyPrice;
        public long SellPrice;
        public List<Wheels> BoughtWheels;
        public List<Wheels> AllWheels;
    }
}
