/* 
 * Name: Jo Lim
 * Date: Mar 25, 2019
 * Last Modified: Mar 25, 2019
 * */

using System.ComponentModel.DataAnnotations;

namespace GOTech.Models
{
    public class Position
    {
        // Primary key
        [Key]
        public virtual int PositionId { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be between {2} ~ {1} characters long.", MinimumLength = 2)]
        public virtual string Title { get; set; }
    }
}