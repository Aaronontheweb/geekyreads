using System.Configuration;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using geekwall.Models;
using Ninject;
using Ninject.Mvc3;
using QDFeedParser;
using QDFeedParser.Xml;

[assembly: WebActivator.PreApplicationStartMethod(typeof(geekwall.AppStart_NinjectMVC3), "Start")]

namespace geekwall {
    public static class AppStart_NinjectMVC3 {
        public static void RegisterServices(IKernel kernel) {
            HttpFeedFactory.SetUseUnsafeHeaderParsing(true);
            kernel.Bind<IFeedLocationService>().ToConstant(new LocalFeedLocationService(Path.Combine(HostingEnvironment.ApplicationPhysicalPath,@"Data\geekfeeds.xml")));
            kernel.Bind<IFeedFactory>().ToConstant(new HttpFeedFactory(new LinqFeedXmlParser()));
        }

        public static void Start() {
            // Create Ninject DI Kernel 
            IKernel kernel = new StandardKernel();

            // Register services with our Ninject DI Container
            RegisterServices(kernel);

            // Tell ASP.NET MVC 3 to use our Ninject DI Container 
            DependencyResolver.SetResolver(new NinjectServiceLocator(kernel));
        }
    }
}
