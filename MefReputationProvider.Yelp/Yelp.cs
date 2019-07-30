using Newtonsoft.Json.Linq;
using SimpleOAuth;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MefReputationProvider.Yelp
{
    [Export(nameof(Yelp), typeof(IReputationSite))]
    public class YellowPages : IReputationSite
    {
        public ReputationDataModel Get(string id)
        {
            string url = string.Format("http://api.yelp.com/v2/business/{0}", id);
            var uriBuilder = new UriBuilder(url);
            const string consumerKey = "";
            const string consumerSecret = "";
            const string token = "";
            const string tokenSecret = "";

            var request = WebRequest.Create(uriBuilder.ToString());
            request.Method = "GET";
            request.SignRequest(
                new Tokens
                {
                    ConsumerKey = consumerKey,
                    ConsumerSecret = consumerSecret,
                    AccessToken = token,
                    AccessTokenSecret = tokenSecret
                }).WithEncryption(EncryptionMethod.HMACSHA1).InHeader();

            JObject result;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    result = JObject.Parse(stream.ReadToEnd());
                }
            }

            var location = result.Value<JObject>("location");
            var address = location.Value<JArray>("address").Children().Select(t => t.Value<string>());

            return new ReputationDataModel
            {
                Id = id,
                ReputationSite = nameof(Yelp),
                Name = result.Value<string>("name"),
                Url = result.Value<string>("url"),
                Phone = result.Value<string>("phone"),
                Address = string.Join(" ", address.ToArray()),
                City = location.Value<string>("city"),
                State = location.Value<string>("state_code"),
                Zip = location.Value<string>("postal_code"),
                TotalRatings = result.Value<int>("review_count"),
                AverageRating = result.Value<decimal>("rating"),
                Reviews = Enumerable.Empty<ReputationReview>(),
            };
        }
    }
}
