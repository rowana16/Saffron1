using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Saffron1.Models
{
    public class Transaction
    {
        //internal fields
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public int TypeTransactionId { get; set; }
        public int CategoryId { get; set; }
        public string EnteredById { get; set; }
        public bool Reconciled { get; set; }
        public float ReconciledAmount { get; set; }
        public string EnteredBy_Id { get; set; }



        //one to one relationships
        public virtual Account Account { get; set; }
        public virtual TypeTransaction Type { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser Enteredby { get; set; }
        public virtual ApplicationUser Enteredby_ { get; set; }



        //one to many and many to many relationships
    }  
    
}