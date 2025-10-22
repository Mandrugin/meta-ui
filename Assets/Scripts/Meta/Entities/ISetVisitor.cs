namespace Meta.Entities
{
    public interface ISetVisitor
    {
        void Visit(Storage storage);
        void Visit(Wallet wallet);
        void Visit(Wheels wheels);
        void Visit(Vehicle vehicle);
    }
}