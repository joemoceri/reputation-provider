using System.Collections.Generic;

namespace MefReputationProvider
{
    public class Repository
    {
        public IEnumerable<ReputationModel> GetReputationConfigurations()
        {
            return new List<ReputationModel>
            {
                new ReputationModel
                {
                    Id = "25857783",
                    Name = "YahooLocal"
                },
                new ReputationModel
                {
                    Id = "1001560363294",
                    Name = "YellowPages"
                },
                new ReputationModel
                {
                    Id = "dentistry-for-kids-and-adults-canyon-country",
                    Name = "Yelp"
                },
                new ReputationModel
                {
                    Id = "541706",
                    Name = "CitySearch"
                },
                new ReputationModel
                {
                    Id = "ChIJR9xquheuEmsRVEI04qWoKlc",
                    Name = "GooglePlaces"
                }
            };
        }
    }
}
