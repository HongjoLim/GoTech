using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GOTech.Models
{
    public class Employee
    {
        // Primary key
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between 2 to 50 characters")]
        public virtual string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between 2 to 50 characters")]
        public virtual string LastName { get; set; }

        // Foreign key
        public virtual int PositionId { get; set; }
        public virtual string Email { get; set; }
        public virtual DateTime HiringDate { get; set; }

        // An employee can participate in only 1 "on-going" project
        public virtual int ProjectId { get; set; }
    }
}