using System;
using Meta.UseCases;

namespace Meta.Presenters
{
    public class VehiclePresenter: IDisposable
    {
        public event Action<WheelsDataView> OnTriedOutWheels = delegate { };
        
        private readonly IHangarUseCase _hangarUseCase;
        private readonly IWheelsChangingUseCase _wheelsChangingUseCase;

        public VehiclePresenter(IHangarUseCase hangarUseCase, IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _hangarUseCase = hangarUseCase;
            _wheelsChangingUseCase = wheelsChangingUseCase;
            _wheelsChangingUseCase.OnWheelsTriedOut += OnOnTriedOutWheels;
        }

        public void Dispose()
        {
            _wheelsChangingUseCase.OnWheelsTriedOut -= OnOnTriedOutWheels;
        }

        protected virtual void OnOnTriedOutWheels(WheelsData wheelsData)
        {
            OnTriedOutWheels(new WheelsDataView(){Id = wheelsData.Id});
        }
    }
}