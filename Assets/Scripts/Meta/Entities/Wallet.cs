namespace Meta.Entities
{
    public class Wallet: IEntity
    {
        public long Hard;
		public long Soft;

        #region Visitor Acceptors
        public T Accept<T>(IGetVisitor<T> getVisitor) => getVisitor.Visit(this);
        public void Accept(ISetVisitor setVisitor) => setVisitor.Visit(this);
        #endregion
    }
}