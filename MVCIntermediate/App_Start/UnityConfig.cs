namespace MVCIntermediate
{
    using MVCIntermediate.Components;
    using System.Web.Mvc;
    using Unity;
    using Unity.Mvc5;

    /// <summary>
    ///     UnityConfig Class
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// RegisterComponents method
        /// </summary>
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            container.RegisterType<ILogger, Logger>();
            //container.RegisterType<HomeController>("custsql",
            //                    new InjectionConstructor(
            //                        new ResolvedParameter<ILogger>("sqlserver")));
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}