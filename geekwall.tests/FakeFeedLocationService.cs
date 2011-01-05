using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using geekwall.Models;

namespace geekwall.tests
{
    public class FakeFeedLocationService : IFeedLocationService
    {
        #region Implementation of IFeedLocationService

        public IEnumerable<Uri> GetFeeds()
        {
            return new Uri[]
                       {
                           new Uri("http://www.aaronstannard.com/syndication.axd"),
                           new Uri("http://teddziuba.com/atom.xml"),
                           new Uri("http://weblogs.asp.net/scottgu/atom.aspx"),
                           new Uri("http://sheddingbikes.com/feed.xml"),
                           new Uri("http://feedproxy.google.com/TechCrunch"), 
                           new Uri("http://syndication.thedailywtf.com/TheDailyWtf"), 
                           new Uri("http://feeds.feedburner.com/rtur")
                       };
        }

        #endregion
    }
}
