using System;
using Meta.UseCases;

namespace Meta.Presenters
{
    public class HangarPresenter : IDisposable
    {
        private readonly IHangarUseCase _hangarUseCase;

        public HangarPresenter(IHangarUseCase hangarUseCase)
        {
            _hangarUseCase = hangarUseCase;
        }

        public void StartWheelsChanging() => _hangarUseCase.StartWheelsChanging();

        public void FinishWheelsChanging() => _hangarUseCase.FinishWheelsChanging();

        public void Dispose()
        {
            // ...
        }
    }
}
