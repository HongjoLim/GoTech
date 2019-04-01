using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GOTech.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<EmployeeProject> EmployeeProject { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new GOTechDBInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}