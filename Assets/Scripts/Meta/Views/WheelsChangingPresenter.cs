using System;
using System.Threading;
using Meta.UseCases;

namespace Meta.Views
{
    public class WheelsChangingPresenter: IDisposable
    {
        private readonly IWheelsChangingUseCase _wheelsChangingUseCase;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public WheelsChangingPresenter(IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _wheelsChangingUseCase = wheelsChangingUseCase;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
