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
        public virtual string Title { get; set; }
    }
}