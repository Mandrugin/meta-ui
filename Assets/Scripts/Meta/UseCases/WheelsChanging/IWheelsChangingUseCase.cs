using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Entities;

namespace UseCases
{
    public interface IWheelsChangingUseCase
    {
        UniTask<bool> BuyWheels(int wheelsIndex);
        UniTask<bool> TryWheelsOut(int wheelsIndex);
        UniTask<bool> SetWheels(int wheelsIndex);
        UniTask<List<Wheels>> GetAllWheels();
        UniTask<List<Wheels>> GetBoughtWheels();
        UniTask<Wheels> GetCurrentWheels();
    }
}
