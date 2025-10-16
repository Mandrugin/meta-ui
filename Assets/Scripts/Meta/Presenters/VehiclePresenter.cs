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
            _wheelsChangingUseCase.OnTriedOutWheels += OnOnTriedOutWheels;
        }

        public void Dispose()
        {
            _wheelsChangingUseCase.OnTriedOutWheels -= OnOnTriedOutWheels;
        }

        protected virtual void OnOnTriedOutWheels(WheelsData wheelsData)
        {
            OnTriedOutWheels(new WheelsDataView
            {
                Id = wheelsData.Id,
                Price = wheelsData.Price
            });
        }
    }
}