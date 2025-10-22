using System.Collections.Generic;

namespace Meta.Entities
{
    public class Vehicle: IEntity
    {
        public string Id;
        public Wheels StockWheels;
        public Wheels CurrentWheels;
        public long BuyPrice;
        public long SellPrice;
        public List<Wheels> BoughtWheels;
        public List<Wheels> AllWheels;

        #region Visitor Acceptors
        public T Accept<T>(IGetVisitor<T> getVisitor) => getVisitor.Visit(this);
        public void Accept(ISetVisitor setVisitor) => setVisitor.Visit(this);
        #endregion
    }
}
