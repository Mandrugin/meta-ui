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
        public static WheelsDataView ToWheelsDataView(this WheelsData wheelsData)
        {
            return new WheelsDataView
            {
                Id = wheelsData.Id,
                Price = wheelsData.Price,
                Status = ""
            };
        }

        public static WheelsData ToWheelsData(this WheelsDataView wheelsDataView)
        {
            return new WheelsData()
            {
                Id = wheelsDataView.Id,
                Price = wheelsDataView.Price,
            };
        }
    }
}