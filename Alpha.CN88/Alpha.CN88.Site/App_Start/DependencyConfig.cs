namespace Alpha.CN88.Site.App_Start
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web.Mvc;
    using CN88.Models;
    using Controllers;
    using Fanex.Data.Repository;
    using global::CN88.Auth.DataAccesses;
    using global::CN88.Auth.Services;
    using Helpers;
    using MediatR;
    using Models;
    using SecurityCode.Services;
    using Services;
    using Services.Impl;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
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
            container.RegisterSingleton<IDynamicRepository, DynamicRepository>();
            container.RegisterSingleton<AuthenticationDataAccess, AuthenticationDataAccessImpl>();
        }

        private static void RegisterServices(Container container)
        {
            container.RegisterSingleton<IUserSession, UserSessionExt>();
            container.RegisterSingleton<IUser, User>();
            container.RegisterSingleton<BrowserHelper, BrowserHelperImpl>();
            container.RegisterSingleton<CachingManager, CachingManagerImpl>();
            container.RegisterSingleton<Encryptor, EncryptorImpl>();

            container.RegisterPerWebRequest<DefaultService, DefaultServiceImpl>();
            container.RegisterPerWebRequest<SiteMenuService, SiteMenuServiceImpl>();
            container.RegisterPerWebRequest<SiteMapReader, SiteMapReaderImpl>();
            container.RegisterPerWebRequest<MenuItemBuilder, MenuItemBuilderImpl>();
            container.RegisterPerWebRequest<SessionManager, SessionManagerImpl>();

            container.RegisterPerWebRequest<LoginParamParser, LoginParamParserImpl>();
            container.RegisterPerWebRequest<AuthenticationService, AuthenticationServiceImpl>();
            container.RegisterPerWebRequest<SessionUpdater, SessionUpdaterImpl>();

            container.RegisterPerWebRequest<HomeService, HomeServiceImpl>();
            container.RegisterPerWebRequest<SecurityCodeService, SecurityCodeServiceImpl>();

            container.RegisterPerWebRequest<PasswordValidator, PasswordValidatorImpl>();
            container.RegisterPerWebRequest<PasswordService, PasswordServiceImpl>();

            container.RegisterPerWebRequest<NickNameService, NickNameServiceImpl>();

            container.RegisterPerWebRequest<SuggestionService, SuggestionServiceImpl>();
        }
    }
}