using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GOTech.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        #region Customized fields
        // Add extra fields to be used throuout our application
        public string FirstName { get; set; }
        public string LastName { get; set; }

        /* Foreign key, this field will tell whether the user is a Manager Developer, Designer, or Salesman
        This filed has to allow null value because external customers will not have any position ids.
             */
        public int? PositionId { get; set; }
        public Position Position { get; set; }

        public DateTime HiringDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public int? ProvinceId { get; set; }
        public Province Province { get; set; }

        public string PostalCode { get; set; }
        #endregion
    }
}