using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;

namespace MefReputationProvider.GooglePlaces
{
    [Export(nameof(GooglePlaces), typeof(IReputationSite))]
    public class GooglePlaces : IReputationSite
    {
        public ReputationDataModel Get(string id)
        {
            string apiKey = ""; // Your api key
            string url = string.Format(@"https://maps.googleapis.com/maps/api/place/details/xml?placeid={0}&key={1}", id, apiKey);
            var doc = XDocument.Load(url);
            var businessResults = doc.Root.Element("result");

            var addressComponents = businessResults.Elements("address_component").Where(e => e.Element("type") != null);
            var street = businessResults.Element("vicinity").Value;
            var cityTypes = new[] { "locality", "sublocality", "sublocality_level_1", "sublocality_level_2", "sublocality_level_3", "sublocality_level_4", "sublocality_level_5" };
            var stateTypes = new[] { "administrative_area_level_1" };
            var zipTypes = new[] { "postal_code" };
            var reviews = businessResults.Elements("review").Select(r => new ReputationReview
            {
                Date = r.Element("time").Value.TryParse<long>().FromUnixTime(),
                Text = r.Element("text").Value,
                Author = r.Element("author_name").Value,
                Rating = r.Element("rating").Value.TryParse<decimal>(),
            });

            return new ReputationDataModel
            {
                Id = id, 
                ReputationSite = nameof(GooglePlaces),
                Name = businessResults.Element("name").Value,
                Url = businessResults.Element("url").Value,
                Phone = businessResults.Element("formatted_phone_number").Value,
                Address = street.Remove(street.LastIndexOf(",")),
                City = addressComponents.FirstOrDefault(e => cityTypes.Any(s => s == e.Element("type").Value)).Element("long_name").Value,
                State = addressComponents.FirstOrDefault(e => stateTypes.Any(s => s == e.Element("type").Value)).Element("long_name").Value,
                Zip = addressComponents.FirstOrDefault(e => zipTypes.Any(s => s == e.Element("type").Value)).Element("long_name").Value,
                TotalRatings = reviews.Count(),
                AverageRating = businessResults.Element("rating").Value.TryParse<decimal>(),
                Reviews = reviews,
            };
        }
    }
}
