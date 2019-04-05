using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GOTech.Models;
using Microsoft.AspNet.Identity.Owin;

/* 
 * Name: Jo Lim
 * Date: Apr 2, 2019
 * Last Modified: Apr 5, 2019
 * Description: This controller provides CRUD function of Employees (ApplicationUser)
 * */

namespace GOTech.Controllers
{
    [Authorize(Roles ="Admin")]
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db;
        private ApplicationUserManager _userManager;

        // This no-parameter controller must exist
        public EmployeesController() { }

        public EmployeesController(ApplicationUserManager userManager, ApplicationDbContext db)
        {
            UserManager = userManager;
            Db = db;
        }

        public ApplicationDbContext Db
        {
            get
            {
                return db ?? new ApplicationDbContext();
            }
            set
            {
                db = value;
            }
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

        // GET
        public ActionResult Index()
        {
            // Find employees among the application users. Employees DO have positionId
            var employees = Db.Users.Where(x => x.PositionId != null);
            
            return View(employees.Include(x=>x.Position));
        }

        // GET
        public ActionResult Details(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = Db.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProvinceName = Db.Provinces.FirstOrDefault(x => x.ProvinceId == user.ProvinceId).ProvinceName;
            return View(user);
        }

        // GET
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
            ViewBag.PositionId = new SelectList(Db.Positions, "PositionId", "Title", user.PositionId);
            ViewBag.ProvinceId = new SelectList(Db.Provinces, "ProvinceId", "ProvinceName", user.ProvinceId);
            return View(user);
        }

        // POST
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,LastName,PositionId,HiringDate,Address,City,ProvinceId,PostalCode,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(user).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index"); 
            }
            ViewBag.PositionId = new SelectList(Db.Positions, "PositionId", "Title", user.PositionId);
            ViewBag.ProvinceId = new SelectList(Db.Provinces, "ProvinceId", "ProvinceName", user.ProvinceId);
            return View(user);
        }

        // GET
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

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed([Bind(Include = "Email")] ApplicationUser user)
        {
            var employee = await UserManager.FindByEmailAsync(user.Email);
            if (employee != null)
            {
                await UserManager.RemoveFromRoleAsync(employee.Id, "Employee");
                await UserManager.DeleteAsync(employee);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
