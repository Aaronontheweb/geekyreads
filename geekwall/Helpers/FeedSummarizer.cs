using System;
using geekwall.ViewModels;
using System.Linq;
using System.Linq.Expressions;
using QDFeedParser;

namespace geekwall.Helpers
{
    public static class FeedSummarizer
    {
        public static FeedSummary SummarizeFeed(IFeed feed, int itemCount)
        {
            return new FeedSummary
                       {
                           FeedGuid = Guid.NewGuid(),
                           FeedTitle = feed.Title,
                           FeedUri = feed.FeedUri,
                           FeedLink = new Uri(feed.Link),
                           Items = feed.Items.Reverse().Skip(feed.Items.Count() - itemCount).Select(SummarizeFeedItem).ToList()
                       };
        }

        public static FeedItemSummary SummarizeFeedItem(IFeedItem feedItem)
        {
            return new FeedItemSummary
                       {
                           ItemTitle = feedItem.Title,
                           ItemUri = new Uri(feedItem.Link)
                       };
        }
    }
}