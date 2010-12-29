using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using geekwall.Models;

namespace geekwall.Controllers
{
    public class HomeController : Controller
    {
        protected readonly IFeedLocationService _feedlocator;

        public HomeController(IFeedLocationService feedlocator)
        {
            _feedlocator = feedlocator;
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(_feedlocator.GetFeeds());
        }

    }
}
