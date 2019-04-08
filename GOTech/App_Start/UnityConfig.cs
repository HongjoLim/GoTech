using GOTech.Controllers;
using GOTech.Models;
using GOTech.Models.API;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using Unity;
using Unity.Exceptions;
using Unity.Injection;
using Unity.Mvc5;

namespace GOTech
{
    public static class UnityConfig
    {
        public static void RegisterTypes(UnityContainer container)
        {
            container.RegisterType<IReviewBL, ReviewBL>();
        }

        public static void RegisterWebApiComponents(HttpConfiguration config)
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            config.DependencyResolver = new UnityResolver(container);
        }


        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // To unit test, I had to register all the controllers (Dependency Injection issue)
            container.RegisterType<IdentityDbContext<ApplicationUser>, ApplicationDbContext>();
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());
            container.RegisterType<EmployeesController>(new InjectionConstructor());
            container.RegisterType<CustomersController>(new InjectionConstructor());
            container.RegisterType<PositionsController>(new InjectionConstructor());
            container.RegisterType<ProjectsController>(new InjectionConstructor());

            RegisterTypes(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }

    public class UnityResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IUnityContainer _container;

        public UnityResolver(IUnityContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            var child = _container.CreateChildContainer();
            return new UnityResolver(child);
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }

}