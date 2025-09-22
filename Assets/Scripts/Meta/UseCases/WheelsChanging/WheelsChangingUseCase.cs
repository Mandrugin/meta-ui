using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;
using Meta.Gateway;

namespace Meta.UseCases
{
    public class WheelsChangingUseCase : UseCase, IWheelsChangingUseCase, IDisposable
    {
        private List<Wheels> _wheels;
        private List<WheelsData> _wheelsDataList = new List<WheelsData>();
        private Vehicle _currentVehicle;
        
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

            Fetch().Forget();
        }
        
        private async UniTaskVoid Fetch()
        {
            _isBusy = true;
            _currentVehicle = await _hangarGateway.GetCurrentVehicle();
            _wheels = await _hangarGateway.GetAllWheels(_currentVehicle);
            _isBusy = false;
        }

        public UniTask<bool> BuyWheels(int wheelsIndex, CancellationToken  cancellationToken)
        {
            throw new NotImplementedException();
        }

        public event Action<WheelsData> OnWheelsTriedOut = delegate { };

        public UniTask<bool> TryWheelsOut(int wheelsIndex, CancellationToken  cancellationToken)
        {
            OnWheelsTriedOut.Invoke(_wheelsDataList[wheelsIndex]);
            return UniTask.FromResult(true);
        }

        public UniTask<bool> SetWheels(int wheelsIndex, CancellationToken  cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async UniTask<List<WheelsData>> GetAllWheels(CancellationToken  cancellationToken)
        {
            _wheelsDataList.Clear();
            await UniTask.WaitForSeconds(1, cancellationToken: cancellationToken);
            await UniTask.WaitWhile(() => _isBusy, PlayerLoopTiming.Update, cancellationToken);
            _wheels.ForEach(x => _wheelsDataList.Add(new WheelsData { Id = x.Id }));
            return _wheelsDataList;
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
            _hangarUseCase.OnStartWheelsChanging -= StartUseCase;
            _hangarUseCase.OnFinishWheelsChanging -= FinishUseCase;
            _cancellationTokenSource?.Dispose();
        }
    }
}
