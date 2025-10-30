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
    public class SpecialOfferFactory: MonoBehaviour, ISpecialOfferFactory
    {
        [SerializeField] private AssetReferenceGameObject specialOfferAssetRef;
        [Inject] SceneContext sceneContext;
        
        private AsyncOperationHandle<GameObject> _specialOfferViewHandle;

        public async UniTask<ISpecialOfferPresenter> GetSpecialOfferPresenter(CancellationToken cancellationToken)
        {
            if (!_specialOfferViewHandle.IsDone)
                _specialOfferViewHandle = specialOfferAssetRef.LoadAssetAsync();
            var prefab = await _specialOfferViewHandle;
            var specialOfferView = Instantiate(prefab, sceneContext.middleLayer).GetComponent<SpecialOfferView>();
            
            return new SpecialOfferPresenter(specialOfferView);
        }

        public void DestroySpecialOfferPresenter(ISpecialOfferPresenter specialOfferPresenter)
        {
            specialOfferPresenter.Dispose();
        }

        public void Dispose()
        {
            _specialOfferViewHandle.Release();
        }
    }
}
