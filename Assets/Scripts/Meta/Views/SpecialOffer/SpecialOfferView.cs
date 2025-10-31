using Cysharp.Threading.Tasks;
using Meta.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace Meta.Views
{
    public class SpecialOfferView: MonoBehaviour, ISpecialOfferView
    {
        [SerializeField] private Button getSpecialOfferButton;
        [SerializeField] private Button dismissSpecialOfferButton;

        private UniTaskCompletionSource<bool> _completionSource;

        public void Awake()
        {
            getSpecialOfferButton.onClick.AddListener(() => _completionSource.TrySetResult(true));
            dismissSpecialOfferButton.onClick.AddListener(() => _completionSource.TrySetResult(false));
        }

        public void Dispose()
        {
            if(this)
                Destroy(gameObject);
        }

        public UniTask<bool> GetUserChoice()
        {
            _completionSource = new UniTaskCompletionSource<bool>();
            return _completionSource.Task;
        }
    }
}
