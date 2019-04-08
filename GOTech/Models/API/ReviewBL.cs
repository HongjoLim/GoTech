using System;
using System.Data.Entity;
using System.Linq;

/* 
 * Name: Jo Lim
 * Date: Apr 8, 2019
 * Last Modified: Apr 8, 2019
 * Description: This calss interacts with DB and provides CRUD function for Review Entity
 * */

namespace GOTech.Models.API
{
    public class ReviewBL : IReviewBL
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void AddReview(Review review)
        {
            db.Reviews.Add(review);
            db.SaveChanges();
        }

        public void DeleteReview(Review review)
        {
            db.Reviews.Remove(review);
            db.SaveChanges();
        }

        public void DeleteReviewById(int id)
        {
            DeleteReview(GetById(id));
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public Review GetById(int id)
        {
            Review review = db.Reviews.Find(id);
            return review;
        }

        public IQueryable<Review> GetReviews()
        {
            return db.Reviews;
        }

        public bool ReviewExists(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateReview(Review review)
        {
            db.Entry(review).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}