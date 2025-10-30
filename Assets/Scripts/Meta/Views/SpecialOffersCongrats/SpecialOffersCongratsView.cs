using Cysharp.Threading.Tasks;
using Meta.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace Meta.Views
{
    public class SpecialOffersCongratsView: MonoBehaviour, ISpecialOffersCongratsView
    {
        [SerializeField] private Button okButton;
        
        private UniTaskCompletionSource _taskCompletionSource = new();

        private void Awake()
        {
            okButton.onClick.AddListener(OnOk);
        }
        
        private void OnOk()
        {
            _taskCompletionSource.TrySetResult();
            _taskCompletionSource = new UniTaskCompletionSource(); // in case we want to reuse the popup
        }

        public UniTask GetClick() => _taskCompletionSource.Task;

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}
