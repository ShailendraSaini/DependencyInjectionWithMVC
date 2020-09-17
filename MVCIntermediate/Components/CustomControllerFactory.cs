namespace MVCIntermediate.Components
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.SessionState;

    /// <summary>
    ///     Custom Controller Factory Class
    /// </summary>
    public class CustomControllerFactory : IControllerFactory
    {
        private readonly string _controllerNamespace;

        /// <summary>
        ///     Parameterized constructor of CustomControllerFactory Class
        /// </summary>
        /// <param name="controllerNamespace"></param>
        public CustomControllerFactory(string controllerNamespace)
        {
            _controllerNamespace = controllerNamespace;
        }

        /// <summary>
        ///     Create Controller method
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            ILogger logger = new Logger();
            Type controllerType = Type.GetType(string.Concat(_controllerNamespace, ".", controllerName, "Controller"));
            IController controller = Activator.CreateInstance(controllerType, new[] { logger }) as Controller;
            return controller;
        }

        /// <summary>
        ///     Get Controller Session Behavior
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

        /// <summary>
        ///     Release Controller method
        /// </summary>
        /// <param name="controller"></param>
        public void ReleaseController(IController controller)
        {
            IDisposable disposable = controller as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }
    }

    //public class CustomControllerFactory : DefaultControllerFactory
    //{
    //    protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
    //    {
    //        ILogger logger = new Logger();
    //        IController controller = Activator.CreateInstance(controllerType, new[] { logger }) as Controller;
    //        return controller;
    //    }
    //}
}