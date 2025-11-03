using Cysharp.Threading.Tasks;

namespace Meta.Services
{
    public class TestAnalytics : UseCases.IAnalyticsService
    {
        public async UniTask InitializeAsync()
        {
            await UniTask.WaitForSeconds(.1f);
        }
    }
}