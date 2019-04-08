using GOTech.Controllers;
using GOTech.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace GOTech
{
    public static class UnityConfig
    {
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

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}