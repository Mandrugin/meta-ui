using Meta.UseCases;

namespace Meta.Presenters
{
    public class SpecialOfferDataView
    {
        public string Id;
        public long TimeStamp;
    }

    public static class SpecialOfferDataViewExtensions
    {
        public static SpecialOfferDataView ToDataView(this SpecialOfferData specialOfferData)
        {
            return new SpecialOfferDataView
            {
                Id = specialOfferData.Id,
                TimeStamp = specialOfferData.TimeStamp
            };
        }
    }
}