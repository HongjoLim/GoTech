namespace GOTech.Migrations
{
    using GOTech.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            context.Positions.AddOrUpdate(
                new Position{ Title = "Developer"},
                new Position { Title = "Salesman" },
                new Position { Title = "Designer" },
                new Position { Title = "Manager" }
                );
        }
    }
}
