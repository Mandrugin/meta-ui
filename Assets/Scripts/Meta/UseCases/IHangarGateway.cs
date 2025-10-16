using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;

namespace Meta.UseCases
{
    public interface IHangarGateway
    {
#region Money
    public event Action<long> OnHardChanged;
    public event Action<long> OnSoftChanged;
    public UniTask<long> GetHardBalance(CancellationToken cancellationToken);
    public UniTask<long> GetSoftBalance(CancellationToken cancellationToken);
#endregion

#region Vehicles
        public UniTask<List<Vehicle>> GetAllVehicles();
        public UniTask<List<Vehicle>> GetBoughtVehicles();
        public UniTask<Vehicle> GetCurrentVehicle();
        public UniTask<bool> SetCurrentVehicle(Vehicle vehicle);
        public UniTask<bool> BuyVehicle(Vehicle vehicle);
#endregion

#region Wheels
        public UniTask<List<Wheels>> GetAllWheels(Vehicle vehicle);
        public UniTask<List<Wheels>> GetBoughtWheels(Vehicle vehicle);
        public UniTask<Wheels> GetCurrentWheels(Vehicle vehicle);
        public UniTaskVoid SetCurrentWheels(Vehicle vehicle, Wheels wheels);
        public UniTask<bool> BuyWheels(Vehicle vehicle, Wheels wheel);
#endregion
    }
}
