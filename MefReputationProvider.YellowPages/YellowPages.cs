using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace MefReputationProvider.YellowPages
{
    [Export(nameof(YellowPages), typeof(IReputationSite))]
    public class YellowPages : IReputationSite
    {
        public ReputationDataModel Get(string id)
        {
            const string apiKey = ""; // your api key
            var listingUrl = string.Format(@"http://pubapi.yp.com/search-api/search/devapi/details?listingid={0}&key={1}", id, apiKey);

            XDocument listingDoc;
            using (var client = new WebClient())
            {
                client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.124 Safari/537.36";

                listingDoc = XDocument.Load(client.OpenRead(listingUrl));
            }

            var listingDetails = listingDoc.Root.Element("listingsDetails").Element("listingDetail");
            var additionalTexts = listingDetails.Element("additionalTexts").Elements();
            var brands = listingDetails.Element("brands").Elements();
            var categories = listingDetails.Element("categories").Elements();
            var socialNetworks = listingDetails.Element("features").Elements("socialNetworks").Elements();

            return new ReputationDataModel
            {
                Id = id,
                ReputationSite = nameof(YellowPages),
                Name = listingDetails.Element("businessName").Value,
                Url = null,
                Phone = listingDetails.Element("phone").Value,
                Address = listingDetails.Element("street").Value,
                City = listingDetails.Element("city").Value,
                State = listingDetails.Element("state").Value,
                Zip = listingDetails.Element("zip").Value,
                TotalRatings = listingDetails.Element("ratingCount").Value.TryParse<int>(),
                AverageRating = listingDetails.Element("averageRating").Value.TryParse<decimal>(),
                Reviews = Enumerable.Empty<ReputationReview>(),
            };
        }
    }
}
