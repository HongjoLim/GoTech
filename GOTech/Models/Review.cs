/* 
 * Name: Jo Lim
 * Date: Mar 25, 2019
 * Last Modified: Mar 25, 2019
 * */

namespace GOTech.Models
{
    public class Review
    {
        public virtual int ReviewId { get; set; }

        // Foreign key
        public virtual int ProjectId { get; set; }
    }
}