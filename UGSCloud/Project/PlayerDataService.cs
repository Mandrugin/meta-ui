using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;
using Unity.Services.CloudCode.Shared;
using Unity.Services.CloudSave.Model;

namespace UGSCloud;

public class PlayerDataService
{
    private const string PlayerNameKey = "PLAYER_NAME";
    
    private readonly ILogger<PlayerDataService> _logger;

    public PlayerDataService(ILogger<PlayerDataService> logger)
    {
        _logger = logger;
    }

    private async Task SavePlayerData(IExecutionContext context, IGameApiClient gameApiClient, string key, string value)
    {
        try
        {
            await gameApiClient.CloudSaveData.SetItemAsync(
                context, 
                context.AccessToken!, 
                context.ProjectId!,
                context.PlayerId!, 
                new SetItemBody(key, value));
                
            _logger.LogInformation("Successfully saved data for key: {Key}", key);
        }
        catch (ApiException ex)
        {
            _logger.LogError("Failed to save data for key {Key}. Error: {Error}", key, ex.Message);
            throw new Exception($"Unable to save player data: {ex.Message}");
        }
    }

    private async Task<string> GetPlayerData(IExecutionContext context, IGameApiClient gameApiClient, string key)
    {
        try
        {
            var result = await gameApiClient.CloudSaveData.GetItemsAsync(
                context, 
                context.AccessToken!,
                context.ProjectId!, 
                context.PlayerId!, 
                new List<string> { key });
                
            var data = result.Data.Results.FirstOrDefault()?.Value?.ToString() ?? string.Empty;
            _logger.LogInformation("Successfully retrieved data for key: {Key}", key);
            return data;
        }
        catch (ApiException ex)
        {
            _logger.LogError("Failed to retrieve data for key {Key}. Error: {Error}", key, ex.Message);
            throw new Exception($"Unable to retrieve player data: {ex.Message}");
        }
    }
    
    [CloudCodeFunction("SayHello")]
    public string Hello(string name)
    {
        return $"Hello, {name}!";
    }

    [CloudCodeFunction("HandleNewPlayerNameEntry")]
    public async  Task<string> HandleNewPlayerNameEntry(IExecutionContext context, IGameApiClient gameApiClient, string newPlayerName)
    {
        if (IsPlayerNameValid(newPlayerName))
        {
            await SavePlayerData(context, gameApiClient, PlayerNameKey, newPlayerName);
            return newPlayerName;
        }
        
        throw new ArgumentException("Invalid player name");
    }

    private bool IsPlayerNameValid(string newPlayerName)
        => newPlayerName.Length is >= 4 and <= 16 && newPlayerName.All(char.IsLetterOrDigit);
}