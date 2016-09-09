using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saffron1.Models
{
    public class Household
    {
        //  internal fields
        public int Id { get; set; }
        public string Name { get; set; }

        // one to one relationships


        // one to many and many to many relationships
        public Household()
        {
            this.Accounts = new HashSet<Account>();
            this.Budgets = new HashSet<Budget>();
            this.Categories = new HashSet<Category>();
        }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Category> Categories { get; set; }

    }
}