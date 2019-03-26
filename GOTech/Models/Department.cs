using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GOTech.Models
{
    public class Department
    {
        public virtual int Id { get; set; }
        public virtual string Name{get; set; }
        public virtual Employee Manager{ get; set; }
        public virtual List<Employee> Members { get; set; }
    }
}