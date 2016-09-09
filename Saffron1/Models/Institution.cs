using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saffron1.Models
{
    public class Institution
    {
        //Internal Fields
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Investment { get; set; }
        public bool Checking { get; set; }
        public bool Savings { get; set; }
        public bool Credit { get; set; }

        //one to one Relationships

        //one to Many or many to many relationships
        public Institution()
        {
            this.Accounts = new HashSet<Account>();
        }

        public virtual ICollection<Account> Accounts { get; set; }


    }
}