using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.UseCases;

namespace Meta.Presenters
{
    public class WheelsChangingPresenter: IDisposable
    {
        private readonly IWheelsChangingUseCase _wheelsChangingUseCase;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private List<WheelsData> _wheelsData;

        public event Action<List<WheelsDataView>> WheelsListChanged = delegate { };

        public WheelsChangingPresenter(IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _wheelsChangingUseCase = wheelsChangingUseCase;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async UniTaskVoid UpdateWheelsData()
        {
            var wheelsDataViews = new  List<WheelsDataView>();
            var wheelsData = await _wheelsChangingUseCase.GetAllWheels(_cancellationTokenSource.Token);
            wheelsData.ForEach(x => wheelsDataViews.Add(new WheelsDataView {Id = x.Id}));
            WheelsListChanged(wheelsDataViews);
        }

        public void TryWheels(WheelsData data)
        {
            
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }
    }
}
