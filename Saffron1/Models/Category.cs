using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saffron1.Models
{
    public class Category
    {
        //internal fields
        public int Id { get; set; }
        public string Name { get; set; }

        //one to one relationships

        //one to many and many to many relationships
        public Category()
        {
            this.Households = new HashSet<Household>();
        }

        public virtual ICollection<Household> Households { get; set; }

    }
}