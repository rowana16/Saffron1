using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saffron1.Models
{
    public class TypeTransaction
    {
        //Internal Fields
        public int Id { get; set; }
        public string Name { get; set; }

        //one to one relationships

        //one to many and many to many relationships
        public TypeTransaction()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public virtual ICollection<Transaction> Transactions { get; set; }



    }
}