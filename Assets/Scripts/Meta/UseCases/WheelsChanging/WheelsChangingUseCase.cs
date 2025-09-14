using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Meta.Entities;

namespace Meta.UseCases
{
    public class WheelsChangingUseCase : IWheelsChangingUseCase
    {
        private List<Wheels> _wheels;
        private Vehicle _currentVehicle;

        private readonly IHangarBackend _hangarBackend;

        public WheelsChangingUseCase(IHangarBackend hangarBackend)
        {
            _hangarBackend = hangarBackend;
            Update().Forget();
        }
        
        private async UniTaskVoid Update()
        {
            _currentVehicle = await _hangarBackend.GetCurrentVehicle();
            _wheels = await _hangarBackend.GetAllWheels(_currentVehicle);
        }

        public UniTask<bool> BuyWheels(int wheelsIndex)
        {
            throw new NotImplementedException();
        }

        public UniTask<bool> TryWheelsOut(int wheelsIndex)
        {
            throw new NotImplementedException();
        }

        public UniTask<bool> SetWheels(int wheelsIndex)
        {
            throw new NotImplementedException();
        }

        public UniTask<List<WheelsData>> GetAllWheels()
        {
            throw new NotImplementedException();
        }

        public UniTask<List<WheelsData>> GetBoughtWheels()
        {
            throw new NotImplementedException();
        }

        public UniTask<WheelsData> GetCurrentWheels()
        {
            throw new NotImplementedException();
        }
    }
}
