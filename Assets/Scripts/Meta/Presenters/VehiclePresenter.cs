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
        public event Action<VehicleDataView> OnVehicleChanged = delegate { };

        private readonly IVehicleUseCase _vehicleUseCase;
        private readonly IWheelsChangingUseCase _wheelsChangingUseCase;
        
        private readonly CancellationTokenSource _cancellationTokenSource = new ();
        
        public VehiclePresenter(IVehicleUseCase vehicleUseCase, IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _vehicleUseCase = vehicleUseCase;
            _wheelsChangingUseCase = wheelsChangingUseCase;
            _vehicleUseCase.OnCurrentVehicleChanged += ChangeCurrentVehicle;
            _wheelsChangingUseCase.OnCurrentWheelsChanged += ChangeCurrentWheels;
            _wheelsChangingUseCase.OnWheelsListChanged += OnWheelsListChanged;
            _vehicleUseCase.UpdateVehicleData(_cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            _vehicleUseCase.OnCurrentVehicleChanged -= ChangeCurrentVehicle;
            _wheelsChangingUseCase.OnCurrentWheelsChanged -= ChangeCurrentWheels;
            _wheelsChangingUseCase.OnWheelsListChanged -= OnWheelsListChanged;
            _cancellationTokenSource.Dispose();
        }

        private void ChangeCurrentVehicle(VehicleData vehicleData)
        {
            OnVehicleChanged.Invoke(new VehicleDataView
            {
                Id = vehicleData.Id
            });
        }

        private void ChangeCurrentWheels(WheelsData wheelsData)
        {
            OnWheelsChanged(new WheelsDataView
            {
                Id = wheelsData.Id,
                Price = wheelsData.Price
            });
        }

        private void OnWheelsListChanged(List<WheelsData> allWheelsData, List<WheelsData> boughtWheelsData, WheelsData setWheelsData)
        {
            ChangeCurrentWheels(setWheelsData);
        }

        public void SetNexVehicle()
        {
            
        }

        public void SetPrevVehicle()
        {
            
        }
    }
}
