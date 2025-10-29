using System;
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
    public class OverlayLoadingFactory : MonoBehaviour, IOverlayLoadingFactory
    {
        [SerializeField] private AssetReferenceGameObject overlayLoadingViewRef;
        [Inject] SceneContext sceneContext;

        private AsyncOperationHandle<GameObject> overlayLoadingViewHandle;

        private OverlayLoadingPresenter _overlayLoadingPresenter;
        private OverlayLoadingView _overlayLoadingView;

        
        public async UniTask<IOverlayLoadingPresenter> GetOverlayLoadingPresenter(CancellationToken cancellationToken)
        {
            if (!_overlayLoadingView)
            {
                overlayLoadingViewHandle = overlayLoadingViewRef.LoadAssetAsync();
                var prefab = await overlayLoadingViewHandle;
                _overlayLoadingView = Instantiate(prefab, sceneContext.overlayLayer).GetComponent<OverlayLoadingView>();
            }
            
            _overlayLoadingPresenter ??= new OverlayLoadingPresenter(_overlayLoadingView);
            
            return _overlayLoadingPresenter;
        }

        public void DestroyOverlayLoadingPresenter()
        {
            if(_overlayLoadingPresenter != null)
            {
                _overlayLoadingPresenter.Dispose();
                _overlayLoadingPresenter = null;
            }
            
            if (_overlayLoadingView)
            {
                Destroy(_overlayLoadingView.gameObject);
                _overlayLoadingView = null;
                overlayLoadingViewHandle.Release();
            }
        }

        public void Dispose()
        {
            DestroyOverlayLoadingPresenter();
        }
    }
}
