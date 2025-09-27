using System;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IHangarUseCase: IUseCase
    {
        public event Action OnStartWheelsChanging;
        public event Action OnFinishWheelsChanging;
        public void StartWheelsChanging();
        public void FinishWheelsChanging();
        public UniTask<VehicleData> GetCurrentVehicle();
    }
}
