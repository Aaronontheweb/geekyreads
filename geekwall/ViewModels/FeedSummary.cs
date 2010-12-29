using System;
using System.Collections.Generic;

namespace geekwall.ViewModels
{
    public class FeedItemSummary
    {
        public Uri ItemUri { get; set; }
        public string ItemTitle { get; set; }
    }

    public class FeedSummary
    {
        public Guid FeedGuid { get; set;}
        public Uri FeedUri { get; set; }
        public Uri FeedLink { get; set;}
        public string FeedTitle { get; set; }
        public IList<FeedItemSummary> Items { get; set; }

        public FeedSummary()
        {
            Items = new List<FeedItemSummary>();
        }
    }
}