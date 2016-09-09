using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saffron1.Models
{
    public class RepeatFrequency
    {
        // Internal Fields
        public int Id { get; set; }
        public string Frequency { get; set; }


        //one to one relationships

        //one to many and many to many relationships
        public RepeatFrequency()
        {
            this.BudgetItems = new HashSet<BudgetItem>();
        }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
    }
}