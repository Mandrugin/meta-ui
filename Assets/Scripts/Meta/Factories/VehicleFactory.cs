using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Presenters;
using Meta.UseCases;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Meta.Factories
{
    public class VehicleFactory : MonoBehaviour, IVehicleFactory
    {
        [SerializeField] private AssetReferenceGameObject vehicleViewRef;
        [SerializeField] private AssetReferenceGameObject vehicleViewNavigationRef;
        [SerializeField] private Transform canvas;

        private AsyncOperationHandle<GameObject> vehicleViewHandle;
        private AsyncOperationHandle<GameObject>  vehicleNavigationHandle;

        private VehiclePresenter _vehiclePresenter;
        private VehicleNavigationPresenter _vehicleNavigationPresenter;
        private VehicleView _vehicleView;
        private VehicleNavigationView vehicleNavigationView;

        public void Dispose()
        {
            DestroyVehiclePresenter();
            DestroyVehicleNavigationPresenter();
        }

        public async UniTask<IVehiclePresenter> GetVehiclePresenter(CancellationToken cancellationToken)
        {
            if (!_vehicleView)
            {
                vehicleViewHandle = vehicleViewRef.LoadAssetAsync();
                var prefab = await vehicleViewHandle;
                _vehicleView = Instantiate(prefab).GetComponent<VehicleView>();
            }
            
            _vehiclePresenter ??= new VehiclePresenter(_vehicleView);
            
            return _vehiclePresenter;
        }

        public async UniTask<IVehicleNavigationPresenter> GetVehicleNavigationPresenter(CancellationToken cancellationToken)
        {
            if (!vehicleNavigationView)
            {
                vehicleNavigationHandle = vehicleViewNavigationRef.LoadAssetAsync();
                var prefab = await vehicleNavigationHandle;
                vehicleNavigationView = Instantiate(prefab, canvas).GetComponent<VehicleNavigationView>();
            }
            
            _vehicleNavigationPresenter ??= new VehicleNavigationPresenter(vehicleNavigationView);
            
            return _vehicleNavigationPresenter;
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
    }
}
