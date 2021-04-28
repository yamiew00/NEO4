using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using WebApi.FeatureStructure;
using WebApi.Setting;

namespace WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            OrderingRule.LoadOrdering();
            FeatureReactor.LoadIFeature();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
