using System;
using Meta.UseCases;
using UnityEngine.Scripting;

namespace Meta.Presenters
{
    [Preserve]
    public class VehiclePresenter: IDisposable
    {
        public event Action<WheelsDataView> OnTriedOutWheels = delegate { };
        
        private readonly IWheelsChangingUseCase _wheelsChangingUseCase;

        public VehiclePresenter(IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _wheelsChangingUseCase = wheelsChangingUseCase;
            _wheelsChangingUseCase.OnWheelsTriedOut += OnWheelsOnWheelsTriedOut;
        }

        public void Dispose()
        {
            _wheelsChangingUseCase.OnWheelsTriedOut -= OnWheelsOnWheelsTriedOut;
        }

        protected virtual void OnWheelsOnWheelsTriedOut(WheelsData wheelsData)
        {
            OnTriedOutWheels(new WheelsDataView
            {
                Id = wheelsData.Id,
                Price = wheelsData.Price
            });
        }
    }
}