/* 
 * Name: Jo Lim
 * Date: Apr 1, 2019
 * Last Modified: Apr 2, 2019
 * Description: This class is to be used to seed default data into our DB
 * */

using GOTech.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GOTech.Models
{
    public class GOTechDBInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            RolesConfiguration.ConfigureUserRoles(context);

            // Seed default provinces
            context.Provinces.Add(new Province { ProvinceName = "AB" });
            context.Provinces.Add(new Province { ProvinceName = "BC" });
            context.Provinces.Add(new Province { ProvinceName = "MB" });
            context.Provinces.Add(new Province { ProvinceName = "NB" });
            context.Provinces.Add(new Province { ProvinceName = "NL" });
            context.Provinces.Add(new Province { ProvinceName = "NT" });
            context.Provinces.Add(new Province { ProvinceName = "NS" });
            context.Provinces.Add(new Province { ProvinceName = "NU" });
            context.Provinces.Add(new Province { ProvinceName = "ON" });
            context.Provinces.Add(new Province { ProvinceName = "PE" });
            context.Provinces.Add(new Province { ProvinceName = "QC" });
            context.Provinces.Add(new Province { ProvinceName = "SK" });
            context.Provinces.Add(new Province { ProvinceName = "YT" });

            // Seed default positions
            context.Positions.Add(new Position { Title = "Developer" });
            context.Positions.Add(new Position { Title = "Salesman" });
            context.Positions.Add(new Position { Title = "Designer" });
            context.Positions.Add(new Position { Title = "Manager" });

            base.Seed(context);
        }
    }
}