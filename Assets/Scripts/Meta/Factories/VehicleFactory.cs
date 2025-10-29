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
    public class VehicleFactory : MonoBehaviour, IVehicleFactory
    {
        [SerializeField] private AssetReferenceGameObject vehicleViewRef;
        [Inject] private SceneContext sceneContext;

        private AsyncOperationHandle<GameObject> vehicleViewHandle;

        private VehiclePresenter _vehiclePresenter;
        private VehicleView _vehicleView;

        public async UniTask<IVehiclePresenter> GetVehiclePresenter(CancellationToken cancellationToken)
        {
            if (!_vehicleView)
            {
                vehicleViewHandle = vehicleViewRef.LoadAssetAsync();
                var prefab = await vehicleViewHandle;
                _vehicleView = Instantiate(prefab).GetComponent<VehicleView>();
                sceneContext.cinemachineCamera.Target.TrackingTarget = _vehicleView.cameraTarget;
            }
            
            _vehiclePresenter ??= new VehiclePresenter(_vehicleView);
            
            return _vehiclePresenter;
        }

        public void DestroyVehiclePresenter()
        {
            if(_vehiclePresenter != null)
            {
                _vehiclePresenter.Dispose();
                _vehiclePresenter = null;
            }
            
            if (_vehicleView)
            {
                Destroy(_vehicleView.gameObject);
                _vehicleView = null;
                vehicleViewHandle.Release();
            }
        }

        public void Dispose()
        {
            DestroyVehiclePresenter();
        }
    }
}
