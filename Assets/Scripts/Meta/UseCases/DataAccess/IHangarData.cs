using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Meta.Entities;

namespace Meta.UseCases
{
    public interface IHangarData
    {
#region Vehicles
        public UniTask<List<Vehicle>> GetAllVehicles();
        public UniTask<List<Vehicle>> GetBoughtVehicles();
        public UniTask<bool> BuyVehicle(Vehicle vehicle);
        public UniTask<Vehicle> GetCurrentVehicle();
        public UniTask<bool> SetCurrentVehicle(Vehicle vehicle);
#endregion

#region Wheels
        public UniTask<List<Wheels>> GetAllWheels(Vehicle vehicle);
        public UniTask<List<Wheels>> GetBoughtWheels(Vehicle vehicle);
        public UniTask<bool> BuyWheels(Vehicle vehicle, Wheels wheel);
        public UniTask<Wheels> GetCurrentWheels(Vehicle vehicle);
        public UniTaskVoid SetCurrentWheels(Vehicle vehicle, Wheels wheels);
#endregion
        
    }
}
