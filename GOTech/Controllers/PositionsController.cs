using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GOTech.Models;

/* 
 * Name: Jo Lim
 * Date: Mar 26, 2019
 * Last Modified: Apr 2, 2019
 * Description: This controller provides CRUD function to Position entity
 *                   This controller will ONLY be accessible to Administrator
 * 
 * TO DO: "PositionService" has to be created becuase some of the action methods in this contorller
 *              has too many logic
 * */

namespace GOTech.Controllers
{
    // Control access
    [Authorize(Roles ="Admin")]
    public class PositionsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public PositionsController(){ }

        public PositionsController(ApplicationDbContext db)
        {
            this._db = db;
        }

        // GET: Positions
        public ActionResult Index()
        {
            // DO NOT show UnAssigned position becuase it cannot be deleted
            var positions = _db.Positions.Where(x => x.Title != "UnAssigned");
            return View(positions);
        }

        // GET: Positions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = _db.Positions.FirstOrDefault(x=>x.PositionId == id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // GET: Positions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PositionId,Title")] Position position)
        {
            if (ModelState.IsValid)
            {
                _db.Positions.Add(position);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(position);
        }

        // GET: Positions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = _db.Positions.FirstOrDefault(x=>x.PositionId == id );
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PositionId,Title")] Position position)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(position).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(position);
        }

        // GET: Positions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Position position = _db.Positions.FirstOrDefault(x=>x.PositionId == id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Position position = _db.Positions.FirstOrDefault(x=>x.PositionId==id);
            _db.Positions.Remove(position);

            // TO DO: This logic has to be moved to "Project Service class"
            // Assign employees temporary id
            var users = _db.Users.Where(x => x.PositionId == id);
            var unAssignedPositionId = _db.Positions.FirstOrDefault(x => x.Title == "UnAssigned").PositionId;
            foreach (ApplicationUser user in users)
            {
                user.PositionId = unAssignedPositionId;
            }

            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
