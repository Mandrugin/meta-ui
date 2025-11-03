using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IAnalyticsService
    {
        UniTask InitializeAsync();
    }
}
