using System;
using System.Collections.Generic;

namespace geekwall.Models
{
    public interface IFeedLocationService
    {
        IEnumerable<Uri> GetFeeds();
    }
}