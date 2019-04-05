using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GOTech.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<EmployeeProject> EmployeeProject { get; set; }

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