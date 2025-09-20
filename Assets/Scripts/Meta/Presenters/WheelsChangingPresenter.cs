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
        private readonly CancellationToken _cancellationToken;

        private List<WheelsData> _wheelsData;

        public event Action<List<WheelsDataView>> WheelsListChanged = delegate { }; 

        public WheelsChangingPresenter(IWheelsChangingUseCase wheelsChangingUseCase, CancellationToken cancellationToken)
        {
            _wheelsChangingUseCase = wheelsChangingUseCase;
            _cancellationToken = cancellationToken;
        }

        public async UniTaskVoid UpdateWheelsData()
        {
            var wheelsDataViews = new  List<WheelsDataView>();
            var wheelsData = await _wheelsChangingUseCase.GetAllWheels(_cancellationToken);
            wheelsData.ForEach(x => wheelsDataViews.Add(new WheelsDataView {Id = x.Id}));
            WheelsListChanged(wheelsDataViews);
        }

        public void TryWheels(WheelsData data)
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
