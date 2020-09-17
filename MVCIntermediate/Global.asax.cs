using MVCIntermediate.Components;
using MVCIntermediate.Controllers;
using System;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVCIntermediate
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundle(BundleTable.Bundles);
            AreaRegistration.RegisterAllAreas();
            BundleTable.EnableOptimizations = true;
            //UnityConfig.RegisterComponents(); /* DI Using unity framework */
            RegisterCustomControllerFactory(); /* DI Using IController Interface */
        }
        private void RegisterCustomControllerFactory()
        {
            /*Inherited DefaultControllerFactory class-- can be customize accordingly overriding GetControllerInstance method*/
            //IControllerFactory factory = new CustomControllerFactory();

            /*Implements IControllerFactory interface*/
            IControllerFactory factory = new CustomControllerFactory("MVCIntermediate.Controllers");
            ControllerBuilder.Current.SetControllerFactory(factory);
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Server.ClearError();

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("exception", exception);
            var statusCode = ((HttpException)exception).GetHttpCode();
            if (exception.GetType() == typeof(HttpException))
            {
                string action;
                switch (statusCode)
                {
                    case 404:
                        // page not found
                        action = "HttpError404";
                        break;
                    case 500:
                        // server error
                        action = "HttpError500";
                        break;
                    default:
                        action = "General";
                        break;
                }
                routeData.Values.Add("statusCode", statusCode);
                routeData.Values.Add("action", action);
                IController controller = new ErrorController();
                controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                Response.End();
            }
        }
    }
}
