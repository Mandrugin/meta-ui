using Cysharp.Threading.Tasks;
using Unity.Services.Analytics;

namespace Meta.Services
{
    public class UgsAnalytics : UseCases.IAnalyticsService
    {
        public async UniTask InitializeAsync()
        {
            AnalyticsService.Instance.StartDataCollection();
        }
    }
}