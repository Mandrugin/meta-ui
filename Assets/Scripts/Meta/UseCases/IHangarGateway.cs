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
        public UniTask<List<Vehicle>> GetAllVehicles(CancellationToken cancellationToken);
        public UniTask<List<Vehicle>> GetBoughtVehicles();
        public UniTask<Vehicle> GetCurrentVehicle(CancellationToken cancellationToken);
        public UniTask<bool> SetCurrentVehicle(Vehicle vehicle);
        public UniTask<bool> BuyVehicle(Vehicle vehicle);
#endregion

#region Wheels
        public UniTask<List<Wheels>> GetAllWheels(string vehicleId, CancellationToken cancellationToken);
        public UniTask<List<Wheels>> GetBoughtWheels(string vehicleId, CancellationToken cancellationToken);
        public UniTask<Wheels> GetSetWheels(Vehicle vehicle, CancellationToken cancellationToken);
        public UniTask<bool> SetWheels(Vehicle vehicle, Wheels wheels, CancellationToken cancellationToken);
        public UniTask<bool> BuyWheels(Vehicle vehicle, Wheels wheel, CancellationToken cancellationToken);
#endregion
    }
}
