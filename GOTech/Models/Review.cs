using System;
using System.ComponentModel.DataAnnotations;

/* 
 * Name: Jo Lim
 * Date: Mar 25, 2019
 * Last Modified: Apr 8, 2019
 * */

namespace GOTech.Models
{
    public class Review
    {
        [Key]
        public virtual int ReviewId { get; set; }

        // Foreign key
        public virtual int ProjectId { get; set; }

        // Foreign key
        public virtual string UserId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0} must be between 2 to 100 characters")]
        public virtual string ReviewBody { get; set; }

        public virtual DateTime PostDate { get; set; }
    }
}