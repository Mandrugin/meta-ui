using System.Collections.Generic;

namespace Meta.Entities
{
    public class Storage: IEntity
    {
        public Wallet Wallet;
        public Vehicle CurrentVehicle;
        public List<Vehicle> BoughtVehicles;
        public List<Vehicle> AllVehicles;

        #region Visitor Acceptors
        public T Accept<T>(IGetVisitor<T> getVisitor) => getVisitor.Visit(this);
        public void Accept(ISetVisitor setVisitor) => setVisitor.Visit(this);
        #endregion
    }
}
