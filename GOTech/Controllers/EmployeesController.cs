using System;
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
 * Date: Apr 2, 2019
 * Last Modified: Apr 5, 2019
 * Description: This controller provides CRUD function of Employees (ApplicationUser)
 * */

namespace GOTech.Controllers
{
    [Authorize(Roles ="Admin")]
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        // This no-parameter controller must exist
        public EmployeesController() { }

        public EmployeesController(ApplicationUserManager userManager, ApplicationDbContext db)
        {
            UserManager = userManager;
            this.db = db;
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
            var employees = db.Users.Where(x => x.PositionId != null);

            ViewBag.Positions = new SelectList(db.Positions, "PositionId", "Title");
            
            return View(employees.Include(x=>x.Position));
        }

        /** This action method is called by Ajax.
         *  When the admin selects a position from the Dropdown list,
         *  the Ajax re-populates employees table to show only those who are in the selected position
         * */
         [HttpPost]
        public ActionResult GetEmployeesByPositionId(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                var parsedId = Convert.ToInt32(id);
                var employees = db.Users.Where(x => x.PositionId == parsedId).ToList();
                return PartialView("_EmployeesTable", employees);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        // GET
        public ActionResult Details(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProvinceName = db.Provinces.FirstOrDefault(x => x.ProvinceId == user.ProvinceId).ProvinceName;
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
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", user.PositionId);
            ViewBag.ProvinceId = new SelectList(db.Provinces, "ProvinceId", "ProvinceName", user.ProvinceId);
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
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index"); 
            }
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", user.PositionId);
            ViewBag.ProvinceId = new SelectList(db.Provinces, "ProvinceId", "ProvinceName", user.ProvinceId);
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
