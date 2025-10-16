using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Meta.UseCases;
using UnityEngine.Scripting;

namespace Meta.Presenters
{
    [Preserve]
    public class WheelsChangingPresenter: IDisposable
    {
        private readonly IHangarUseCase _hangarUseCase;
        private readonly IWheelsChangingUseCase _wheelsChangingUseCase;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public event Action OnStartUseCase = delegate { };
        public event Action OnFinishUseCase = delegate { };
        public WheelsChangingPresenter(IHangarUseCase hangarUseCase, IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _hangarUseCase = hangarUseCase;
            _wheelsChangingUseCase = wheelsChangingUseCase;
            _wheelsChangingUseCase.OnStartUseCase += Start;
            _wheelsChangingUseCase.OnFinishUseCase += Finish;
            
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _wheelsChangingUseCase.OnStartUseCase -= Start;
            _wheelsChangingUseCase.OnFinishUseCase -= Finish;
            _cancellationTokenSource.Dispose();
        }

        private void Start() => OnStartUseCase.Invoke();
        private void Finish() => OnFinishUseCase.Invoke();

        public async UniTask<List<WheelsDataView>> GetAllWheels(VehicleDataView vehicleDataView)
        {
            var wheelsData = await _wheelsChangingUseCase.GetAllWheels(new VehicleData{Id = vehicleDataView.Id}, _cancellationTokenSource.Token);
            var wheelsDataViews = new List<WheelsDataView>();
            foreach (var x in wheelsData)
                wheelsDataViews.Add(new WheelsDataView
                {
                    Id = x.Id,
                    Price = x.Price
                });
            return wheelsDataViews;
        }
        public void TryWheels(WheelsDataView wheelsDataView)
        {
            _wheelsChangingUseCase.TryWheelsOut(new WheelsData{Id = wheelsDataView.Id, Price = wheelsDataView.Price}, _cancellationTokenSource.Token);
        }

        public async Task<VehicleDataView> GetCurrentVehicle(CancellationToken token)
        {
            VehicleData vehicleData = await _hangarUseCase.GetCurrentVehicle(token);
            return new VehicleDataView
            {
                Id = vehicleData.Id,
            };
        }

        public async UniTask<List<WheelsDataView>> GetWheelsDataView(CancellationToken cancellationToken)
        {
            var vehicle = await _hangarUseCase.GetCurrentVehicle(cancellationToken);
            var allWheelsData = await _wheelsChangingUseCase.GetAllWheels(vehicle, cancellationToken);
            var boughtWheelsData = await _wheelsChangingUseCase.GetBoughtWheels(vehicle, cancellationToken);
            var currentWheelsData = await _wheelsChangingUseCase.GetCurrentWheels(vehicle, cancellationToken);
            var wheelsDataViews = new List<WheelsDataView>();

            foreach (var wheelsData in allWheelsData)
            {
                var wheelsDataView = new WheelsDataView
                {
                    Id = wheelsData.Id,
                    Price = wheelsData.Price,
                    Status = ""
                };

                if (boughtWheelsData.Contains(wheelsData))
                {
                    wheelsDataView.Status = "Bought";
                }

                if (wheelsData.Equals(currentWheelsData))
                    wheelsDataView.Status = "Current";
                
                wheelsDataViews.Add(wheelsDataView);
            }
            
            return wheelsDataViews;
        }
    }
}
