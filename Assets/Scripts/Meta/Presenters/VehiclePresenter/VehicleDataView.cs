using Meta.UseCases;

namespace Meta.Presenters
{
    public struct VehicleDataView
    {
        public string Id;
    }

    public static class VehicleDataViewExtensions
    {
        public static VehicleDataView ToVehicleDataView(this VehicleData vehicleData)
        {
            return new VehicleDataView
            {
                Id = vehicleData.Id,
            };
        }
    }
}