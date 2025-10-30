using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Presenters;
using Meta.UseCases;
using Meta.Views;
using UnityEngine;
using VContainer;

namespace Meta.Factories
{
    public class SpecialOfferFactory: MonoBehaviour, ISpecialOfferFactory
    {
        [SerializeField] private SpecialOffersPopUpsConfig specialOffersPopUpsConfig;
        [Inject] SceneContext sceneContext;
        
        public async UniTask<ISpecialOfferPresenter> GetSpecialOfferPresenter(
            string specialOfferId,
            CancellationToken cancellationToken)
        {
            var assetRef = specialOffersPopUpsConfig.specialOfferConfigs
                .First(x => x.specialOfferId == specialOfferId).specialOfferAssetReference;
            var prefab = await assetRef.LoadAssetAsync();
            assetRef.ReleaseAsset();
            var specialOfferView = Instantiate(prefab, sceneContext.frontLayer).GetComponent<SpecialOfferView>();
            var specialOfferPresenter = new SpecialOfferPresenter(specialOfferView);
            return specialOfferPresenter;
        }

        public void DestroySpecialOfferPresenter(ISpecialOfferPresenter specialOfferPresenter)
        {
            specialOfferPresenter.Dispose();
        }

        public void Dispose()
        {
            
        }
    }
}
