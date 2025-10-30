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

        private OverlayLoadingPresenter _overlayLoadingPresenter;
        
        public async UniTask<IOverlayLoadingPresenter> GetOverlayLoadingPresenter(CancellationToken cancellationToken)
        {
            if (_overlayLoadingPresenter != null)
            {
                _overlayLoadingPresenter.ShowOverlayLoading();
                return _overlayLoadingPresenter;
            }
            
            var prefab = await overlayLoadingViewRef.LoadAssetAsync();
            overlayLoadingViewRef.ReleaseAsset();
            var overlayLoadingView = Instantiate(prefab, sceneContext.overlayLayer).GetComponent<OverlayLoadingView>();
            
            _overlayLoadingPresenter ??= new OverlayLoadingPresenter(overlayLoadingView);
            
            return _overlayLoadingPresenter;
        }

        public void DestroyOverlayLoadingPresenter(IOverlayLoadingPresenter overlayLoadingPresenter)
        {
            _overlayLoadingPresenter.HideOverlayLoading();
        }

        public void Dispose()
        {
            _overlayLoadingPresenter.Dispose();
        }
    }
}
