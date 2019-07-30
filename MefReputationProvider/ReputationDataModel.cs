using System.Collections.Generic;

namespace MefReputationProvider
{
    public class ReputationDataModel
    {
        public string Id { get; set; }
        public string ReputationSite { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public int TotalRatings { get; set; }
        public decimal AverageRating { get; set;}
        public IEnumerable<ReputationReview> Reviews { get; set; }
    }
}
