namespace Meta.Entities
{
    public class Wheels: IEntity
    {
        public string Id;
        public long Price;

        #region Visitor Acceptors
        public T Accept<T>(IGetVisitor<T> getVisitor) => getVisitor.Visit(this);
        public void Accept(ISetVisitor setVisitor) => setVisitor.Visit(this);
        #endregion
    }
}
