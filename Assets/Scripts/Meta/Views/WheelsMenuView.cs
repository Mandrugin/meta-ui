using UnityEngine;
using UseCases;
using VContainer;

public class WheelsMenuView : MonoBehaviour
{
    [Inject]
    IWheelsChangingUseCase _wheelsChangingUseCase;
}
