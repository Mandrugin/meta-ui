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
    public class SpecialOffersFactory: MonoBehaviour, ISpecialOffersFactory
    {
        [SerializeField] private AssetReferenceGameObject specialOffersAssetRef;
        [Inject] SceneContext sceneContext;
        
        private AsyncOperationHandle<GameObject> _specialOffersViewHandle;

        public async UniTask<ISpecialOffersPresenter> GetSpecialOffersPresenter(CancellationToken cancellationToken)
        {
            if (!_specialOffersViewHandle.IsDone)
                _specialOffersViewHandle = specialOffersAssetRef.LoadAssetAsync();
            var prefab = await _specialOffersViewHandle;
            var specialOffersView = Instantiate(prefab, sceneContext.middleLayer).GetComponent<SpecialOffersView>();
            
            return new SpecialOffersPresenter(specialOffersView);
        }

        public void DestroySpecialOffersPresenter(ISpecialOffersPresenter specialOffersPresenter)
        {
            specialOffersPresenter.Dispose();
        }

        public void Dispose()
        {
            _specialOffersViewHandle.Release();
        }
    }
}
