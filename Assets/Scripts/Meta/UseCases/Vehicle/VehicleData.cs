using Meta.Entities;

namespace Meta.UseCases
{
    public struct VehicleData
    {
        public string Id;
    }

    public static class VehicleDataExtensions
    {
        public static VehicleData ToVehicleData(this Vehicle vehicle)
        {
            return new VehicleData
            {
                Id = vehicle.Id,
            };
        }
    }
}