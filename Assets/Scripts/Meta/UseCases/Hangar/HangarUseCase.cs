using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public class HangarUseCase : IHangarUseCase, IDisposable
    {
        private readonly IHangarBackend _hangarBackend;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public HangarUseCase(IHangarBackend hangarBackend)
        {
            _hangarBackend = hangarBackend;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public UniTask<VehicleData> GetCurrentVehicle()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
