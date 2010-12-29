using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using geekwall.Helpers;
using QDFeedParser;

namespace geekwall.Controllers
{
    public class FeedController : AsyncController
    {
        protected IFeedFactory _feedfactory;

        public FeedController(IFeedFactory factory)
        {
            _feedfactory = factory;
        }

        //
        // GET: /Feed/

        public void FeedAsync(string feeduri, int itemCount)
        {
            AsyncManager.OutstandingOperations.Increment();
            _feedfactory.BeginCreateFeed(new Uri(feeduri),
                async => 
                    AsyncManager.Sync(() =>
                                            {
                                                AsyncManager.Parameters[
                                                    "feed"] =
                                                    _feedfactory.
                                                        EndCreateFeed(async);
                                                AsyncManager.Parameters["itemCount"] = itemCount;
                                                AsyncManager.
                                                    OutstandingOperations.
                                                    Decrement();
                                            }));
        }

        public JsonResult FeedCompleted(IFeed feed, int itemCount)
        {
            return Json(FeedSummarizer.SummarizeFeed(feed, itemCount), JsonRequestBehavior.AllowGet);
        }

    }
}
