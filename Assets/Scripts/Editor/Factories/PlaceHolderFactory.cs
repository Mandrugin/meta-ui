using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Presenters;
using Meta.UseCases;
using Meta.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Meta.Factories
{
    public class PlaceHolderFactory: MonoBehaviour, IPlaceHolderFactory
    {
        [SerializeField] private AssetReferenceGameObject placeHolderAssetRef;
        [SerializeField] private Transform parent;
        
        private AsyncOperationHandle<GameObject> _placeHolderViewHandle;

        public async UniTask<IPlaceHolderPresenter> GetPlaceHolderPresenter(CancellationToken cancellationToken)
        {
            if (!_placeHolderViewHandle.IsDone)
                _placeHolderViewHandle = placeHolderAssetRef.LoadAssetAsync();
            var prefab = await _placeHolderViewHandle;
            var placeHolderView = Instantiate(prefab, parent).GetComponent<PlaceHolderView>();
            
            return new PlaceHolderPresenter(placeHolderView);
        }

        public void DestroyPlaceHolderPresenter(IPlaceHolderPresenter placeHolderPresenter)
        {
            placeHolderPresenter.Dispose();
        }

        public void Dispose()
        {
            _placeHolderViewHandle.Release();
        }
    }
}
