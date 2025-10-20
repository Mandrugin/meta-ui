using System;

namespace Meta.UseCases
{
    public interface IFactory: IDisposable
    {
        void Create();
        void Destroy();
    }
    
    public interface IHangarFactory: IFactory {}
    public interface IVehicleFactory: IFactory {}
    public interface IWheelsChangingFactory: IFactory {}
}
