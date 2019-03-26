using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GOTech.Models
{
    public class Position
    {
        // Primary key
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
    }
}