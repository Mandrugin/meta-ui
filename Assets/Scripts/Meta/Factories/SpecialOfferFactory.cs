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
        [SerializeField] private AssetReferenceGameObject SpecialOfferAssetRef;
        [Inject] SceneContext sceneContext;
        
        private AsyncOperationHandle<GameObject> _SpecialOfferViewHandle;

        public async UniTask<ISpecialOfferPresenter> GetSpecialOfferPresenter(CancellationToken cancellationToken)
        {
            if (!_SpecialOfferViewHandle.IsDone)
                _SpecialOfferViewHandle = SpecialOfferAssetRef.LoadAssetAsync();
            var prefab = await _SpecialOfferViewHandle;
            var SpecialOfferView = Instantiate(prefab, sceneContext.middleLayer).GetComponent<SpecialOfferView>();
            
            return new SpecialOfferPresenter(SpecialOfferView);
        }

        public void DestroySpecialOfferPresenter(ISpecialOfferPresenter SpecialOfferPresenter)
        {
            SpecialOfferPresenter.Dispose();
        }

        public void Dispose()
        {
            _SpecialOfferViewHandle.Release();
        }
    }
}
