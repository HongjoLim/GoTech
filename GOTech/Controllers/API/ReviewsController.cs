using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using GOTech.Models;
using GOTech.Models.API;

/* 
 * Name: Jo Lim
 * Date: Apr 8, 2019
 * Last Modified: Apr 8, 2019
 * Description: This api controller provides CRUD function of "Review" Entity
 * */

namespace GOTech.Controllers.API
{
    public class ReviewsController : ApiController
    {
        private IReviewBL _service;

        public ReviewsController(){ }

        public ReviewsController(IReviewBL service)
        {
            this._service = service;
        }

        // GET: api/Reviews
        public IQueryable<Review> GetReviews()
        {
            return _service.GetReviews();
        }

        // GET: api/Reviews/5
        [ResponseType(typeof(Review))]
        public IHttpActionResult GetReview(int id)
        {
            Review review = _service.GetById(id);
            if (review == null)
            {
                return NotFound();
            }

            return Ok(review);
        }

        // PUT: api/Reviews/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReview(int id, Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != review.ReviewId)
            {
                return BadRequest();
            }
            
            try
            {
                _service.UpdateReview(review);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Reviews
        [ResponseType(typeof(Review))]
        public IHttpActionResult PostReview(Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _service.AddReview(review);

            return CreatedAtRoute("DefaultApi", new { id = review.ReviewId }, review);
        }

        // DELETE: api/Reviews/5
        [ResponseType(typeof(Review))]
        public IHttpActionResult DeleteReview(int id)
        {
            Review review = _service.GetById(id);
            if (review == null)
            {
                return NotFound();
            }

            _service.DeleteReview(review);

            return Ok(review);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _service.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReviewExists(int id)
        {
            return _service.GetById(id) != null;
        }
    }
}