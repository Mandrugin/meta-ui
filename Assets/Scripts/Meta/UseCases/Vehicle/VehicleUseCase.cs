using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Scripting;

namespace Meta.UseCases
{
    [Preserve]
    public class VehicleUseCase : IVehicleUseCase, IDisposable
    {
        private readonly IHangarGateway _hangarGateway;
        private readonly UseCaseMediator _useCaseMediator;

        public event Action OnShowPresenter = delegate { };

        public VehicleUseCase(IHangarGateway hangarGateway, UseCaseMediator useCaseMediator)
        {
            _hangarGateway = hangarGateway;
            _useCaseMediator = useCaseMediator;
        }

        public void Dispose()
        {
            
        }

        public event Action OnHidePresenter = delegate { };

        public void ShowPresenter()
        {
            OnShowPresenter.Invoke();
        }

        public void HidePresenter()
        {
            OnHidePresenter.Invoke();
        }

        public async UniTask<VehicleData> GetCurrentVehicle(CancellationToken cancellationToken)
        {
            return await UniTask.FromResult(new VehicleData());
        }

        public async UniTask UpdateVehicleData(CancellationToken token)
        {
            var currentVehicle = await _hangarGateway.GetCurrentVehicle(token);
            if (currentVehicle == null)
                throw new Exception("Cannot update find current vehicle");

            _useCaseMediator.ChangeCurrentVehicle(currentVehicle.ToVehicleData());
        }
    }
}