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
    public class SpecialOffersCongratsFactory: MonoBehaviour, ISpecialOffersCongratsFactory
    {
        [SerializeField] private SpecialOffersPopUpsConfig specialOffersPopUpsConfig;
        [Inject] SceneContext sceneContext;
        
        public async UniTask<ISpecialOfferCongratsPresenter> GetInfoPopupPresenter(
            string specialOfferId,
            CancellationToken cancellationToken)
        {
            var assetRef = specialOffersPopUpsConfig.specialOfferConfigs
                .First(x => x.specialOfferId == specialOfferId).congratulationAssetReference;
            var prefab = await assetRef.LoadAssetAsync();
            assetRef.ReleaseAsset();
            var infoPopupView = Instantiate(prefab, sceneContext.frontLayer).GetComponent<SpecialOffersCongratsView>();
            
            return new SpecialOffersCongratsPresenter(infoPopupView);
        }

        public void DestroyInfoPopupPresenter(ISpecialOfferCongratsPresenter specialOfferCongratsPresenter)
        {
            specialOfferCongratsPresenter.Dispose();
        }

        public void Dispose()
        {
            // ...
        }
    }
}
