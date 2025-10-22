namespace Meta.Entities
{
    public interface IGetVisitor<out T>
    {
        T Visit(Storage storage);
        T Visit(Wallet wallet);
        T Visit(Vehicle vehicle);
        T Visit(Wheels wheels);
    }
}
