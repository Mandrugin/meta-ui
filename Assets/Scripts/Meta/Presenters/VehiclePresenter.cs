using System;
using Meta.UseCases;
using UnityEngine.Scripting;

namespace Meta.Presenters
{
    [Preserve]
    public class VehiclePresenter: IDisposable
    {
        public event Action<WheelsDataView> OnTriedOutWheels = delegate { };
        
        private readonly IHangarUseCase _hangarUseCase;

        public VehiclePresenter(IHangarUseCase hangarUseCase)
        {
            _hangarUseCase = hangarUseCase;
            _hangarUseCase.OnTryWheelsOut += OnOnTriedOutWheels;
        }

        public void Dispose()
        {
            _hangarUseCase.OnTryWheelsOut -= OnOnTriedOutWheels;
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