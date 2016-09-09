using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saffron1.Models
{
    public class BudgetItem
    {
        //internal fields
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int BudgetId { get; set; }
        public float Amount { get; set; }
        public int RepeatFrequencyId { get; set; }
        public int DayOfMonth { get; set; }

        //one to one relationships
        public virtual Category Category { get; set; }
        public virtual Budget Budget { get; set; }
        public virtual RepeatFrequency RepeatFrequency { get; set; }

        //one to many and many to many relationships
    }
}