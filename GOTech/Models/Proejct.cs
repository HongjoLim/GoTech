/* 
 * Name: Jo Lim
 * Date: Mar 25, 2019
 * Last Modified: Mar 25, 2019
 * */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        
        public List<Review> Reviews { get; set; }

    }

    /* This class is for the joined table (employee - project) to give a many-to-many relationship between 2 entities
     * 
     **/

    public class EmployeeProject
    {
        public virtual int Id { get; set; }
        public virtual int ApplicationUserId { get; set; }
        public virtual int ProjectId { get; set; }
    
    }
}