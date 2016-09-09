using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saffron1.Models
{
    public class AccountType
    {
        //Internal Fields
        public int ID { get; set; }
        public string Name { get; set; }



        //one to one Relationships

        //one to Many or many to many relationships

        public AccountType()
        {
            this.Accounts = new HashSet<Account>();
        }

        public ICollection<Account> Accounts { get; set; }

    }
}