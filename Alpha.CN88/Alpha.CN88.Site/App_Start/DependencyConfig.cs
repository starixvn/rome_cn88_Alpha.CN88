namespace Alpha.CN88.Site.App_Start
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web.Mvc;
    using CN88.Models;
    using Controllers;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Helpers;
    using MediatR;
    using Models;
    using Services;
    using Services.CustomerList;
    using Services.Impl;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using Web.Helpers;
    using Web.Helpers.Impl;

    public static class DependencyConfig
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed")]
        public static void Init()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            RegisterMediator(container);

            RegisterDataAccesses(container);

            RegisterServices(container);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // This is an extension method from the integration package as well.
            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void RegisterMediator(Container container)
        {
            var assemblies = GetAssemblies();
            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.Register(typeof(IAsyncRequestHandler<,>), assemblies);

            container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);

            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(SiteBaseController).Assembly;
        }

        private static void RegisterDataAccesses(Container container)
        {
            container.Register<IDynamicRepository, DynamicRepository>(Lifestyle.Singleton);

            container.RegisterPerWebRequest<CustomerListDataAccess, CustomerListDataAccessImpl>();
        }

        private static void RegisterServices(Container container)
        {
            container.RegisterSingleton<IUserSession, UserSessionExt>();
            container.RegisterSingleton<IUser, User>();
            container.RegisterSingleton<BrowserHelper, BrowserHelperImpl>();
            container.RegisterSingleton<CachingManager, CachingManagerImpl>();

            container.RegisterPerWebRequest<CustomerListService, CustomerListServiceImpl>();
            container.RegisterPerWebRequest<DefaultService, DefaultServiceImpl>();
            container.RegisterPerWebRequest<SiteMenuService, SiteMenuServiceImpl>();
            container.RegisterPerWebRequest<SiteMapReader, SiteMapReaderImpl>();
            container.RegisterPerWebRequest<MenuItemBuilder, MenuItemBuilderImpl>();
            container.RegisterPerWebRequest<SessionManager, SessionManagerImpl>();
        }
    }
}