using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;
using Meta.UseCases;
using Unity.Services.CloudCode;
using Unity.Services.CloudCode.GeneratedBindings;
using UnityEngine;

namespace Meta.Gateways
{
    public class UgsHangarGateway : IHangarGateway
    {
        public event Action<long> OnSoftChanged;
        public event Action<long> OnHardChanged;
        
        private readonly IAuthenticatorService _authenticatorService;
        
        private readonly PlayerDataServiceBindings _playerDataServiceBindings;

        public async UniTask PlayWithUgs()
        {
            var playerName = "Etishka";
            Debug.Log($"send \"{playerName}\" player name");
            var response = await _playerDataServiceBindings.SayHello(playerName);
            Debug.Log($"response is: \"{response}\"");
            await SaveNewPlayerName(playerName);
        }

        private async UniTask SaveNewPlayerName(string newPlayerName)
        {
            try
            {
                var playerName = await _playerDataServiceBindings.HandleNewPlayerNameEntry(newPlayerName);
                Debug.Log($"Saved new  player name in the could: {playerName}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public UgsHangarGateway(IAuthenticatorService authenticatorService)
        {
            _authenticatorService = authenticatorService;
            
            if(!_authenticatorService.IsAuthenticated)
            {
                Debug.LogError("player is not authenticated");
                return;
            }

            _playerDataServiceBindings = new PlayerDataServiceBindings(CloudCodeService.Instance);
            
            PlayWithUgs().Forget();
        }

        public UniTask<long> GetHardBalance(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask<long> GetSoftBalance(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask<List<Vehicle>> GetAllVehicles(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask<List<Vehicle>> GetBoughtVehicles()
        {
            throw new NotImplementedException();
        }

        public UniTask<Vehicle> GetSetVehicle(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask<bool> SetSetVehicle(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public UniTask<bool> BuyVehicle(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public UniTask<List<Wheels>> GetAllWheels(Vehicle vehicle, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask<List<Wheels>> GetBoughtWheels(Vehicle vehicle, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask<Wheels> GetSetWheels(Vehicle vehicle, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask<bool> SetWheels(Vehicle vehicle, Wheels wheels, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UniTask<bool> BuyWheels(Vehicle vehicle, Wheels wheel, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}