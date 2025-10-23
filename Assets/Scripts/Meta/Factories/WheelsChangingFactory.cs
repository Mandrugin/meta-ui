using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Presenters;
using Meta.UseCases;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Meta.Factories
{
    public class WheelsChangingFactory : MonoBehaviour, IWheelsChangingFactory
    {
        [SerializeField] private AssetReferenceGameObject wheelsChangingViewRef;
        [SerializeField] private Transform canvas;
        
        private AsyncOperationHandle<GameObject> _wheelsChangingViewHandle;
        private WheelsChangingPresenter _wheelsChangingPresenter;
        private WheelsChangingView _wheelsChangingView;
        
        public async UniTask<IWheelsChangingPresenter> GetWheelsChangingPresenter(CancellationToken cancellationToken)
        {
            if (!_wheelsChangingView)
            {
                _wheelsChangingViewHandle = wheelsChangingViewRef.LoadAssetAsync();
                var prefab = await _wheelsChangingViewHandle;
                _wheelsChangingView = Instantiate(prefab, canvas).GetComponent<WheelsChangingView>();
            }
            
            _wheelsChangingPresenter ??= new WheelsChangingPresenter(_wheelsChangingView);
            
            return _wheelsChangingPresenter;
        }

        public void DestroyWheelsChangingPresenter()
        {
            if(_wheelsChangingPresenter != null)
            {
                _wheelsChangingPresenter.Dispose();
                _wheelsChangingPresenter = null;
            }
            
            if (_wheelsChangingView)
            {
                Destroy(_wheelsChangingView.gameObject);
                _wheelsChangingView = null;
                _wheelsChangingViewHandle.Release();
            }
        }

        public void Dispose()
        {
            DestroyWheelsChangingPresenter();
        }
    }
}
