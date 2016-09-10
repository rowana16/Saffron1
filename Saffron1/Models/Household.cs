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
            this.Users = new HashSet<ApplicationUser>();
            this.Invitees = new HashSet<Invitee>();
        }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Invitee> Invitees { get; set; }

    }
}