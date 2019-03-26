using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GOTech.Models
{
    public class Review
    {
        public virtual int Id { get; set; }

        // Foreign key
        public virtual int ProjectId { get; set; }
    }
}