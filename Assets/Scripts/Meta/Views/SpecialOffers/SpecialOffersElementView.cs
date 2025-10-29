using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Meta.Views
{
    public class SpecialOffersElementView : MonoBehaviour
    {
        public Transform parent;
        public event Action<string> onClick = delegate { };
        
        [SerializeField] private Button button;
        [SerializeField] private GameObject loadingIndicator;

        public string Id { get; private set; }

        public async UniTask Init(string id, AssetReferenceGameObject assetReferenceGameObject)
        {
            Id = id;
            if (assetReferenceGameObject.IsValid() && assetReferenceGameObject.IsDone)
            {
                Instantiate(assetReferenceGameObject.Asset, parent);
                return;
            }

            var go = await assetReferenceGameObject.LoadAssetAsync();
            Instantiate(go, parent);
        }

        private void Awake()
        {
            button.onClick.AddListener(() => onClick.Invoke(Id));
        }
    }
}