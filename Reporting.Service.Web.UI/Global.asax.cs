using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Reporting.Service.Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.SetLogWriter(new Microsoft.Practices.EnterpriseLibrary.Logging.LogWriterFactory().Create());
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
