using System;

namespace Meta.UseCases
{
    public interface IUseCase
    {
        event Action OnStartUseCase;
        event Action OnFinishUseCase;
        void StartUseCase();
        void FinishUseCase();
    }
}