using System;
using Meta.UseCases;
using UnityEngine.Scripting;

namespace Meta.Presenters
{
    [Preserve]
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
