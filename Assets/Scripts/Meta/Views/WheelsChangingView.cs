using UnityEngine;

namespace Meta.Views
{
    public class WheelsChangingView : MonoBehaviour
    {
        WheelsChangingPresenter _wheelsChangingPresenter;

        private void Construct(WheelsChangingPresenter wheelsChangingPresenter)
        {
            _wheelsChangingPresenter = wheelsChangingPresenter;
        }
    }
}
