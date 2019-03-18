using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GOTech.Models
{
    public class Client
    {
        public virtual int Id{get; set;}
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

    }
}