using GOTech.Models;
using System.Data.Entity.Migrations;

namespace GOTech.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<GOTech.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            context.Positions.AddOrUpdate(
                 new Position { Title = "Developer" },
                 new Position { Title = "Salesman" },
                 new Position { Title = "Designer" },
                 new Position { Title = "Manager" }
                 );
        }
    }
}
