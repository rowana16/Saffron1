using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saffron1.Models
{
    public class Budget
    {
        //internal fields
        public int Id { get; set; }
        public string Name { get; set; }
        public int HouseholdId { get; set; }

        //one to one relationships
        public virtual Household Household { get; set; }


        //one to many and many to many relationships

    }
}