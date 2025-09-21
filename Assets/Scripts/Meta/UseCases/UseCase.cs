using System;

namespace Meta.UseCases
{
    public class UseCase : IUseCase
    {
        public event Action OnStartUseCase = delegate { };
        public event Action OnFinishUseCase = delegate { };
        public void StartUseCase() => OnStartUseCase.Invoke();
        public void FinishUseCase() => OnFinishUseCase.Invoke();
    }
}