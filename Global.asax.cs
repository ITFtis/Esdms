using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Esdms
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //log4net
            log4net.Config.XmlConfigurator.Configure(); // must have this line
            var Logger = log4net.LogManager.GetLogger(typeof(MvcApplication));

            try
            {
                if (AppConfig.IsBkTask)
                {
                    logger.Info("BkTask±Ò°Ê(Application_Start):" + DateFormat.ToDate6(DateTime.Now));

                    var task = new BkTask();
                    task.Run();
                }
            }
            catch (Exception ex)
            {
                logger.Error("BkTask¿ù»~:" + ex.Message);
            }
        }
    }
}
