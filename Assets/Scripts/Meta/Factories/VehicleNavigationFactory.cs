using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Presenters;
using Meta.UseCases;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;

namespace Meta.Factories
{
    public class VehicleNavigationFactory: MonoBehaviour, IVehicleNavigationFactory
    {
        [SerializeField] private AssetReferenceGameObject vehicleViewNavigationRef;
        [Inject] private SceneContext sceneContext;
        
        private AsyncOperationHandle<GameObject>  vehicleNavigationHandle;

        private VehicleNavigationPresenter _vehicleNavigationPresenter;
        private VehicleNavigationView vehicleNavigationView;
        
        public async UniTask<IVehicleNavigationPresenter> GetVehicleNavigationPresenter(CancellationToken cancellationToken)
        {
            if (!vehicleNavigationView)
            {
                vehicleNavigationHandle = vehicleViewNavigationRef.LoadAssetAsync();
                var prefab = await vehicleNavigationHandle;
                vehicleNavigationView = Instantiate(prefab, sceneContext.frontLayer).GetComponent<VehicleNavigationView>();
            }
            
            _vehicleNavigationPresenter ??= new VehicleNavigationPresenter(vehicleNavigationView);
            
            return _vehicleNavigationPresenter;
        }
        
        public void DestroyVehicleNavigationPresenter()
        {
            if(_vehicleNavigationPresenter != null)
            {
                _vehicleNavigationPresenter.Dispose();
                _vehicleNavigationPresenter = null;
            }
            
            if (vehicleNavigationView)
            {
                Destroy(vehicleNavigationView.gameObject);
                vehicleNavigationView = null;
                vehicleNavigationHandle.Release();
            }            
        }

        public void Dispose()
        {
            DestroyVehicleNavigationPresenter();
        }
    }
}