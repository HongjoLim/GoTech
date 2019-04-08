using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/* 
 * Name: Jo Lim
 * Date: Mar 25, 2019
 * Last Modified: Apr 6, 2019
 * */

namespace GOTech.Models
{
    public class Project
    {
        [Key]
        public virtual int ProjectId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between 2 to 50 characters")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.MultilineText)]
        public virtual string Description { get; set; }
        public virtual bool Ongoing { get; set; }
        
        // 1 Project has multiple reviews
        public List<Review> Reviews { get; set; }

    }

    /* This class is for the joined entities "Employee (ApplicationUser)" & "Project" 
     * to give a Many-to-Many relationship between 2 entities
     **/

    public class EmployeeProject
    {
        [Key]
        public virtual int Id { get; set; }

        // ApplicationUserId is a string value
        public virtual string ApplicationUserId { get; set; }
        public virtual int ProjectId { get; set; }
    
    }
}