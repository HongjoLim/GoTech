using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GOTech.Models
{
    public class Person
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Contact { get; set; }
    }
}