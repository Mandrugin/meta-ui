using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public class VehicleUseCase : IVehicleUseCase
    {
        public event Action OnShowPresenter = delegate { };
        public event Action OnHidePresenter = delegate { };

        public void ShowPresenter()
        {
            OnShowPresenter.Invoke();
        }

        public void HidePresenter()
        {
            OnHidePresenter.Invoke();
        }

        public event Action<VehicleData> OnCurrentVehicleChanged = delegate { };
        
        public async UniTask<VehicleData> GetCurrentVehicle(CancellationToken cancellationToken)
        {
            return await UniTask.FromResult(new VehicleData());
        }
    }
}