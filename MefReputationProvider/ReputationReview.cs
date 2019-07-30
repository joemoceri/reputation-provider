using System;

namespace MefReputationProvider
{
    public class ReputationReview
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public decimal Rating { get; set; }
        public DateTime Date { get; set; }
    }
}
