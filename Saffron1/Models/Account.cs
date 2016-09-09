using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saffron1.Models
{
    public class Account
    {
        //internal fields
        public int Id { get; set; }       
        public string Name { get; set; }
        public float Balance { get; set; }
        public float ReconciledBalance { get; set; }

        public int HouseholdId { get; set; }
        public int InstitutionId { get; set; }
        public int AccountTypeId { get; set; }
        //one to one relationships
        public virtual Household Household { get; set; }

        //one to many and many to many relationships
        public Account()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public virtual ICollection<Transaction> Transactions { get; set; }

    }
}