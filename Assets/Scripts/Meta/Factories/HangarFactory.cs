using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Presenters;
using Meta.UseCases;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Meta.Factories
{
    public class HangarFactory : MonoBehaviour, IHangarFactory
    {
        [SerializeField] private AssetReferenceGameObject hangarViewRef;
        [SerializeField] private Transform canvas;
        
        private AsyncOperationHandle<GameObject> _hangarViewHandle;
        private HangarPresenter _hangarPresenter;
        private HangarView _hangarView;

        public async UniTask<IHangarPresenter> GetHangarPresenter(CancellationToken cancellationToken)
        {
            if (!_hangarView)
            {
                _hangarViewHandle = hangarViewRef.LoadAssetAsync();
                var prefab = await _hangarViewHandle;
                _hangarView = Instantiate(prefab, canvas).GetComponent<HangarView>();
            }
            
            _hangarPresenter ??= new HangarPresenter(_hangarView);
            
            return _hangarPresenter;
        }

        public void DestroyHangarPresenter()
        {
            if(_hangarPresenter != null)
            {
                _hangarPresenter.Dispose();
                _hangarPresenter = null;
            }
            
            if (_hangarView)
            {
                Destroy(_hangarView.gameObject);
                _hangarView = null;
                _hangarViewHandle.Release();
            }
        }

        public void Dispose()
        {
            DestroyHangarPresenter();
        }
    }
}
