using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudCode.Shared;
using Unity.Services.Economy.Model;

namespace UGSCloud;

public class PlayerEconomyService
{
    public const string HardCurrencyKey = "HARD";
    public const string WheelsKey = "WHEELS";
    public const string StartingWheels = "Buggy-1";
    
    private readonly ILogger<PlayerDataService> _logger;

    public PlayerEconomyService(ILogger<PlayerDataService> logger)
    {
        _logger = logger;
    }

    [CloudCodeFunction("GetPlayerHard")]
    public async Task<long> GetPlayerHard(IExecutionContext context, IGameApiClient gameApiClient)
    {
        return await GetCurrencyAmount(context, gameApiClient, HardCurrencyKey);
    }
    
    private async Task<long> GetCurrencyAmount(IExecutionContext context, IGameApiClient gameApiClient, string key)
    {
        try
        {
            var playerCurrenciesData = await gameApiClient.EconomyCurrencies.GetPlayerCurrenciesAsync(
                context,
                context.AccessToken,
                context.ProjectId,
                context.PlayerId!
            );

            CurrencyBalanceResponse? targetCurrency =
                playerCurrenciesData.Data.Results.FirstOrDefault(currency => currency.CurrencyId == key);

            if (targetCurrency != null)
            {
                return (long)targetCurrency.Balance;
            }
            else
            {
                throw new Exception($"Could not find player currency with key {key}");
            }
        }
        catch (ApiException e)
        {
            throw new Exception($"Failed to get currency {key} for player {context.PlayerId}. Error: {e.Message}");
        }
    }
}
