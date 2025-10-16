using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Scripting;

namespace Meta.UseCases
{
    [Preserve]
    public class WheelsChangingUseCase : UseCase, IWheelsChangingUseCase, IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource;

        private readonly IHangarGateway _hangarGateway;
        private readonly IHangarUseCase _hangarUseCase;
        
        private bool _isBusy;

        public WheelsChangingUseCase(IHangarGateway hangarGateway, IHangarUseCase hangarUseCase)
        {
            _hangarGateway = hangarGateway;
            _hangarUseCase = hangarUseCase;
            _hangarUseCase.OnStartWheelsChanging += StartUseCase;
            _hangarUseCase.OnFinishWheelsChanging += FinishUseCase;
        }

        public UniTask<bool> BuyWheels(WheelsData wheelsData, CancellationToken  cancellationToken)
        {
            throw new NotImplementedException();
        }


        public async UniTask<bool> TryWheelsOut(WheelsData wheelsData, CancellationToken  cancellationToken)
        {
            var vehicle = await _hangarGateway.GetCurrentVehicle(cancellationToken);
            var allWheels = await _hangarGateway.GetAllWheels(vehicle.Id, cancellationToken);
            var wheels = allWheels.FirstOrDefault(x => x.Id == wheelsData.Id);
            if (wheels == null)
                return false;
            _hangarUseCase.TryWheelsOut(wheelsData);
            return true;
        }

        public UniTask<bool> SetWheels(WheelsData wheelsData, CancellationToken  cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async UniTask<List<WheelsData>> GetAllWheels(VehicleData vehicleData, CancellationToken  cancellationToken)
        {
            var wheelsDataList = new List<WheelsData>();
            var wheels = await _hangarGateway.GetAllWheels(vehicleData.Id, cancellationToken);
            wheels.ForEach(x => wheelsDataList.Add(new WheelsData
            {
                Id = x.Id,
                Price = x.Price
            }));
            return wheelsDataList;
        }

        public async UniTask<List<WheelsData>> GetBoughtWheels(VehicleData vehicleData, CancellationToken  cancellationToken)
        {
            var wheelsDataList = new List<WheelsData>();
            var wheels = await _hangarGateway.GetBoughtWheels(vehicleData.Id, cancellationToken);
            wheels.ForEach(x => wheelsDataList.Add(new WheelsData
            {
                Id = x.Id,
                Price = x.Price
            }));
            return wheelsDataList;
        }

        public async UniTask<WheelsData> GetCurrentWheels(VehicleData vehicleData, CancellationToken  cancellationToken)
        {
            var currentWheels = await _hangarGateway.GetCurrentWheels(vehicleData.Id, cancellationToken);
            return new WheelsData
            {
                Id = currentWheels.Id,
                Price = currentWheels.Price
            };
        }

        public void Dispose()
        {
            _hangarUseCase.OnStartWheelsChanging -= StartUseCase;
            _hangarUseCase.OnFinishWheelsChanging -= FinishUseCase;
            _cancellationTokenSource?.Dispose();
        }
    }
}
