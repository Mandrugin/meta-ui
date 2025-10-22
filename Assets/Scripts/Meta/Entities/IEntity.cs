namespace Meta.Entities
{
    public interface IEntity
    {
        public T Accept<T>(IGetVisitor<T> getVisitor);
        void Accept(ISetVisitor setVisitor);
    }
}