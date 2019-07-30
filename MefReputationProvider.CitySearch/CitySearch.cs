using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;

namespace MefReputationProvider.CitySearch
{
    [Export(nameof(CitySearch), typeof(IReputationSite))]
    public class CitySearch : IReputationSite
    {
        public ReputationDataModel Get(string id)
        {
            string publisherKey = ""; // your publisher key
            string url = string.Format(@"https://api.citygridmedia.com/content/places/v2/detail?id={0}&id_type=cs&client_ip=127.0.0.1&publisher={1}", id, publisherKey);
            var doc = XDocument.Load(url);
            var locationResults = doc.Root.Element("location");
            var address = locationResults.Element("address");
            var contactInfo = locationResults.Element("contact_info");
            var reviewInfo = locationResults.Element("review_info");
            var categories = locationResults.Elements("categories");
            var reviewResults = locationResults.Element("review_info");
            var reviews = reviewResults.Element("reviews").Elements().Select(e => new ReputationReview
            {
                
                Id = e.Element("review_id").Value,
                Author = e.Element("review_author").Value,
                Text = e.Element("review_text").Value,
                Date = e.Element("review_date").Value.TryParse<DateTime>(),
                Rating = e.Element("review_rating").Value.TryParse<decimal>(),
            });

            return new ReputationDataModel
            {
                Id = id,
                ReputationSite = nameof(CitySearch),
                Name = locationResults.Element("name").Value,
                Url = contactInfo.Element("display_url").Value,
                Phone = contactInfo.Element("display_phone").Value,
                Address = address.Element("street").Value,
                City = address.Element("city").Value,
                State = address.Element("state").Value,
                Zip = address.Element("postal_code").Value,
                TotalRatings = reviewResults.Element("total_user_reviews").Value.TryParse<int>(),
                AverageRating = reviewResults.Element("overall_review_rating").Value.TryParse<decimal>(),
                Reviews = reviews,
            };
        }
    }
}
