using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;

namespace Meta.UseCases
{
    public interface IHangarGateway: IDisposable
    {
#region Money
    public event Action<long> OnSoftChanged;
    public event Action<long> OnHardChanged;
    public UniTask<long> GetHardBalance(CancellationToken cancellationToken);
    public UniTask<long> GetSoftBalance(CancellationToken cancellationToken);
#endregion

#region Vehicles
        public UniTask<List<Vehicle>> GetAllVehicles(CancellationToken cancellationToken);
        public UniTask<List<Vehicle>> GetBoughtVehicles();
        public UniTask<Vehicle> GetSetVehicle(CancellationToken cancellationToken);
        public UniTask<bool> SetSetVehicle(Vehicle vehicle);
        public UniTask<bool> BuyVehicle(Vehicle vehicle);
#endregion

#region Wheels
        public UniTask<List<Wheels>> GetAllWheels(Vehicle vehicle, CancellationToken cancellationToken);
        public UniTask<List<Wheels>> GetBoughtWheels(Vehicle vehicle, CancellationToken cancellationToken);
        public UniTask<Wheels> GetSetWheels(Vehicle vehicle, CancellationToken cancellationToken);
        public UniTask<bool> SetWheels(Vehicle vehicle, Wheels wheels, CancellationToken cancellationToken);
        public UniTask<bool> BuyWheels(Vehicle vehicle, Wheels wheel, CancellationToken cancellationToken);
#endregion
    }
}
