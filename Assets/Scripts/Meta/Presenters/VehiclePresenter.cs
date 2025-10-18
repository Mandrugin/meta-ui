using System;
using System.Collections.Generic;
using System.Threading;
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
        
        private readonly CancellationTokenSource _cancellationTokenSource = new ();
        
        public VehiclePresenter(IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _wheelsChangingUseCase = wheelsChangingUseCase;
            _wheelsChangingUseCase.OnCurrentWheelsChanged += ChangeCurrentWheels;
            _wheelsChangingUseCase.OnWheelsListChanged += OnWheelsListChanged;
            _wheelsChangingUseCase.UpdateWheelsData(_cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            _wheelsChangingUseCase.OnCurrentWheelsChanged -= ChangeCurrentWheels;
            _wheelsChangingUseCase.OnWheelsListChanged -= OnWheelsListChanged;
            _cancellationTokenSource.Dispose();
        }

        private void ChangeCurrentWheels(WheelsData wheelsData)
        {
            OnWheelsChanged(new WheelsDataView
            {
                Id = wheelsData.Id,
                Price = wheelsData.Price
            });
        }

        private void OnWheelsListChanged(List<WheelsData> arg1, List<WheelsData> arg2, WheelsData arg3)
        {
            ChangeCurrentWheels(arg3);
        }
    }
}