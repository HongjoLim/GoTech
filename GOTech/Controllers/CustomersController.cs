using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GOTech.Models;
using Microsoft.AspNet.Identity.Owin;

/* 
 * Name: Jo Lim
 * Date: Apr 3, 2019
 * Last Modified: Apr 3, 2019
 * Description: This controller provides RUD function of Customers (ApplicationUser)
 * */

namespace GOTech.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public CustomersController() { }

        public CustomersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Customers
        public ActionResult Index()
        {
            var users = db.Users.AsQueryable();

            // Only select customers from application user table. Customers have NULL value for position id
            var customers = from customer in users
                            where (customer.PositionId == null &&
                            // Do not include admin user
                            customer.UserName != "admin@gotech.com")
                            select customer;
            return View(customers.Include(x => x.Province));
        }

        // GET: Customers/Details/5
        public async Task<ActionResult> Details(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByNameAsync(email);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProvinceName = db.Provinces.FirstOrDefault(x => x.ProvinceId == user.ProvinceId).ProvinceName;
            return View(user);
        }

        // GET: Customers/Edit/5
        public async Task<ActionResult> Edit(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByNameAsync(email);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProvinceId = new SelectList(db.Provinces, "ProvinceId", "ProvinceName", user.ProvinceId);
            return View(user);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,LastName,PositionId,HiringDate,Address,City,ProvinceId,PostalCode,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProvinceId = new SelectList(db.Provinces, "ProvinceId", "ProvinceName", user.ProvinceId);
            return View(user);
        }

        // GET: Customers/Delete/5
        public async Task<ActionResult> Delete(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed([Bind(Include = "Email")] ApplicationUser user)
        {
            var customer = await UserManager.FindByEmailAsync(user.Email);
            if (customer != null)
            {
                // Delete the user from the user-roles matching table
                await UserManager.RemoveFromRoleAsync(customer.Id, "Customer");
                
                // Delete the user
                await UserManager.DeleteAsync(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
