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
    public class AuthenticatorFactory: MonoBehaviour, IAuthenticatorFactory
    {
        [SerializeField] private AssetReferenceGameObject authenticatorAssetRef;
        [Inject] private SceneContext sceneContext;
        
        private AsyncOperationHandle<GameObject> _authenticatorViewHandle;

        public async UniTask<IAuthenticatorPresenter> GetAuthenticatorPresenter(CancellationToken cancellationToken)
        {
            _authenticatorViewHandle = authenticatorAssetRef.LoadAssetAsync();
            var prefab = await _authenticatorViewHandle;
            var authenticatorView = Instantiate(prefab, sceneContext.middleLayer).GetComponent<AuthenticatorView>();
            
            return new AuthenticatorPresenter(authenticatorView);
        }

        public void DestroyAuthenticatorPresenter(IAuthenticatorPresenter authenticatorPresenter)
        {
            authenticatorPresenter.Dispose();
        }

        public void Dispose()
        {
            _authenticatorViewHandle.Release();
        }
    }
}
