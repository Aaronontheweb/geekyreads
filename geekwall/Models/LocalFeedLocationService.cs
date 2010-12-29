using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace geekwall.Models
{
    public class LocalFeedLocationService : IFeedLocationService
    {
        private readonly string _xmlPath;

        public LocalFeedLocationService(string xmlpath)
        {
            this._xmlPath = xmlpath;
        }

        public IEnumerable<Uri> GetFeeds()
        {
            var doc = XDocument.Load(this._xmlPath);
            var feeditems = from feedrow in doc.Root.Elements("feed")
                            select new Uri(feedrow.Value);
            return feeditems;
        }
    }
}