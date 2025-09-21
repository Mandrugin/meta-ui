using System;
using System.Threading;

namespace Meta.UseCases
{
    public class HangarUseCase : UseCase, IHangarUseCase, IDisposable
    {
        public event Action OnStartWheelsChanging = delegate { };
        public event Action OnFinishWheelsChanging = delegate { };

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public void StartWheelsChanging()
        {
            OnStartWheelsChanging.Invoke();
        }

        public void FinishWheelsChanging() => OnFinishWheelsChanging.Invoke();

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
