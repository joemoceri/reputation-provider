using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;

namespace MefReputationProvider.YellowBook
{
    [Export(nameof(YahooLocal), typeof(IReputationSite))]
    public class YahooLocal : IReputationSite
    {
        public ReputationDataModel Get(string id)
        {
            string url = string.Format(@"https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20local.search%20where%20listing_id%20%3D%20{0}", id);

            XNamespace ns = "urn:yahoo:lcl";
            var doc = XDocument.Load(url);
            var lol = ns + "Result";
            var lol2 = $"{ns}Title";
            var businessResults = doc.Root.Element("results").Element(ns + "Result");

            var ratingResults = businessResults.Element(ns + "Rating");

            return new ReputationDataModel
            {
                Id = id,
                ReputationSite = nameof(YahooLocal),
                Name = businessResults.Element(ns + "Title").Value,
                Url = businessResults.Element(ns + "Url").Value,
                Phone = businessResults.Element(ns + "Phone").Value,
                Address = businessResults.Element(ns + "Address").Value,
                City = businessResults.Element(ns + "City").Value,
                State = businessResults.Element(ns + "State").Value,
                Zip = null,
                TotalRatings = ratingResults.Element(ns + "TotalRatings").Value.TryParse<int>(),
                AverageRating = ratingResults.Element(ns + "AverageRating").Value.TryParse<decimal>(),
                Reviews = Enumerable.Empty<ReputationReview>()
            };
        }
    }
}
