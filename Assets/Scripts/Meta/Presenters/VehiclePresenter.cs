using System;
using System.Collections.Generic;
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
        public event Action<VehicleDataView> OnVehicleChanged = delegate { };

        private readonly IVehicleUseCase _vehicleUseCase;
        private readonly IWheelsChangingUseCase _wheelsChangingUseCase;
        private readonly UseCaseMediator _useCaseMediator;
        
        private readonly CancellationTokenSource _cancellationTokenSource = new ();
        
        public VehiclePresenter(IVehicleUseCase vehicleUseCase, UseCaseMediator useCaseMediator, IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _vehicleUseCase = vehicleUseCase;
            _useCaseMediator = useCaseMediator;
            _wheelsChangingUseCase = wheelsChangingUseCase;

            _useCaseMediator.OnCurrentVehicleChanged += ChangeCurrentVehicle;
            _useCaseMediator.OnCurrentWheelsChanged += ChangeCurrentWheels;
            _wheelsChangingUseCase.OnWheelsListChanged += OnWheelsListChanged;
            _vehicleUseCase.UpdateVehicleData(_cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            _useCaseMediator.OnCurrentVehicleChanged -= ChangeCurrentVehicle;
            _useCaseMediator.OnCurrentWheelsChanged -= ChangeCurrentWheels;
            _wheelsChangingUseCase.OnWheelsListChanged -= OnWheelsListChanged;
            _cancellationTokenSource.Dispose();
        }

        private void ChangeCurrentVehicle(VehicleData vehicleData)
        {
            OnVehicleChanged.Invoke(vehicleData.ToVehicleDataView());
        }

        private void ChangeCurrentWheels(WheelsData wheelsData)
        {
            OnWheelsChanged(wheelsData.ToWheelsDataView());
        }

        private void OnWheelsListChanged(List<WheelsData> allWheelsData, List<WheelsData> boughtWheelsData, WheelsData setWheelsData)
        {
            ChangeCurrentWheels(setWheelsData);
        }

        public void SetNexVehicle()
        {
            _vehicleUseCase.SetNextVehicle(_cancellationTokenSource.Token).Forget();
        }

        public void SetPrevVehicle()
        {
            _vehicleUseCase.SetPrevVehicle(_cancellationTokenSource.Token).Forget();
        }
    }
}
