using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Entities;

namespace UseCases
{
    public class WheelsChangingUseCase : IWheelsChangingUseCase
    {
        private List<Wheels> _wheels;
        private Vehicle _currentVehicle;

        private IHangarData _hangarData;

        public WheelsChangingUseCase(IHangarData hangarData)
        {
            _hangarData = hangarData;
            Update().Forget();
        }
        
        private async UniTaskVoid Update()
        {
            _currentVehicle = await _hangarData.GetCurrentVehicle();
            _wheels = await _hangarData.GetAllWheels(_currentVehicle);
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
