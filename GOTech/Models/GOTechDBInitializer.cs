using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GOTech.Models
{
    public class GOTechDBInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // Seed default positions
            context.Positions.Add(new Position { Title = "Developer" });
            context.Positions.Add(new Position { Title = "Salesman" });
            context.Positions.Add(new Position { Title = "Designer" });
            context.Positions.Add(new Position { Title = "Manager" });

            base.Seed(context);
        }
    }
}