using System;

namespace Meta.Presenters
{
    public interface ISpecialOfferView: IDisposable
    {
        event Action<string> OnGetSpecialOffer;
        event Action DismissSpecialOffer;
        public void Init(string specialOfferId);
    }
}
