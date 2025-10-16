using System;

namespace Meta.UseCases
{
    public struct WheelsData : IEquatable<WheelsData>
    {
        public string Id;
        public long Price;

        public bool Equals(WheelsData other) => Id == other.Id;

        public override bool Equals(object obj)
        {
            return obj is WheelsData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Price);
        }
    }
}
