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
    public class AuthenticatorFactory: MonoBehaviour, IAuthenticatorFactory
    {
        [SerializeField] private AssetReferenceGameObject authenticatorView;
        [SerializeField] private Transform canvas;
        
        private AsyncOperationHandle<GameObject> _authenticatorViewHandle;
        private AuthenticatorPresenter _authenticatorPresenter;
        private AuthenticatorView _authenticatorView;

        public async UniTask<IAuthenticatorPresenter> GetAuthenticatorPresenter(CancellationToken cancellationToken)
        {
            if (!_authenticatorView)
            {
                _authenticatorViewHandle = authenticatorView.LoadAssetAsync();
                var prefab = await _authenticatorViewHandle;
                _authenticatorView = Instantiate(prefab, canvas).GetComponent<AuthenticatorView>();
            }
            
            _authenticatorPresenter ??= new AuthenticatorPresenter(_authenticatorView);
            
            return _authenticatorPresenter;
        }

        public void DestroyAuthenticatorPresenter()
        {
            if(_authenticatorPresenter != null)
            {
                _authenticatorPresenter.Dispose();
                _authenticatorPresenter = null;
            }
            
            if (_authenticatorView)
            {
                Destroy(_authenticatorView.gameObject);
                _authenticatorView = null;
                _authenticatorViewHandle.Release();
            }
        }

        public void Dispose()
        {
            DestroyAuthenticatorPresenter();
        }
    }
}
