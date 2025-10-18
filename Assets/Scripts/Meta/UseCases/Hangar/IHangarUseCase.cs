using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IHangarUseCase: IUseCase
    {
        public event Action<long> OnHardChanged;
        public event Action<long> OnSoftChanged;
        public UniTask<long> GetHardBalance(CancellationToken cancellationToken);
        public UniTask<long> GetSoftBalance(CancellationToken cancellationToken);
        public void StartWheelsChanging();
        public void FinishWheelsChanging();
    }
}
