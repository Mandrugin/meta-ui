using Meta.UseCases;

namespace Meta.Presenters
{
    public struct WheelsDataView
    {
        public string Id;
        public long Price;
        public string Status;
    }

    public static class WheelsDataViewExtensions
    {
        public static WheelsDataView ToWheelsDataView(this WheelsData wheelsDataView)
        {
            return new WheelsDataView
            {
                Id = wheelsDataView.Id,
                Price = wheelsDataView.Price,
                Status = ""
            };
        }
    }
}