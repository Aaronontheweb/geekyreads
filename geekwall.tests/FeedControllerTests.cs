using System.Threading;
using System.Web.Script.Serialization;
using geekwall.Controllers;
using geekwall.Helpers;
using geekwall.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Linq;
using System.Linq.Expressions;
using QDFeedParser;
using System.Web.Mvc;

namespace geekwall.tests
{
    /// <summary>
    ///This is a test class for the FeedController ASP.NET MVC asynchronous controller and is intended
    ///to contain all FeedControllerTests Unit Tests
    ///</summary>
    [TestClass()]
    public class FeedControllerTests
    {
        const string Feeduri = "http://www.aaronstannard.com/syndication.axd";
        const int ItemCount = 3;

        private static IFeedFactory _factory;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _factory = new HttpFeedFactory();
        }

        /// <summary>
        ///This test verifies that the first part of the async method on the event controller makes all of the proper calls
        /// to Quick and Dirty Feed Parser
        ///</summary>
        [TestMethod()]
        public void FeedAsyncMethodShouldPopulateAsyncParameters()
        {
            var controller = new FeedController(_factory);
            var waitHandle = new AutoResetEvent(false);
            // Create and attach event handler for the "Finished" event 
            EventHandler eventHandler = (sender, e) => waitHandle.Set();
            controller.AsyncManager.Finished += eventHandler;

            
            if(_factory.PingFeed(new Uri(Feeduri)))
            {
                controller.FeedAsync(Feeduri, ItemCount);

                const int msTimeout = 5000;
                if (!waitHandle.WaitOne(msTimeout, false))
                {
                    Assert.Fail("Test timed out.");
                }

                var response = controller.AsyncManager.Parameters["feed"] as IFeed;
                Assert.IsNotNull(response);
                Assert.AreEqual(Feeduri, response.FeedUri.AbsoluteUri);
                Assert.IsTrue(response.Items.Count() > 0);
            }
            else
            {
                Assert.Inconclusive(string.Format("Unable to ping feed at uri {0}", Feeduri));
            }
            
            
        }

        /// <summary>
        ///A test to see if the FeedCompleted operation returns a properly serialized JSON object
        ///</summary>
        [TestMethod()]
        public void FeedCompletedShouldReturnValidJsonObject()
        {
            var controller = new FeedController(_factory);
            IFeed feed = null;
            var serializer = new JavaScriptSerializer();
            JsonResult expected = null;
            JsonResult actual;

            if (_factory.PingFeed(new Uri(Feeduri)))
            {
                feed = _factory.CreateFeed(new Uri(Feeduri));
                var jsonExpected = serializer.Serialize(FeedSummarizer.SummarizeFeed(feed, ItemCount));

                Assert.IsNotNull(feed);
                Assert.AreEqual(Feeduri, feed.FeedUri.AbsoluteUri);

                actual = controller.FeedCompleted(feed, ItemCount);
                var jsonActual = serializer.Serialize(actual.Data);

                //The Guid ids are going to be different, but the lengths of the response should be the same
                Assert.AreEqual(jsonExpected.Length, jsonActual.Length);
                Assert.IsTrue(jsonActual.Length > 0); //Double check to see that we didn't manage to serialize anything
            }
            else
            {
                Assert.Inconclusive(string.Format("Unable to ping feed at uri {0}", Feeduri));
            }

        }

        [TestMethod()]
        public void CanQueryFeedsInBulk()
        {
            var controller = new FeedController(_factory);
            var waitHandle = new AutoResetEvent(false);
            IFeedLocationService locationService = new FakeFeedLocationService();

            // Create and attach event handler for the "Finished" event 
            EventHandler eventHandler = (sender, e) => waitHandle.Set();
            controller.AsyncManager.Finished += eventHandler;

            foreach(var item in locationService.GetFeeds())
            {
                if (_factory.PingFeed(item))
                {
                    controller.FeedAsync(item.AbsoluteUri, ItemCount);

                    const int msTimeout = 5000;
                    if (!waitHandle.WaitOne(msTimeout, false))
                    {
                        Assert.Fail("Test timed out.");
                    }

                    var response = controller.AsyncManager.Parameters["feed"] as IFeed;
                    Assert.IsNotNull(response);
                    Assert.AreEqual(item.AbsoluteUri, response.FeedUri.AbsoluteUri);
                    var summary = FeedSummarizer.SummarizeFeed(response, ItemCount);
                    Assert.AreEqual(summary.Items.Count, ItemCount);
                }
                else
                {
                    Assert.Inconclusive(string.Format("Unable to ping feed at uri {0}", item));
                }
            }
            
        }
    }
}
