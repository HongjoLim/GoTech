using System.Linq;

/* 
 * Name: Jo Lim
 * Date: Apr 8, 2019
 * Last Modified: Apr 8, 2019
 * Description: This interface act as the repository for Review entity
 * */

namespace GOTech.Models.API
{
    public interface IReviewBL
    {
        void AddReview(Review a);
        bool ReviewExists(int id);
        void DeleteReview(Review a);
        void DeleteReviewById(int id);
        void Dispose();
        IQueryable<Review> GetReviews();
        Review GetById(int id);
        void UpdateReview(Review a);

    }
}
