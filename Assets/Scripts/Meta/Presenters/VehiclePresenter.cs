using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.UseCases;
using UnityEngine.Scripting;

namespace Meta.Presenters
{
    [Preserve]
    public class VehiclePresenter: IDisposable
    {
        public event Action<WheelsDataView> OnWheelsChanged = delegate { };

        private readonly IHangarUseCase _hangarUseCase;
        private readonly IWheelsChangingUseCase _wheelsChangingUseCase;
        
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public VehiclePresenter(IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _wheelsChangingUseCase = wheelsChangingUseCase;
            _wheelsChangingUseCase.OnWheelsTriedOut += ChangeWheels;
            _wheelsChangingUseCase.GetCurrentWheels(_cancellationTokenSource.Token).ContinueWith(ChangeWheels);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _wheelsChangingUseCase.OnWheelsTriedOut -= ChangeWheels;
        }

        protected virtual void ChangeWheels(WheelsData wheelsData)
        {
            OnWheelsChanged(new WheelsDataView
            {
                Id = wheelsData.Id,
                Price = wheelsData.Price
            });
        }
        
        
    }
}