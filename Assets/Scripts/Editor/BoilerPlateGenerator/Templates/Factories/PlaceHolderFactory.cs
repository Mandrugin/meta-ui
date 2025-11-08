using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Presenters;
using Meta.UseCases;
using Meta.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;

namespace Meta.Factories
{
    public class PlaceHolderFactory: MonoBehaviour, IPlaceHolderFactory
    {
        [SerializeField] private AssetReferenceGameObject placeHolderAssetRef;
        [Inject] private SceneContext sceneContext;
        
        private AsyncOperationHandle<GameObject> _placeHolderViewHandle;

        public async UniTask<IPlaceHolderPresenter> GetPlaceHolderPresenter(CancellationToken cancellationToken)
        {
            if (!_placeHolderViewHandle.IsValid() || !_placeHolderViewHandle.IsDone)
                _placeHolderViewHandle = placeHolderAssetRef.LoadAssetAsync();
            var prefab = await _placeHolderViewHandle;
            prefab.gameObject.SetActive(false);                 //--------------------------------//
            var placeHolderView = Instantiate(prefab, sceneContext.middleLayer).GetComponent<PlaceHolderView>();
            await UniTask.WaitForEndOfFrame(cancellationToken); //-to avoid the object flickering-//
            placeHolderView.gameObject.SetActive(true);         //-before it starts animate-------//

            return new PlaceHolderPresenter(placeHolderView);
        }

        public void DestroyPlaceHolderPresenter(IPlaceHolderPresenter placeHolderPresenter)
        {
            placeHolderPresenter.Dispose();
        }

        public void Dispose()
        {
            _placeHolderViewHandle.IsValid();
                _placeHolderViewHandle.Release();
        }
    }
}
