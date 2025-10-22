using System.Linq;
using Meta.Entities;

namespace Meta.UseCases
{
    public class BuyPriceVisitor: IGetVisitor<long>
    {
        public long Visit(Storage storage)
        {
            return storage.BoughtVehicles.Sum(Visit);
        }

        public long Visit(Wallet wallet)
        {
            throw new System.NotImplementedException();
        }

        public long Visit(Vehicle vehicle)
        {
            var price = vehicle.BuyPrice;
            foreach (var wheels in vehicle.BoughtWheels)
            {
                price += Visit(wheels);
            }

            return price;
        }

        public long Visit(Wheels wheels)
        {
            return wheels.Price;
        }
    }
}
