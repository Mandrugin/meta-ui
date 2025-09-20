using System;
using System.Collections.Generic;
using System.Threading;
using Meta.UseCases;

namespace Meta.Views
{
    public class WheelsChangingPresenter: IDisposable
    {
        private readonly IWheelsChangingUseCase _wheelsChangingUseCase;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private List<WheelsData> _wheelsData;

        public event Action<List<WheelsData>> WheelsListChanged; 

        public WheelsChangingPresenter(IWheelsChangingUseCase wheelsChangingUseCase, CancellationTokenSource cancellationTokenSource)
        {
            _wheelsChangingUseCase = wheelsChangingUseCase;
            _cancellationTokenSource = cancellationTokenSource;
        }

        public void TryWheels(WheelsData data)
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
