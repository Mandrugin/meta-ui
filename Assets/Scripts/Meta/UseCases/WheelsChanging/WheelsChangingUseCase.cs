using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;

namespace Meta.UseCases
{
    public class WheelsChangingUseCase : IWheelsChangingUseCase, IDisposable
    {
        private List<Wheels> _wheels;
        private Vehicle _currentVehicle;
        
        private CancellationTokenSource _cancellationTokenSource;

        private readonly IHangarBackend _hangarBackend;

        public WheelsChangingUseCase(IHangarBackend hangarBackend)
        {
            _hangarBackend = hangarBackend;
            Update().Forget();
        }
        
        private async UniTaskVoid Update()
        {
            _currentVehicle = await _hangarBackend.GetCurrentVehicle();
            _wheels = await _hangarBackend.GetAllWheels(_currentVehicle);
        }

        public UniTask<bool> BuyWheels(int wheelsIndex, CancellationToken  cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask<bool> TryWheelsOut(int wheelsIndex, CancellationToken  cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask<bool> SetWheels(int wheelsIndex, CancellationToken  cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask<List<WheelsData>> GetAllWheels(CancellationToken  cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask<List<WheelsData>> GetBoughtWheels(CancellationToken  cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask<WheelsData> GetCurrentWheels(CancellationToken  cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
