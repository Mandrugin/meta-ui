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
        
        private bool isBusy = false;

        public WheelsChangingUseCase(IHangarBackend hangarBackend)
        {
            _hangarBackend = hangarBackend;
            Fetch().Forget();
        }
        
        private async UniTaskVoid Fetch()
        {
            isBusy = true;
            _currentVehicle = await _hangarBackend.GetCurrentVehicle();
            _wheels = await _hangarBackend.GetAllWheels(_currentVehicle);
            isBusy = false;
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

        public async UniTask<List<WheelsData>> GetAllWheels(CancellationToken  cancellationToken)
        {
            var wheelsDataList = new List<WheelsData>();
            await UniTask.WaitForSeconds(1, cancellationToken: cancellationToken);
            await UniTask.WaitWhile(() => isBusy, PlayerLoopTiming.Update, cancellationToken);
            _wheels.ForEach(x => wheelsDataList.Add(new WheelsData { Id = x.Id }));
            return wheelsDataList;
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
